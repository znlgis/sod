//ver 4.5 dbmstype auto get;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
//using System.Drawing.Design;
using System.Collections;
using PWMIS.DataProvider.Data;
using PWMIS.DataProvider.Adapter;
using System.Configuration;
using PWMIS.Common;

namespace PWMIS.Web.Controls
{
    /// <summary>
    /// 单击事件委托定义
    /// </summary>
    public delegate void ClickEventHandler(object sender, EventArgs e);

    /// <summary>
    /// 数据帮定委托定义
    /// </summary>
    public delegate void DataBoundHandler(object sender, EventArgs e);

    /// <summary>
    /// Web 分页工具条
    /// 邓太华 2007.1.10 Ver 1.0，2008.5.8 Ver 1.0.1.2，2008.7.24 Ver 1.0.1.3
    /// Ver 1.0.1 增加数据访问功能
    /// Ver 1.0.1.1 自动从配置文件设置全局默认配置参数，例如分页大小
    /// Ver 1.0.1.2 除了可以自动配置分页大小外，还可以设置特定的分页大小。
    /// Ver 1.0.1.3 支持GridView
    /// </summary>
    [System.Drawing.ToolboxBitmap(typeof(ControlIcon), "DataPageToolBar.bmp")]
    [DefaultProperty("AllCount"),
    DefaultEvent("PageChangeIndex"),
        ToolboxData("<{0}:ProPageToolBar runat=server></{0}:ProPageToolBar>")]
    public class ProPageToolBar : System.Web.UI.WebControls.WebControl, INamingContainer
    {
        #region 内部控件定义
        protected Label lblAllCount = new Label();
        protected Label lblCPA = new Label();
        protected LinkButton lnkFirstPage = new LinkButton();
        protected LinkButton lnkPrePage = new LinkButton();
        protected LinkButton lnkNextPage = new LinkButton();
        protected LinkButton lnkLastPage = new LinkButton();
        protected TextBox txtNavePage = new TextBox();
        protected DropDownList dlPageSize = new DropDownList();
        protected LinkButton lnkGo = new LinkButton();
        #endregion
        /// <summary>
        /// 未初始化的数值
        /// </summary>
        const int UNKNOW_NUM = -999;

        #region 局部变量申明
        private int PageIndex = UNKNOW_NUM;//
        private int _AllCount;
        private int _PageSize;
        private int _CurrentPage;
        private bool hasSetBgColor;

        private bool ChangePageProperty = false;
        private bool _AutoIDB = false;
        private bool _AutoConfig = false;
        private bool _AutoBindData = false;
        private bool _UserChangePageSize = true;
        private bool _ShowEmptyData = true;

        private string text;
        private string _BindControl;
        private string _SQL;
        private string _Where;
        private string _ConnectionString = string.Empty;
        private string _ErrorMessage = string.Empty;
        private DataProvider.Data.CommonDB _DAO;
        private DBMSType _DBMSType = DBMSType.SqlServer;
        private System.Data.IDataParameter[] _Parameters;

        #endregion

        private DataProvider.Data.CommonDB DAO
        {
            get
            {
                if (_DAO == null)
                {
                    CheckAutoConfig();
                    CheckAutoIDB();
                }
                if (_DAO == null)
                    throw new Exception("未实例化数据访问组件,请确认已经进行了正确的配置！");

                //edit dbmstype:
                this.DBMSType = _DAO.CurrentDBMSType;
                return _DAO;
            }
            set
            {
                _DAO = value;
            }

        }

        #region 其它属性定义
        [Bindable(true),
        Category("Appearance"),
        Description("分页说明"),
        DefaultValue("")]
        public string Text
        {
            get
            {
                return text;

            }

            set
            {
                text = value;
            }
        }

        private string FontSize
        {
            get
            {
                return this.Font.Size.Unit.ToString();//fontsize;
            }
        }

        /// <summary>
        /// 分页工具条的样式,0-默认，1-不显示记录条数；2-不显示页跳转；3-既不显示记录条数，也不显示页跳转
        /// </summary>
        [Bindable(true),
        Category("分页属性"),
        Description("分页工具条的分页样式，0-默认，1-不显示记录条数；2-不显示页跳转；3-既不显示记录条数，也不显示页跳转")
        ]
        public int PageToolBarStyle
        {
            get
            {
                if (ViewState["_PageToolBarStyle"] != null)
                    return (int)ViewState["_PageToolBarStyle"];
                else
                    return 0;
            }
            set
            {
                ViewState["_PageToolBarStyle"] = value;
            }
        }


        #endregion

