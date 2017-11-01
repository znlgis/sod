using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using PWMIS.EnterpriseFramework.Common;
using PWMIS.EnterpriseFramework.Common.Encrypt;

namespace PWMIS.EnterpriseFramework.Service.Runtime
{
    /// <summary>
    /// 管理端服务类
    /// </summary>
    public class ManageService:ServiceBase
    {
        /// <summary>
        /// 连接服务,仅允许内网IP连接本服务
        /// </summary>
        /// <param name="identityName">标识名称，例如用户的域登陆名称</param>
        /// <returns>是否允许连接管理服务</returns>
        public bool Connect(string identityName)
        {
            string logFileName = base.CurrentContext.Host.LogDirectory + "ManageLog.txt";
            WriteLogFile(logFileName, identityName+" 连接服务器；");
            string ip = base.CurrentContext.Request.ClientIP;
            IpUtility ipu = new IpUtility();
            if (!ipu.IsInner(ip))
            {
                WriteLogFile(logFileName, ip+" [拒绝]该IP连接服务器；");
                return false;
            }
            WriteLogFile(logFileName, ip + " [允许]该IP连接服务器；");
            return true;
        }
        /// <summary>
        /// 是否是管理员，只有管理密码正确，才是管理员
        /// </summary>
        /// <param name="password">管理密码，默认为空密码</param>
        /// <returns></returns>
        public bool IsManager(string password)
        {
            string localPwd = getLocalPassword();
            if (localPwd == "")
                return true;

            if (EncryptHelper.MD5Encrypt(password) == localPwd)
                return true;
            else
                return false;

        }
        /// <summary>
        /// 修改管理密码，只有旧密码正确，且新密码不为空，则可修改成功
        /// </summary>
        /// <param name="oldPwd">旧密码</param>
        /// <param name="newPwd">新密码</param>
        /// <returns></returns>
        public bool ChangePassword(string oldPwd, string newPwd)
        {
            if (string.IsNullOrEmpty(newPwd))
                return false;

            string localPwd = getLocalPassword();
            if ((localPwd=="" && oldPwd=="") ||  EncryptHelper.MD5Encrypt(oldPwd) == localPwd)
            {
                localPwd = EncryptHelper.MD5Encrypt(newPwd);
                string fullFileName = getPwdFileName();
                string folder= Path.GetDirectoryName(fullFileName);
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                File.WriteAllText(fullFileName, localPwd);
                return true;
            }
            return false;
        }

        string getLocalPassword()
        {
            string fullFileName = getPwdFileName();
            //第一次使用，没有密钥文件，默认为空密码
            if (File.Exists(fullFileName))
            {
                string text = File.ReadAllText(fullFileName);
                return text;
            }
            return "";
        }
        string getPwdFileName()
        {
            string fileName = "En" + base.CurrentContext.Host.RegServerPort;
            string fullFileName = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "ManageService", fileName);
            return fullFileName;
        }

        void WriteLogFile(string fileName, string logMsg)
        {
            try
            {
                string text = string.Format("\r\n[{0}] {1}\r\n", DateTime.Now.ToString(), logMsg);
                System.IO.File.AppendAllText(fileName, text);
            }
            catch
            {

            }
        }
    }
}
