/*
 * ========================================================================
 * Copyright(c) 2006-2010 PWMIS, All Rights Reserved.
 * Welcom use the PDF.NET (PWMIS Data Process Framework).
 * See more information,Please goto http://www.pwmis.com/sqlmap 
 * ========================================================================
 * 该类的作用
 * 
 * 作者：邓太华     时间：2008-10-12
 * 版本：V4.5
 * 
 * 修改者：         时间：2012-11-1                
 * 修改说明：收集数据的时候，改进对SQLite的支持
 * ========================================================================
*/
using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using PWMIS.DataProvider.Data;
using PWMIS.Common;
using PWMIS.Web.Controls;
using PWMIS.Common.DataMap;

namespace PWMIS.DataForms.Adapter
{
    ///// <summary>
    ///// 用户使用数据控件的自定义方法委托
    ///// </summary>
    ///// <param name="dataControl"></param>
    //public delegate void UseDataControl(IDataControl dataControl);

    /// <summary>
    /// 智能Web窗体数据处理类，包括数据收集和数据持久化（保存到数据库）等方法；如果使用事务请勿使用该类中间的静态方法。
    /// </summary>
    public class MyWebForm : MyDataForm
    {
        //private bool _CheckRowUpdateCount = false;//是否检查更新结果所影响的行数，如果检查，那么受影响的行不大于0将抛出错误。
        //private CommonDB _dao = null;
        private static MyWebForm _instance = null;
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public MyWebForm()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        

