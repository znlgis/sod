using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SimpleAccessWinForm
{
    class UserTypeInfo
    {
        public string UserTypeName { get; set; }
        public int UserTypeCode { get; set; }
    }

    class UserTypeInfoDataSource
    {

        public UserTypeInfoDataSource()
        {

        }


        public static void InitDataSource(BindingSource bs)
        {
            bs.Add(new UserTypeInfo() { UserTypeName = "团体用户", UserTypeCode = 2 });
            bs.Add(new UserTypeInfo() { UserTypeName = "VIP用户", UserTypeCode = 4 });
            bs.Add(new UserTypeInfo() { UserTypeName = "普通用户", UserTypeCode = 8 });
        }
    }
}
