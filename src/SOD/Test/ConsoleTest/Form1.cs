using System;
using System.Windows.Forms;

namespace ConsoleTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Text = textBox1.Text;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Text = textBox1.Text;
            Close();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Text = textBox1.Text;
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            textBox1.Focus();
        }
    }
}