        #region 内部控件样式名定义
        public string css_linkStyle = "";
        public string css_btnStyle = "";
        public string css_txtStyle = "";
        #endregion

        #region 分页属性定义

        /// <summary>
        /// 当前所在页码，默认值1
        /// </summary>
        [Bindable(true),
        Category("分页属性"),
        Description("当前所在页")
        ]
        public int CurrentPage
        {
            get
            {
                if (ViewState[this.ID + "_CurrentPage"] != null)
                    _CurrentPage = (int)ViewState[this.ID + "_CurrentPage"];
                return _CurrentPage <= 0 ? 1 : _CurrentPage;
            }
            set
            {
                if (value < 0) value = 1;
                _CurrentPage = value;
                ViewState[this.ID + "_CurrentPage"] = value;
                PageIndex = value;
                ChangePageProperty = true;
                this.txtNavePage.Text = value.ToString();
            }
        }
        /// <summary>
        /// 记录总数，默认值0
        /// </summary>
        [Bindable(true),
        Category("分页属性"),
        Description("记录总数"),
        DefaultValue(0)]
        public int AllCount
        {
            get
            {
                if (ViewState[this.ID + "_AllCount"] != null)
                    _AllCount = (int)ViewState[this.ID + "_AllCount"];
                return _AllCount;
            }
            set
            {
                if (value < 0 && value != -1) value = 0;
                _AllCount = value;
                ViewState[this.ID + "_AllCount"] = value;
                ChangePageProperty = true;
                this.lblAllCount.Text = value.ToString();
            }
        }
        /// <summary>
        /// 页面大小，默认值10，输入0表示从系统自动获取配置值
        /// </summary>
        [Bindable(true),
        Category("分页属性"),
        Description("每页面分页记录大小，默认值10,输入0表示从系统自动获取配置值"),
        DefaultValue(10)]
        public int PageSize
        {
            get
            {
                if (ViewState[this.ID + "_PageSize"] != null)
                {
                    _PageSize = (int)ViewState[this.ID + "_PageSize"];
                    return _PageSize <= 0 ? 10 : _PageSize;
                }

                //设置默认分页大小
                if (this.AutoConfig && _PageSize == 0)
                {
                    string defaultPageSize = ConfigurationSettings.AppSettings["PageSize"];
                    if (defaultPageSize != null && defaultPageSize != "")
                    {
                        _PageSize = int.Parse(defaultPageSize);
                        return _PageSize;
                    }
                    else
                    {
                        _PageSize = 10;
                    }
                }
                else
                {
                    _PageSize = _PageSize <= 0 ? 10 : _PageSize;
                }
                return _PageSize;
            }
            set
            {
                if (this.AutoConfig && value == 0)
                {
                    string defaultPageSize = ConfigurationSettings.AppSettings["PageSize"];
                    if (defaultPageSize != null && defaultPageSize != "")
                    {
                        _PageSize = int.Parse(defaultPageSize);
                    }
                    else
                    {
                        _PageSize = 10;
                    }
                    value = _PageSize;
                }
                if (value < 0) value = 10;
                _PageSize = value;
                ViewState[this.ID + "_PageSize"] = value;
                ChangePageProperty = true;
            }
        }
        /// <summary>
        /// 页面总数，只读
        /// </summary>
        [Bindable(true),
        Category("分页属性"),
        Description("页面总数，只读"),
        DefaultValue(1)]
        public int PageCount
        {
            get
            {
                int AllPage = AllCount / PageSize;
                if ((AllPage * PageSize) < AllCount) AllPage++;
                if (AllPage <= 0) AllPage = 1;
                return AllPage;
            }

        }

        [
        Category("分页属性"),
        Description("是否允许用户在浏览页面的时候改变分页大小"),
        DefaultValue(true)]
        public bool UserChangePageSize
        {
            get
            {
                if (ViewState[this.ID + "_UserChangePageSize"] != null)
                    _UserChangePageSize = (bool)ViewState[this.ID + "_UserChangePageSize"];
                return _UserChangePageSize;
            }
            set
            {
                _UserChangePageSize = value;
                ViewState[this.ID + "_UserChangePageSize"] = value;
            }
        }
        #endregion

        #region 分页事件


        /// <summary>
        /// 页面改变事件
        /// </summary>
        [Category("分页事件"),
        Description("页面改变事件")]
        public event ClickEventHandler PageChangeIndex;

        /// <summary>
        /// 目标控件完成数据绑定之前的事件
        /// </summary>
        [Category("Data"),
        Description("目标控件完成数据绑定之前的事件")]
        public event DataBoundHandler DataControlDataBinding;
        /// <summary>
        /// 目标控件完成数据绑定完成事件
        /// </summary>
        [Category("Data"),
        Description("目标控件完成数据绑定完成事件")]
        public event DataBoundHandler DataControlDataBound;

