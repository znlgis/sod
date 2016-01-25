/*
 * ========================================================================
 * Copyright(c) 2006-2010 PWMIS, All Rights Reserved.
 * Welcom use the PDF.NET (PWMIS Data Process Framework).
 * See more information,Please goto http://www.pwmis.com/sqlmap 
 * ========================================================================
 * 该类的作用是作为“智能数据表单窗体”的抽象类，用于WebForm、WinForm的基础类
 * 
 * 作者：邓太华     时间：2013-3-22
 * 版本：V5.2
 * 
 * 修改者：         时间：2015-3-7                
 * 修改说明：修复智能表单控件在Access 等数据库上由于参数顺序引起的查询问题
 * 
 *  修改者：         时间：2015-7-8                
 * 修改说明：修复设置为主键的表单控件，无法插入主键数据的问题
 *           修复设置为主键的表单控件，对应的数据库标识字段插入的问题
 * ========================================================================
*/
using System;
using System.Collections.Generic;
using System.Text;
using PWMIS.Common;
using System.Data;
using PWMIS.DataProvider.Data;
using PWMIS.DataProvider.Adapter;


namespace PWMIS.DataForms.Adapter
{
    /// <summary>
    /// 用户使用数据控件的自定义方法委托
    /// </summary>
    /// <param name="dataControl"></param>
    public delegate void UseDataControl(IDataControl dataControl);

    /// <summary>
    /// 我的数据窗体，是Web/WinForm 数据窗体抽象类
    /// </summary>
    public abstract class MyDataForm
    {
        /// <summary>
        /// 是否检查更新结果所影响的行数，如果检查，那么受影响的行不大于0将抛出错误。
        /// </summary>
        public bool CheckAffectRowCount
        {
            get;
            set;
        }

        private CommonDB _dao = null;
        /// <summary>
        /// 获取或者设置数据访问对象，默认使用静态 CommonDB 类的实例对象，
        /// 如果使用事务并且有并发访问，请设置 CommonDB 的动态实例，例如：MyDB.GetDBHelper();
        /// </summary>
        public CommonDB DAO
        {
            get
            {
                if (_dao == null)
                    _dao = MyDB.Instance;
                return _dao;
            }
            set
            {
                _dao = value;
            }
        }

        public IBCommand IBCommand
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public MyDB MyDB
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public IDataControl IDataControl
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        /// <summary>
        /// 获取选择和删除查询的SQL语句
        /// </summary>
        /// <param name="IBControls">已经收集的控件集合</param>
        /// <returns> ArrayList 中的成员为 IBCommand 对象，包含具体的CRUD SQL</returns>
        public static List<IBCommand> GetSelectAndDeleteCommand(List<IDataControl> IBControls)
        {
            List<IBCommand> IBCommands = new List<IBCommand>();
            //获取表单中的CRUD 语句。
            while (IBControls.Count > 0)
            {
                string strTableName = "";
                string strSelect = "";
                string strDelete = "";
                string strFields = "";
                //string strValues="";

                string strCondition = "";
                int nullCount = 0;


                for (int i = 0; i < IBControls.Count; i++)// object objCtr in IBControls)
                {
                    object objCtr = IBControls[i];
                    if (objCtr != null)
                    {
                        IDataControl ibCtr = objCtr as IDataControl;
                        //只有非只读的控件才可以更新数据到数据库
                        if (ibCtr != null)
                        {
                            if (strTableName == "")
                            {
                                strTableName = ibCtr.LinkObject;
                                strSelect = "SELECT ";
                                strDelete = "DELETE FROM [" + strTableName + "]";

                            }
                            //找到当前处理的表，只读的字段也可以处理
                            if (strTableName == ibCtr.LinkObject && ibCtr.LinkObject != "")
                            {
                                string cValue = ibCtr.GetValue().ToString().Replace("'", "''");
                                if (ibCtr.PrimaryKey)
                                {
                                    if (cValue != "")
                                    {
                                        //防止SQL注入式攻击
                                        cValue = (ibCtr.SysTypeCode == System.TypeCode.String || ibCtr.SysTypeCode == System.TypeCode.DateTime ? "'" + cValue + "'" : Convert.ToDouble(cValue).ToString());
                                    }
                                    strCondition += " And [" + ibCtr.LinkProperty + "] = " + cValue;
                                }
                                string temp = "[" + ibCtr.LinkProperty + "],";
                                if (ibCtr.LinkProperty != "" && strFields.IndexOf(temp) == -1)
                                    strFields += temp;

                                IBControls[i] = null;
                            }
                            if (ibCtr.LinkObject == "" || ibCtr.LinkProperty == "")
                                IBControls[i] = null;

                        }
                    }
                    else
                        nullCount++;

                }//end for

                if (strFields == "")
                    break;

                strSelect += strFields.TrimEnd(',') + " FROM [" + strTableName + "] WHERE 1=1 " + strCondition;
                strDelete += " WHERE 1=1 " + strCondition;

                IBCommand ibcmd = new IBCommand(strTableName);
                ibcmd.SelectCommand = strSelect;
                ibcmd.DeleteCommand = strDelete;

                IBCommands.Add(ibcmd);

                if (nullCount >= IBControls.Count - 1)
                    break;
            }//end while

            return IBCommands;
        }

