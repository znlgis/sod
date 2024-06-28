namespace SimpleDemo.Interface
{
    public interface IEquipment : IIdentityProperty<int>
    {

        /// <summary>
        /// ID号 
        /// </summary>
        /// <remarks>
        /// 自增
        /// </remarks>

        public int ID
        {
            get;
            set;
        }


        /// <summary>
        /// 设备编码 
        /// </summary>
        /// <remarks>
        /// 16位编码
        /// </remarks>

        public string EquipmentID
        {
            get;
            set;
        }


        /// <summary>
        /// 设备名称 
        /// </summary>
        public string EquipmentName
        {
            get;
            set;
        }


        /// <summary>
        /// 设备IP 
        /// </summary>
        public string IP
        {
            get;
            set;
        }


        /// <summary>
        /// 设备端口 
        /// </summary>
        public int Port
        {
            get;
            set;
        }


        /// <summary>
        /// 所属服务 
        /// </summary>
        public string ParentService
        {
            get;
            set;
        }


        /// <summary>
        /// 设备类型 
        /// </summary>
        public int EquipmentType
        {
            get;
            set;
        }


        /// <summary>
        /// 设备型号 
        /// </summary>
        public string EquipmentModel
        {
            get;
            set;
        }


        /// <summary>
        /// 安装位置 
        /// </summary>
        public string InstallLocation
        {
            get;
            set;
        }


        /// <summary>
        /// 厂商ID 
        /// </summary>
        public string ManufacturerID
        {
            get;
            set;
        }


        /// <summary>
        /// 设备国标编码 
        /// </summary>
        /// <remarks>
        /// 20位编码
        /// </remarks>

        public string DevCodeGB
        {
            get;
            set;
        }


        /// <summary>
        /// 经度 
        /// </summary>
        public double DLng
        {
            get;
            set;
        }


        /// <summary>
        /// 维度 
        /// </summary>
        public double DLat
        {
            get;
            set;
        }


        /// <summary>
        /// 高度 
        /// </summary>
        public int IAltitude
        {
            get;
            set;
        }


        /// <summary>
        /// MAC地址 
        /// </summary>
        public string MacAddress
        {
            get;
            set;
        }


        /// <summary>
        /// 维护单位 
        /// </summary>
        /// <remarks>
        /// 管辖范围，单位米
        /// </remarks>

        public string Maintenance
        {
            get;
            set;
        }


        /// <summary>
        /// 固件版本 
        /// </summary>
        public string FirmwareVersion
        {
            get;
            set;
        }


        /// <summary>
        /// 设备用户名 
        /// </summary>
        public string UserName
        {
            get;
            set;
        }


        /// <summary>
        /// 设备密码 
        /// </summary>
        public string Password
        {
            get;
            set;
        }


        /// <summary>
        /// 是否在线 
        /// </summary>
        /// <remarks>
        /// 1在线，0离线
        /// </remarks>

        public int IsOnline
        {
            get;
            set;
        }


        /// <summary>
        /// 离线发生时间 
        /// </summary>
        public DateTime AtOffLine
        {
            get;
            set;
        }


        /// <summary>
        /// 备注 
        /// </summary>
        public string Memo
        {
            get;
            set;
        }

    }
}
