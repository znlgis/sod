using System;
using System.Collections.Generic;
using System.Text;

namespace MessagePublishService
{
    /// <summary>
    /// 消息相关的系统用户信息
    /// </summary>
    public class MessageUser
    {
        public string Name { get; set; }
        public string Password { get; set; }
        /// <summary>
        /// 客户的硬盘号
        /// </summary>
        public string HID { get; set; }
        /// <summary>
        /// 是否已经验证
        /// </summary>
        public bool Validated { get; set; }

        /// <summary>
        /// 根据协议的消息字符串，获取用户信息。
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static MessageUser GetUserFromMessageString(string message)
        {
            string[] arr=message.Split(';');
            if (arr.Length >= 3)
                return new MessageUser() { Name = arr[0], Password = arr[1], HID=arr[2] };
            else
                return null;

        }
       
    }
}
