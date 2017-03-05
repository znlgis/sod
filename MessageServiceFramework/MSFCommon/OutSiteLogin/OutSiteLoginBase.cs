using System;
using System.Collections.Generic;
using System.Text;

namespace TranstarAuction.Common
{
    /// <summary>
    /// 外站登录处理基础类 by dengtaihua,2011.11.15
    /// </summary>
    public abstract class OutSiteLoginBase
    {
        private string identityName;//当前登录的用户名

        /// <summary>
        /// 以一个登录的用户标示初始化本类
        /// </summary>
        /// <param name="identityName"></param>
        public OutSiteLoginBase(string identityName)
        {
            this.identityName = identityName;
            this.PrivateKey = "IComefromYouxinpai";
        }

        #region 属性

        string _localLoginPage = "http://paimai.ucar.cn/login.aspx";
        /// <summary>
        /// 本站的登录页面地址
        /// </summary>
        public string LocalLoginPage
        {
            get
            {
                return _localLoginPage;
            }
            set
            {
                _localLoginPage = value;
            }
        }

        string _privateKey = "";
        /// <summary>
        /// 跨站登录的私钥
        /// </summary>
        public string PrivateKey
        {
            get
            {
                return _privateKey;
            }
            set
            {
                _privateKey = value.Substring(0, 16);
            }
        }

        string _loginOkPage = "";
        /// <summary>
        /// 到外站登录成功后，要跳转到的页面
        /// </summary>
        public string LoginOKPage
        {
            get
            {
                return _loginOkPage;
            }
            set
            {
                _loginOkPage = value;
            }
        }

        /// <summary>
        /// 出错信息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 登录到外站的验证跨站登录的Url
        /// </summary>
        public string OutSiteLoginUrl { get; set; }
        /// <summary>
        /// 外站默认的登录页
        /// </summary>
        public string OutSiteDefaultLoginUrl { get; set; }

        #endregion

        /// <summary>
        /// 获取车商通／优信拍用户名
        /// </summary>
        /// <param name="para">可选的参数</param>
        /// <returns></returns>
        protected abstract string GetTranstarUser(object para);

        /// <summary>
        /// 检查外部用户是否在本地存在，如果存在，则返回本地对照的登录用户名
        /// </summary>
        /// <param name="transtarUser">外部用户名（如车商通）</param>
        /// <returns>如果不存在，返回空值</returns>
        protected abstract string GetLocalUserFromDB(string userName);

        /// <summary>
        /// 跳转到指定的网页
        /// </summary>
        /// <param name="url"></param>
        private void Redirect(string url)
        {
            if (!string.IsNullOrEmpty(url))
                System.Diagnostics.Process.Start(url);

        }

        /// <summary>
        /// 登录到外站,如果登录成功，将由外站登录页跳转到　LoginOKPage　指定的页面
        /// </summary>
        public void OutSiteLogin()
        {
            if (string.IsNullOrEmpty(this.identityName))
            {
                 Redirect(this.LocalLoginPage);
            }
            else
            {
                string key = this.CreateOutSiteLoginString(this.identityName);
                if (key == "")
                {
                    //先检查当前登录用户是否在对照表中，如不存在，则跳转到车商通登录页
                     Redirect(this.OutSiteDefaultLoginUrl);
                }
                else
                {
                    string url = this.OutSiteLoginUrl + "?key=" + key+"&Redirect="+this.LoginOKPage;
                    //url = HttpContext.Current.Server.UrlPathEncode(url);
                     Redirect(url);
                }

            }

        }

        /// <summary>
        /// 接受登录，如果验证来自外部的登录信息成功，则执行本地的登录方法，最后跳转到登录成功页面
        /// </summary>
        /// <param name="localLogin">本地的登录方法</param>
        public void AccecptLogin(Action<string> localLogin)
        {
            //string key = HttpContext.Current.Request.QueryString["key"];
            //if (string.IsNullOrEmpty(key))
            //{
            //    this.ErrorMessage = "没有key参数";
            //    return;
            //}
            //string encryptedText = key.Replace(" ", "+");
            //string source = CryptoHelper.Decrypt(encryptedText, this.PrivateKey);
            ////检查登陆信息是否合法
            //string message = "";
            //if (CheckOutSiteLoginString(source, out message))
            //{
            //    string uname = message;//得到本站的登录用户名，例如，对优信拍而言，这里将得到优信拍的用户名
            //    localLogin(uname);
            //    HttpContext.Current.Response.Redirect(this.LoginOKPage);
            //}
            //else
            //{
            //    //登录失败，到优信拍的登录首页
            //    this.ErrorMessage = message;
            //    HttpContext.Current.Response.Redirect(this.LocalLoginPage);
            //}
        }

        #region 私有方法
        /// <summary>
        /// 根据优信拍用户名生成到外站的登录字符串
        /// </summary>
        /// <param name="auctionUser">优信拍用户名</param>
        /// <returns></returns>
        string CreateOutSiteLoginString(string auctionUser)
        {
            //获取车商通／优信拍用户名
            string transtarUser = this.GetTranstarUser(auctionUser);
            if (transtarUser != "")
            {
                string source = "youxinpai " + transtarUser + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                return CryptoHelper.Encrypt(source, this.PrivateKey);
            }
            else
            {
                return "";
            }
        }



        /// <summary>
        /// 检查外部站点的登录信息串是否合法
        /// </summary>
        /// <param name="source">登录信息串</param>
        /// <param name="message">处理结果，如果成功，则返回本地系统的登录用户名，否则返回具体的错误信息</param>
        /// <returns>检查是否成功</returns>
        bool CheckOutSiteLoginString(string source, out string message)
        {
            message = "";
            string[] arrTemp = source.Split(' ');
            if (arrTemp[0] != "youxinpai")
            {
                message = "前缀信息错误";
                return false;
            }

            string userName = arrTemp[1];//车商通用户名
            string localName = GetLocalUserFromDB(userName);
            if (string.IsNullOrEmpty(localName))
            {
                message = "车商通用户名不存在";
                return false;
            }
            else
            {
                message = localName;
            }

            DateTime atTime;
            string timeString = arrTemp[2] + " " + arrTemp[3];
            if (!DateTime.TryParse(timeString, out atTime))
            {
                message = "时间解析错误";
                return false;
            }
            if (DateTime.Now.Subtract(atTime).TotalMinutes > 10)
            {
                message = "登录超时";
                return false;
            }
            return true;

        }
        #endregion


    }//end class
}
