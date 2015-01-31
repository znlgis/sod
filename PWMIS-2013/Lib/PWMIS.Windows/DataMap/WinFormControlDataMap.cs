using System;
using System.Collections.Generic;
using System.Text;
using PWMIS.DataMap;
using System.Windows.Forms;
using System.Collections;
using PWMIS.Common;

namespace PWMIS.DataMap
{
    //class WinFormControlDataMap
    //{
    //}

    /// <summary>
    /// Web 窗体控件数据映射类，提供从实体对象填充数据到窗体数据控件和收集数据到实体对象的功能。
    /// </summary>
    public class WinFormControlDataMap : ControlDataMap
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public WinFormControlDataMap()
        {

        }

        /// <summary>
        /// 在控件集合中查找IDataControl,并放到一个IDataControl集合实例中
        /// </summary>
        /// <param name="controls">容器的控件集合</param>
        /// <param name="dcList">IDataControl集合实例</param>
        /// <returns>IDataControl集合实例</returns>
        public List<IDataControl> FindControl(System.Windows.Forms.Control.ControlCollection controls, List<IDataControl> dcList)
        {
            foreach (Control ctr in controls)
            {
                if (ctr is IDataControl)
                    dcList.Add(ctr as IDataControl);
                else
                    if (ctr.HasChildren)
                        dcList = FindControl(ctr.Controls, dcList);
            }
            return dcList;
        }

        /// <summary>
        /// 在控件中查找所有的IDataControl（深度查找）,并放到一个IDataControl集合实例中
        /// </summary>
        /// <param name="control">容器控件</param>
        /// <param name="dcList">IDataControl集合实例</param>
        /// <returns>IDataControl集合实例</returns>
        public static List<IDataControl> FindControls(Control control, List<IDataControl> dcList)
        {
            return new WinFormControlDataMap().FindControl(control.Controls, dcList);
        }

        /// <summary>
        /// 从实体类中填充数据
        /// </summary>
        /// <param name="objEntityClass">实体类</param>
        /// <param name="controls">控件集合</param>
        public static void FillDataFromEntityClass(object objEntityClass, ICollection controls)
        {
            if (objEntityClass == null)
                throw new Exception("实体类不能为空！");
            if (controls == null || controls.Count == 0)
                throw new Exception("控件集合不能为空！");
            new ControlDataMap().FillData(objEntityClass, controls, true);
        }

        /// <summary>
        /// 从控件集合收集数据到实体类中
        /// </summary>
        /// <param name="objEntityClass">实体类</param>
        /// <param name="controls">控件集合</param>
        public static void CollectDataToEntityClass(object objEntityClass, ICollection controls)
        {
            if (objEntityClass == null)
                throw new Exception("实体类不能为空！");
            if (controls == null || controls.Count == 0)
                throw new Exception("控件集合不能为空！");
            new ControlDataMap().CollectData(objEntityClass, controls, true);
        }

        /// <summary>
        /// 让容器中所有的BrainControl值设置为空(深度搜索) 
        /// </summary>
        /// <param name="conlObject">容器对象</param>
        public static void ClearData(System.Windows.Forms.Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                if (control is IDataControl)
                {
                    ((IDataControl)control).SetValue("");
                }
                else
                {
                    ClearData(control.Controls);
                }
            }
        }


    }
}
