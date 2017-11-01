using System;
using System.Collections.Generic;
using System.Text;

using System.Xml.Serialization;
using System.IO;
using Microsoft.Win32;

namespace TranstarAuction.Model
{
    public enum CloseOptions { MIN, EXIT }
    public enum UserStoreOptions { RUN,DOCUMENT,CUSTOMER }

    [Serializable]
    public class UserSettingModel
    {
        [XmlIgnore]
        public LocationStoreUserInfoModel lsuiModel { get; set; }//本地用户信息（登录列表项）

        /// <summary>
        /// 默认构造函数，只为反序列化操作提供
        /// </summary>
        public UserSettingModel() { lsuiModel = new LocationStoreUserInfoModel(); }
        public UserSettingModel(LocationStoreUserInfoModel lsuim)
        {
            lsuiModel = lsuim;
        }
        /// <summary>
        /// 用户帐号
        /// </summary>
        public string Account
        {
            get { return lsuiModel.Account; }
            set { lsuiModel.Account = value; }
        }

        /// <summary>
        /// 记住密码
        /// </summary>
        public bool IsRemember
        {
            get { return lsuiModel.IsRemember; }
            set { lsuiModel.IsRemember = value; }
        }

        /// <summary>
        /// 自动登录
        /// </summary>
        public bool IsAutoLogin
        {
            get { return lsuiModel.IsAutoLogin; }
            set { lsuiModel.IsAutoLogin = value; }
        }

        /// <summary>
        /// 开机启动
        /// </summary>
        public bool IsAutoRun { get; set; }
        /// <summary>
        /// 顶端显示
        /// </summary>
        public bool IsTopMost { get; set; }
        /// <summary>
        /// 存储目录
        /// </summary>
        public string StorePath { get; set; }
        /// <summary>
        /// 退出选项
        /// </summary>
        public CloseOptions ExitOrMin { get; set; }
        /// <summary>
        /// 存储目录选项
        /// </summary>
        public UserStoreOptions StoreDirectory { get; set; }
        /// <summary>
        /// 退出提醒
        /// </summary>
        public bool IsRemind { get; set; }
        /// <summary>
        /// 保存用户配置
        /// </summary>
        public void SaveUserSetting(string ExecutablePath)
        {
            SetAutoRun(ExecutablePath);
            ConfigurationManager.Set(CommonMethods.GetStorePath(Account + ".config"), this);
        }
        /// <summary>
        /// 加载用户配置
        /// </summary>
        /// <returns></returns>
        public UserSettingModel LoadUserSetting(string ExecutablePath)
        {
            UserSettingModel u = ConfigurationManager.Get(CommonMethods.GetStorePath(Account + ".config")) as UserSettingModel;
            if (u != null)
                u.GetAutoRun(ExecutablePath);
            else
                u = this;
            return u;
        }
        /// <summary>
        /// 加载用户配置，并使用lsui进行合并
        /// </summary>
        /// <returns></returns>
        public UserSettingModel LoadUserSetting(string ExecutablePath, LocationStoreUserInfoModel lsui)
        {
            UserSettingModel u = ConfigurationManager.Get(CommonMethods.GetStorePath(Account + ".config")) as UserSettingModel;
            if (u != null)
            {
                u.GetAutoRun(ExecutablePath);
                u.lsuiModel = lsui;
            }
            else
                u = this;
            return u;
        }
        /// <summary>
        /// 获取自动运行状态（注册表）
        /// </summary>
        /// <param name="ExecutablePath">应用程序执行目录</param>
        private void GetAutoRun(string ExecutablePath)
        {
            string KJLJ = ExecutablePath;
            string newKJLJ = KJLJ.Substring(KJLJ.LastIndexOf("\\") + 1);
            RegistryKey loca_chek = Registry.CurrentUser;
            try
            {
                RegistryKey run_Check = loca_chek.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                object kjlj = run_Check.GetValue(newKJLJ);
                if (kjlj != null)
                {
                    if (kjlj.ToString().ToLower() != "false")
                    {
                        IsAutoRun = true;
                    }
                    else
                    {
                        IsAutoRun = false;
                    }
                }
            }
            catch
            {
                //MessageBox.Show("注册表访问失败！请检查注册表权限！");
            }
        }

        /// <summary>
        /// 设置或取消开机启动
        /// </summary>
        /// <param name="ExecutablePath">应用程序执行目录</param>
        private void SetAutoRun(string ExecutablePath)
        {
            string KJLJ = ExecutablePath;
            if (!System.IO.File.Exists(KJLJ))//判断指定文件是否存在
                return;
            string newKJLJ = KJLJ.Substring(KJLJ.LastIndexOf("\\") + 1);
            try
            {
                RegistryKey Rkey =
                    Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

                if (Rkey == null)
                    Rkey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");

                if (IsAutoRun == false) Rkey.SetValue(newKJLJ, false);//取消开机运行
                else Rkey.SetValue(newKJLJ, KJLJ);//设置开机运行
                Rkey.Close();
            }
            catch(Exception ex)
            {
                //throw ex; 
            }
        }
    }
    public sealed class ConfigurationManager
    {
        private static UserSettingModel m_humanResource = null;
        private ConfigurationManager() { }

        public static UserSettingModel Get(string path)
        {
            if (!File.Exists(path)) return null;
            if (m_humanResource == null)
            {
                FileStream fs = null;
                try
                {
                    XmlSerializer xs = new XmlSerializer(typeof(UserSettingModel));
                    fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                    m_humanResource = (UserSettingModel)xs.Deserialize(fs);
                    fs.Close();
                }
                catch
                {
                    if (fs != null)
                        fs.Close();
                    //throw new Exception("Xml deserialization failed!");
                }
            }
            return m_humanResource;
        }

        public static void Set(string path, UserSettingModel humanResource)
        {
            if (humanResource == null)
                throw new Exception("Parameter humanResource is null!");

            FileStream fs = null;
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(UserSettingModel));
                fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
                xs.Serialize(fs, humanResource);
                m_humanResource = null;
                fs.Close();
            }
            catch
            {
                if (fs != null)
                    fs.Close();
                //throw new Exception("Xml serialization failed!");
            }
        }
    }

}
