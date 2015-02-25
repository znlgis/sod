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
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace PWMIS.Web.Controls
{
    /// <summary>
    /// DataGrid 增强，支持动态客户端选择，鼠标动态跟随。
    /// 邓太华 2008.5.6 Ver 1.1
    /// </summary>
    [DefaultProperty("Text"),
        ToolboxData("<{0}:ProDataGrid runat=server ></{0}:ProDataGrid>")]
    public class ProDataGrid : System.Web.UI.WebControls.DataGrid
    {
        private string text;
        private string m_SelectedFieldValue = string.Empty;
        private string m_CssClassRowSelected = string.Empty;
        private string m_CssClassRowMouseMove = string.Empty;
        private string m_ScriptPath = string.Empty;
        private bool m_ClientSelectMode = false;
        private bool m_MorePageSeleced = false;
        private bool m_DefaultSet = true;
        private bool m_ShowCheckColumn = true;
        private bool m_ShowcheckControl = true;
        private string m_ClientSelectedValue = string.Empty;
        private string m_CheckAllText = "选择";
        private string m_CheckItemText = string.Empty;
        private int m_SelectedFromCellIndex = 1;

        private TemplateColumn tm;

        private string SelectValueList = string.Empty;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ProDataGrid()
        {
            //m_DefaultSet=true;
            this.EnsureChildControls();
        }

        //		private void MyDataGrid_ItemCreated( object sender , System.Web.UI.WebControls.DataGridItemEventArgs e ) 
        //		{ 
        ////			if ( e.Item.ItemType == ListItemType.Pager ) 
        ////			{ 
        ////				Literal msg = new Literal(); 
        ////				msg.Text = "把这三句代码替换成呈现实际翻页控件外观的代码。"; 
        ////				( ( TableCell ) e.Item.Controls[0] ).Controls.Add( msg ); 
        ////			} 
        //			
        //		} 

        /// <summary>
        /// 选择框所在列
        /// </summary>
        [Browsable(false)]
        public TemplateColumn CheckColumn
        {
            get
            {
                //				if(tm!=null)
                //					return tm;

                //				if(this.Columns.Count >0)
                //				{
                //					if(this.Columns [0] is TemplateColumn )
                //					{
                //						if(this.Columns [0].HeaderText =="CheckColumn")
                //							tm=this.Columns [0] as TemplateColumn;
                //					}
                //				}
                //				if( tm==null)
                //				{
                //					tm=new TemplateColumn();
                //				
                //					ColumnTemplate ItemTemplate=new ColumnTemplate();
                //					ItemTemplate.IsMoreSelect=m_ClientSelectMode;
                //					tm.ItemTemplate=ItemTemplate;
                //					
                //					ColumnTemplate2 tmHead=new ColumnTemplate2 ();
                //					tmHead.IsMoreSelect =m_ClientSelectMode;
                //					tmHead.CheckAllText =this.CheckHeaderText  ;//"全选";
                //					tm.HeaderTemplate =tmHead;
                //					tm.HeaderText="CheckColumn";						 
                //					this.Columns.AddAt(0,tm);
                //				}
                return tm;
            }
            set
            {
                tm = value;
            }
        }
        /// <summary>
        /// 客户端选择的值
        /// </summary>
        [Browsable(false)]
        public string ClientSelectedValue
        {
            get
            {
                return m_ClientSelectedValue;
            }
            set
            {
                m_ClientSelectedValue = value;
            }
        }

        /// <summary>
        /// 客户端多选的时候，是否记录上次选择的值，通常用于多页选择。
        /// </summary>
        [Description("客户端多选的时候，是否记录上次选择的值，通常用于多页选择。"), Bindable(true),
        Category("Behavior"),
        DefaultValue(false)]
        public bool MorePageSeleced
        {
            get
            {
                return m_MorePageSeleced;
            }
            set
            {
                m_MorePageSeleced = value;
            }
        }

        /// <summary>
        /// 客户端选择的方式,False=单选,True=多选
        /// </summary>
        [Description("客户端选择的方式,False=单选,True=多选"), Bindable(true),
        Category("Behavior"),
        DefaultValue(false)
        ]
        public bool ClientSelectMode
        {
            get
            {
                if (ViewState["ClientSelectMode"] != null)
                    m_ClientSelectMode = (bool)ViewState["ClientSelectMode"];
                return m_ClientSelectMode;
            }
            set
            {
                //VS2008 属性变量在构造函数之前先设置，与VS2003 不同，所以在程序中引用全局变量会出问题
                m_ClientSelectMode = value;
                ViewState["ClientSelectMode"] = value;

                if (m_ClientSelectMode)
                {
                    text = "多选状态";
                }
                else
                {
                    m_MorePageSeleced = false; //分页选择仅在多选模式有效
                    text = "单选状态";
                }
                //SetCheckColumnInfo();

            }
        }

        /// <summary>
        /// 客户端选择的脚本文件地址
        /// </summary>
        [Description("客户端选择的脚本文件地址"), Bindable(true),
        Category("Behavior"),
        DefaultValue("")]
        public string ScriptPath
        {
            get
            {
                return m_ScriptPath;
            }
            set
            {
                m_ScriptPath = value;
            }
        }

        /// <summary>
        /// 用于选择一行时的用户CSS类名
        /// </summary>
        [Description("用于选择一行时的用户CSS类名"), Bindable(true),
        Category("Appearance"),
        DefaultValue("")]
        public string CssClassRowSelected
        {
            get
            {
                return m_CssClassRowSelected;
            }
            set
            {
                m_CssClassRowSelected = value;
            }
        }

        /// <summary>
        /// 用于鼠标悬浮在一行时的用户CSS类名
        /// </summary>
        [Description("用于鼠标悬浮在一行时的用户CSS类名"), Bindable(true),
        Category("Appearance"),
        DefaultValue("")]
        public string CssClassRowMouseMove
        {
            get
            {
                return m_CssClassRowMouseMove;
            }
            set
            {
                m_CssClassRowMouseMove = value;
            }
        }

        /// <summary>
        /// 每一行选择的值，对应于某一个列
        /// </summary>
        [Description("每一行选择的值，对应于某一个列。如果值不为空，优先于SelectedFromCellIndex 属性"), Bindable(true),
        Category("Data"),
        DefaultValue("")]
        public string SelectedFieldValue
        {
            get
            {
                return m_SelectedFieldValue;

            }

            set
            {
                m_SelectedFieldValue = value;
            }
        }

        /// <summary>
        /// 选择值所在的单元格的索引，小于1该属性无效
        /// </summary>
        [Description("选择值所在的单元格的索引，小于1该属性无效"), Bindable(true),
        Category("Data"),
        DefaultValue("1")]
        public int SelectedFromCellIndex
        {
            get
            {
                return m_SelectedFromCellIndex;

            }

            set
            {
                //if(value <this.Columns.Count )
                m_SelectedFromCellIndex = value;
            }
        }


        /// <summary>
        /// 该属性用于表示控件显示的文字内容
        /// </summary>
        [Description("该属性用于表示控件显示的文字内容"), Bindable(true),
            Category("Appearance"),
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

        /// <summary>
        /// 是否显示选择列
        /// </summary>
        [Description("是否显示选择列"), Bindable(true),
        Category("Appearance"),
        DefaultValue(true)]
        public bool ShowCheckColumn
        {
            get
            {
                return m_ShowCheckColumn;
            }

            set
            {
                m_ShowCheckColumn = value;

                if (tm != null)
                    tm.Visible = value;
            }
        }

        /// <summary>
        /// 是否显示选择列中的选择控件
        /// </summary>
        [Description("是否显示选择列中的选择控件"), Bindable(true),
        Category("Appearance"),
        DefaultValue(true)]
        public bool ShowcheckControl
        {
            get
            {
                return m_ShowcheckControl;
            }

            set
            {
                m_ShowcheckControl = value;
            }
        }


        /// <summary>
        /// 选择列标题行文字
        /// </summary>
        [Description("选择列标题行文字"), Bindable(true),
        Category("Appearance"),
        DefaultValue("")]
        public string CheckHeaderText
        {
            get
            {
                return m_CheckAllText;
            }

            set
            {
                m_CheckAllText = value;
                if (tm != null)
                {
                    ColumnTemplate2 tmHead = (ColumnTemplate2)tm.HeaderTemplate;
                    tmHead.CheckAllText = value;
                }
            }
        }

        /// <summary>
        /// 选择列的文本
        /// </summary>
        [Description("选择列的文本"), Bindable(true),
        Category("Appearance"),
        DefaultValue("")]
        public string CheckItemText
        {
            get
            {
                return m_CheckItemText;
            }

            set
            {
                m_CheckItemText = value;
            }
        }

        /// <summary>
        /// 是否应用默认的样式和脚本设置
        /// </summary>
        [Description("是否应用默认的样式和脚本设置"), Bindable(true),
        Category("Appearance"),
        DefaultValue(true),
        DesignOnly(true)]
        public bool DefaultSet
        {
            get
            {
                return m_DefaultSet;
            }

            set
            {
                m_DefaultSet = value;
                if (!m_DefaultSet)//清除默认设置
                {
                    this.CssClass = "";
                    this.AlternatingItemStyle.CssClass = "";
                    this.ItemStyle.CssClass = "";
                    this.HeaderStyle.CssClass = "";
                    this.CssClassRowMouseMove = "";
                    this.CssClassRowSelected = "";
                    //this.ScriptPath ="";
                }
                SetDefaultInfo();
            }
        }

        /// <summary>
        /// 增加首个模版列，可以有选择功能。
        /// </summary>
        private void SetCheckColumnInfo()
        {
            if (tm == null)
            {
                tm = new TemplateColumn();

                ColumnTemplate ItemTemplate = new ColumnTemplate();
                ItemTemplate.IsMoreSelect = ClientSelectMode;
                tm.ItemTemplate = ItemTemplate;

                ColumnTemplate2 tmHead = new ColumnTemplate2();
                tmHead.IsMoreSelect = this.ClientSelectMode;// m_ClientSelectMode;
                tmHead.CheckAllText = this.CheckHeaderText;//"全选";
                tm.HeaderTemplate = tmHead;
                tm.HeaderText = this.CheckHeaderText;
                tm.Visible = this.ShowCheckColumn;
                //tm.HeaderStyle.Width=100;

                if (this.Columns.Count > 0)
                {
                    if (this.Columns[0] is TemplateColumn)
                    {
                        this.Columns.RemoveAt(0);
                    }
                }

                this.Columns.AddAt(0, tm);
            }
            else
            {
                ColumnTemplate ItemTemplate = (ColumnTemplate)tm.ItemTemplate;
                ItemTemplate.IsMoreSelect = this.ClientSelectMode;
                //tm.ItemTemplate=ItemTemplate;

                ColumnTemplate2 tmHead = (ColumnTemplate2)tm.HeaderTemplate;
                tmHead.CheckAllText = this.CheckHeaderText;// "全选2";
                tmHead.IsMoreSelect = ClientSelectMode;
                tm.HeaderText = this.CheckHeaderText;
                tm.Visible = this.ShowCheckColumn;
            }

        }



        /// <summary>
        /// /默认样式等设置
        /// </summary>
        private void SetDefaultInfo()
        {
            if (m_DefaultSet)
            {
                //默认样式名
                if (this.CssClass == "")
                    this.CssClass = "dg_table";
                if (this.AlternatingItemStyle.CssClass == "")
                    this.AlternatingItemStyle.CssClass = "dg_alter";
                if (this.ItemStyle.CssClass == "")
                    this.ItemStyle.CssClass = "dg_item";
                if (this.HeaderStyle.CssClass == "")
                    this.HeaderStyle.CssClass = "dg_header";
                if (this.CssClassRowMouseMove == "")
                    this.CssClassRowMouseMove = "Umove";
                if (this.CssClassRowSelected == "")
                    this.CssClassRowSelected = "Uselected";
                if (this.ScriptPath == "")
                    if (this.ClientSelectMode)
                        this.ScriptPath = "multipleTableRow.js";
                    else
                        this.ScriptPath = "singleTableRow.js";


            }
            //			if(m_DefaultSet)
            //			{
            //				this.CssClass ="dg_table";
            //								
            //				this.AlternatingItemStyle .CssClass ="dg_alter"; 
            //								
            //				this.ItemStyle .CssClass ="dg_item";
            //								
            //				this.HeaderStyle.CssClass ="dg_header";
            //								
            //				this.CssClassRowMouseMove ="Umove";
            //								
            //				this.CssClassRowSelected ="Uselected";
            //				if(this.ScriptPath =="")
            //					if(this.ClientSelectMode )
            //						this.ScriptPath ="multipleTableRow.js";
            //					else
            //						this.ScriptPath ="singleTableRow.js";
            //			}

        }

        /// <summary> 
        /// 将此控件呈现给指定的输出参数。
        /// </summary>
        /// <param name="output"> 要写出到的 HTML 编写器 </param>
        protected override void Render(HtmlTextWriter output)
        {
            //output.Write(Text);
            //SetDefaultInfo();



            base.Render(output);

            string script = "<script language=\"javascript\">\n " +
                "<!--\n";
            if (ClientSelectMode)
            {
                script += "SetCheckValues();\n";
            }
            //if(m_MorePageSeleced)//允许多页选择
            script += "InitLastSelected('" + m_ClientSelectedValue + "');\n";
            script += "//-->\n</script>\n";
            output.Write(script);

        }

        /// <summary>
        /// 重写初始化事件
        /// </summary>
        /// <param name="e">事件对象</param>
        protected override void OnInit(EventArgs e)
        {
            //此处设置运行时的效果 是 关键 
            SetCheckColumnInfo();
            SetDefaultInfo();
            base.OnInit(e);
            if(!this.DesignMode )
                this.ClientSelectedValue = this.Page.Request.Form["CID"];
            //			if(m_DefaultSet)
            //			{
            //                //自定义属性必须在控件运行初始化时候设置默认值
            //				if(this.CssClassRowMouseMove =="")
            //					this.CssClassRowMouseMove ="Umove";
            //				if(this.CssClassRowSelected =="")
            //					this.CssClassRowSelected ="Uselected";
            //				if(this.ScriptPath =="")
            //					this.ScriptPath ="singleTableRow.js";
            //			}
            //			if(this.CheckAllText=="")
            //				this.CheckAllText ="全选";

            if (ClientSelectMode)
            {
                text = "多选状态";//this.Columns[0].GetType().ToString ();
                //this.Columns.RemoveAt (1);//运行时删除空白模板列
            }
            else
            {
                m_MorePageSeleced = false; //分页选择仅在多选模式有效
                text = "单选状态";

            }


            //this.Columns.RemoveAt (1);//运行时删除空白模板列

        }

        /// <summary>
        /// 预呈现处理，主要处理多页选择记录问题
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            //此处设置设计时的效果 
            //SetCheckColumnInfo();
            base.OnPreRender(e);
            RegisterMyClientScript();


            //
            if (ClientSelectMode)
            {
                string OldSelectValueList = this.Page.Request.Form["SelectValueList"];
                if (SelectValueList != string.Empty)//如果经过了数据绑定
                    SelectValueList = SelectValueList.Remove(SelectValueList.Length - 1, 1);
                else
                    SelectValueList = OldSelectValueList;

                this.Page.ClientScript.RegisterHiddenField("SelectValueList", SelectValueList);//记录复选框当前的值

                string CurrentSelectedValue = this.Page.Request.Form["CID"];//当前选择的值
                string SHValue = this.Page.Request.Form["SHValue"];//客户端发生选择事件的标记
                //多页选择处理
                if (m_MorePageSeleced)
                {
                    string LastSelectedValues = this.Page.Request.Form["LastSelectedValues"];
                    if (CurrentSelectedValue != null && SHValue == "-1")//客户端发生了选择事件
                    {
                        if (LastSelectedValues != "")
                        {
                            //如果 LastSelectedValues 中的项 在OldSelectValueList 中存在，则删除相关项。
                            LastSelectedValues = DeleRepStringList(LastSelectedValues, OldSelectValueList);
                            if (LastSelectedValues != "")
                            {
                                LastSelectedValues = LastSelectedValues.Replace(",,", "");
                                m_ClientSelectedValue = CurrentSelectedValue + "," + LastSelectedValues;
                            }
                            else
                                m_ClientSelectedValue = CurrentSelectedValue;
                        }
                        else
                            m_ClientSelectedValue = CurrentSelectedValue;
                    }
                    else
                    {
                        if (LastSelectedValues != "" && SHValue == "-1")
                        {
                            //如果 LastSelectedValues 中的项 在OldSelectValueList 中存在，则删除相关项。
                            LastSelectedValues = DeleRepStringList(LastSelectedValues, OldSelectValueList);
                            if (LastSelectedValues != "")
                            {
                                LastSelectedValues = LastSelectedValues.Replace(",,", "");
                                m_ClientSelectedValue = LastSelectedValues;
                            }
                            else
                                m_ClientSelectedValue = "";
                        }
                        else
                            m_ClientSelectedValue = LastSelectedValues;

                    }
                    this.Page.ClientScript.RegisterHiddenField("LastSelectedValues", m_ClientSelectedValue);
                }
                else
                {
                    m_ClientSelectedValue = CurrentSelectedValue;
                }

            }

        }

        /// <summary>
        /// 去除字符串中的重复项
        /// </summary>
        /// <param name="ObjStr">目标字符串列表，形如 “1,2,3”</param>
        /// <param name="SourceStr">源字符串列表，形如 “1,2,3”</param>
        /// <returns>返回目标串</returns>
        private string DeleRepStringList(string ObjStr, string SourceStr)
        {
            string limit = ",";
            string strTemp = string.Empty;
            string[] arrLSV = ObjStr.Split(limit.ToCharArray());
            SourceStr += ",";
            ObjStr += ",";
            for (int i = 0; i < arrLSV.Length; i++)
            {
                strTemp = arrLSV[i] + ",";
                if (strTemp != "," && SourceStr.IndexOf(strTemp) != -1)
                    ObjStr = ObjStr.Replace(strTemp, "");
            }
            return ObjStr;
        }

        //		/// <summary>
        //		/// 重写加载事件
        //		/// </summary>
        //		/// <param name="e">事件对象</param>
        //		protected override void OnLoad(EventArgs e)
        //		{
        //			base.OnLoad (e);
        //			
        //			//RegisterMyClientScript();
        //		}

        /// <summary>
        /// 注册选择脚本
        /// </summary>
        private void RegisterMyClientScript()
        {
            this.Page.ClientScript.RegisterHiddenField("SHValue", "");//注册单选值控件
            //string SingleScriptPath="singleTableRow.js";
            //注册选择样式脚本
            string script = "<script language=\"javascript\" src=\"" + m_ScriptPath + "\" type=\"text/Jscript\"></script>\n " +
                "<script language=\"javascript\">\n " +
                "<!--\n" +
                "cssRowSelected=\"" + m_CssClassRowSelected + "\";\n" +
                "cssRowMouseMove=\"" + CssClassRowMouseMove + "\";\n" +
                "//-->\n" +
                "</script>";
            this.Page.ClientScript.RegisterClientScriptBlock(this.GetType (),"ClientSelect", script);
        }

        /// <summary>
        /// 重写数据项绑定事件
        /// </summary>
        /// <param name="e">网个项目事件</param>

        protected override void OnItemDataBound(DataGridItemEventArgs e)
        {
            base.OnItemDataBound(e);
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.SelectedItem)
            {
                //SelectedFieldValue 在实例控件绑定，例如 DataBinder.Eval(e.Item.DataItem, "ProductID").ToString();  
                string Value = (m_SelectedFieldValue == "" ? (this.SelectedFromCellIndex >= 1 ? e.Item.Cells[this.SelectedFromCellIndex].Text : "") : m_SelectedFieldValue);
                string argstring = "this,'" + Value + "'";
                e.Item.Attributes.Add("onclick", "myclick(" + argstring + ")");
                e.Item.Attributes.Add("onmouseover", "mymove(this)");
                e.Item.Attributes.Add("onmouseout", "myout(this)");

                //				if(e.Item.Cells[0].HasControls () && e.Item.Cells[0].Controls.Count >0)
                //				{
                //					LiteralControl cb=(LiteralControl)e.Item.Cells[0].Controls [0];// .FindControl ("CID"); 
                //					if(cb!=null)
                //					{
                //						string text=this.CheckItemText ==string.Empty ?(e.Item.ItemIndex+1).ToString ():this.CheckItemText ;
                //						if(m_ClientSelectMode)
                //							cb.Text = GetSelectTableHTML("checkbox",text,m_SelectedFieldValue);// "<input type='checkbox' name='CID' >" +(e.Item.ItemIndex+1).ToString ();
                //						else
                //							cb.Text = GetSelectTableHTML("radio",text,m_SelectedFieldValue);//"<input type='radio' name='CID' >" +(e.Item.ItemIndex+1).ToString ();
                //						//绑定序号
                //						SelectValueList+=m_SelectedFieldValue+",";//复选框选择值列表，在没有数据绑定的时候备用
                //					
                //					}
                //				}

                string text = this.CheckItemText == string.Empty ? (e.Item.ItemIndex + 1).ToString() : this.CheckItemText;
                if (ClientSelectMode)
                    e.Item.Cells[0].Text = GetSelectTableHTML("checkbox", text, Value);// "<input type='checkbox' name='CID' >" +(e.Item.ItemIndex+1).ToString ();
                else
                    e.Item.Cells[0].Text = GetSelectTableHTML("radio", text, Value);//"<input type='radio' name='CID' >" +(e.Item.ItemIndex+1).ToString ();
                //绑定序号
                SelectValueList += Value + ",";//复选框选择值列表，在没有数据绑定的时候备用
            }
            if (e.Item.ItemType == ListItemType.Header)
            {
                if (e.Item.Cells[0].HasControls() && e.Item.Cells[0].Controls.Count > 0)
                {
                    LiteralControl cb = (LiteralControl)e.Item.Cells[0].Controls[0];// .FindControl ("CID"); 
                    if (cb != null)
                    {
                        string text = this.CheckHeaderText;
                        if (this.ClientSelectMode)
                            cb.Text = "<input type=\"checkbox\" id=\"CheckAll\" name=\"CheckAll\" value=\"ON\" onclick=\"CheckedAll()\"><label for=\"CheckAll\">" + text + "</label>";
                        else
                            cb.Text = text;

                    }
                }
            }
        }

        //		/// <summary>
        //		/// 重写网格项创建事件
        //		/// </summary>
        //		/// <param name="e">网格项事件</param>
        //		protected override void OnItemCreated(DataGridItemEventArgs e)
        //		{
        //			base.OnItemCreated (e);
        //			if(e.Item.ItemType ==ListItemType.Header )
        //			{
        //				if(e.Item.Cells[0].HasControls () && e.Item.Cells[0].Controls.Count >0)
        //				{
        //					LiteralControl cb=(LiteralControl)e.Item.Cells[0].Controls [0];// .FindControl ("CID"); 
        //					if(cb!=null)
        //					{
        //						string text=this.CheckHeaderText ;
        //						if(this.ClientSelectMode)
        //							cb.Text = "<input type=\"checkbox\" id=\"CheckAll\" name=\"CheckAll\" value=\"ON\" onclick=\"CheckedAll()\"><label for=\"CheckAll\">"+text+"</label>";
        //						else
        //							cb.Text =text;
        //											
        //					}
        //				}
        //			}
        //		}

        private string GetSelectTableHTML(string typeName, string text, string Value)
        {
            string style = this.ShowcheckControl ? "" : "style='display:none'";
            string sTable = @"<table width=""100%"" >
					<tr><td width=""20""><input type=""@typeName"" name=""CID"" value=""@Value"" @style></td>
						<td >@text</td>
					</tr></table>";
            sTable = sTable.Replace("@typeName", typeName).Replace("@text", text)
                .Replace("@Value", Value).Replace("@style", style);
            return sTable;
        }

    }



    ///  ColumnTemplate 从ITemplate继承。
    ///  "InstantiateIn"定义子控件的属于谁

    /// <summary>
    /// 多选模版列
    /// </summary>
    internal class ColumnTemplate : ITemplate
    {
        //
        private string _type = "checkbox";
        private bool _IsMoreSelect = true;

        /// <summary>
        /// 是否是多选
        /// </summary>
        public bool IsMoreSelect
        {
            get { return _IsMoreSelect; }
            set
            {
                _IsMoreSelect = value;
                if (value)
                    _type = "checkbox";
                else
                    _type = "radio";
            }
        }
        /// <summary>
        /// 定义子控件的属于谁
        /// </summary>
        /// <param name="container">容器</param>
        public void InstantiateIn(Control container)
        {

            //			LiteralControl l1=new LiteralControl ("select");
            //			l1.ID ="ItemLit1";
            //			container.Controls.Add(l1);

        }

    }

    /// <summary>
    /// 全选模版列
    /// </summary>
    internal class ColumnTemplate2 : ITemplate
    {
        private string _text = "";
        private bool _IsMoreSelect = true;

        /// <summary>
        /// 是否是多选
        /// </summary>
        public bool IsMoreSelect
        {
            get { return _IsMoreSelect; }
            set
            {
                _IsMoreSelect = value;
            }
        }
        /// <summary>
        /// “选择所有”的文字
        /// </summary>
        public string CheckAllText
        {
            set { _text = value; }
            get { return _text; }
        }
        /// <summary>
        /// 定义子控件的属于谁
        /// </summary>
        /// <param name="container">容器对象</param>
        public void InstantiateIn(Control container)
        {
            string ls = _text;
            if (this.IsMoreSelect)
                ls = "<input type=\"checkbox\" id=\"CheckAll\" name=\"CheckAll\" value=\"ON\" onclick=\"CheckedAll()\"><label for=\"CheckAll\">" + _text + "</label>";
            LiteralControl l1 = new LiteralControl(ls);
            container.Controls.Add(l1);

        }

    }

}
