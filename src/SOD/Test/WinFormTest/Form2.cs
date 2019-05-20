using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;

namespace WinFormTest
{
    public partial class Form2 : Form
    {
        private CefSharp.WinForms.ChromiumWebBrowser cw;

        public Form2()
        {
            InitializeComponent();

            string path = System.IO.Path.Combine(System.Environment.CurrentDirectory, "HTMLPage1.html");
            System.Uri uri = new Uri(path);
            var setting = new CefSharp.CefSettings();
            var osVersion = Environment.OSVersion;
            //Disable GPU for Windows 7  ,8,8.1 
            if (osVersion.Version.Major == 6 )
            {
                // Disable GPU in WPF and Offscreen examples until #1634 has been resolved
                setting.CefCommandLineArgs.Add("disable-gpu", "1");
            }


            CefSharp.Cef.Initialize(setting); 

            cw = new CefSharp.WinForms.ChromiumWebBrowser(uri.AbsoluteUri);

            cw.Dock = DockStyle.Fill;
            this.Controls.Add(cw);
            cw.RegisterJsObject("jsObj", new JsEvent(), CefSharp.BindingOptions.DefaultBinder);
        }

        private void Form2_Load(object sender, EventArgs e)
        {
           
        }
    }
}