        /// <summary>
        /// 智能Web窗体数据处理类 的静态实例
        /// </summary>
        public static MyWebForm Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MyWebForm();
                return _instance;
            }
        }

        /// <summary>
        /// Web 窗体控件数据映射类
        /// </summary>
        public static WebControlDataMap DataMap
        {
            get {
                return new WebControlDataMap();
            }
        }

       
        /// <summary>
        /// 清除窗体上的智能控件的值
        /// </summary>
        /// <param name="Controls">窗体控件集和</param>
        public static void ClearIBData(ControlCollection Controls)
        {
            //使用匿名委托
            UseDataControl clearData = delegate(IDataControl dataControl)
            {
                dataControl.SetValue("");
            };
            DoDataControls(Controls, clearData);

        }

        /// <summary>
        /// 使用自定义的方法处理控件集合中每一个智能窗体数据控件，使用户不必额外遍历控件集合。
        /// </summary>
        /// <param name="controls">窗体容器控件的控件集合</param>
        /// <param name="useMethod">自定义的方法</param>
        public static void DoDataControls(ControlCollection controls, UseDataControl useMethod)
        {
            foreach (IDataControl item in GetIBControls(controls))
                useMethod(item);
        }

        /// <summary>
        /// 从控件集合的每个元素及其子元素中寻找所有的智能数据控件并返回智能控件列表
        /// </summary>
        /// <param name="controls">控件集合，如页面容器对象</param>
        /// <returns>智能控件列表</returns>
        public static List<IDataControl> GetIBControls(ControlCollection controls)
        {
            List<IDataControl> IBControls = new List<IDataControl>();
            findIBControls(IBControls, controls);
            return IBControls;
        }

        #region 页面数据收集

        /// <summary>
        /// 寻找智能控件，将其放到数组列表中
        /// </summary>
        /// <param name="arrIBs">存放控件的数组</param>
        /// <param name="controls">要寻找的原控件集合</param>
        private static void findIBControls(List<IDataControl> arrIBs, ControlCollection controls)
        {
            foreach (Control ctr in controls)
            {
                if (ctr is IDataControl)
                {
                    arrIBs.Add(ctr as IDataControl);
                    //数据控件是最基本的控件，如果是复合控件，将不再搜索其内部的子数据控件
                }
                else if (ctr.HasControls())
                {
                    findIBControls(arrIBs, ctr.Controls);
                }
            }

        }


        /// <summary>
        /// 获取选择和删除查询的SQL语句
        /// </summary>
        /// <param name="Controls">要收集的控件集合</param>
        /// <returns> ArrayList 中的成员为 IBCommand 对象，包含具体的CRUD SQL</returns>
        public static List<IBCommand> GetSelectAndDeleteCommand(ControlCollection Controls)
        {
            List<IDataControl> IBControls = new List<IDataControl>();
            findIBControls(IBControls, Controls);
            return GetSelectAndDeleteCommand(IBControls);
        }

        /// <summary>
        /// 收集窗体中的智能控件，组合成能够直接用于数据库插入和更新 查询的 SQL语句
        /// 一个窗体中可以同时处理多个表的数据操作
        /// 如果控件的数据属性设置为只读，那么该控件的值不会更新到数据库；如果该控件的数据属性设置为主键，那么更新语句将附带该条件
        /// 邓太华 2008.1.15
        /// </summary>
        /// <returns>
        /// ArrayList 中的成员为 IBCommand 对象，包含具体的CRUD SQL
        ///</returns>
        public static List<IBCommand> GetIBFormData(ControlCollection Controls, CommonDB DB)
        {
            List<IDataControl> IBControls = new List<IDataControl>();
            findIBControls(IBControls, Controls);

            return MyDataForm.GetIBFormDataInner(IBControls, DB);
        }

        #endregion

        #region 数据填充以及持久化数据


        /// <summary>
        /// 自动更新窗体数据
        /// </summary>
        /// <param name="Controls">控件集合</param>
        /// <returns></returns>
        public List<IBCommand> AutoUpdateIBFormData(ControlCollection Controls)
        {
            List<IBCommand> ibCommandList = GetIBFormData(Controls, this.DAO);
            AutoUpdateIBFormDataInner(ibCommandList);
            return ibCommandList;
        }

        /// <summary>
        /// 自动更新含有GUID主键或字符型主键的窗体数据，注该控件必须设置PrimaryKey属性
        /// </summary>
        /// <param name="Controls">控件集合</param>
        /// <param name="guidControl">Gudi或字符型主键控件</param>
        /// <returns>更新是否成功</returns>
        public bool AutoUpdateIBFormData(ControlCollection Controls, IDataControl guidControl)
        {
            List<IBCommand> ibCommandList = GetIBFormData(Controls, this.DAO);
            return AutoUpdateIBFormDataInner(ibCommandList, guidControl);
        }

        /// <summary>
        /// 自动填充智能窗体控件的数据
        /// </summary>
        /// <param name="Controls">要填充的窗体控件集和</param>
        public void AutoSelectIBForm(ControlCollection Controls)
        {
            List<IDataControl> IBControls = new List<IDataControl>();
            findIBControls(IBControls, Controls);

            AutoSelectIBFormInner(IBControls);
        }

        /// <summary>
        /// 从数据集DataSet填充数据到数据控件上面，DataSet中的表名称必须和数据控件的LinkObject匹配（不区分大小写）
        /// </summary>
        /// <param name="Controls">要填充的窗体控件集和</param>
        /// <param name="dsSource">提供属于源的数据集</param>
        public void AutoSelectIBForm(ControlCollection Controls, DataSet dsSource)
        {
            List<IDataControl> IBControls = new List<IDataControl>();
            findIBControls(IBControls, Controls);

            AutoSelectIBFormInner(IBControls, dsSource);    
        }

        /// <summary>
        /// 从实体类填充数据到页面控件
        /// </summary>
        /// <param name="Controls"></param>
        /// <param name="entity"></param>
        public void AutoSelectIBForm(ControlCollection Controls, IEntity entity)
        {
            List<IDataControl> IBControls = new List<IDataControl>();
            findIBControls(IBControls, Controls);

            AutoSelectIBFormInner(IBControls, entity);

        }


        /// <summary>
        /// 自动删除智能窗体控件的持久化数据
        /// </summary>
        /// <param name="Controls">要处理的窗体控件集和</param>
        /// <returns>操作受影响的记录行数</returns>
        public int AutoDeleteIBForm(ControlCollection Controls)
        {
            List<IDataControl> IBControls = new List<IDataControl>();
            findIBControls(IBControls, Controls);

            return AutoDeleteIBFormInner(IBControls);
        }

        #endregion
    }
}
