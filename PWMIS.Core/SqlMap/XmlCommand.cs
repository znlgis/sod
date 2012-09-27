/*
 * ========================================================================
 * Copyright(c) 2006-2010 PWMIS, All Rights Reserved.
 * Welcom use the PDF.NET (PWMIS Data Process Framework).
 * See more information,Please goto http://www.pwmis.com/sqlmap 
 * ========================================================================
 * 该类的作用
 * 
 * 作者：邓太华     时间：2008-10-12
 * 版本：V3.0
 * 
 * 修改者：         时间：                
 * 修改说明：
 * ========================================================================
*/
using System;
using System.Xml;
using System.IO;
using System.Reflection;
using System.Data;
using PWMIS.Common;

namespace PWMIS.DataMap.SqlMap
{
    /// <summary>
    /// SQL-MAP XML命令类
    /// </summary>
    /// <remarks></remarks>
    public class XmlCommand
    {
        private CommandType _CommandType;
        private string _CommandName;
        private string _CommandGroupName;
        private string _QueryType;
        private string _QueryString;
        private string _SqlTextPath;
        private DBMSType _ScriptType;
        private string _ErrDescription;
        private static string Cache_SqlText = "";
        private static System.Collections.Generic.Dictionary<string, string> dictCacheSqlText = new System.Collections.Generic.Dictionary<string, string>();
        //缓存的配置文件
        string strBaseNodePath;
        //实际查找的路径
        string strOldBaseNodePath;
        //基本查找路径模板

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="SqlTextPath">配置文件路径</param>
        /// <param name="ScriptType">数据类型</param>
        /// <remarks></remarks>
        public XmlCommand(string SqlTextPath, DBMSType ScriptType)
        {
            this.SqlTextPath = SqlTextPath;
            this.ScriptType = ScriptType;
        }

        /// <summary>
        /// 数据库脚本类型，符合 enumScriptType 的枚举类型
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public DBMSType ScriptType
        {
            get { return _ScriptType; }
            set
            {
                _ScriptType = value;
                strOldBaseNodePath = "/SqlMap/Script[@Type='" + _ScriptType.ToString() + "']/CommandClass[@Name='@@CommandGroupName']";
            }
        }

        /// <summary>
        /// 获取脚本的命令类型
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public CommandType CommandType
        {
            get { return _CommandType; }
        }

        /// <summary>
        /// 获取脚本的命令组(class)名称
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string CommandClassName
        {
            get { return _CommandGroupName; }
            set
            {
                _CommandGroupName = value;
                strBaseNodePath = strOldBaseNodePath.Replace("@@CommandGroupName", _CommandGroupName);
            }
        }

        /// <summary>
        /// 获取脚本的命令名称
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string CommandName
        {
            get { return _CommandName; }
        }

        /// <summary>
        /// 获取脚本的查询类型，通常是 Select,Delete,Update 等
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string QueryType
        {
            get { return _QueryType; }
        }

        /// <summary>
        /// 获取错误信息
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string ErrDescription
        {
            get { return _ErrDescription; }
        }

        /// <summary>
        /// 获取命令表示的查询字符串，通常是一个带参数的命令文本或存储过程
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string QueryString
        {
            get { return _QueryString; }
        }

        /// <summary>
        /// SQL脚本配置文件的路径
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string SqlTextPath
        {
            get { return _SqlTextPath; }
            set { _SqlTextPath = value; }
        }


