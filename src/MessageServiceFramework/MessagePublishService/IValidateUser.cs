using MessagePublishService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PWMIS.EnterpriseFramework.Message.PublishService
{
    /// <summary>
    /// 访问消息服务框架的用户验证接口
    /// </summary>
    public interface IValidateUser
    {
        /// <summary>
        /// 验证用户。如果验证不通过，将拒绝此用户访问服务。
        /// </summary>
        /// <param name="user">访问消息服务框架的用户信息，这些信息在服务代理类上可以设置</param>
        /// <returns>验证是否通过</returns>
        bool Validate(MessageUser user);
    }

    /// <summary>
    /// 示例的简单用户验证类
    /// </summary>
    public class SimpleMessageUserValidater : IValidateUser
    {
        public SimpleMessageUserValidater()
        {
            this.UserName = "PDF.NET.MSF";
            this.Password = "20111230";
        }
        public string UserName { get; set; }
        public string Password { get; set; }

        public bool Validate(MessageUser user)
        {
            return user.Name == this.UserName && user.Password == this.Password;
        }
    }
}
