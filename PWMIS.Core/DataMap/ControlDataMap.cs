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
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Collections;
using System.Data;
using System.Reflection;
using PWMIS.Common;
using System.Collections.Generic;
using PWMIS.DataMap.Entity;

namespace PWMIS.DataMap
{


    /// <summary>
    /// BrainControl 的摘要说明。
    /// </summary>
    public class ControlDataMap
    {
        public ControlDataMap()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        #region 数据填充
        public void FillData(DataTable dtTmp, ICollection controls)
        {
            //处理DataTable
            foreach (object control in controls)
            {
                if (control is IDataControl)
                {
                    IDataControl brainControl = control as IDataControl;
                    if (dtTmp.TableName == brainControl.LinkObject
                        && dtTmp.Columns.Contains(brainControl.LinkProperty))
                    {
                        brainControl.SetValue(dtTmp.Rows[0][brainControl.LinkProperty]);
                    }
                }
            }
        }

        public void FillData(DataSet objData, ICollection controls)
        {
            //2、如果传入的是DataSet

            foreach (DataTable dtTmp in objData.Tables)
            {
                FillData(dtTmp, controls);
            }
        }

        public  void FillData(object objData, ICollection controls, bool isEntityClass)
        {
            if (!isEntityClass)
                return;
            //处理实体对象
            Type type = objData.GetType();
            //Object obj = type.InvokeMember(null, BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.CreateInstance, null, null, null);
            string typeName = type.Name;
            foreach (object control in controls)
            {
                if (control is IDataControl)
                {
                    IDataControl brainControl = control as IDataControl;
                    if (brainControl.LinkObject == typeName &&  !string.IsNullOrEmpty(brainControl.LinkProperty)) // obj.GetType().Name
                    {
                        object DataObj = type.InvokeMember(brainControl.LinkProperty, BindingFlags.GetProperty, null, objData, null);
                        if (DataObj == null && (brainControl.SysTypeCode == TypeCode.DateTime))
                        {
                            brainControl.SetValue(DBNull.Value);
                            continue;
                        }
                        brainControl.SetValue(DataObj);
                    }
                }

            }
        }

        public void FillData(object objData, ICollection controls)
        {
            if (objData is DataTable)
                FillData(objData as DataTable, controls);
            else if (objData is DataSet)
                FillData(objData as DataSet, controls);
            else
                FillData(objData, controls, true);

        }

        #endregion

        #region 数据收集
        public void CollectData(DataTable objData, ICollection controls)
        {
            DataTable dtTmp = objData as DataTable;
            if (dtTmp.Rows.Count == 0)
            {
                DataRow dr = dtTmp.NewRow();
                //
                foreach (DataColumn dataCol in dtTmp.PrimaryKey)
                {
                    dr[dataCol] = System.Guid.NewGuid().ToString("N");// PrimaryKey.GetPrimaryKey();
                }
                dr = GetDateTable(dtTmp.TableName, dr, controls);
                dtTmp.Rows.Add(dr);
            }
            else
            {
                //目前只处理表中只有一条记录的情况
                DataRow dr = dtTmp.Rows[0];
                dr = GetDateTable(dtTmp.TableName, dr, controls);
            }
        }

        public void CollectData(DataSet objData, ICollection controls)
        {
            //2、如果传入的是DataSet

            foreach (DataTable dtTmp in (objData as DataSet).Tables)
            {
                CollectData(dtTmp, controls);
            }
        }

