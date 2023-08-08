//ver 4.5 dbmstype auto get;

using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PWMIS.Common;
using PWMIS.DataProvider.Adapter;
using PWMIS.DataProvider.Data;

//using System.Drawing.Design;

namespace PWMIS.Web.Controls
{
    /// <summary>
    ///     单击事件委托定义
    /// </summary>
    public delegate void ClickEventHandler(object sender, EventArgs e);

    /// <summary>
    ///     数据帮定委托定义
    /// </summary>
    public delegate void DataBoundHandler(object sender, EventArgs e);

    /// <summary>
    ///     Web 分页工具条
    ///     邓太华 2007.1.10 Ver 1.0，2008.5.8 Ver 1.0.1.2，2008.7.24 Ver 1.0.1.3
    ///     Ver 1.0.1 增加数据访问功能
    ///     Ver 1.0.1.1 自动从配置文件设置全局默认配置参数，例如分页大小
    ///     Ver 1.0.1.2 除了可以自动配置分页大小外，还可以设置特定的分页大小。
    ///     Ver 1.0.1.3 支持GridView
    /// </summary>
    [ToolboxBitmap(typeof(ControlIcon), "DataPageToolBar.bmp")]
    [DefaultProperty("AllCount")]
    [DefaultEvent("PageChangeIndex")]
    [ToolboxData("<{0}:ProPageToolBar runat=server></{0}:ProPageToolBar>")]
    public class ProPageToolBar : WebControl, INamingContainer
    {
        /// <summary>
        ///     未初始化的数值
        /// </summary>
        private const int UNKNOW_NUM = -999;

