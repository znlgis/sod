using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace PWMIS.DataProvider.Data
{
    class DataParameterFormat: IFormatProvider, ICustomFormatter
    {
        private List<IDataParameter> paras;// = new List<IDataParameter>();
        private CommonDB DB;// = null;
        private int paraIndex;

        public DataParameterFormat(CommonDB db)
        {
            this.DB = db;
            paras = new List<IDataParameter>();
            paraIndex = 0;
        }

        public IDataParameter[] DataParameters
        {
            get {
                return paras.ToArray();
            }
        }

        #region IFormatProvider 成员

        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
                return this;
            else
                return null;

        }

        #endregion

        #region ICustomFormatter 成员

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            string paraName = DB.GetParameterChar + "P" + paraIndex++;
            if (format == null)
            {
                paras.Add(DB.GetParameter(paraName, arg));
            }
            else
            {
                if (format.IndexOf('.') > 0)
                {
                    if (arg != null && arg.GetType() == typeof(decimal))
                    {
                        //decimal 类型，制定小数位数和精度
                        string[] arr = format.Split('.');
                        byte precision = 0;
                        byte scale = 0;
                        if (byte.TryParse(arr[0], out precision) && byte.TryParse(arr[1], out scale))
                        {
                            IDataParameter para = DB.GetParameter(paraName, DbType.Decimal, sizeof(decimal), ParameterDirection.Input, precision, scale);
                            paras.Add(para);
                        }
                        else
                        {
                            paras.Add(DB.GetParameter(paraName, arg));
                        }
                    }
                    else
                    {
                        paras.Add(DB.GetParameter(paraName, arg));
                    }
                }
                else
                {
                    if (arg != null )
                    {
                        if (arg.GetType() == typeof(string))
                        {
                            //字符串类型，制定参数的长度 
                            int size = 0;
                            if (int.TryParse(format, out size))
                            {
                                IDataParameter para = DB.GetParameter(paraName, DbType.String, size);
                                para.Value = arg;
                                paras.Add(para);
                            }
                            else
                            {
                                paras.Add(DB.GetParameter(paraName, arg));
                            }
                        }
                        else if (arg is IDataParameter)
                        {
                            paras.Add((IDataParameter)arg);
                        }
                        else
                        {
                            paras.Add(DB.GetParameter(paraName, arg));
                        }
                        
                    }
                    else
                    {
                        paras.Add(DB.GetParameter(paraName, arg));
                    }
                }//end if
            
            }
            return paraName;
        }

        #endregion
    }
}