        public  void CollectData(object objData, ICollection controls, bool isEntityClass)
        {
            if (!isEntityClass)
                return;
            //处理实体对象
            if (objData is EntityBase)
            {
                EntityBase entity = objData as EntityBase;

                foreach (object control in controls)
                {
                    if (control is IDataControl)
                    {
                        //情况单独处理
                        IDataControl brainControl = control as IDataControl;
                        if (brainControl.IsValid)
                        {
                            //string cao = obj.GetType().Name;
                            if (brainControl.LinkObject == entity.TableName && brainControl.LinkProperty != "")
                            {
                                object oValue = brainControl.GetValue();
                                //add 2008.7.22
                                if (brainControl.SysTypeCode != TypeCode.String && (oValue == null || oValue.ToString() == ""))
                                    continue;

                                //EditFlag 邓太华 2006.9.17 处理 System.DBNull.Value 情况
                                if (oValue != System.DBNull.Value)
                                {
                                    entity.setProperty(brainControl.LinkProperty, oValue);
                                }
                            }
                        }
                        else
                        {
                            throw new Exception("绑定到属性字段[" + brainControl.LinkProperty + "]前未通过服务器验证！");
                        }


                    }

                }
            }
            else
            {
                Type type = objData.GetType();
                object obj = type.InvokeMember(null, BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.CreateInstance, null, null, null);
                foreach (object control in controls)
                {
                    if (control is IDataControl)
                    {
                        //情况单独处理
                        IDataControl brainControl = control as IDataControl;
                        if (brainControl.IsValid)
                        {
                            //string cao = obj.GetType().Name;
                            if (brainControl.LinkObject == obj.GetType().Name && brainControl.LinkProperty != "")
                            {
                                object oValue = brainControl.GetValue();
                                //add 2008.7.22
                                if (brainControl.SysTypeCode != TypeCode.String && (oValue == null || oValue.ToString() == ""))
                                    continue;

                                //EditFlag 邓太华 2006.9.17 处理 System.DBNull.Value 情况
                                if (oValue == System.DBNull.Value)
                                {
                                    type.InvokeMember(brainControl.LinkProperty, BindingFlags.SetProperty, Type.DefaultBinder, objData, new object[] { null });
                                    continue;
                                }
                                if (brainControl.SysTypeCode == TypeCode.Empty)
                                    throw new Exception("收集数据控件的数据失败，SysTypeCode 属性不能为Empty ！");
                                type.InvokeMember(brainControl.LinkProperty, BindingFlags.SetProperty, Type.DefaultBinder, objData, new object[] { Convert.ChangeType(oValue, brainControl.SysTypeCode) });
                            }

                        }
                        else
                        {
                            throw new Exception("绑定到属性字段[" + brainControl.LinkProperty + "]前未通过服务器验证！");
                        }


                    }

                }
            }
        }

        /// <summary>
        /// 从智能数据控件中收集数据到一个新的对象中（如实体对象）
        /// </summary>
        /// <typeparam name="T">返回的对象类型</typeparam>
        /// <param name="controls">控件列表</param>
        /// <returns>一个新的对象</returns>
        public T CollectDataToObject<T>(List<IDataControl> controls) where T : class,new() 
        {
            Type type = typeof (T);
            T objData = new T();

            foreach (IDataControl brainControl in controls)
            {
                if (brainControl.IsValid)
                {
                    //string cao = obj.GetType().Name;
                    if (brainControl.LinkObject == type.Name  && brainControl.LinkProperty != "")
                    {
                        object oValue = brainControl.GetValue();
                        //add 2008.7.22
                        if (brainControl.SysTypeCode != TypeCode.String && (oValue == null || oValue.ToString() == ""))
                            continue;

                        //EditFlag 邓太华 2006.9.17 处理 System.DBNull.Value 情况
                        if (oValue == System.DBNull.Value)
                        {
                            type.InvokeMember(brainControl.LinkProperty, BindingFlags.SetProperty, Type.DefaultBinder, objData, new object[] { null });
                            continue;
                        }

                        type.InvokeMember(brainControl.LinkProperty, BindingFlags.SetProperty, Type.DefaultBinder, objData, new object[] { Convert.ChangeType(oValue, brainControl.SysTypeCode) });
                    }

                }
                else
                {
                    throw new Exception("绑定到属性字段[" + brainControl.LinkProperty + "]前未通过服务器验证！");
                }
                    
            }
            return objData;
        }

        private DataRow GetDateTable(string TableName, DataRow dr, ICollection Controls)
        {
            foreach (object control in Controls)
            {
                if (control is IDataControl)
                {
                    IDataControl brainControl = control as IDataControl;
                    if (brainControl.LinkObject == TableName)
                    {
                        if (brainControl.Validate() && !brainControl.IsNull)
                        {
                            dr[brainControl.LinkProperty] = brainControl.GetValue();
                        }
                        else
                        {
                            throw new Exception("数据类型错误：" + brainControl.LinkProperty);
                        }
                    }
                }

            }
            return dr;
        }

