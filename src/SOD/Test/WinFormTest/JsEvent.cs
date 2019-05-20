using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormTest
{
    public class JsEvent
    {
        public string MessageText = string.Empty;


        public void ShowTest()
        {
            MessageBox.Show("this in C#.\n\r" + MessageText);
        }
    }

}
