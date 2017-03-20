using PWMIS.EnterpriseFramework.Common;
using PWMIS.EnterpriseFramework.Common.Encrypt;
using PWMIS.EnterpriseFramework.Service.Basic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using TranstarAuction.Model;
using TranstarAuction.Presenters.ViewInterface;


namespace TranstarAuction.Presenters.Presenter
{
    public class LoginPresenter : PresenterBase
    {
        private const string secretkey = "LocationStoreUserInfo";
        //public List<LocationStoreUserInfoModel> storeusers;
        public BindingList<LocationStoreUserInfoModel> storeusers;
        public IUserLoginView View;

        //可用的服务器列表
        //public List<ServiceRegModel> ActiveServerList;

        public LoginPresenter(IUserLoginView view)
        {
            this.View = view;
            LoadLocationStoreUserInfo();
        }

        public void Login()
        {
            if (string.IsNullOrEmpty(View.LoginName))
            {
                View.ShowMessage("登录名称不能为空！");
                return;
            }
            if (string.IsNullOrEmpty(View.LoginPwd))
            {
                View.ShowMessage("登录密码不能为空！");
                return;
            }

            //string url = "Service://User/Login/System.String=aaa&System.String=123";
             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "User";
            request.MethodName = "Login2";
            request.Parameters = new object[] { EncryptHelper.DesEncrypt(View.LoginName), EncryptHelper.DesEncrypt(View.LoginPwd) };

            //base.ServiceProxy.RequestService<UserLoginInfoModel>(request,  DataType.Json, loginInfo =>
            base.ServiceProxy.RequestService<UserLoginInfoModel>(request,  DataType.Json, loginInfo =>
            {
                if (loginInfo == null)
                {
                    View.ShowMessage("对不起，网络繁忙，请稍后重试！");
                }
                else
                {
                    if (loginInfo.LoginResult)
                    {
                        if (!string.IsNullOrEmpty(loginInfo.ServiceUri))
                            PresenterBase.ServiceUri = loginInfo.ServiceUri;
                        PresenterBase.CurrentUser = loginInfo.User;
                        View.RedirectMainForm();
                    }
                    else
                    {
                        View.ShowMessage(loginInfo.LoginResultMessage);
                    }
                }
            });
        }

        protected override void OnServiceProxyError(string errorMessage)
        {
            View.ShowMessage("登录失败，原因：可能是网络错误，请检查防火墙设置！");
        }

        void MyAction(string para)
        {
            View.ShowMessage(para);
        }

        public void LoadLocationStoreUserInfo()
        {
            storeusers = new BindingList<LocationStoreUserInfoModel>(LocationStoreUserInfoModel.LoadLocationStoreUserInfoList());
        }

        public void SaveLocationStoreUserInfo(LocationStoreUserInfoModel userInfo)
        {
            userInfo.SaveLocationStoreUserInfo();
        }

        public void DelLocationStoreUserInfo(LocationStoreUserInfoModel userInfo)
        {
            userInfo.DeleteLocationStoreUserInfo();
        }
    }

    public class LoginCompleteArgs : EventArgs
    {
        public bool IsLogin { get; set; }
        public LocationStoreUserInfoModel LoginInfo { get; set; }
    }
}