        /// <summary>
        /// 改变页码索引
        /// </summary>
        /// <param name="e">目标</param>
        protected void changeIndex(EventArgs e)
        {
            if (PageChangeIndex != null)
            {
                PageChangeIndex(this, e);
            }
            //if(this.Site !=null && ! this.Site.DesignMode  )//在运行时
            //{
            if (this.AutoBindData)
            {
                if (this.Page.IsPostBack)
                {
                    this.BindResultData();
                }
            }
            //}
        }

        #endregion

        #region 公开的方法

        /// <summary>
        /// 获取一个实例查询参数
        /// </summary>
        /// <returns></returns>
        public System.Data.IDataParameter GetParameter()
        {
            return DAO.GetParameter();
        }

        /// <summary>
        /// 获取一个实例查询参数
        /// </summary>
        /// <param name="paraName"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public System.Data.IDataParameter GetParameter(string paraName, object Value)
        {
            return DAO.GetParameter(paraName, Value);
        }

        /// <summary>
        /// 根据提供的分页查询和控件提供的数据访问信息，从数据源获取数据。
        /// </summary>
        /// <returns></returns>
        public object GetDataSource()
        {
            if (this.AllCount == 0)
                this.AllCount = -1;//特殊处理，获取记录数量为0时的架构
            DAO.ConnectionString = this.ConnectionString;
            object result = DAO.ExecuteDataSet(this.SQLbyPaging, System.Data.CommandType.Text, this.Parameters);
            if (this.AllCount == -1)
                this.AllCount = 0;
            if (DAO.ErrorMessage != "")
                throw new Exception(DAO.ErrorMessage + ";SQL=" + this.SQLbyPaging);
            return result;
            //DAO.
        }

        /// <summary>
        /// 获取结果集记录数量
        /// </summary>
        /// <returns></returns>
        public int GetResultDataCount()
        {
            //创建一个新统计参数，避免参数在集合中已经存在的问题。
            System.Data.IDataParameter[] countParas = null;
            if (this.Parameters != null && this.Parameters.Length > 0)
            {
                countParas = (System.Data.IDataParameter[])this.Parameters.Clone();
                for (int i = 0; i < countParas.Length; i++)
                {
                    countParas[i] = DAO.GetParameter(countParas[i].ParameterName, countParas[i].Value);
                }
            }

            DAO.ConnectionString = this.ConnectionString;
            object count = DAO.ExecuteScalar(this.SQLbyCount, System.Data.CommandType.Text, countParas);
            if (count != null)
                return Convert.ToInt32(count);//(int)count 在Oracle 将会失败。
            else
                throw new Exception(DAO.ErrorMessage);
        }

        /// <summary>
        /// 将数据源的分页数据绑定到绑定目标控件上，支持GridView
        /// </summary>
        public void BindResultData()
        {
            string BindToControlID = this.BindToControl;
            if (BindToControlID != null && BindToControlID != "")
            {
                if (DataControlDataBinding != null)
                {
                    DataControlDataBinding(this, new EventArgs());
                }
                //下面的方式如果本控件在用户控件中，将查找不到。
                //Control ctr= this.Page.FindControl (BindToControlID); 
                Control ctr = FindMyControl(this, BindToControlID);

                if (ctr is GridView)
                {
                    ((GridView)ctr).DataSource = this.GetDataSource();
                    ctr.DataBind();
                }
                else if (ctr is DataGrid)
                {
                    ((DataGrid)ctr).DataSource = this.GetDataSource();
                    ctr.DataBind();
                }
                else if (ctr is DataList)
                {
                    ((DataList)ctr).DataSource = this.GetDataSource();
                    ctr.DataBind();
                }
                else if (ctr is Repeater)
                {
                    ((Repeater)ctr).DataSource = this.GetDataSource();
                    ctr.DataBind();
                }
                else
                {
                    throw new Exception("控件" + BindToControlID + "不支持数据绑定，请确保绑定目标控件是DataGrid,DataList,Repeater类型！");
                }
                if (DataControlDataBound != null)
                {
                    DataControlDataBound(this, new EventArgs());
                }
            }
        }
        //      在用户控件中，仍然找不到该目标控件
        private Control FindMyControl(Control sourceControl, string objControlID)
        {
            //宽度优先策略
            foreach (Control ctr in sourceControl.Parent.Controls)
            {
                if (ctr.ID == objControlID)
                    return ctr;
            }
            foreach (Control ctr in sourceControl.Parent.Controls)
            {
                Control objCtr = FindMyControl(ctr, objControlID);
                if (objCtr != null)
                    return objCtr;
            }
            return null;
        }

