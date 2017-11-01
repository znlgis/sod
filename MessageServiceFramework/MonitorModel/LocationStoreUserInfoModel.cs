using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using PWMIS.EnterpriseFramework.Common;

namespace TranstarAuction.Model
{
    [Serializable]
    public class LocationStoreUserInfoModel 
    {
        public string Account { get; set; }
        public string Password { get; set; }
        public bool IsRemember { get; set; }
        public bool IsAutoLogin { get; set; }
        public DateTime LastLoginTime { get; set; }

        public LocationStoreUserInfoModel()
        {
            Account = string.Empty;
            Password = string.Empty;
            IsRemember = false;
            IsAutoLogin = false;
        }

        public LocationStoreUserInfoModel(string account)
        {
            Account = account;
            LoadLocationStoreUserInfo();
        }

        public static List<LocationStoreUserInfoModel> LoadLocationStoreUserInfoList()
        {
            List<LocationStoreUserInfoModel> listlsui = new List<LocationStoreUserInfoModel>();
            try
            {
                object obj = ObjFileStore.LoadObj(CommonMethods.GetStorePath("data.bin"));
                LocationStoreUserInfoModel[] sui = obj as LocationStoreUserInfoModel[];
                if (sui != null)
                {
                    listlsui = new List<LocationStoreUserInfoModel>(sui);
                    listlsui.Sort((o1, o2) => o1.LastLoginTime > o2.LastLoginTime ? 1 : 0);
                }
            }
            catch
            {
                return listlsui;
            }
            return listlsui;
        }

        protected void LoadLocationStoreUserInfo()
        {
            List<LocationStoreUserInfoModel> storeusers = LoadLocationStoreUserInfoList();
            LocationStoreUserInfoModel exists = storeusers.Find(ui => string.Compare(ui.Account, Account) == 0);
            if (exists != null)
            {
                Account = exists.Account;
                Password = exists.Password;
                IsRemember = exists.IsRemember;
                IsAutoLogin = exists.IsAutoLogin;
                LastLoginTime = exists.LastLoginTime;
            }
        }

        public void DeleteLocationStoreUserInfo()
        {
            if (string.IsNullOrEmpty(Account)) return;
            List<LocationStoreUserInfoModel> storeusers = LoadLocationStoreUserInfoList();
            LocationStoreUserInfoModel exists = storeusers.Find(ui => string.Compare(ui.Account, Account) == 0);
            if (exists != null)
            {
                storeusers.Remove(exists);
            }

            try
            {
                ObjFileStore.SaveObj(CommonMethods.GetStorePath("data.bin"), storeusers.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SaveLocationStoreUserInfo()
        {
            if (string.IsNullOrEmpty(Account)) return;
            List<LocationStoreUserInfoModel> storeusers = LoadLocationStoreUserInfoList();
            if (IsAutoLogin) IsRemember = true;
            LocationStoreUserInfoModel exists = storeusers.Find(ui => string.Compare(ui.Account, Account) == 0);
            if (exists != null)
            {
                storeusers.Remove(exists);
            }
            storeusers.Add(this);

            try
            {
                ObjFileStore.SaveObj(CommonMethods.GetStorePath("data.bin"), storeusers.ToArray());
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
