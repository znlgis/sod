//-----------------------------------------------------------------
// SOD Framework (https://github.com/znlgis/sod)
// EntityCreateTool (Ver 1.0 Date:2023-5-1) Created SOD Entity File.
// Created Date: 2024-6-20 15:39:14
// Please do not modify this file.
//-----------------------------------------------------------------
using PWMIS.DataMap.Entity;
using SimpleDemo.Interface;

namespace SimpleDemo.Entity
{
    /// <summary>
    /// 1.设备信息 
    /// </summary>
    public class EquipmentEntity:EntityBase, IEquipment
    {
        public EquipmentEntity()
        {
            TableName = "Equipment";
            IdentityName = "ID"; 
            PrimaryKeys.Add("ID");
        }

        
        /// <summary>
        /// ID号 
        /// </summary>
        /// <remarks>
        /// 自增
        /// </remarks>

        public int ID
        {
            get { return getProperty<int>("ID"); }
            set { setProperty("ID", value); }
        }


        /// <summary>
        /// 设备编码  
        /// </summary>
        /// <remarks>
        /// 16位编码
        /// </remarks>

        public string EquipmentID
        {
            get { return getProperty<string>("EquipmentID"); }
            set { setProperty("EquipmentID", value,32); }
        }


        /// <summary>
        /// 设备名称  
        /// </summary>
        public string EquipmentName
        {
            get { return getProperty<string>("EquipmentName"); }
            set { setProperty("EquipmentName", value,32); }
        }


        /// <summary>
        /// 设备IP  
        /// </summary>
        public string IP
        {
            get { return getProperty<string>("IP"); }
            set { setProperty("IP", value,32); }
        }


        /// <summary>
        /// 设备端口 
        /// </summary>
        public int Port
        {
            get { return getProperty<int>("Port"); }
            set { setProperty("Port", value); }
        }


        /// <summary>
        /// 所属服务  
        /// </summary>
        public string ParentService
        {
            get { return getProperty<string>("ParentService"); }
            set { setProperty("ParentService", value,32); }
        }


        /// <summary>
        /// 设备类型 
        /// </summary>
        public int EquipmentType
        {
            get { return getProperty<int>("EquipmentType"); }
            set { setProperty("EquipmentType", value); }
        }


        /// <summary>
        /// 设备型号  
        /// </summary>
        public string EquipmentModel
        {
            get { return getProperty<string>("EquipmentModel"); }
            set { setProperty("EquipmentModel", value,32); }
        }


        /// <summary>
        /// 安装位置  
        /// </summary>
        public string InstallLocation
        {
            get { return getProperty<string>("InstallLocation"); }
            set { setProperty("InstallLocation", value,32); }
        }


        /// <summary>
        /// 厂商ID  
        /// </summary>
        public string ManufacturerID
        {
            get { return getProperty<string>("ManufacturerID"); }
            set { setProperty("ManufacturerID", value,32); }
        }


        /// <summary>
        /// 设备国标编码  
        /// </summary>
        /// <remarks>
        /// 20位编码
        /// </remarks>

        public string DevCodeGB
        {
            get { return getProperty<string>("DevCodeGB"); }
            set { setProperty("DevCodeGB", value,32); }
        }


        /// <summary>
        /// 经度 
        /// </summary>
        public double DLng
        {
            get { return getProperty<double>("dLng"); }
            set { setProperty("dLng", value); }
        }


        /// <summary>
        /// 维度 
        /// </summary>
        public double DLat
        {
            get { return getProperty<double>("dLat"); }
            set { setProperty("dLat", value); }
        }


        /// <summary>
        /// 高度 
        /// </summary>
        public int IAltitude
        {
            get { return getProperty<int>("iAltitude"); }
            set { setProperty("iAltitude", value); }
        }


        /// <summary>
        /// MAC地址  
        /// </summary>
        public string MacAddress
        {
            get { return getProperty<string>("MacAddress"); }
            set { setProperty("MacAddress", value,32); }
        }


        /// <summary>
        /// 维护单位  
        /// </summary>
        /// <remarks>
        /// 管辖范围，单位米
        /// </remarks>

        public string Maintenance
        {
            get { return getProperty<string>("Maintenance"); }
            set { setProperty("Maintenance", value,50); }
        }


        /// <summary>
        /// 固件版本  
        /// </summary>
        public string FirmwareVersion
        {
            get { return getProperty<string>("FirmwareVersion"); }
            set { setProperty("FirmwareVersion", value,32); }
        }


        /// <summary>
        /// 设备用户名  
        /// </summary>
        public string UserName
        {
            get { return getProperty<string>("UserName"); }
            set { setProperty("UserName", value,50); }
        }


        /// <summary>
        /// 设备密码  
        /// </summary>
        public string Password
        {
            get { return getProperty<string>("Password"); }
            set { setProperty("Password", value,50); }
        }


        /// <summary>
        /// 是否在线 
        /// </summary>
        /// <remarks>
        /// 1在线，0离线
        /// </remarks>

        public int IsOnline
        {
            get { return getProperty<int>("IsOnline"); }
            set { setProperty("IsOnline", value); }
        }


        /// <summary>
        /// 离线发生时间 
        /// </summary>
        public DateTime AtOffLine
        {
            get { return getProperty<DateTime>("AtOffLine"); }
            set { setProperty("AtOffLine", value); }
        }


        /// <summary>
        /// 备注  
        /// </summary>
        public string Memo
        {
            get { return getProperty<string>("Memo"); }
            set { setProperty("Memo", value,500); }
        }


       
    }
}