        protected void AutoUpdateIBFormDataInner(List<IBCommand> ibCommandList)
        {
            int result = 0;
            foreach (object item in ibCommandList)
            {
                IBCommand command = (IBCommand)item;
                bool insertFlag = false;
                if (command.InsertedID > 0)
                    result = DAO.ExecuteNonQuery(command.UpdateCommand, CommandType.Text, command.Parameters);//修改未合并
                else if (command.InsertedID == -2)
                {
                    result = DAO.ExecuteNonQuery(command.UpdateCommand, CommandType.Text, command.Parameters);
                    if (result <= 0)
                        insertFlag = true;
                }
                else
                {
                    insertFlag = true;

                }
                if (insertFlag)
                {
                    object id = 0;
                    result = DAO.ExecuteInsertQuery(command.InsertCommand, CommandType.Text, command.InsertParameters, ref id,null);
                    if (id != DBNull.Value)
                    {
                        try
                        {
                            command.InsertedID = Convert.ToInt32(id);
                        }
                        catch
                        { }
                    }
                    else
                    {
                        command.InsertedID = -2;
                    }
                }
                if (result <= 0 && CheckAffectRowCount)
                    throw new Exception("在更新表" + command.TableName + "中未取得受影响的行数，数据错误信息：" + DAO.ErrorMessage);

            }
        }

        protected bool AutoUpdateIBFormDataInner(List<IBCommand> ibCommandList, IDataControl pkControl)
        {
            object guidObj = pkControl.GetValue();
            //if (guidObj == null || guidObj.ToString() == "")
            //    throw new Exception("GUID 主键或字符型主键列更新数据不能为空！");
            //if (pkControl.ReadOnly)
            //    throw new Exception("GUID 主键或字符型主键列更新数据时不能设置为只读！");
            if (!pkControl.PrimaryKey)
                throw new Exception("GUID 主键或字符型主键列更新数据时必须设置PrimaryKey属性！");

            string guidText = guidObj.ToString();

            int result = 0;
            foreach (IBCommand command in ibCommandList)
            {
                if (command.TableName == pkControl.LinkObject)
                {
                    string sql = "SELECT [" + pkControl.LinkProperty + "] FROM [" + pkControl.LinkObject + "] WHERE [" + pkControl.LinkProperty + "] = '" + guidText + "'";
                    object guidInDb = DAO.ExecuteScalar(sql);
                    if (guidInDb != null && guidInDb.ToString() == guidText)
                    {
                        //在数据库中有该记录
                        result = DAO.ExecuteNonQuery(command.UpdateCommand, CommandType.Text, command.Parameters);
                    }
                    else
                    {
                        result = DAO.ExecuteNonQuery(command.InsertCommand, CommandType.Text, command.InsertParameters);
                    }
                    return result > 0;
                }

            }
            return false;
        }