        public void CollectData(object objData, ICollection controls)
        {
            if (objData is DataTable)
                CollectData(objData as DataTable, controls);
            else if (objData is DataSet)
                CollectData(objData as DataSet, controls);
            else
                CollectData(objData, controls, true);
        }


        #endregion

        /// <summary>
        /// 收集控件的查询字符串，例如已经为控件指定了查询条件比较符号。
        /// </summary>
        /// <param name="conlObject">容器对象</param>
        /// <returns>查询字符串</returns>
        public static string CollectQueryString(ICollection  Controls)
        {
            string conditin = string.Empty;
            foreach (object  control in Controls)
            {
                if (control is IDataControl && control is IQueryControl)
                {
                    //((IDataControl)control).SetValue("");
                    IDataControl ibC = (IDataControl)control;
                    object Value = ibC.GetValue();
                    //如果控件值为空,那么跳过.
                    if (Value == null || Value.ToString() == "")
                        continue;

                    string compareSymbol = ((IQueryControl)ibC).CompareSymbol;
                    string queryFormatString = ((IQueryControl)ibC).QueryFormatString;
                    //默认的比较符号为 等于 "="
                    if (compareSymbol == "") compareSymbol = "=";
                    conditin += " And " + ibC.LinkObject + "." + ibC.LinkProperty + " " + compareSymbol + " ";

                    if (ibC.SysTypeCode == TypeCode.String || ibC.SysTypeCode == TypeCode.DateTime)
                    {
                        string sValue = Value.ToString().Replace("'", "''");
                        if (queryFormatString != "")
                        {
                            sValue = String.Format(queryFormatString, sValue);
                            conditin += sValue.ToString();
                        }
                        else
                        {
                            if (compareSymbol.Trim().ToLower() == "like")
                                conditin += "'%" + sValue + "%'";
                            else
                                conditin += "'" + sValue + "'";
                        }
                    }
                    else
                        conditin += Value.ToString();
                    ////
                    //if (tb.QueryFormatString != "")
                    //{
                    //    conditin += String.Format(tb.QueryFormatString, tb.Text.Replace("'", "''"));
                    //}
                    //else
                    //{
                    //    if (tb.SysTypeCode == TypeCode.String)
                    //        if (tb.CompareSymbol.Trim().ToLower() == "like")
                    //            conditin += "'%" + tb.Text.Replace("'", "''") + "%'";
                    //        else
                    //            conditin += "'" + tb.Text.Replace("'", "''") + "'";
                    //    else
                    //        conditin += Value.ToString();
                    //}
                    //    //文本框的特殊处理
                    //    if (ibC is CBrainTextBox)
                    //    {

                    //        CBrainTextBox tb = control as CBrainTextBox;
                    //        if (tb.QueryFormatString != "")
                    //        {
                    //            conditin += String.Format(tb.QueryFormatString, tb.Text.Replace("'", "''"));
                    //        }
                    //        else
                    //        {
                    //            if (tb.SysTypeCode == TypeCode.String)
                    //                if (tb.CompareSymbol.Trim().ToLower() == "like")
                    //                    conditin += "'%" + tb.Text.Replace("'", "''") + "%'";
                    //                else
                    //                    conditin += "'" + tb.Text.Replace("'", "''") + "'";
                    //            else
                    //                conditin += Value.ToString();
                    //        }
                    //    }
                    //    else if (ibC is WelTop.ControlLibrary.Controls.Calendar)
                    //    {
                    //        //处理日历控件
                    //        WelTop.ControlLibrary.Controls.Calendar tb = control as WelTop.ControlLibrary.Controls.Calendar;
                    //        if (tb.QueryFormatString != "")
                    //        {
                    //            conditin += String.Format(tb.QueryFormatString, tb.Text);
                    //        }
                    //        else
                    //        {
                    //            if (tb.SysTypeCode == TypeCode.String || tb.SysTypeCode == TypeCode.DateTime)
                    //                conditin += "'" + tb.Text.Replace("'", "''") + "'";
                    //            else
                    //                conditin += Value.ToString();
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (ibC.SysTypeCode == TypeCode.String)
                    //            conditin += "'" + Value.ToString().Replace("'", "''") + "'";
                    //        else
                    //            conditin += Value.ToString();
                    //    }
                    //}
                    //else
                    //{
                    //    conditin += CollectQueryString(control);
                    //}
                }
            }
            return conditin;
        }

        

    }

}