        /// <summary>
        /// 重新绑定数据和计算本次查询的记录数量，并设定当前页码在第一页
        /// </summary>
        public void ReBindResultData()
        {
            this.CurrentPage = 1;
            this.AllCount = this.GetResultDataCount();
            this.BindResultData();
        }

        #endregion

        #region 数据分页属性
        /// <summary>
        /// 需要绑定分页的控件，如DataGrid,DataList,Repeater 。
        /// </summary>
        [DefaultValue(null),
        Category("Data"),
        Description("需要绑定分页的控件，如DataGrid,DataList,Repeater 。"),
        TypeConverter(typeof(ControlListIDConverter))]
        public string BindToControl
        {
            get
            {
                return _BindControl;
            }
            set
            {
                _BindControl = value;
            }
        }

        /// <summary>
        /// 用于分页查询的原始 SQL 语句
        /// </summary>
        [DefaultValue(null),
        Category("Data"),
        Description("用于分页查询的原始 SQL 语句")]
        public string SQL
        {
            get
            {
                if (ViewState[this.ID + "_SQL"] != null)
                    _SQL = (string)ViewState[this.ID + "_SQL"];
                return _SQL;
            }
            set
            {
                _SQL = value;
                ViewState[this.ID + "_SQL"] = value;
            }
        }

        /// <summary>
        /// 分页查询参数,在运行时请调用 GetParameter方法 添加成员。
        /// </summary>
        [DefaultValue(null),
        Category("Data"),
        Description("分页查询参数,在运行时请调用 GetParameter方法 添加成员。")]
        public System.Data.IDataParameter[] Parameters
        {
            get
            {
                if (_Parameters != null)
                    return _Parameters;
                else
                {

                    if (System.Web.HttpContext.Current.Session[this.ID + "_Parameters"] != null)
                    {
                        System.Data.IDataParameter[] p0 = (System.Data.IDataParameter[])System.Web.HttpContext.Current.Session[this.ID + "_Parameters"];
                        System.Data.IDataParameter[] p1 = new System.Data.IDataParameter[p0.Length];
                        for (int i = 0; i < p0.Length; i++)
                        {
                            p1[i] = this.GetParameter(p0[i].ParameterName, p0[i].Value);//创建新参数
                        }

                        return p1;
                    }
                }
                return null;

            }
            set
            {
                _Parameters = value;
                System.Web.HttpContext.Current.Session[this.ID + "_Parameters"] = _Parameters;
            }
        }

        /// <summary>
        /// 生成的用于分页查询的 SQL 语句
        /// </summary>
        [DefaultValue(null),
        Category("Data"),
        Description("生成的用于分页查询的 SQL 语句")]
        public string SQLbyPaging
        {
            get
            {
                if (this.SQL == null) return "";
                SQLPage.DbmsType = this.DBMSType;
                return SQLPage.MakeSQLStringByPage(this.SQL, this.Where, this.PageSize, this.CurrentPage, this.AllCount);
            }
        }

        /// <summary>
        /// 生成的用于统计分页查询总记录数的 SQL 语句
        /// </summary>
        [DefaultValue(null),
        Category("Data"),
        Description("生成的用于统计分页查询总记录数的 SQL 语句")]
        public string SQLbyCount
        {
            get
            {
                if (this.SQL == null) return "";
                SQLPage.DbmsType = this.DBMSType;
                return SQLPage.MakeSQLStringByPage(this.SQL, this.Where, this.PageSize, this.CurrentPage, 0);
            }
        }

        /// <summary>
        /// 指定用于分页查询所支持的数据库管理系统类型名称
        /// </summary>
        [DefaultValue(DBMSType.SqlServer),
        Category("Data"),
        Description("指定用于分页查询所支持的数据库管理系统类型名称")]
        [TypeConverter(typeof(EnumConverter))]
        public DBMSType DBMSType
        {
            get
            {
                return _DBMSType;
            }
            set
            {
                _DBMSType = value;

            }
        }

        /// <summary>
        /// 是否自动化数据库实例对象，如果是，将采用DataProvider 数据访问块，能够方便的获取参数，生成结果数据集。如果未能正确配置，将不能设置为True 。
        /// </summary>
        [DefaultValue(false),
        Category("Data"),
        Description("是否自动化数据库实例对象，如果是，将采用DataProvider 数据访问块，能够方便的获取参数，生成结果数据集。如果未能正确配置，将不能设置为True 。")]
        public bool AutoIDB
        {
            get
            {
                return _AutoIDB;
            }
            set
            {
                _AutoIDB = value;

            }
        }

