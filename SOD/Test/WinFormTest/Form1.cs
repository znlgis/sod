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
    public partial class Form1 : Form
    {
        private bool isPaste = false;
        private bool isEnter = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void HighLightText()
        {
            string[] keywords = {"select ","insert ","into "," VALUES","create ","table ","distinct ","from ",
"where ","order ","by ","group ",
"sum "," is ","null","isnull"};

            string[] functions = { "isnull", "count", "sum" };
            string[] strings = { @"'((.|\n)*?)'" };
            string[] whiteSpace = { "\t", "\n", " " };

            this.sqlRichTextBox.SelectAll();
            this.sqlRichTextBox.SelectionColor = Color.Black;

            this.HighLightText(keywords, Color.Blue);
            this.HighLightText(functions, Color.Magenta);
            this.HighLightText(strings, Color.Red);
            this.HighLightText(whiteSpace, Color.Black);

            sqlRichTextBox.SelectionStart = sqlRichTextBox.SelectionStart + 10;
            sqlRichTextBox.SelectionLength = 0;
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            HighLightText();

        }

        private void HighLightText(string[] wordList, Color color)
        {
            foreach (string word in wordList)
            {
                Regex r = new Regex(word, RegexOptions.IgnoreCase);

                foreach (Match m in r.Matches(this.sqlRichTextBox.Text))
                {
                    this.sqlRichTextBox.Select(m.Index, m.Length);
                    this.sqlRichTextBox.SelectionColor = color;
                }
            }
        }

        private void sqlRichTextBox_TextChanged(object sender, EventArgs e)
        {
            //HighLightText();
        }

        private void sqlRichTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (isPaste)
            {
                isPaste = false;
                HighLightText();
            }
            //这里处理输入的单词
            if (isEnter)
            {
                isEnter = false;
                HighLightText();
            }
            
        }

        private void sqlRichTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                //MessageBox.Show("粘贴");
                isPaste = true;
            }

            if (e.KeyCode == Keys.Enter)
            {
                this.isEnter = true;
            }
        }

        int writeCoun = 0;
        private void button2_Click(object sender, EventArgs e)
        {
            //因为写日志已经是异步的，所以下面的侧似乎是否开线程，不影响结果
            for (int i = 0; i < 100; i++)
            {
                //System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(obj => {
                //    WriteLog("abcuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuu" + obj.ToString());
                //}));
                //writeCoun++;
                //t.Start(writeCoun);
                writeCoun++;
                WriteLog("abcuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuu" + writeCoun);
               
            }
            //System.Threading.Thread.CurrentThread.w
            MessageBox.Show("写完了");
          
        }

        private void WriteLog(string log)
        {
            //edit at 2012.10.17 改成无锁异步写如日志文件
            using (FileStream fs = new FileStream("log.txt", FileMode.Append, FileAccess.Write, FileShare.Write, 1024, FileOptions.Asynchronous))
            {
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(log + "\r\n");
                IAsyncResult writeResult = fs.BeginWrite(buffer, 0, buffer.Length,
                    (asyncResult) =>
                    {
                        FileStream fStream = (FileStream)asyncResult.AsyncState;
                        fStream.EndWrite(asyncResult);
                        //fs.Close();//这里加了会报错
                        //System.Threading.Thread.Sleep(10000);
                    },
                    fs);
                //fs.EndWrite(writeResult);//这种方法异步起不到效果
                fs.Flush();
                //fs.Close();//可以不用加
            }
        }
    }
}
