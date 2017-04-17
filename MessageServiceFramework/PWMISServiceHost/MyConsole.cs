using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PWMIS.EnterpriseFramework.Service.Host
{
    class MyConsole
    {
        public static event EventHandler<StringEventArgs> WritedText;

        public static void Write(string text)
        {
            Console.Write(text);
            OnWritedText(text);
        }
        public static void WriteLine()
        {
            Console.WriteLine();
        }

        public static void WriteLine(string text)
        {
            Console.WriteLine(text);
            OnWritedText(text+"\r\n");
        }

        public static void WriteLine(string text,params object[] paras)
        {
            string temp = string.Format(text, paras);
            Console.WriteLine(temp);
            OnWritedText(temp + "\r\n");
        }


        private static void OnWritedText(string text)
        {
            if (WritedText != null)
                WritedText(null,new StringEventArgs (text));
        }
    }

    class StringEventArgs : EventArgs
    {
        public string Text { get; private set; }
        public StringEventArgs(string text)
        {
            this.Text = text;
        }
    }
}
