/*
 * ========================================================================
 * Copyright(c) 2008-2010北京高阳金信信息技术有限公司, All Rights Reserved.
 * ========================================================================
 *  依赖注入容器配置信息类
 * 
 * 作者：吴周钦     时间：2010-06-11
 * 版本：V1.0.1
 * 
 * 修改者：邓太华         时间：2010-06-18                
 * 修改说明：改进配置方式
 * ========================================================================
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Configuration;

namespace PWMIS.EnterpriseFramework.IOC
{
    public class IOCConfigEntity
    {
        //<GroupConfig>
        //    <Group ID="1" ParentID="0" Name="MB" >MB分组描述</Group>
        //    ...
        //</GroupConfig>
        public List<Group> GroupConfig { get; set; }
        public class Group
        {
            public String ID { get; set; }
            public String ParentID { get; set; }
            public String Name { get; set; }
            public String Value { get; set; }
        }

        public List<InterfaceRecord> SystemInterface { get; set; }
        public class InterfaceRecord
        {
            public String Name { get; set; }
            public String InterfaceFullName { get; set; }
            public String Assmebly { get; set; }
        }

        public List<IOC> GroupSet { get; set; }
        public class IOC
        {
            public String Name { get; set; }
            public List<IocProvider> IocProviderList { get; set; }

        }
    }

    public class IocProvider
    {
        public String Key { get; set; }
        public String InterfaceName { get; set; }
        public String FullClassName { get; set; }
        public String Assembly { get; set; }
        public IOCConfigEntity.IOC CurrentIOC { get; set; }
    }

    public class IOCConfig
    {
        private static IOCConfigEntity m_IOCConfigEntity;
        private static object lock_obj = new object();

        /// <summary>
        /// 获取IOC配置信息类
        /// </summary>
        public static IOCConfigEntity IOCConfigEntity
        {
            get
            {
                if (m_IOCConfigEntity == null)
                {
                    lock (lock_obj)
                    {
                        if (m_IOCConfigEntity == null)
                        {
                            IOCConfig config = new IOCConfig();
                            m_IOCConfigEntity = config.GetIOCConfigEntity(ConfigFileName);
                        }
                    }
                }
                return m_IOCConfigEntity;
            }
        }

        private static string ConfigFileName
        {
            get
            {
                string configFileName = System.Configuration.ConfigurationManager.AppSettings["IOCConfigFile"];//ConfigurationManager.AppSettings [""] ;
                if (string.IsNullOrEmpty(configFileName))
                    throw new Exception("未指定IOC配置文件的地址，请在应用程序配置节中配置 IOCConfigFile 项。");
                return configFileName;
            }
        }

        /// <summary>
        /// 增加一个Ioc提供程序的配置
        /// </summary>
        /// <param name="iocName">所在的Ioc名称，没有将添加</param>
        /// <param name="provider">提供程序信息</param>
        /// <returns>添加是否成功</returns>
        public static bool AddIocProvider(string iocName, IocProvider provider)
        {
            IOCConfig config = new IOCConfig();
            return config.AddIocProvider(ConfigFileName, iocName, provider);
        }

        public bool AddIocProvider(String configFileName, string iocName, IocProvider provider)
        {
            XElement xml = XElement.Load(configFileName);
            XElement groupSet = xml.Element("GroupSet");//读取GroupSet节
            IEnumerable<XElement> iOCElements = groupSet.Elements("IOC");//所有IOC节点
            XElement objIocItem = null;
            foreach (XElement iOCElement in iOCElements)//遍历所有Add节点，生成InterfaceRecord对象
            {
                if ((String)iOCElement.Attribute("Name") == iocName)
                {
                    foreach (IocProvider item in GetIocProviderList(iOCElement, null))
                    {
                        if (item.Key == provider.Key)
                        {
                            return false;//已经存在，不能添加
                        }
                    }
                    //不存在该Key，添加
                    objIocItem = iOCElement;
                    break;
                }
            }

            if (objIocItem == null)
            {
                //添加IOC节点
                XElement ioc = new XElement("IOC");
                ioc.SetAttributeValue("Name", iocName);
                objIocItem = ioc;
                groupSet.Add(objIocItem);
            }

            XElement newItem = new XElement("Add");
            newItem.SetAttributeValue("Key", provider.Key);
            newItem.SetAttributeValue("InterfaceName", provider.InterfaceName);
            newItem.SetAttributeValue("FullClassName", provider.FullClassName);
            newItem.SetAttributeValue("Assembly", provider.Assembly);

            objIocItem.Add(newItem);
            xml.Save(configFileName);
            return true;
        }

        public IOCConfigEntity GetIOCConfigEntity(String configFileName)
        {
            XElement xml = XElement.Load(configFileName);
            IOCConfigEntity iOCConfigEntity = new IOCConfigEntity();
            iOCConfigEntity.GroupConfig = new List<IOCConfigEntity.Group>();
            iOCConfigEntity.SystemInterface = new List<IOCConfigEntity.InterfaceRecord>();
            iOCConfigEntity.GroupSet = new List<IOCConfigEntity.IOC>();

            //<GroupConfig>
            //    <Group ID="1" ParentID="0" Name="MB" >MB分组描述</Group>
            //    ...
            //</GroupConfig>
            XElement groupConfig = xml.Element("GroupConfig");
            IEnumerable<XElement> groupElements = groupConfig.Elements("Group");
            foreach (XElement groupElement in groupElements)
            {
                IOCConfigEntity.Group group = new IOCConfigEntity.Group();

                group.ID = (String)groupElement.Attribute("ID");
                group.ParentID = (String)groupElement.Attribute("ParentID");
                group.Name = (String)groupElement.Attribute("Name");
                group.Value = (String)groupElement.Value;

                iOCConfigEntity.GroupConfig.Add(group);
            }

            //<SystemInterface>
            //  <Add Name="" Interface="" Assembly=""/>
            //  ...
            //</SystemInterface>
            XElement systemInterface = xml.Element("SystemInterface");
            IEnumerable<XElement> interfaceRecordsElements = systemInterface.Elements("Add");
            foreach (XElement interfaceRecordsElement in interfaceRecordsElements)
            {
                IOCConfigEntity.InterfaceRecord interfaceRecord = new IOCConfigEntity.InterfaceRecord();

                interfaceRecord.Name = (String)interfaceRecordsElement.Attribute("Name");
                interfaceRecord.InterfaceFullName = (String)interfaceRecordsElement.Attribute("Interface");
                interfaceRecord.Assmebly = (String)interfaceRecordsElement.Attribute("Assembly");

                iOCConfigEntity.SystemInterface.Add(interfaceRecord);
            }

            //<GroupSet>
            //     <IOC Name="MB">
            //     </IOC>
            //      ...
            //</GroupSet>
            XElement groupSet = xml.Element("GroupSet");//读取GroupSet节
            IEnumerable<XElement> iOCElements = groupSet.Elements("IOC");//所有IOC节点
            foreach (XElement iOCElement in iOCElements)//遍历所有Add节点，生成InterfaceRecord对象
            {
                IOCConfigEntity.IOC iOC = new IOCConfigEntity.IOC();

                iOC.Name = (String)iOCElement.Attribute("Name");
                iOC.IocProviderList = this.GetIocProviderList(iOCElement, iOC);

                iOCConfigEntity.GroupSet.Add(iOC);
            }


            return iOCConfigEntity;
        }

        //<IOC>
        //    <Add Key=""  InterfaceName=""  FullClassName="" Assembly="" />
        //    ...
        //</IOC>
        //读取所有Add节点，返回List<IocProvider>
        public List<IocProvider> GetIocProviderList(XElement iOC, IOCConfigEntity.IOC ioc)
        {
            List<IocProvider> list = new List<IocProvider>();

            IEnumerable<XElement> addElements = iOC.Elements("Add");

            foreach (XElement addElement in addElements)
            {
                IocProvider iocProvider = new IocProvider();

                iocProvider.Key = (String)addElement.Attribute("Key");
                iocProvider.InterfaceName = (String)addElement.Attribute("InterfaceName");
                iocProvider.FullClassName = (String)addElement.Attribute("FullClassName");
                iocProvider.Assembly = (String)addElement.Attribute("Assembly");
                iocProvider.CurrentIOC = ioc;
                list.Add(iocProvider);
            }

            return list;
        }

    }



}
