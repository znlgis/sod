#region << 版 本 注 释 >>

/*
 * ========================================================================
 * Copyright(c) 2008-2017 拼威&敏思--PWMIS, All Rights Reserved.
 * ========================================================================
 *
 * 内存数据库，参见 http://www.cnblogs.com/bluedoctor/archive/2011/09/20/2182722.html
 *
 *
 * 作者：转自网上     时间：2011/9/2 15:19:30
 * 版本：V2.0.0
 *
 * 修改者：         时间： 2013.5.15
 * 修改说明：使用.NET 4.0 线程安全的集合
 * ========================================================================
 */

#endregion

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using PWMIS.DataMap.Entity;

namespace PWMIS.MemoryStorage
{
    /// <summary>
    ///     PDF.NET内存数据库
    /// </summary>
    public class MemDB : IDisposable
    {
        private static readonly object lock_obj = new object();
        private readonly ConcurrentDictionary<string, object> mem_data;
        private readonly List<Func<bool>> methodList;
        private string _path = "";
        private bool isDisposed;
        private bool running;

        public MemDB(string dbPath)
        {
            Path = dbPath;
            mem_data = new ConcurrentDictionary<string, object>();
            methodList = new List<Func<bool>>();
            WriteLog("初始化数据库成功，基础目录：" + Path);
        }


