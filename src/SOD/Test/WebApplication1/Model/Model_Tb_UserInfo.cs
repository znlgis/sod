/*
 鏈被鐢盤WMIS 瀹炰綋绫荤敓鎴愬伐鍏?Ver 1.1)鑷姩鐢熸垚
 http://www.pwmis.com/sqlmap
 浣跨敤鍓嶈鍏堝湪椤圭洰宸ョ▼涓紩鐢?PWMIS.SqlMapper.Entity.dll
 2010-2-1 17:16:42

*/

using System;
using PWMIS.DataMap.Entity;

namespace TestWebAppModel
{
    public class Tb_UserInfo : EntityBase
    {
        public Tb_UserInfo()
        {
            TableName = "Tb_UserInfo";
            IdentityName = "ID";
            PrimaryKeys.Add("ID");

            PropertyNames = new[] { "ID", "UserName", "Sex", "IDCode", "Nation", "Stature", "Remark", "Birthday" };
            PropertyValues = new object[PropertyNames.Length];
            //AddProperty("ID", default(System.Int32));
            //AddProperty("UserName", default(System.String));
            //AddProperty("Sex", default(System.Boolean));
            //AddProperty("IDCode", default(System.String));
            //AddProperty("Nation", default(System.String));
            //AddProperty("Stature", default(System.Double));
            //AddProperty("Remark", default(System.String));
            //AddProperty("Birthday", default(System.DateTime));
        }


        public int ID
        {
            get => (int)getProperty("ID", TypeCode.Int32);
            set => setProperty("ID", value);
        }

        public string UserName
        {
            get => (string)getProperty("UserName", TypeCode.String);
            set => setProperty("UserName", value);
        }

        public bool Sex
        {
            get => (bool)getProperty("Sex", TypeCode.Boolean);
            set => setProperty("Sex", value);
        }

        public string IDCode
        {
            get => (string)getProperty("IDCode", TypeCode.String);
            set => setProperty("IDCode", value);
        }

        public string Nation
        {
            get => (string)getProperty("Nation", TypeCode.String);
            set => setProperty("Nation", value);
        }

        public double Stature
        {
            get => (double)getProperty("Stature", TypeCode.Double);
            set => setProperty("Stature", value);
        }

        public string Remark
        {
            get => (string)getProperty("Remark", TypeCode.String);
            set => setProperty("Remark", value);
        }

        public DateTime Birthday
        {
            get => (DateTime)getProperty("Birthday", TypeCode.DateTime);
            set => setProperty("Birthday", value);
        }


        #region ISerializable 鎴愬憳

        //void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        //{
        //    //base.GetObjectData(info, context);
        //}


        //private Tb_UserInfo(SerializationInfo info, StreamingContext context)
        //{
        //    //PropertyNames = (string[])info.GetValue("names", typeof(string[]));
        //    //PropertyValues = (object[])info.GetValue("values", typeof(object[]));
        //    //_identity = info.GetString("_identity");
        //    //_pks = (List<string>)info.GetValue("_pks", typeof(List<string>));
        //    //_tableName = info.GetString("_tableName");
        //}

        #endregion
    }
}