using System;
using System.Collections.Generic;

using TranstarAuction.Model;
using Microsoft.Win32;

namespace TranstarAuction.Presenters.Presenter
{
    public class SysConfigPresenter
    {
        #region 绑定属性：ViewModel
        /// <summary>
        /// 记住关闭选项
        /// </summary>
        public bool IsRemind
        {
            get { return UsModel.IsRemind; }
            set { UsModel.IsRemind = value; OnModifyNotify(); }
        }
        /// <summary>
        /// 关闭时退出
        /// </summary>
        public bool IsExit
        {
            get { return usModel.ExitOrMin == CloseOptions.EXIT; }
            set
            {
                if (value)
                {
                    usModel.ExitOrMin = CloseOptions.EXIT;
                    OnModifyNotify();
                }
            }
        }
        /// <summary>
        /// 关闭时最小化
        /// </summary>
        public bool IsMin
        {
            get
            { return usModel.ExitOrMin == CloseOptions.MIN; }
            set
            {
                if (value)
                {
                    usModel.ExitOrMin = CloseOptions.MIN;
                    OnModifyNotify();
                }
            }
        }

        /// <summary>
        /// 保存安装目录
        /// </summary>
        public bool IsRunPath
        {
            get { return usModel.StoreDirectory == UserStoreOptions.RUN; }
            set
            {
                if (value)
                {
                    usModel.StoreDirectory = UserStoreOptions.RUN;
                    OnModifyNotify();
                }
            }
        }
        /// <summary>
        /// 保存我的文档目录
        /// </summary>
        public bool IsDocumentPath
        {
            get { return usModel.StoreDirectory == UserStoreOptions.DOCUMENT; }
            set
            {
                if (value)
                {
                    usModel.StoreDirectory = UserStoreOptions.DOCUMENT;
                    OnModifyNotify();
                }
            }
        }
        /// <summary>
        /// 保存自定义目录
        /// </summary>
        public bool IsCustomerPath
        {
            get { return usModel.StoreDirectory == UserStoreOptions.CUSTOMER; }
            set
            {
                if (value)
                {
                    usModel.StoreDirectory = UserStoreOptions.CUSTOMER;
                    OnModifyNotify();
                }
            }
        }

        /// <summary>
        /// 开机启动
        /// </summary>
        public bool IsAutoRun
        {
            get { return UsModel.IsAutoRun; }
            set { UsModel.IsAutoRun = value; OnModifyNotify(); }
        }
        /// <summary>
        /// 顶端显示
        /// </summary>
        public bool IsTopMost 
        {
            get { return UsModel.IsTopMost; }
            set { UsModel.IsTopMost = value; OnModifyNotify(); }
        }
        /// <summary>
        /// 存储目录
        /// </summary>
        public string StorePath
        {
            get { return UsModel.StorePath; }
            set { UsModel.StorePath = value; OnModifyNotify(); }
        }

        /// <summary>
        /// 记住密码
        /// </summary>
        public bool IsRemember
        {
            get { return UsModel.IsRemember; }
            set 
            { 
                UsModel.IsRemember = value;
                if (value)
                    OnModifyNotify();
                else
                    IsAutoLogin = false;
            }
        }

        /// <summary>
        /// 自动登录
        /// </summary>
        public bool IsAutoLogin
        {
            get { return UsModel.IsAutoLogin; }
            set
            {
                UsModel.IsAutoLogin = value;
                if (value)
                    IsRemember = true;
                else
                    OnModifyNotify(); 
            }
        }
        #endregion
        private UserSettingModel usModel;
        /// <summary>
        /// 用户配置
        /// </summary>
        public UserSettingModel UsModel
        {
            get { return usModel; }
            set { usModel = value; }
        }

        private LocationStoreUserInfoModel lsuiModel;
        private string executablePath = string.Empty;

        public event Action<object> ModifyNotify;
        private void OnModifyNotify()
        {
            if (ModifyNotify != null)
                ModifyNotify(this);
        }

        public SysConfigPresenter(LocationStoreUserInfoModel lsui, string execpath)
        {
            lsuiModel = lsui;
            executablePath = execpath;
            usModel = new UserSettingModel(lsuiModel).LoadUserSetting(executablePath, lsui);
        }

        /// <summary>
        /// 保存用户配置（包括登录列表项与用户配置）
        /// </summary>
        public void SaveLocationStoreUserInfo()
        {
            lsuiModel.SaveLocationStoreUserInfo();
            usModel.SaveUserSetting(executablePath);
        }

        /// <summary>
        /// 加载用户配置
        /// </summary>
        public void LoadUserSetting()
        {
            UsModel = usModel.LoadUserSetting(executablePath);
        }

        /// <summary>
        /// 保存本地用户信息（登录列表项）
        /// </summary>
        /// <param name="isremember">是否记住密码</param>
        /// <param name="isautologin">是否自动登录</param>
        public void SaveLocationStoreUserInfo(bool isremember, bool isautologin)
        {
            lsuiModel.IsRemember = isremember;
            lsuiModel.IsAutoLogin = isautologin;
            lsuiModel.SaveLocationStoreUserInfo();
        }

    }
}