        /// <summary>
        ///     数据所在的目录
        /// </summary>
        public string Path
        {
            get => _path;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _path = value;
                    if (!Directory.Exists(_path))
                        Directory.CreateDirectory(_path);

                    if (!Directory.Exists(FilePath))
                        Directory.CreateDirectory(FilePath);
                    if (!Directory.Exists(LogPath))
                        Directory.CreateDirectory(LogPath);
                }
            }
        }

        /// <summary>
        ///     数据文件路径
        /// </summary>
        private string FilePath => _path + "\\Data";

        /// <summary>
        ///     日志文件路径
        /// </summary>
        private string LogPath => _path + "\\Log";

        /// <summary>
        ///     关闭引擎
        /// </summary>
        public void Dispose()
        {
            if (!isDisposed)
                TurnOff();
        }

        /// <summary>
        ///     从数据文件载入实体数据（不会影响内存数据），建议使用Get的泛型方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> LoadEntity<T>() where T : EntityBase, new()
        {
            var t = typeof(T);
            var fileName = FilePath + "\\" + t.FullName + ".pmdb";
            if (File.Exists(fileName))
            {
                byte[] buffer = null;
                using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    var length = fs.Length;
                    buffer = new byte[length];
                    fs.Read(buffer, 0, (int)length);
                    fs.Close();
                }

                var result = PdfNetSerialize<T>.BinaryDeserializeArray(buffer);

                WriteLog("加载数据 " + fileName + " 成功！");
                return result.ToList();
            }

            return null;
        }

        /// <summary>
        ///     直接保存实体数据，如果文件已经存在则覆盖（不会影响内存数据）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public bool SaveEntity<T>(T[] entitys) where T : EntityBase, new()
        {
            var t = typeof(T);
            if (entitys == null)
            {
                WriteLog(t.FullName + " 数据为空！");
                return false;
            }

            var count = entitys.Length;
            if (count > 0)
            {
                WriteLog("开始写入数据，条数：" + count);

                var fileName = FilePath + "\\" + t.FullName + ".pmdb";
                var buffer = PdfNetSerialize<T>.BinarySerialize(entitys);
                using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(buffer, 0, buffer.Length);
                    fs.Flush();
                    fs.Close();
                }

                WriteLog("保存数据 " + fileName + " 成功！");
                return true;
            }

            return false;
        }

        /// <summary>
        ///     清除内存数据库的实体类数据文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void DropEntity<T>() where T : EntityBase, new()
        {
            var t = typeof(T);
            var fileName = FilePath + "\\" + t.FullName + ".pmdb";
            File.Delete(fileName);
            WriteLog("【删除】数据 " + fileName + " 成功！");
        }


        /// <summary>
        ///     （延迟）保存数据，该方法会触发数据真正保存到磁盘，请添加、修改数据后调用该方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Save<T>() where T : EntityBase, new()
        {
            AddSaveMethod(() =>
                {
                    var t = typeof(T);
                    var key = t.FullName;
                    if (mem_data.ContainsKey(key))
                        //此处将触发key 对应的数据的保存动作
                        lock (lock_obj)
                        {
                            var entitys = (BlockingCollection<T>)mem_data[key];
                            return SaveEntity(entitys.ToArray());
                        }

                    return false;
                }
            );
        }


        private void AddSaveMethod(Func<bool> toDo)
        {
            if (!methodList.Contains(toDo))
                methodList.Add(toDo);
        }

        /// <summary>
        ///     后台线程保存数据，必须区别该方法运行在单线程中
        /// </summary>
        protected internal void AutoSaveData()
        {
            //开启线程定时处理
            var t = new Thread(() =>
            {
                //监视更改，直到关闭数据库
                while (running)
                {
                    Flush();
                    Thread.Sleep(8000);
                }
            });
            running = true;
            t.Name = "PDF MemoryDB";
            t.Start();
            WriteLog("后台数据监视线程已开启！");
        }

        /// <summary>
        ///     将数据真正保持到磁盘
        /// </summary>
        protected internal void Flush()
        {
            foreach (var item in methodList.ToArray())
            {
                item();
                methodList.Remove(item);
            }
        }

        /// <summary>
        ///     （从缓存中）获取数据的镜像，至少包含0个元素的结果数组（结果不会为空引用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> Get<T>() where T : EntityBase, new()
        {
            var key = getKey<T>();
            return Get<T>(key).ToList();
        }

        /// <summary>
        ///     获取数据集合的直接引用，可以直接操作该集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public BlockingCollection<T> GetCollection<T>() where T : EntityBase, new()
        {
            var key = getKey<T>();
            return Get<T>(key);
        }

        private BlockingCollection<T> Get<T>(string key) where T : EntityBase, new()
        {
            //先从内存加载
            BlockingCollection<T> entitys = null;
            if (mem_data.ContainsKey(key))
            {
                entitys = (BlockingCollection<T>)mem_data[key];
            }
            else
            {
                //再从文件加载
                var list = LoadEntity<T>();
                entitys = new BlockingCollection<T>();
                if (list != null)
                    foreach (var item in list)
                        entitys.Add(item);

                mem_data[key] = entitys;
            }

            return entitys;
        }

        private string getKey<T>()
        {
            var t = typeof(T);
            var key = t.FullName;
            return key;
        }

        /// <summary>
        ///     添加一项到内存数据库
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <param name="item">实体对象</param>
        /// <returns>是否成功</returns>
        public bool Add<T>(T item) where T : EntityBase, new()
        {
            var key = getKey<T>();
            var data = Get<T>(key);
            data.Add(item);
            return true;
        }

        /// <summary>
        ///     从内存数据库移除一项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove<T>(T item) where T : EntityBase, new()
        {
            var key = getKey<T>();
            var list = Get<T>(key);
            var flag = list.TryTake(out item);
            return flag;
        }

        /// <summary>
        ///     刷新数据（例如直接调用了更新数据到磁盘的方法 SaveEntity 之后
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Refresh<T>()
        {
            var key = getKey<T>();
            if (mem_data.ContainsKey(key))
            {
                object Value;
                mem_data.TryRemove(key, out Value);
            }
        }

        /// <summary>
        ///     关闭数据库，清理资源
        /// </summary>
        public void Close()
        {
            running = false;
            mem_data.Clear();
            methodList.Clear();
            WriteLog("数据库已关闭！");
        }

        /// <summary>
        ///     关闭引擎（保存数据并且关闭数据库）
        /// </summary>
        public void TurnOff()
        {
            Flush();
            Close();
            isDisposed = true;
        }

        private void WriteLog(string text)
        {
            var fileName = LogPath + "\\pmdbLog_" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            File.AppendAllText(fileName, DateTime.Now.ToLongTimeString() + " " + text + "\r\n");
        }
    }

    /*  使用示例：
     *  /// <summary>
        /// 保存问题的回答结果
        /// </summary>
        /// <param name="uid">用户标识</param>
        /// <param name="answerValue">每道题的得分</param>
        public void SaveAnswerResult(string uid, int[] answerValue)
        {
            MemDB db = MemDBEngin.GetDB();
            QuestionResult[] resultList= db.Get<QuestionResult>();

                        QuestionResult oldResult = resultList.Where(p => p.UID == uid).FirstOrDefault();
                        if (oldResult != null)
                        {
                            oldResult.AnswerValue = answerValue;
                            oldResult.AnswerDate = DateTime.Now;

                                        }
                                        else
                                        {
                                            QuestionResult qr = new QuestionResult();
                                            qr.UID = uid;
                                            qr.AnswerValue = answerValue;
                                            qr.AnswerDate = DateTime.Now;

                                                            db.Add(qr);
                                                        }
                                                        db.Save<QuestionResult>();
                                                    }

                                                            /// <summary>
                                                            /// 载入某用户的答案数据
                                                            /// </summary>
                                                            /// <param name="uid"></param>
                                                            /// <returns></returns>
                                                            public int[] LoadAnswerResult(string uid)
                                                            {
                                                                MemDB db = MemDBEngin.GetDB();
                                                                QuestionResult[] resultList = db.Get<QuestionResult>();

                                                                            QuestionResult oldResult = resultList.Where(p => p.UID == uid).FirstOrDefault();
                                                                            if (oldResult != null)
                                                                                return oldResult.AnswerValue;
                                                                            else
                                                                                return null;
                                                                        }
                                                                     *
                                                                     *
                                                                     * 如果想查看内存数据库的运行日志，在MVC项目中，可以加入这样的一个Action：
                                                                     *
                                                                     *  //查看内存数据库工作日志
                                                                        public ActionResult ShowMemDBLog(string date)
                                                                        {
                                                                            //date="2011-10-09";
                                                                            string fileName =MemDBEngin.DbSource + "\\Log\\pmdbLog_"+date+".txt";
                                                                            if (System.IO.File.Exists(fileName))
                                                                            {
                                                                                return File(fileName, "text/plain");
                                                                            }
                                                                            else
                                                                            {
                                                                                return Content("日志文件不存在");
                                                                            }
                                                                        }
                                                                     */
}