        private CommonDB DAO
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
                DBMSType = _DAO.CurrentDBMSType;
                return _DAO;
            }
            set => _DAO = value;
        }

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

        #region 局部变量申明

        private int PageIndex = UNKNOW_NUM; //
        private int _AllCount;
        private int _PageSize;
        private int _CurrentPage;
        private bool hasSetBgColor;

        private bool ChangePageProperty;
        private bool _UserChangePageSize = true;
        private bool _ShowEmptyData = true;

        private string _SQL;
        private string _Where;
        private CommonDB _DAO;
        private IDataParameter[] _Parameters;

        #endregion

        #region 其它属性定义

        [Bindable(true)]
        [Category("Appearance")]
        [Description("分页说明")]
        [DefaultValue("")]
        public string Text { get; set; }

        private string FontSize => Font.Size.Unit.ToString(); //fontsize;

        /// <summary>
        ///     分页工具条的样式,0-默认，1-不显示记录条数；2-不显示页跳转；3-既不显示记录条数，也不显示页跳转
        /// </summary>
        [Bindable(true)]
        [Category("分页属性")]
        [Description("分页工具条的分页样式，0-默认，1-不显示记录条数；2-不显示页跳转；3-既不显示记录条数，也不显示页跳转")]
        public int PageToolBarStyle
        {
            get
            {
                if (ViewState["_PageToolBarStyle"] != null)
                    return (int)ViewState["_PageToolBarStyle"];
                return 0;
            }
            set => ViewState["_PageToolBarStyle"] = value;
        }

        #endregion

        #region 内部控件样式名定义

        public string css_linkStyle = "";
        public string css_btnStyle = "";
        public string css_txtStyle = "";

        #endregion

        #region 分页属性定义

        /// <summary>
        ///     当前所在页码，默认值1
        /// </summary>
        [Bindable(true)]
        [Category("分页属性")]
        [Description("当前所在页")]
        public int CurrentPage
        {
            get
            {
                if (ViewState[ID + "_CurrentPage"] != null)
                    _CurrentPage = (int)ViewState[ID + "_CurrentPage"];
                return _CurrentPage <= 0 ? 1 : _CurrentPage;
            }
            set
            {
                if (value < 0) value = 1;
                _CurrentPage = value;
                ViewState[ID + "_CurrentPage"] = value;
                PageIndex = value;
                ChangePageProperty = true;
                txtNavePage.Text = value.ToString();
            }
        }

        /// <summary>
        ///     记录总数，默认值0
        /// </summary>
        [Bindable(true)]
        [Category("分页属性")]
        [Description("记录总数")]
        [DefaultValue(0)]
        public int AllCount
        {
            get
            {
                if (ViewState[ID + "_AllCount"] != null)
                    _AllCount = (int)ViewState[ID + "_AllCount"];
                return _AllCount;
            }
            set
            {
                if (value < 0 && value != -1) value = 0;
                _AllCount = value;
                ViewState[ID + "_AllCount"] = value;
                ChangePageProperty = true;
                lblAllCount.Text = value.ToString();
            }
        }

        /// <summary>
        ///     页面大小，默认值10，输入0表示从系统自动获取配置值
        /// </summary>
        [Bindable(true)]
        [Category("分页属性")]
        [Description("每页面分页记录大小，默认值10,输入0表示从系统自动获取配置值")]
        [DefaultValue(10)]
        public int PageSize
        {
            get
            {
                if (ViewState[ID + "_PageSize"] != null)
                {
                    _PageSize = (int)ViewState[ID + "_PageSize"];
                    return _PageSize <= 0 ? 10 : _PageSize;
                }

                //设置默认分页大小
                if (AutoConfig && _PageSize == 0)
                {
                    var defaultPageSize = ConfigurationSettings.AppSettings["PageSize"];
                    if (defaultPageSize != null && defaultPageSize != "")
                    {
                        _PageSize = int.Parse(defaultPageSize);
                        return _PageSize;
                    }

                    _PageSize = 10;
                }
                else
                {
                    _PageSize = _PageSize <= 0 ? 10 : _PageSize;
                }

                return _PageSize;
            }
            set
            {
                if (AutoConfig && value == 0)
                {
                    var defaultPageSize = ConfigurationSettings.AppSettings["PageSize"];
                    if (defaultPageSize != null && defaultPageSize != "")
                        _PageSize = int.Parse(defaultPageSize);
                    else
                        _PageSize = 10;
                    value = _PageSize;
                }

                if (value < 0) value = 10;
                _PageSize = value;
                ViewState[ID + "_PageSize"] = value;
                ChangePageProperty = true;
            }
        }

        /// <summary>
        ///     页面总数，只读
        /// </summary>
        [Bindable(true)]
        [Category("分页属性")]
        [Description("页面总数，只读")]
        [DefaultValue(1)]
        public int PageCount
        {
            get
            {
                var AllPage = AllCount / PageSize;
                if (AllPage * PageSize < AllCount) AllPage++;
                if (AllPage <= 0) AllPage = 1;
                return AllPage;
            }
        }

        [Category("分页属性")]
        [Description("是否允许用户在浏览页面的时候改变分页大小")]
        [DefaultValue(true)]
        public bool UserChangePageSize
        {
            get
            {
                if (ViewState[ID + "_UserChangePageSize"] != null)
                    _UserChangePageSize = (bool)ViewState[ID + "_UserChangePageSize"];
                return _UserChangePageSize;
            }
            set
            {
                _UserChangePageSize = value;
                ViewState[ID + "_UserChangePageSize"] = value;
            }
        }

        #endregion

        #region 分页事件

        /// <summary>
        ///     页面改变事件
        /// </summary>
        [Category("分页事件")]
        [Description("页面改变事件")]
        public event ClickEventHandler PageChangeIndex;

        /// <summary>
        ///     目标控件完成数据绑定之前的事件
        /// </summary>
        [Category("Data")]
        [Description("目标控件完成数据绑定之前的事件")]
        public event DataBoundHandler DataControlDataBinding;

        /// <summary>
        ///     目标控件完成数据绑定完成事件
        /// </summary>
        [Category("Data")]
        [Description("目标控件完成数据绑定完成事件")]
        public event DataBoundHandler DataControlDataBound;

        /// <summary>
        ///     改变页码索引
        /// </summary>
        /// <param name="e">目标</param>
        protected void changeIndex(EventArgs e)
        {
            if (PageChangeIndex != null) PageChangeIndex(this, e);
            //if(this.Site !=null && ! this.Site.DesignMode  )//在运行时
            //{
            if (AutoBindData)
                if (Page.IsPostBack)
                    BindResultData();
            //}
        }

        #endregion

        #region 公开的方法

        /// <summary>
        ///     获取一个实例查询参数
        /// </summary>
        /// <returns></returns>
        public IDataParameter GetParameter()
        {
            return DAO.GetParameter();
        }

        /// <summary>
        ///     获取一个实例查询参数
        /// </summary>
        /// <param name="paraName"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public IDataParameter GetParameter(string paraName, object Value)
        {
            return DAO.GetParameter(paraName, Value);
        }

        /// <summary>
        ///     根据提供的分页查询和控件提供的数据访问信息，从数据源获取数据。
        /// </summary>
        /// <returns></returns>
        public object GetDataSource()
        {
            if (AllCount == 0)
                AllCount = -1; //特殊处理，获取记录数量为0时的架构
            DAO.ConnectionString = ConnectionString;
            object result = DAO.ExecuteDataSet(SQLbyPaging, CommandType.Text, Parameters);
            if (AllCount == -1)
                AllCount = 0;
            if (DAO.ErrorMessage != "")
                throw new Exception(DAO.ErrorMessage + ";SQL=" + SQLbyPaging);
            return result;
            //DAO.
        }

        /// <summary>
        ///     获取结果集记录数量
        /// </summary>
        /// <returns></returns>
        public int GetResultDataCount()
        {
            //创建一个新统计参数，避免参数在集合中已经存在的问题。
            IDataParameter[] countParas = null;
            if (Parameters != null && Parameters.Length > 0)
            {
                countParas = (IDataParameter[])Parameters.Clone();
                for (var i = 0; i < countParas.Length; i++)
                    countParas[i] = DAO.GetParameter(countParas[i].ParameterName, countParas[i].Value);
            }

            DAO.ConnectionString = ConnectionString;
            var count = DAO.ExecuteScalar(SQLbyCount, CommandType.Text, countParas);
            if (count != null)
                return Convert.ToInt32(count); //(int)count 在Oracle 将会失败。
            throw new Exception(DAO.ErrorMessage);
        }

        /// <summary>
        ///     将数据源的分页数据绑定到绑定目标控件上，支持GridView
        /// </summary>
        public void BindResultData()
        {
            var BindToControlID = BindToControl;
            if (BindToControlID != null && BindToControlID != "")
            {
                if (DataControlDataBinding != null) DataControlDataBinding(this, new EventArgs());
                //下面的方式如果本控件在用户控件中，将查找不到。
                //Control ctr= this.Page.FindControl (BindToControlID); 
                var ctr = FindMyControl(this, BindToControlID);

                if (ctr is GridView)
                {
                    ((GridView)ctr).DataSource = GetDataSource();
                    ctr.DataBind();
                }
                else if (ctr is DataGrid)
                {
                    ((DataGrid)ctr).DataSource = GetDataSource();
                    ctr.DataBind();
                }
                else if (ctr is DataList)
                {
                    ((DataList)ctr).DataSource = GetDataSource();
                    ctr.DataBind();
                }
                else if (ctr is Repeater)
                {
                    ((Repeater)ctr).DataSource = GetDataSource();
                    ctr.DataBind();
                }
                else
                {
                    throw new Exception("控件" + BindToControlID + "不支持数据绑定，请确保绑定目标控件是DataGrid,DataList,Repeater类型！");
                }

                if (DataControlDataBound != null) DataControlDataBound(this, new EventArgs());
            }
        }

        //      在用户控件中，仍然找不到该目标控件
        private Control FindMyControl(Control sourceControl, string objControlID)
        {
            //宽度优先策略
            foreach (Control ctr in sourceControl.Parent.Controls)
                if (ctr.ID == objControlID)
                    return ctr;
            foreach (Control ctr in sourceControl.Parent.Controls)
            {
                var objCtr = FindMyControl(ctr, objControlID);
                if (objCtr != null)
                    return objCtr;
            }

            return null;
        }

        /// <summary>
        ///     重新绑定数据和计算本次查询的记录数量，并设定当前页码在第一页
        /// </summary>
        public void ReBindResultData()
        {
            CurrentPage = 1;
            AllCount = GetResultDataCount();
            BindResultData();
        }

        #endregion

        #region 数据分页属性

        /// <summary>
        ///     需要绑定分页的控件，如DataGrid,DataList,Repeater 。
        /// </summary>
        [DefaultValue(null)]
        [Category("Data")]
        [Description("需要绑定分页的控件，如DataGrid,DataList,Repeater 。")]
        [TypeConverter(typeof(ControlListIDConverter))]
        public string BindToControl { get; set; }

        /// <summary>
        ///     用于分页查询的原始 SQL 语句
        /// </summary>
        [DefaultValue(null)]
        [Category("Data")]
        [Description("用于分页查询的原始 SQL 语句")]
        public string SQL
        {
            get
            {
                if (ViewState[ID + "_SQL"] != null)
                    _SQL = (string)ViewState[ID + "_SQL"];
                return _SQL;
            }
            set
            {
                _SQL = value;
                ViewState[ID + "_SQL"] = value;
            }
        }

        /// <summary>
        ///     分页查询参数,在运行时请调用 GetParameter方法 添加成员。
        /// </summary>
        [DefaultValue(null)]
        [Category("Data")]
        [Description("分页查询参数,在运行时请调用 GetParameter方法 添加成员。")]
        public IDataParameter[] Parameters
        {
            get
            {
                if (_Parameters != null) return _Parameters;

                if (HttpContext.Current.Session[ID + "_Parameters"] != null)
                {
                    var p0 = (IDataParameter[])HttpContext.Current.Session[ID + "_Parameters"];
                    var p1 = new IDataParameter[p0.Length];
                    for (var i = 0; i < p0.Length; i++) p1[i] = GetParameter(p0[i].ParameterName, p0[i].Value); //创建新参数

                    return p1;
                }

                return null;
            }
            set
            {
                _Parameters = value;
                HttpContext.Current.Session[ID + "_Parameters"] = _Parameters;
            }
        }

        /// <summary>
        ///     生成的用于分页查询的 SQL 语句
        /// </summary>
        [DefaultValue(null)]
        [Category("Data")]
        [Description("生成的用于分页查询的 SQL 语句")]
        public string SQLbyPaging
        {
            get
            {
                if (SQL == null) return "";
                SQLPage.DbmsType = DBMSType;
                return SQLPage.MakeSQLStringByPage(SQL, Where, PageSize, CurrentPage, AllCount);
            }
        }

        /// <summary>
        ///     生成的用于统计分页查询总记录数的 SQL 语句
        /// </summary>
        [DefaultValue(null)]
        [Category("Data")]
        [Description("生成的用于统计分页查询总记录数的 SQL 语句")]
        public string SQLbyCount
        {
            get
            {
                if (SQL == null) return "";
                SQLPage.DbmsType = DBMSType;
                return SQLPage.MakeSQLStringByPage(SQL, Where, PageSize, CurrentPage, 0);
            }
        }

        /// <summary>
        ///     指定用于分页查询所支持的数据库管理系统类型名称
        /// </summary>
        [DefaultValue(DBMSType.SqlServer)]
        [Category("Data")]
        [Description("指定用于分页查询所支持的数据库管理系统类型名称")]
        [TypeConverter(typeof(EnumConverter))]
        public DBMSType DBMSType { get; set; } = DBMSType.SqlServer;

        /// <summary>
        ///     是否自动化数据库实例对象，如果是，将采用DataProvider 数据访问块，能够方便的获取参数，生成结果数据集。如果未能正确配置，将不能设置为True 。
        /// </summary>
        [DefaultValue(false)]
        [Category("Data")]
        [Description("是否自动化数据库实例对象，如果是，将采用DataProvider 数据访问块，能够方便的获取参数，生成结果数据集。如果未能正确配置，将不能设置为True 。")]
        public bool AutoIDB { get; set; }

        private void CheckAutoIDB()
        {
            if (HttpContext.Current == null) //   this.Site !=null && this.Site.DesignMode
                return; //在设计时退出下面逻辑判断
            if (AutoIDB) //如果自动实例化数据库访问对象
                try
                {
                    ErrorMessage = "";
                    if (DAO == null)
                        DAO = MyDB.GetDBHelper(DBMSType, ConnectionString);
                    AutoIDB = true;
                }
                catch (Exception e)
                {
                    AutoIDB = false;
                    ErrorMessage = e.Message;
                }
        }

        /// <summary>
        ///     是否自动从应用程序配置文件获取数据访问配置信息，只有已经正确地配置了信息才可以返回True 。
        /// </summary>
        [DefaultValue(false)]
        [Category("Data")]
        [Description("是否自动从应用程序配置文件获取数据访问和其它配置信息，只有已经正确地配置了信息才可以返回True 。")]
        public bool AutoConfig { get; set; }

        private void CheckAutoConfig()
        {
            if (HttpContext.Current == null) //   this.Site !=null && this.Site.DesignMode
                return; //在设计时退出下面逻辑判断
            if (AutoConfig)
            {
                ErrorMessage = "";
                var strConn = "";
                //处理数据库管理系统类型
                var strDBMSType = ConfigurationSettings.AppSettings["EngineType"]; //统一从 DBMSType 获取

                if (strDBMSType != null && strDBMSType != "")
                {
                    if (Enum.IsDefined(typeof(DBMSType), strDBMSType))
                        DBMSType = (DBMSType)Enum.Parse(typeof(DBMSType), strDBMSType);
                    else
                        AutoConfig = false;

                    //处理连接字符串
                    var ConnStrKey = string.Empty;
                    switch (DBMSType)
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
                    AutoConfig = false;
                else
                    ConnectionString = strConn.Replace("~", Context.Request.PhysicalApplicationPath); //替换相对路径

                if (!AutoConfig) //在设计时不生成错误信息，因为VS2003设计时无法读取配置信息
                {
                    ErrorMessage = "未能正确配置数据访问信息，请检查是否已经在应用程序配置文件中进行了正确的配置";
                    AutoConfig = false;
                }
                else
                {
                    AutoIDB = AutoConfig; //如果正确配置，那么自动化数据库访问对象实例
                }
            }
        }

        /// <summary>
        ///     是否在运行时自动绑定分页数据，依赖于 AutoIDB 属性等于True
        /// </summary>
        [DefaultValue(false)]
        [Category("Data")]
        [Description("是否在运行时自动绑定分页数据，依赖于 AutoIDB 属性等于True")]
        public bool AutoBindData { get; set; } = false;

        /// <summary>
        ///     错误信息
        /// </summary>
        [DefaultValue("")]
        [Category("Data")]
        [Description("错误信息")]
        public string ErrorMessage { get; private set; } = string.Empty;

        //set{ _ErrorMessage=value;}
        /// <summary>
        ///     数据库连接字符串
        /// </summary>
        [DefaultValue("")]
        [Category("Data")]
        [Description("数据库连接字符串")]
        public string ConnectionString { get; set; } = string.Empty;

        /// <summary>
        ///     指定分页查询的附加条件，注意简单查询与复杂查询的条件限定方式。
        /// </summary>
        [DefaultValue("")]
        [Category("Data")]
        [Description("指定分页查询的附加条件，注意简单查询与复杂查询的条件限定方式。")]
        public string Where
        {
            get
            {
                if (ViewState[ID + "_Where"] != null)
                    _Where = (string)ViewState[ID + "_Where"];
                return _Where;
            }
            set
            {
                _Where = value;
                ViewState[ID + "_Where"] = value;
            }
        }

        /// <summary>
        ///     如果记录数量为0，根据设置是否显示数据架构，如果需要显示架构，那么将执行数据绑定方法。
        /// </summary>
        [DefaultValue(true)]
        [Category("Data")]
        [Description("如果记录数量为0，根据设置是否显示数据架构，如果需要显示架构，那么将执行数据绑定方法。")]
        public bool ShowEmptyData
        {
            get
            {
                if (ViewState[ID + "_ShowEmptyData"] != null)
                    _ShowEmptyData = (bool)ViewState[ID + "_ShowEmptyData"];
                return _ShowEmptyData;
            }
            set
            {
                _ShowEmptyData = value;
                ViewState[ID + "_ShowEmptyData"] = value;
            }
        }

        #endregion

        #region 基类重载的方法

        /// <summary>
        ///     将此控件呈现给指定的输出参数。
        /// </summary>
        /// <param name="output"> 要写出到的 HTML 编写器 </param>
        protected override void Render(HtmlTextWriter output)
        {
            if (ChangePageProperty) ChangePageProperty = false;
            //this.SetPageInfo ();
            SetPageInfo();
            ForeColor = ForeColor;
            EnsureChildControls();

            //处理表头样式
            output.Write("<table width='" + Width + "' height='" + Height
                         + "' bgcolor='" + ConvertColorFormat(BackColor)
                         + "' bordercolor='" + ConvertColorFormat(BorderColor)
                         + "' border='" + BorderWidth
                         + "' style='border-style:" + BorderStyle
                         + ";border-collapse:collapse' cellpadding='0'><tr><td><table width='100%' style='color:" +
                         ConvertColorFormat(ForeColor)
                         + " ;font-size:" + FontSize + "; font-family:" + Font.Name + "' class='"
                         + CssClass + "'><tr><td valign='baseline'>"
                         + Text + "</td><td valign='baseline'>");
            //添加控件
            //1-不显示记录条数；2-不显示页跳转；3-既不显示记录条数，也不显示页跳转

            var type = PageToolBarStyle;

            //1或3，不显示记录条数
            if (type != 1 && type != 3)
            {
                var currSize = PageSize;
                if (PageCount == CurrentPage)
                    currSize = AllCount - PageSize * (CurrentPage - 1);

                output.Write(currSize + "/"); //AllCount-PageSize*(PageNumber-1)
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
        ///     重写 OnLoad 事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            //			//在这里处理有关数据绑定属性
            //			CheckAutoConfig ();
            //			CheckAutoIDB();


            if (Site != null && Site.DesignMode) //在设计时
                return;
            if (AutoBindData)
                if (!Page.IsPostBack)
                {
                    AllCount = GetResultDataCount();
                    //如果记录数量为0，根据设置是否显示数据架构，如果需要显示架构，那么将执行数据绑定方法。
                    if (!ShowEmptyData && AllCount == 0)
                        return;

                    BindResultData();
                    //this.SetPageInfo ();
                }
        }


        /// <summary>
        ///     重写 CSS 样式名
        /// </summary>
        public override string CssClass
        {
            get => base.CssClass;
            set
            {
                base.CssClass = value;
                foreach (WebControl ctr in Controls) ctr.CssClass = value;
            }
        }

        /// <summary>
        ///     重写前景色
        /// </summary>
        public override Color ForeColor
        {
            get => base.ForeColor;
            set
            {
                base.ForeColor = value;
                foreach (WebControl ctr in Controls) ctr.ForeColor = value;
            }
        }

        public override Color BackColor
        {
            get
            {
                if (!hasSetBgColor)
                    return Color.White;
                return base.BackColor;
            }
            set
            {
                hasSetBgColor = true;
                base.BackColor = value;
            }
        }

        /// <summary>
        ///     创建子控件
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

            lblAllCount.Text = AllCount.ToString();
            lnkFirstPage.Text = "首页";
            lnkPrePage.Text = "上一页";
            lnkNextPage.Text = "下一页";
            lnkLastPage.Text = "尾页";
            txtNavePage.Width = 30;
            lnkGo.Text = "Go";


            if (UserChangePageSize)
            {
                Controls.Add(dlPageSize);
                dlPageSize.AutoPostBack = true;
                dlPageSize.Items.Clear();
                for (var i = 5; i <= 50; i += 5) dlPageSize.Items.Add(i.ToString());
                dlPageSize.SelectedValue = PageSize.ToString();
                dlPageSize.SelectedIndexChanged += dlPageSize_SelectedIndexChanged;
            }

            lnkFirstPage.Click += lnkFirstPage_Click;
            lnkPrePage.Click += lnkPrePage_Click;
            lnkNextPage.Click += lnkNextPage_Click;
            lnkLastPage.Click += lnkLastPage_Click;
            lnkGo.Click += lnkGo_Click;
        }

        #endregion

        #region 内部事件处理

        /// <summary>
        ///     RGB颜色值到Html颜色值转换
        /// </summary>
        /// <param name="RGBColor"></param>
        /// <returns></returns>
        private string ConvertColorFormat(Color RGBColor)
        {
            //return "RGB(" + RGBColor.R.ToString() + "," + RGBColor.G.ToString() + "," + RGBColor.G.ToString() + ")";
            //2019.3.6 修改，原有方法有问题，可能取不到RGB分值
            return ColorTranslator.ToHtml(RGBColor);
        }

        /// <summary>
        ///     设置分页状态信息
        /// </summary>
        private void SetPageInfo()
        {
            if (PageIndex == UNKNOW_NUM)
                PageIndex = CurrentPage;

            if (PageIndex > PageCount) PageIndex = PageCount;
            else if (PageIndex == -1) PageIndex = PageCount;
            else if (PageIndex < 1) PageIndex = 1;

            if (AllCount == 0)
                lblCPA.Text = "0/0";
            else
                lblCPA.Text = PageIndex + "/" + PageCount;
            txtNavePage.Text = PageIndex.ToString();
            CurrentPage = PageIndex;

            if (PageCount == 1) lnkGo.Enabled = false;

            if (PageIndex == 0)
            {
                lnkFirstPage.Enabled = false;
                lnkPrePage.Enabled = false;
                lnkLastPage.Enabled = false;
                lnkNextPage.Enabled = false;
                //this.btnNavePage .Enabled =false;
                return;
            }

            if (PageIndex == 1)
            {
                lnkFirstPage.Enabled = false;
                lnkPrePage.Enabled = false;
            }
            else
            {
                lnkFirstPage.Enabled = true;
                lnkPrePage.Enabled = true;
            }

            if (PageIndex < PageCount)
            {
                lnkLastPage.Enabled = true;
                lnkNextPage.Enabled = true;
            }
            else
            {
                lnkLastPage.Enabled = false;
                lnkNextPage.Enabled = false;
            }
        }

        /// <summary>
        ///     下一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnkNextPage_Click(object sender, EventArgs e)
        {
            PageIndex = CurrentPage;
            PageIndex++;
            SetPageInfo();
            changeIndex(e);
        }

        /// <summary>
        ///     上一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnkPrePage_Click(object sender, EventArgs e)
        {
            PageIndex = CurrentPage;
            PageIndex--;
            SetPageInfo();
            changeIndex(e);
        }

        /// <summary>
        ///     尾页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnkLastPage_Click(object sender, EventArgs e)
        {
            PageIndex = -1;
            SetPageInfo();
            changeIndex(e);
        }

        /// <summary>
        ///     首页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnkFirstPage_Click(object sender, EventArgs e)
        {
            PageIndex = 1;
            SetPageInfo();
            changeIndex(e);
        }


        /// <summary>
        ///     初始化链接文字样式
        /// </summary>
        private void InitStyle()
        {
            if (css_linkStyle != "")
                lnkFirstPage.Attributes.Add("class", css_linkStyle);
            if (css_linkStyle != "")
                lnkNextPage.Attributes.Add("class", css_linkStyle);
            if (css_linkStyle != "")
                lnkLastPage.Attributes.Add("class", css_linkStyle);
            if (css_linkStyle != "")
                lnkPrePage.Attributes.Add("class", css_linkStyle);
            if (css_linkStyle != "")
                lnkGo.Attributes.Add("class", css_linkStyle);
            if (css_txtStyle != "")
                txtNavePage.Attributes.Add("class", css_txtStyle);
        }

        /// <summary>
        ///     转到某页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnkGo_Click(object sender, EventArgs e)
        {
            try
            {
                PageIndex = int.Parse(txtNavePage.Text.Trim());
                SetPageInfo();
                changeIndex(e);
            }
            catch
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "pageErr",
                    "<script language='javascript'>alert('请填写数字页码！');</script>");
            }
        }

        /// <summary>
        ///     改变页大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            PageSize = int.Parse(dlPageSize.SelectedValue);
            CurrentPage = 1;
            PageIndex = CurrentPage;
            SetPageInfo();
            BindResultData();
        }

        #endregion

        #region 获取绑定控件列表 类

        /// <summary>
        ///     获取绑定控件列表 类
        /// </summary>
        public class ControlListIDConverter : StringConverter
        {
            /// <summary>
            ///     false
            /// </summary>
            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
            {
                return false;
            }

            /// <summary>
            ///     true
            /// </summary>
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                return true;
            }

            /// <summary>
            ///     获取所有运行时的目标控件的ID
            /// </summary>
            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                if (context == null)
                    return null;
                var al = new ArrayList();
                foreach (IComponent ic in context.Container.Components)
                {
                    if (ic is ProPageToolBar)
                        continue;
                    if (ic is DataGrid || ic is Repeater || ic is DataList ||
                        ic is GridView) //|| ic is System.Web.UI.WebControls.ListView 可能需要3.5框架
                        al.Add(((Control)ic).ID);
                }

                return new StandardValuesCollection(al);
            }
        }

        public DBMSType DBMSType1
        {
            get => throw new NotImplementedException();
            set { }
        }

        public SQLPage SQLPage
        {
            get => throw new NotImplementedException();
            set { }
        }

        public DataBoundHandler DataBoundHandler
        {
            get => throw new NotImplementedException();
            set { }
        }

        public ClickEventHandler ClickEventHandler
        {
            get => throw new NotImplementedException();
            set { }
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