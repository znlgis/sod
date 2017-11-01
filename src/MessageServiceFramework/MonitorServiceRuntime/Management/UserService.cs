using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TranstarAuction.Model;
using TranstarAuctionBIZ;
using TranstarAuction.Common;
using TranstarAuction.Service.Basic;
using TranstarAuction.Service.Runtime;
using TranstarAuction.Repository.Entitys;
using TranstarAuction.Service.Runtime.Principal;
using TranstarAuction.Service.Group;
using TranstarAuction.Service.Client.Model;


namespace TranstarAuction.Service
{
    public class UserService : IService
    {
        private IServiceContext currentContext;

        public UserService() { }
        public UserService(UserModel user)
        {
            this.User = user;
        }
        public UserModel User { get; set; }

        //Url: //UserService/Login/string:LoginName;string:LoginPwd
        public UserLoginInfoModel Login()
        {
            string message;
            UserBIZ biz = new UserBIZ();
            UserModel user = biz.Login(this.User.LoginName, EncryptPwd(this.User.LoginPwd), out message);
            UserLoginInfoModel model = new UserLoginInfoModel();
            model.User = user;
            model.LoginResult = user != null;
            model.LoginResultMessage = message;


            return model;
        }

        public UserLoginInfoModel LoginByUserModel(UserModel user)
        {
            this.User = user;
            return this.Login();
        }

        /// <summary>
        /// 处理密码
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        private string EncryptPwd(string pwd)
        {
            return EncryptHelper.DesEncrypt(pwd);
        }

        /// <summary>
        /// 在系统中检查用户登录的凭据是否还在
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public bool CheckUserIdentity()
        {
            //假设服务上有300个用户，没处理一次，需要9秒左右，加上轮询间隔，总共不超过15秒
            //执行该方法的总的时间必须 小于每个凭据的过期时间
            currentContext.BatchInterval = 5000;
            currentContext.ParallelExecute = false;

            System.Diagnostics.Debug.WriteLine("--------------CheckUserIdentity Client IP:{0},{1}", this.currentContext.Request.ClientIP, this.currentContext.Request.ClientIdentity);
            ServiceAuthentication auth = new ServiceAuthentication(this.currentContext);
            ServiceIdentity user = auth.GetIdentity();
            return user != null;
        }

        /// <summary>
        /// 获取系统中的所有服务凭据
        /// </summary>
        /// <returns></returns>
        public List<ServiceIdentity> GetAllIdentitys()
        {
            currentContext.BatchInterval = 30000;//该值必须大于凭据的超时时间
            return ServiceIdentityContainer.Instance.GetAllIdentitys();
        }

        /// <summary>
        /// 获取当前服务器上的所有服务凭据
        /// </summary>
        /// <returns></returns>
        public List<ServiceIdentity> GetIdentitys()
        {
            currentContext.BatchInterval = 30000;//该值必须大于凭据的超时时间
            currentContext.SendEmptyResult = true;
            string currUri = currentContext.Host.GetUri();
            List<ServiceIdentity> all = ServiceIdentityContainer.Instance.GetAllIdentitys();
            List<ServiceIdentity> result = all.FindAll(p => p.Uri == currUri);
            return result;
        }

        /// <summary>
        /// 注销登录
        /// </summary>
        /// <returns></returns>
        public bool Logout()
        {
            ServiceAuthentication auth = new ServiceAuthentication(this.currentContext);
            ServiceIdentity user = auth.GetIdentity();
            if (user != null)
            {
                UserBIZ biz = new UserBIZ();
                biz.SaveLogoutLog(user.Id);
                return auth.SignOut(user);
            }
            return false;
        }

        public bool ProcessRequest(IServiceContext context)
        {
            this.currentContext = context;

            if (context.Request.MethodName.Substring(0, 5) == "Login")
            {
                this.User = new UserModel();
                this.User.LoginName = (string)context.Request.Parameters[0];
                this.User.LoginPwd = (string)context.Request.Parameters[1];
                //Login2 方法为加密的登陆方法
                if (context.Request.MethodName == "Login2")
                {
                    this.User.LoginName = EncryptHelper.DesDecrypt(this.User.LoginName);
                    this.User.LoginPwd = EncryptHelper.DesDecrypt(this.User.LoginPwd);
                }

                UserLoginInfoModel result = Login();
                if (result.LoginResult)
                {
                    //分配服务地址
                    DispatchService disp = new DispatchService();
                    disp.CurrentContext = context;
                    ServiceRegModel server = disp.DoDispatch();
                    //在外网测试有问提，暂时屏蔽 2012.5.24
                    result.ServiceUri = server.GetUri();

                    //如果登录成功，设置登录凭据
                    ServiceIdentity newUser = new ServiceIdentity();
                    newUser.Id = result.User.UserID;
                    newUser.Name = result.User.LoginName;
                    newUser.Expire = new TimeSpan(0, 0, 20); //20秒过期，客户端必须订阅CheckUserIdentity 方法
                    newUser.IsAuthenticated = true;
                    newUser.Uri = result.ServiceUri;
                    System.Diagnostics.Debug.WriteLine("--------------newUser.Uri={0} ; Client IP:{1}", newUser.Uri, context.Request.ClientIP);

                    ServiceAuthentication auth = new ServiceAuthentication(context);
                    //如果相同的用户账号已经在别的机器登录过，则取消之前的登录凭据
                    ServiceIdentity oldUser = auth.FindIdentity(newUser);
                    if (oldUser != null)
                        auth.SignOut(oldUser);

                    auth.Authenticate(newUser);
                   
                }
                context.Response.WriteJsonString(result);
                
                return false;
            }
            context.SessionRequired = true;
            return true;
        }


        public void CompleteRequest(IServiceContext context)
        {
            if (context.Request.MethodName == "CheckUserIdentity")
            {
                if (context.Response.AllText == "true")
                    context.Response.Clear();
            }
        }


        public bool IsUnSubscribe
        {
            get { return false; }
        }
    }
}