        private void CheckAutoIDB()
        {
            if (System.Web.HttpContext.Current == null)//   this.Site !=null && this.Site.DesignMode
            {
                return;	//在设计时退出下面逻辑判断
            }
            if (_AutoIDB)//如果自动实例化数据库访问对象
            {
                try
                {
                    _ErrorMessage = "";
                    if (DAO == null)
                        DAO = MyDB.GetDBHelper(this.DBMSType, this.ConnectionString);
                    _AutoIDB = true;
                }
                catch (Exception e)
                {
                    _AutoIDB = false;
                    _ErrorMessage = e.Message;
                }
            }
        }

        /// <summary>
        /// 是否自动从应用程序配置文件获取数据访问配置信息，只有已经正确地配置了信息才可以返回True 。
        /// </summary>
        [DefaultValue(false),
        Category("Data"),
        Description("是否自动从应用程序配置文件获取数据访问和其它配置信息，只有已经正确地配置了信息才可以返回True 。")]
        public bool AutoConfig
        {
            get
            {
                return _AutoConfig;
            }
            set
            {
                _AutoConfig = value;

            }
        }

        private void CheckAutoConfig()
        {
            if (System.Web.HttpContext.Current == null)//   this.Site !=null && this.Site.DesignMode
            {
                return;	//在设计时退出下面逻辑判断
            }
            if (_AutoConfig)
            {
                _ErrorMessage = "";
                string strConn = "";
                //处理数据库管理系统类型
                string strDBMSType = ConfigurationSettings.AppSettings["EngineType"];//统一从 DBMSType 获取

                if (strDBMSType != null && strDBMSType != "")
                {
                    if (System.Enum.IsDefined(typeof(DBMSType), strDBMSType))
                        this.DBMSType = (DBMSType)System.Enum.Parse(typeof(DBMSType), strDBMSType);
                    else
                        _AutoConfig = false;

                    //处理连接字符串
                    string ConnStrKey = string.Empty;
                    switch (this.DBMSType)
                    {
                        case DBMSType.Access:
                            ConnStrKey = "OleDbConnectionString";
                            break;
                        case DBMSType.SqlServer:
                            ConnStrKey = "SqlServerConnectionString";
                            break;
                        case DBMSType.Oracle:
                            ConnStrKey = "OracleConnectionString";
                            break;
                        case DBMSType.MySql:
                            ConnStrKey = "OdbcConnectionString";
                            break;
                        case DBMSType.UNKNOWN:
                            ConnStrKey = "OdbcConnectionString";
                            break;
                    }
                    strConn = ConfigurationSettings.AppSettings[ConnStrKey];
                }
                else
                {
                    //未指定，从最后一个connectionStrings 配置节读取
                    if (ConfigurationManager.ConnectionStrings.Count > 0)
                    {
                        DAO = MyDB.GetDBHelper();
                        strConn = DAO.ConnectionString;
                    }
                }

                if (strConn == null || strConn == "")
                    _AutoConfig = false;
                else
                    this.ConnectionString = strConn.Replace("~", Context.Request.PhysicalApplicationPath);//替换相对路径

                if (!_AutoConfig)//在设计时不生成错误信息，因为VS2003设计时无法读取配置信息
                {
                    _ErrorMessage = "未能正确配置数据访问信息，请检查是否已经在应用程序配置文件中进行了正确的配置";
                    _AutoConfig = false;
                }
                else
                    AutoIDB = _AutoConfig;//如果正确配置，那么自动化数据库访问对象实例


            }
        }

        /// <summary>
        /// 是否在运行时自动绑定分页数据，依赖于 AutoIDB 属性等于True
        /// </summary>
        [DefaultValue(false),
        Category("Data"),
        Description("是否在运行时自动绑定分页数据，依赖于 AutoIDB 属性等于True")]
        public bool AutoBindData
        {
            get
            {
                return _AutoBindData;
            }
            set
            {
                _AutoBindData = value;
            }
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        [DefaultValue(""),
        Category("Data"),
        Description("错误信息")]
        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            //set{ _ErrorMessage=value;}
        }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        [DefaultValue(""),
        Category("Data"),
        Description("数据库连接字符串")]
        public string ConnectionString
        {
            get
            {
                return _ConnectionString;
            }
            set
            {
                _ConnectionString = value;
            }
        }