        protected void AutoSelectIBFormInner(List<IDataControl> IBControls)
        {
            List<IDataControl> IBControls2 = new List<IDataControl>();
            foreach (IDataControl obj in IBControls)
            {
                IBControls2.Add(obj);
            }
            //IBControls2 将会被请空
            List<IBCommand> ibCommandList = GetSelectAndDeleteCommand(IBControls2);

            foreach (IBCommand command in ibCommandList)
            {
                IDataReader reader = DAO.ExecuteDataReaderWithSingleRow(command.SelectCommand);
                if (reader != null && reader.Read())
                {
                    foreach (object obj in IBControls)
                    {
                        IDataControl ibCtr = obj as IDataControl;
                        if (ibCtr.LinkObject == command.TableName)
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                if (reader.GetName(i) == ibCtr.LinkProperty)
                                {
                                    ibCtr.SetValue(reader[i]);
                                    break;
                                }
                            }
                        }
                    }
                }
                //应该在此处关闭阅读器，否则在事物中出现Command对象繁忙的问题
                reader.Close();
            }
        }

        protected void AutoSelectIBFormInner(List<IDataControl> IBControls, DataSet dsSource)
        {
            foreach (DataTable dt in dsSource.Tables)
            {
                string tableName = dt.TableName;
                foreach (object obj in IBControls)
                {
                    IDataControl ibCtr = obj as IDataControl;
                    if (string.Compare(ibCtr.LinkObject, tableName, true) == 0)
                    {
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            if (string.Compare(dt.Columns[i].ColumnName, ibCtr.LinkProperty, true) == 0)
                            {
                                ibCtr.SetValue(dt.Rows[0][i]);
                                break;
                            }
                        }
                    }
                }
            }

        }

        protected void AutoSelectIBFormInner(List<IDataControl> IBControls,IEntity entity)
        {
            foreach (object obj in IBControls)
            {
                IDataControl ibCtr = obj as IDataControl;
                foreach (string key in entity.PropertyNames)
                {
                    if (string.Compare(key, ibCtr.LinkProperty, true) == 0)
                    {
                        ibCtr.SetValue(entity.PropertyList(key));
                        break;
                    }
                }

            }
        }

        protected int AutoDeleteIBFormInner(List<IDataControl> IBControls)
        {
            List<IDataControl> IBControls2 = new List<IDataControl>();
            foreach (IDataControl obj in IBControls)
            {
                IBControls2.Add(obj);
            }
            //IBControls2 将会被请空
            List<IBCommand> ibCommandList = GetSelectAndDeleteCommand(IBControls2);
            int count = 0;
            foreach (IBCommand command in ibCommandList)
            {
                count += DAO.ExecuteNonQuery(command.DeleteCommand);
            }
            return count;
        }

        /// <summary>
        /// 获取更新表单数据的智能表单命令，以备将这些控件中的数据保存到数据库
        /// <remarks>
        /// 注意：1，主键数据可以插入数据库，但不可直接更新，
        ///          如果想更新主键字段，需要另外增加一个数据文本框控件，绑定主键字段名，但不设置主键属性，
        ///          同时放置一个标签数据控件，设置为主键属性并需要运行是赋值给此控件。
        ///       2，自增主键字段对应的数据控件，需要设置主键属性并且不可以更新此控件的值到数据库。
        /// </remarks>
        /// </summary>
        /// <param name="IBControls"></param>
        /// <param name="DB"></param>
        /// <returns></returns>
        protected static List<IBCommand> GetIBFormDataInner(List<IDataControl> IBControls, CommonDB DB)
        {
            List<IBCommand> IBCommands = new List<IBCommand>();
            //获取表单中的CRUD 语句。
            while (IBControls.Count > 0)
            {
                string strTableName = "";
                string strInsert = "";
                string strFields = "";
                string strValues = "";
                string strUpdate = "";
                string strCondition = "";
                int nullCount = 0;
                int ID = -1;

                int paraIndex = 0;
                //edit at 2015.3.7
                //定义不同的参数列表，修正Access 等数据库根据参数顺序而不是参数名匹配进行查询的问题
                List<IDataParameter> paraUpdateList = new List<IDataParameter>();//更新，删除参数列表
                List<IDataParameter> paraInsertList = new List<IDataParameter>();//插入参数列表
                List<IDataParameter> paraPKs = new List<IDataParameter>();//主键参数列表

                for (int i = 0; i < IBControls.Count; i++)// object objCtr in IBControls)
                {
                    object objCtr = IBControls[i];
                    if (objCtr != null)
                    {
                        IDataControl ibCtr = objCtr as IDataControl;
                        //只有非只读的控件才可以更新数据到数据库
                        if (ibCtr != null)
                        {
                            if (strTableName == "" && ibCtr.LinkObject != "")
                            {
                                strTableName = ibCtr.LinkObject;
                                strInsert = "INSERT INTO [" + strTableName + "](";
                                strUpdate = "UPDATE [" + strTableName + "] SET ";
                            }
                            //找到当前处理的表，只有非只读的字段可以更新
                            if (strTableName == ibCtr.LinkObject && ibCtr.LinkProperty != "")
                            {
                                #region 原来获取值得方法
                                //string cValue=ibCtr.GetValue ().ToString ().Replace ("'","''");

                                ////dth,2008.4.11 处理字符串类型为空的情况
                                ////防止SQL注入式攻击
                                ////不论是否为空都进行字符串类型测试
                                //if(ibCtr.SysTypeCode==System.TypeCode.String || ibCtr.SysTypeCode==System.TypeCode.Empty)
                                //{
                                //    cValue="'"+ cValue +"'";
                                //}
                                //else
                                //{
                                //    if(cValue!="")
                                //    {
                                //        if(ibCtr.SysTypeCode==System.TypeCode.Boolean )
                                //            cValue=(cValue.ToUpper ()=="TRUE"?"1":"0");
                                //        else if(ibCtr.SysTypeCode==System.TypeCode.DateTime )
                                //        {
                                //            if (DB.CurrentDBMSType == DBMSType.SQLite)
                                //                cValue = "'" + DateTime.Parse(cValue).ToString("s") + "'";
                                //            else
                                //                cValue = "'" + cValue + "'";//SQL SERVER 日期格式

                                //        }
                                //        else if(ibCtr.SysTypeCode==System.TypeCode.DBNull )
                                //        {
                                //            cValue="NULL";
                                //        }
                                //        else if(ibCtr.SysTypeCode==System.TypeCode.Object )
                                //        {
                                //            //Object 标记不做任何处理，例如可以使用最大值加一获取主键值

                                //        }
                                //        else if(!(ibCtr.SysTypeCode==System.TypeCode.String || ibCtr.SysTypeCode==System.TypeCode.Empty))
                                //        {
                                //            //如果不是字符串那么试图进行数字转换
                                //            cValue=Convert.ToDouble (cValue).ToString ();
                                //        }

                                //    }
                                //}
                                #endregion

                                string ctlParaName = string.Empty;
                                object ctlValue = ibCtr.GetValue();

                                //非只读的数据才更新
                                if (ctlValue != DBNull.Value)
                                {
                                    ctlParaName = DB.GetParameterChar + "P" + paraIndex++;
                                    IDataParameter para = DB.GetParameter(ctlParaName, ctlValue);
                                    if (ibCtr.SysTypeCode == System.TypeCode.String || ibCtr.SysTypeCode == System.TypeCode.Empty)
                                    {
                                        if (ibCtr is IDataTextBox)
                                        {
                                            int maxStringLength = ((IDataTextBox)ibCtr).MaxLength;
                                            if(maxStringLength>0)
                                                ((IDbDataParameter)para).Size = maxStringLength;
                                        }
                                    }
                                   

                                    //2010.1.25 取消 ibCtr.PrimaryKey 不能更新的限制，例如可以让GUID主键列可以更新
                                    //2015.3.7 恢复 ibCtr.PrimaryKey 不能更新的限制，
                                    //如果需要更新主键，请设置另外一个控件并将它绑定位主键字段但不设置 PrimaryKey 属性
                                    //如果是自增列，设置该列的控件属性为 只读属性即可。

                                    if (ibCtr.PrimaryKey ) //只要是主键就作为更新的条件
                                    {
                                        if (!string.IsNullOrEmpty(ctlValue.ToString()))
                                        {
                                            strCondition += " And [" + ibCtr.LinkProperty + "] = " + ctlParaName;
                                            if (ibCtr.SysTypeCode == System.TypeCode.Int32)
                                                ID = int.Parse(ctlValue.ToString());
                                            else
                                                ID = -2;//主键可能是非数字型
                                            paraPKs.Add(para);
                                            //主键数据也可能需要插入
                                            if (!ibCtr.ReadOnly)
                                            {
                                                strFields += "[" + ibCtr.LinkProperty + "],";
                                                strValues += ctlParaName + ",";
                                                paraInsertList.Add(para);
                                            }
                                        }
                                    }
                                    else if (!ibCtr.ReadOnly)
                                    {
                                        strFields += "[" + ibCtr.LinkProperty + "],";
                                        strValues += ctlParaName + ",";
                                        strUpdate += "[" + ibCtr.LinkProperty + "] = " + ctlParaName + ",";

                                        paraUpdateList.Add(para);
                                        paraInsertList.Add(para);
                                    }
                                }


                            }
                            IBControls[i] = null;
                        }
                    }
                    else
                        nullCount++;

                }//end for

                if (nullCount >= IBControls.Count - 1)
                    break;

                strInsert += strFields.TrimEnd(',') + ") VALUES (" + strValues.TrimEnd(',') + ")";
                strUpdate = strUpdate.TrimEnd(',') + " WHERE 1=1 " + strCondition;

                IBCommand ibcmd = new IBCommand(strTableName);
                ibcmd.InsertCommand = strInsert;
                ibcmd.UpdateCommand = strUpdate;
                //if( ID>0) 
                ibcmd.InsertedID = ID;
                ibcmd.InsertParameters = paraInsertList.ToArray();

                paraUpdateList.AddRange(paraPKs);
                ibcmd.Parameters = paraUpdateList.ToArray();

                IBCommands.Add(ibcmd);
            }//end while

            return IBCommands;
        }
    }
}
