using System;
using System.Collections.Generic;
using System.Text;

namespace PWMIS.EnterpriseFramework.Service.Basic
{
    /// <summary>
    /// 服务方法参数类
    /// </summary>
    public class ServiceMethodParameter
    {
        public string ParameterTypeName { get; set; }
        public Type ParameterType { get; set; }
        public object ParameterValue { get; set; }
    }
    /// <summary>
    /// 参数解析类
    /// </summary>
    public class ParameterParse
    {
        public static ServiceMethodParameter Parse(string input)
        {
            string[] arr = input.Split('=');
            Type t = Type.GetType(arr[0].Replace("%Eqv;", "="), false, true);
            object Value = null;
            if (t != null)
            {
                if (t == typeof(string))
                {
                    Value = arr[1].Replace("%Eqv;", "=").Replace("%And;", "&").Replace("%Psp;", "/");
                }
                else if (t == typeof(DateTime))
                {
                    Value = new DateTime(long.Parse(arr[1]));
                }
                else if (t == typeof(Guid))
                {
                    Value = Guid.Parse(arr[1]);
                }
                else if (t.IsValueType)
                {
                    Value = Convert.ChangeType(arr[1], t);
                }
                else
                {
                    string temp = arr[1].Replace("%Eqv;", "=").Replace("%And;", "&").Replace("%Psp;", "/");
                    Value = Newtonsoft.Json.JsonConvert.DeserializeObject(temp, t);
                }
            }
            else
            {
                Value = arr[1].Replace("%Eqv;", "=").Replace("%And;", "&").Replace("%Psp;", "/");
            }

            return new ServiceMethodParameter() { ParameterType = t, ParameterTypeName = arr[0].Replace("%Eqv;", "="), ParameterValue = Value };
        }

        public static string GetParameterString(ServiceMethodParameter para)
        {
            //return typeName + "=" + para.ParameterValue.ToString();
            return GetParaObjString(para.ParameterValue);
        }

        public static string GetParameterString(object[] paras)
        {
            if (paras == null)
                return "";
            string[] strArr = new string[paras.Length];
            for (int i = 0; i < paras.Length; i++)
            {
                object obj = paras[i];
                strArr[i] = GetParaObjString(obj);
            }
            return string.Join("&", strArr);
        }

        private static string GetParaObjString(object obj)
        {
            Type type = obj.GetType();
            string fullName = type.FullName.Replace("=", "%Eqv;");
            string strTemp = string.Empty;
            if (type == typeof(string))
            {
                strTemp = ((string)obj).Replace("=", "%Eqv;").Replace("&", "%And;").Replace("/", "%Psp;");
            }
            else if (type == typeof(DateTime))
            {
                strTemp = ((DateTime)obj).Ticks.ToString();
            }
            else if (obj is ValueType)
            {
                strTemp = obj.ToString();
            }
            else
            {
                strTemp = Newtonsoft.Json.JsonConvert.SerializeObject(obj).Replace("=", "%Eqv;").Replace("&", "%And;").Replace("/", "%Psp;");
            }
            return fullName + "=" + strTemp;
        }

        public static ServiceMethodParameter[] GetParameters(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return new ServiceMethodParameter[] { };
            }
            else
            {
                string[] strArr = input.Split('&');
                ServiceMethodParameter[] paras = new ServiceMethodParameter[strArr.Length];
                for (int i = 0; i < paras.Length; i++)
                {
                    ServiceMethodParameter para = Parse(strArr[i]);
                    paras[i] = para;
                }
                return paras;
            }

        }

        public static object GetObject(ServiceMethodParameter para)
        {
            if (para.ParameterType != null)
            {
                if (para.ParameterType.IsEnum)
                {
                    return para.ParameterValue = Enum.Parse(para.ParameterType, para.ParameterValue.ToString());
                }
                return Newtonsoft.Json.JsonConvert.DeserializeObject(para.ParameterValue.ToString(), para.ParameterType);
            }
            else
                return null;
        }
    }
}