        /// <summary>
        /// 指定分页查询的附加条件，注意简单查询与复杂查询的条件限定方式。
        /// </summary>
        [DefaultValue(""),
        Category("Data"),
        Description("指定分页查询的附加条件，注意简单查询与复杂查询的条件限定方式。")]
        public string Where
        {
            get
            {
                if (ViewState[this.ID + "_Where"] != null)
                    _Where = (string)ViewState[this.ID + "_Where"];
                return _Where;
            }
            set
            {
                _Where = value;
                ViewState[this.ID + "_Where"] = value;
            }
        }

        /// <summary>
        /// 如果记录数量为0，根据设置是否显示数据架构，如果需要显示架构，那么将执行数据绑定方法。
        /// </summary>
        [DefaultValue(true),
        Category("Data"),
        Description("如果记录数量为0，根据设置是否显示数据架构，如果需要显示架构，那么将执行数据绑定方法。")]
        public bool ShowEmptyData
        {
            get
            {
                if (ViewState[this.ID + "_ShowEmptyData"] != null)
                    _ShowEmptyData = (bool)ViewState[this.ID + "_ShowEmptyData"];
                return _ShowEmptyData;
            }
            set
            {
                _ShowEmptyData = value;
                ViewState[this.ID + "_ShowEmptyData"] = value;
            }
        }

        #endregion

        #region 基类重载的方法
        /// <summary> 
        /// 将此控件呈现给指定的输出参数。
        /// </summary>
        /// <param name="output"> 要写出到的 HTML 编写器 </param>
        protected override void Render(HtmlTextWriter output)
        {
            if (ChangePageProperty)
            {
                ChangePageProperty = false;
                //this.SetPageInfo ();
            }
            this.SetPageInfo();
            this.ForeColor = this.ForeColor;
            this.EnsureChildControls();

            //处理表头样式
            output.Write("<table width='" + this.Width.ToString() + "' height='" + this.Height
                + "' bgcolor='" + ConvertColorFormat(this.BackColor)
                + "' bordercolor='" + ConvertColorFormat(this.BorderColor)
                + "' border='" + this.BorderWidth.ToString()
                + "' style='border-style:" + this.BorderStyle.ToString()
                + ";border-collapse:collapse' cellpadding='0'><tr><td><table width='100%' style='color:" + ConvertColorFormat(this.ForeColor)
                + " ;font-size:" + this.FontSize + "; font-family:" + this.Font.Name + "' class='"
                + this.CssClass + "'><tr><td valign='baseline'>"
                + this.Text + "</td><td valign='baseline'>");
            //添加控件
            //1-不显示记录条数；2-不显示页跳转；3-既不显示记录条数，也不显示页跳转

            int type = this.PageToolBarStyle;

            //1或3，不显示记录条数
            if (type != 1 && type != 3)
            {
                int currSize = PageSize;
                if (this.PageCount == this.CurrentPage)
                    currSize = this.AllCount - this.PageSize * (this.CurrentPage - 1);

                output.Write(currSize.ToString() + "/");//AllCount-PageSize*(PageNumber-1)
                lblAllCount.RenderControl(output);
                output.Write("条，");
            }

            //
            if (UserChangePageSize)
            {
                output.Write("\n");
                dlPageSize.RenderControl(output);
                output.Write("条/页，");
            }
            lblCPA.RenderControl(output);
            output.Write("页</td><td>");
            lnkFirstPage.RenderControl(output);
            output.Write("\n");
            lnkPrePage.RenderControl(output);
            output.Write("\n");
            lnkNextPage.RenderControl(output);
            output.Write("\n");
            lnkLastPage.RenderControl(output);

            //2或者3 不显示页跳转
            if (type != 2 && type != 3)
            {
                output.Write("\n到");
                txtNavePage.RenderControl(output);
                output.Write("页\n");
                lnkGo.RenderControl(output);
            }
            output.Write("</td></tr></table></td></tr></table>");

        }

        /// <summary>
        /// 重写 OnLoad 事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            //			//在这里处理有关数据绑定属性
            //			CheckAutoConfig ();
            //			CheckAutoIDB();


            if (this.Site != null && this.Site.DesignMode)//在设计时
            {
                return;
            }
            if (this.AutoBindData)
            {
                if (!this.Page.IsPostBack)
                {
                    this.AllCount = this.GetResultDataCount();
                    //如果记录数量为0，根据设置是否显示数据架构，如果需要显示架构，那么将执行数据绑定方法。
                    if (!this.ShowEmptyData && this.AllCount == 0)
                        return;

                    this.BindResultData();

                    //this.SetPageInfo ();
                }
            }
        }


