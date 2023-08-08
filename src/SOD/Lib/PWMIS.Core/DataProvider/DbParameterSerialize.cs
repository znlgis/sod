using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace PWMIS.DataProvider.Data
{
    /// <summary>
    ///     数据参数序列化类
    /// </summary>
    public class DbParameterSerialize
    {
        /// <summary>
        ///     将数据查询参数序列化
        /// </summary>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static string Serialize(IDataParameter[] paras)
        {
            if (paras == null || paras.Length == 0)
                return string.Empty;
            var list = new List<MyDbParameter>();
            foreach (var para in paras)
                if (para == null)
                {
                    list.Add(null);
                }
                else
                {
                    var para1 = (IDbDataParameter)para;
                    var item = new MyDbParameter();
                    item.Name = para1.ParameterName;
                    item.Length = para1.Size;
                    item.Value = para1.Value == DBNull.Value ? null : para1.Value;
                    item.ParaDbType = para1.DbType;
                    list.Add(item);
                }

            var sb = new StringBuilder();
            var xw = XmlWriter.Create(sb);
            var xs = new XmlSerializer(typeof(List<MyDbParameter>));
            xs.Serialize(xw, list);
            var strEntity = sb.ToString();
            return strEntity;
        }

        /// <summary>
        ///     将符合本类序列化结果的字符串执行反序列化
        /// </summary>
        /// <param name="input"></param>
        /// <param name="db">数据访问对象</param>
        /// <returns></returns>
        public static IDataParameter[] DeSerialize(string input, AdoHelper db)
        {
            var xs = new XmlSerializer(typeof(List<MyDbParameter>));
            var desObj = xs.Deserialize(new StringReader(input));
            var list = (List<MyDbParameter>)desObj;
            var result = new List<IDataParameter>();
            foreach (var item in list)
                if (item == null)
                {
                    result.Add(null);
                }
                else
                {
                    var para = (IDbDataParameter)db.GetParameter(item.Name, item.ParaDbType);
                    para.Value = item.Value;
                    para.Size = item.Length;
                    result.Add(para);
                }

            return result.ToArray();
        }
    }

    public class MyDbParameter
    {
        public string Name { get; set; }
        public int Length { get; set; }
        public object Value { get; set; }
        public DbType ParaDbType { get; set; }
    }
}