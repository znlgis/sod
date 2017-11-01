using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TranstarAuction.Presenters.Presenter;

namespace MonitorTerminal.UserControls
{
    /// <summary>
    /// LogManager.xaml 的交互逻辑
    /// </summary>
    public partial class LogManagerUC : BaseUserControl
    {
        ManagePresenter presenter = new ManagePresenter();
        int logTextRowCount = 0;

        public LogManagerUC()
        {
            InitializeComponent();
            this.progressBar1.Visibility = Visibility.Hidden;
        }

        public override void CloseServiceProxy()
        {
            presenter.Close();
        }

        public override void LoadData(CustomEntities.ServiceItem servicenode)
        {
            base.LoadData(servicenode);
            presenter.ServiceProxy.ServiceBaseUri = servicenode.ServiceUri;
        }

        private void btnDownLogFile_Click(object sender, RoutedEventArgs e)
        {
            string sourceFile = txtLogFile.Text;
            if (sourceFile == "")
            {
                MessageBox.Show("请输入服务器上的日志文件名！");
                return;
            }
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
            sfd.AddExtension = true;
            sfd.Filter = "日志文件|*.txt|所有文件|*.*";
            sfd.DefaultExt = "*.txt";
            if (sfd.ShowDialog().Value)
            {
                this.progressBar1.Visibility = Visibility.Visible;

                presenter.DownloadLargeFile(sourceFile, sfd.FileName, 
                    writingProcess => {
                        Dispatcher.BeginInvoke(new Action(() => 
                        { 
                            lblOptMsg.Content = string.Format("已经下载{0}% ...", writingProcess.ToString("F2"));
                            progressBar1.Value = writingProcess;
                        }
                        ));
                    }, 
                    writedLength => {
                        if (writedLength == 0)
                        {
                            MessageBox.Show("服务器读取文件异常，下载文件失败。");
                            Dispatcher.BeginInvoke(new Action(() => { 
                                lblOptMsg.Content = "服务器读取文件异常，下载文件失败";
                                this.progressBar1.Visibility = Visibility.Hidden;
                            }));
                        }
                        else
                        {
                            MessageBox.Show("下载文件完成。");
                            Dispatcher.BeginInvoke(new Action(() => { 
                                lblOptMsg.Content = "下载文件完成。";
                                this.progressBar1.Visibility = Visibility.Hidden;
                            }));
                        }
                    }
                    );
            }
            
        }

        private void btnCurrLog_Click(object sender, RoutedEventArgs e)
        {
            if ("开启日志监控" == this.btnCurrLog.Content.ToString())
            {
                this.txtLogText.Text = "";
                this.btnCurrLog.Content = "停止日志监控";
                this.btnCurrLog.Background = Brushes.Yellow;
                presenter.ServiceProxy.SubscribeCommandMessage("RemoteConsoleOutput", text =>
                {
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (this.txtLogText.Text.Length>102400)
                        {
                            this.txtLogText.Text = this.txtLogText.Text.Substring((int)(this.txtLogText.Text.Length * 0.9));
                            logTextRowCount =0;
                        }
                        this.txtLogText.Text += text;
                        this.txtLogText.ScrollToEnd();
                        logTextRowCount++;
                        this.txtEnd.Text = logTextRowCount.ToString();
                       
                    }));
                });
            }
            else
            {
                this.btnCurrLog.Content = "开启日志监控";
                this.btnCurrLog.Background = Brushes.GreenYellow;
                presenter.ServiceProxy.Close();
            }
           
        }


        private void btnSeeLogText_Click(object sender, RoutedEventArgs e)
        {
            string sourceFile = txtLogFile.Text;
            if (sourceFile == "")
            {
                MessageBox.Show("请输入服务器上的日志文件名！");
                return;
            }
            if (this.btnCurrLog.Content.ToString() == "停止日志监控")
            {
                MessageBox.Show("请先停止日志监控！");
                return;
            }
            int beginRow;//txtBegin.Text
            int endRow;
            int.TryParse(txtBegin.Text, out beginRow);
            int.TryParse(txtEnd.Text, out endRow);

            this.lblOptMsg.Content = "服务正在处理... ...";
            presenter.RequestLogFileText(str => {
                Dispatcher.BeginInvoke(new Action(()=>{
                    this.txtLogText.Text = str;
                     this.lblOptMsg.Content = "服务处理完成。";
                }));
            }, sourceFile, beginRow, endRow);

        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //this.txtLogText.Width = LogFormGrid.ActualWidth - this.dgFiles.ActualWidth - 50;
            //this.txtLogText.Height = LogFormGrid.ActualHeight - 100;
            //this.dgFiles.Height = this.txtLogText.Height;
        }

        private void BaseUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            presenter.RequestLogFileList(list =>
            {
                Dispatcher.BeginInvoke(new Action(() => {
                    var result = from item in list
                                 orderby item.LastWriteTime
                                 select new {名称=item.Name,修改日期=item.LastWriteTime,大小字节=item.Length };
                    this.dgFiles.ItemsSource = result.ToList();
                }));
            });
        }

        private void dgFiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic item = dgFiles.SelectedItem;
            if (item != null)
            {
                string text = item.名称;
                this.txtLogFile.Text = text;
                this.txtBegin.Text = "0";
                this.txtEnd.Text = "1000";
            }
        }

        private void btnGoEnd_Click(object sender, RoutedEventArgs e)
        {
            string sourceFile = txtLogFile.Text;
            if (sourceFile == "")
            {
                MessageBox.Show("请输入服务器上的日志文件名！");
                return;
            }
            this.lblOptMsg.Content = "服务正在处理... ...";
            presenter.RequestLogFileRowsCount(count =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    int begin = count - 1000;
                    if (begin < 0)
                        begin = 0;
                    this.txtBegin.Text = begin.ToString();
                    this.txtEnd.Text = count.ToString();
                    this.lblOptMsg.Content = "服务处理完成。";
                }));
            }, sourceFile);
        }
    }
}