        /// <summary>
        /// 重写 CSS 样式名
        /// </summary>
        public override string CssClass
        {
            get
            {
                return base.CssClass;
            }
            set
            {
                base.CssClass = value;
                foreach (System.Web.UI.WebControls.WebControl ctr in this.Controls)
                {
                    ctr.CssClass = value;
                }
            }
        }

        /// <summary>
        /// 重写前景色
        /// </summary>
        public override System.Drawing.Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
                foreach (System.Web.UI.WebControls.WebControl ctr in this.Controls)
                {
                    ctr.ForeColor = value;
                }
            }
        }

        public override System.Drawing.Color BackColor
        {
            get
            {
                if (!hasSetBgColor)
                    return System.Drawing.Color.White;
                return base.BackColor;
            }
            set
            {
                hasSetBgColor = true;
                base.BackColor = value;
            }
        }

        /// <summary>
        /// 创建子控件
        /// </summary>
        protected override void CreateChildControls()
        {
            //base.CreateChildControls ();
            Controls.Clear();
            Controls.Add(lblAllCount);
            Controls.Add(lblCPA);
            Controls.Add(lnkFirstPage);
            Controls.Add(lnkPrePage);
            Controls.Add(lnkNextPage);
            Controls.Add(lnkLastPage);
            Controls.Add(txtNavePage);
            Controls.Add(lnkGo);

            lblAllCount.Text = this.AllCount.ToString();
            lnkFirstPage.Text = "首页";
            lnkPrePage.Text = "上一页";
            lnkNextPage.Text = "下一页";
            lnkLastPage.Text = "尾页";
            txtNavePage.Width = 30;
            lnkGo.Text = "Go";


            if (this.UserChangePageSize)
            {
                Controls.Add(dlPageSize);
                dlPageSize.AutoPostBack = true;
                dlPageSize.Items.Clear();
                for (int i = 5; i <= 50; i += 5)
                {
                    dlPageSize.Items.Add(i.ToString());
                }
                dlPageSize.SelectedValue = this.PageSize.ToString();
                this.dlPageSize.SelectedIndexChanged += new EventHandler(dlPageSize_SelectedIndexChanged);
            }

            this.lnkFirstPage.Click += new System.EventHandler(this.lnkFirstPage_Click);
            this.lnkPrePage.Click += new System.EventHandler(this.lnkPrePage_Click);
            this.lnkNextPage.Click += new System.EventHandler(this.lnkNextPage_Click);
            this.lnkLastPage.Click += new System.EventHandler(this.lnkLastPage_Click);
            this.lnkGo.Click += new System.EventHandler(this.lnkGo_Click);


        }
        #endregion

        #region 内部事件处理

        /// <summary>
        /// RGB颜色值到Html颜色值转换
        /// </summary>
        /// <param name="RGBColor"></param>
        /// <returns></returns>
        private string ConvertColorFormat(System.Drawing.Color RGBColor)
        {
            return "RGB(" + RGBColor.R.ToString() + "," + RGBColor.G.ToString() + "," + RGBColor.G.ToString() + ")";
        }

        /// <summary>
        /// 设置分页状态信息
        /// </summary>
        private void SetPageInfo()
        {
            if (PageIndex == UNKNOW_NUM)
                PageIndex = this.CurrentPage;

            if (PageIndex > PageCount) PageIndex = PageCount;
            else if (PageIndex == -1) PageIndex = PageCount;
            else if (PageIndex < 1) PageIndex = 1;

            if (this.AllCount == 0)
                this.lblCPA.Text = "0/0";
            else
                this.lblCPA.Text = PageIndex.ToString() + "/" + PageCount.ToString();
            this.txtNavePage.Text = PageIndex.ToString();
            this.CurrentPage = PageIndex;

            if (this.PageCount == 1) this.lnkGo.Enabled = false;

            if (PageIndex == 0)
            {
                this.lnkFirstPage.Enabled = false;
                this.lnkPrePage.Enabled = false;
                this.lnkLastPage.Enabled = false;
                this.lnkNextPage.Enabled = false;
                //this.btnNavePage .Enabled =false;
                return;

            }

            if (PageIndex == 1)
            {
                this.lnkFirstPage.Enabled = false;
                this.lnkPrePage.Enabled = false;

            }
            else
            {
                this.lnkFirstPage.Enabled = true;
                this.lnkPrePage.Enabled = true;
            }
            if (PageIndex < PageCount)
            {
                this.lnkLastPage.Enabled = true;
                this.lnkNextPage.Enabled = true;

            }
            else
            {
                this.lnkLastPage.Enabled = false;
                this.lnkNextPage.Enabled = false;
            }
        }

        /// <summary>
        /// 下一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnkNextPage_Click(object sender, System.EventArgs e)
        {
            PageIndex = this.CurrentPage;
            PageIndex++;
            SetPageInfo();
            this.changeIndex(e);

        }

        /// <summary>
        /// 上一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnkPrePage_Click(object sender, System.EventArgs e)
        {
            PageIndex = this.CurrentPage;
            PageIndex--;
            SetPageInfo();
            this.changeIndex(e);
        }

        /// <summary>
        /// 尾页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnkLastPage_Click(object sender, System.EventArgs e)
        {
            PageIndex = -1;
            SetPageInfo();
            this.changeIndex(e);
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnkFirstPage_Click(object sender, System.EventArgs e)
        {
            PageIndex = 1;
            SetPageInfo();
            this.changeIndex(e);
        }


        /// <summary>
        /// 初始化链接文字样式
        /// </summary>
        private void InitStyle()
        {
            if (css_linkStyle != "")
                this.lnkFirstPage.Attributes.Add("class", css_linkStyle);
            if (css_linkStyle != "")
                this.lnkNextPage.Attributes.Add("class", css_linkStyle);
            if (css_linkStyle != "")
                this.lnkLastPage.Attributes.Add("class", css_linkStyle);
            if (css_linkStyle != "")
                this.lnkPrePage.Attributes.Add("class", css_linkStyle);
            if (css_linkStyle != "")
                this.lnkGo.Attributes.Add("class", css_linkStyle);
            if (css_txtStyle != "")
                this.txtNavePage.Attributes.Add("class", css_txtStyle);
        }

        /// <summary>
        /// 转到某页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnkGo_Click(object sender, System.EventArgs e)
        {
            try
            {
                PageIndex = Int32.Parse(this.txtNavePage.Text.Trim());
                SetPageInfo();
                this.changeIndex(e);

            }
            catch
            {
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "pageErr", "<script language='javascript'>alert('请填写数字页码！');</script>");
            }
        }

        /// <summary>
        /// 改变页大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.PageSize = int.Parse(dlPageSize.SelectedValue);
            this.CurrentPage = 1;
            this.PageIndex = this.CurrentPage;
            SetPageInfo();
            this.BindResultData();
        }
        #endregion

        #region 获取绑定控件列表 类
        /// <summary>
        /// 获取绑定控件列表 类
        /// </summary>
        public class ControlListIDConverter : StringConverter
        {
            /// <summary>
            /// false
            /// </summary>
            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
            {
                return false;
            }
            /// <summary>
            /// true
            /// </summary>
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                return true;
            }
            /// <summary>
            /// 获取所有运行时的目标控件的ID
            /// </summary>
            public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                if (context == null)
                    return null;
                ArrayList al = new ArrayList();
                foreach (IComponent ic in context.Container.Components)
                {
                    if (ic is ProPageToolBar)
                        continue;
                    if (ic is DataGrid || ic is Repeater || ic is DataList || ic is GridView)//|| ic is System.Web.UI.WebControls.ListView 可能需要3.5框架
                    {
                        al.Add(((Control)ic).ID);
                    }
                }
                return new TypeConverter.StandardValuesCollection(al);
            }
        }

        public DBMSType DBMSType1
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public SQLPage SQLPage
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public DataBoundHandler DataBoundHandler
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public ClickEventHandler ClickEventHandler
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        //		public class DBMSConverter:TypeConverter
        //		{
        //			public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        //			{
        //				if(sourceType==typeof(string ))
        //				{
        //					return true;
        //				}
        //				return base.CanConvertFrom (context, sourceType);
        //			}
        //
        //			public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        //			{
        //				if( value.GetType() == typeof(string) )
        //				{
        //					DBMSType dbms=DBMSType.UNKNOWN  ;
        //					if(System.Enum.IsDefined (typeof(DBMSType),value) )
        //					{
        //						dbms=(DBMSType)System.Enum.Parse (typeof(DBMSType),value.ToString (),false); 
        //					}
        //					return dbms;
        //				}
        //				else
        //					return base.ConvertFrom(context, culture, value);
        //
        //			}
        //
        //			/// <summary>
        //			/// true
        //			/// </summary>
        //			public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        //			{
        //				return true;
        //			}
        //
        //			public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        //			{
        //				if(context==null)
        //					return null;
        //				string[] dbmsTypeNames=System.Enum.GetNames (typeof(DBMSType ));
        //				return new TypeConverter.StandardValuesCollection(dbmsTypeNames);
        // 			}
        //
        //		}

        #endregion


    }
}
