using System;
using System.Collections.Generic;

namespace TranstarAuction.Presenters.ViewInterface
{
     public interface IUserLoginView
    {
         string LoginName { get; set; }
         string LoginPwd { get; set; }
         /// <summary>
         /// 显示信息
         /// </summary>
         /// <param name="message"></param>
         void ShowMessage(string message);
         /// <summary>
         /// 跳转到主窗体
         /// </summary>
         void RedirectMainForm();
    }
}