        /// <summary>
        /// 获取命令，通过命令名或者带命名组名称的命令名，获取命令信息
        /// </summary>
        /// <param name="CommandName"></param>
        /// <returns>命令文本</returns>
        /// <remarks></remarks>
        public string GetCommand(string CommandName)
        {
            int at = CommandName.IndexOf('.');
            if (at != -1)
            {
                //包含命令组名称
                this.CommandClassName = CommandName.Substring(0, at);//VB: Strings.Left(CommandName, at);
                CommandName = CommandName.Substring(at + 1);//VB: Strings.Mid(CommandName, at + 2);
            }

            _ErrDescription = string.Empty;
            if (this.CommandClassName == string.Empty)
            {
                _ErrDescription = "未设置命令组(class)名称！";
                return "";
            }

            XmlDocument doc = new XmlDocument();
            try
            {
                // doc.Load(_SqlTextPath) '可以考虑采用XML文件流的方式加快读取
                //dth,2008.4.8

                //由于可能在多个程序集中使用嵌入式SQL-MAP配置文件，所以不再缓存该配置文件 
                //dth,2010.3.19
                //if (Cache_SqlText.Length == 0) 
                //{
                
                if (_SqlTextPath.StartsWith("@R://"))
                {
                   
                    //从嵌入式资源文件读取
                    //PDF.NET 2.0 方式，从一个程序集加载SQL-MAP配置文件
                    //string[] source = _SqlTextPath.Substring(5).Split(',');
                    //Assembly ass = Assembly.Load(source[0]);
                    //Stream so = ass.GetManifestResourceStream(source[1]);

                    //PDF.NET 3.1 方式,从当前调用者程序集加载SQL-MAP配置文件(大小写不敏感)
                    
                    //string[] stackList =Environment.StackTrace.Split ('\r','\n');
                    //if (stackList.Length > 8)//此段代码不可放到方法外面去
                    //{
                    //    string callingAssembly = stackList[8].Trim().ToLower();
                    //    string[] source = _SqlTextPath.Substring(5).Split(',');//程序集+SQL-MAP配置文件成对出现
                    //    bool flag = false;
                    //    for (int i = 0; i < source.Length; i = i + 2)
                    //    {
                    //        if (callingAssembly.IndexOf(source[i].ToLower()) > 0)
                    //        {
                    //            string xmlString = string.Empty;
                    //            if (dictCacheSqlText.ContainsKey(source[i]))
                    //            {
                    //                //从缓存读取
                    //                xmlString = dictCacheSqlText[source[i]];
                    //            }
                    //            else
                    //            {
                    //                Assembly ass = Assembly.Load(source[i]);
                    //                Stream so = ass.GetManifestResourceStream(source[i + 1]);
                    //                StreamReader sr = new StreamReader(so);
                    //                xmlString = sr.ReadToEnd();
                    //                sr.Close();
                    //                dictCacheSqlText.Add(source[i], xmlString);
                    //            }
                    //            doc.LoadXml(xmlString);
                    //            flag = true;
                    //            break;
                    //        }

                    //    }
                    //    if (!flag )
                    //        throw new Exception("SQL-MAP配置文件名设置错误，当前调用堆栈中未匹配指定的程序集名称。错误位置：在应用程序配置文件中 add key=\"SqlMapFile\" value=\"" + _SqlTextPath+"\"\r\n 堆栈信息："+ stackList[8]);
                    //}
                    //else
                    //{
                    //    throw new Exception("SQL-MAP配置文件名设置时候发生意外错误，未获得指定的堆栈信息。堆栈层数：" + stackList.Length + "，详细堆栈信息：\r\n" + Environment.StackTrace);//SQL-MAP配置文件名设置错误，
                    //}
                   
                    //4.0 版本处理多个SQL-MAP文件的方式
                    
                    string[] source = _SqlTextPath.Substring(5).Split(',');//程序集+SQL-MAP配置文件成对出现
                    if (source.Length != 2)
                        throw new Exception("嵌入的SQL-MAP资源文件的申明应该以 \"程序集名称,程序集默认名称空间.SqlMap文件名.config\" 的格式 ");
                    string xmlString = string.Empty;
                    if (dictCacheSqlText.ContainsKey(_SqlTextPath))//SqlMap文件可能在一个程序集中
                    {
                        //从缓存读取
                        xmlString = dictCacheSqlText[_SqlTextPath];
                    }
                    else
                    {
                        //Assembly ass = Assembly.Load(source[0]);
                        //Stream so = ass.GetManifestResourceStream(source[1]);
                        //StreamReader sr = new StreamReader(so);
                        //xmlString = sr.ReadToEnd();
                        //sr.Close();
                        xmlString=PWMIS.Core.CommonUtil.GetAssemblyResource(source[0], source[1]);
                        dictCacheSqlText[_SqlTextPath]= xmlString;
                    }
                    doc.LoadXml(xmlString);
                   
                    
                }
                else
                {
                    //从指定的文件路径读取
                    if (Cache_SqlText.Length == 0)
                    {
                        StreamReader sr = new StreamReader(_SqlTextPath);
                        Cache_SqlText = sr.ReadToEnd();
                        sr.Close();
                    }
                    //从单一缓存文件读取
                    doc.LoadXml(Cache_SqlText);
                   
                }

               



                //Create an XmlNamespaceManager for resolving namespaces.
                //Dim nsmgr As XmlNamespaceManager = New XmlNamespaceManager(doc.NameTable)
                //nsmgr.AddNamespace("bk", "urn:samples")
                //Select the book node with the matching attribute value.
                XmlNode SqlText = default(XmlNode);
                XmlElement root = doc.DocumentElement;
                string objPath = strBaseNodePath + "/*[@CommandName='" + CommandName + "']";
                SqlText = root.SelectSingleNode(objPath);
                if ((SqlText != null) && SqlText.HasChildNodes)
                {
                    //foreach (XmlNode node in SqlText.ChildNodes)
                    //{
                    //    if ((node.Attributes["CommandName"] != null) && node.Attributes["CommandName"].Value == CommandName)
                    //    {
                    //        _CommandName = CommandName;
                    //        _CommandType = (CommandType)Enum.Parse(typeof(CommandType), node.Attributes["CommandType"].Value);
                    //        _QueryType = node.Name;
                    //        //.Attributes("QueryType").Value
                    //        return node.InnerText;
                    //    }
                    //}

                    XmlNode node = SqlText;
                    _CommandName = CommandName;
                    _CommandType = (CommandType)Enum.Parse(typeof(CommandType), node.Attributes["CommandType"].Value);
                    _QueryType = node.Name;
                    //.Attributes("QueryType").Value
                    return node.InnerText;
                }
            }
            catch (Exception ex)
            {
                _ErrDescription = ex.ToString();
            }

            return "";
        }

    }

}

