using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDemo.Model
{
    public class CommonUtil
    {
        /// <summary>
        /// 创建哈希字符串适用于任何 MD5 哈希函数 （在任何平台） 上创建 32 个字符的十六进制格式哈希字符串
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Get32MD5One(string source)
        {
            using (System.Security.Cryptography.MD5 md5Hash = System.Security.Cryptography.MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(source));
                System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                string hash = sBuilder.ToString();
                return hash.ToUpper();
            }
        }

        //MySQL 日期字符串转日期类型：
        //日期格式：yyyyMMddHHmmss
        // SELECT szTime, STR_TO_DATE(szTime, '%Y%m%d%H%i%s') as 'date1' from `Alarms` where AtTime is null limit 100;
        // update `Alarms` set AtTime= STR_TO_DATE(szTime, '%Y%m%d%H%i%s') where AtTime is null ;

        /// <summary>
        /// 将字符串解析为日期类型数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="style">字符串格式，如果为空则使用默认格式 yyyy-MM-dd HH:mm:ss </param>
        /// <returns></returns>
        public static DateTime ParseDateTimeString(string value, string style = "yyyyMMddHHmmss")
        {
            DateTime date;
            if (string.IsNullOrEmpty(style))
            {
                if (DateTime.TryParse(value, out date))
                    return date;
                else
                    style = "yyyy-MM-dd HH:mm:ss";
            }
            DateTime.TryParseExact(value, style, null, DateTimeStyles.None, out date);
            return date;
        }

        /// <summary>
        /// 压缩URL中相同路径的表示
        /// </summary>
        /// <param name="urls">路径列表，建议urls长度大于36个元素开启压缩</param>
        /// <param name="length">要开启压缩所需的路径列表元素最少数量</param>
        /// <returns>处理后以逗号分隔的字符串，以[URLZIP]开头</returns>
        public static string UrlZip(string[] urls, int length = 10)
        {
            if (urls == null || urls.Length == 0)
                return "";
            if (urls.Length < length)
            {
                return string.Join(",", urls);
            }
            string lastBasePath = "";
            Array.Sort(urls);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("[URLZIP],");
            foreach (string url in urls)
            {
                int index = url.LastIndexOf("/");
                string basePath = url.Substring(0, index + 1);
                string fileName = url.Substring(index + 1);
                if (lastBasePath != basePath)
                {
                    sb.Append(basePath);
                    sb.Append(',');
                    lastBasePath = basePath;
                }
                sb.Append(fileName);
                sb.Append(',');
            }
            return sb.ToString();
        }
        /// <summary>
        /// 将压缩的URL还原成带全路径的URL
        /// </summary>
        /// <param name="input">以[URLZIP]开头的逗号分隔压缩URL字符串</param>
        /// <returns>逗号分隔的非压缩URL字符串</returns>
        public static string UrlUnZip(string input)
        {
            if (input is null)
            {
                return null;
            }

            if (input.StartsWith("[URLZIP],"))
            {
                string[] urlArr = input.Split(',');
                System.Text.StringBuilder sb2 = new System.Text.StringBuilder();
                string basePath = urlArr[1];
                for (int i = 2; i < urlArr.Length - 1; i++)
                {
                    if (urlArr[i].EndsWith("/"))
                    {
                        basePath = urlArr[i];
                    }
                    else
                    {
                        sb2.Append(basePath);
                        sb2.Append(urlArr[i]);
                        sb2.Append(",");
                        //sb2.AppendLine(); 
                    }
                }
                //return sb2.ToString(0, sb2.Length - 3);//去除最后的换行和逗号
                return sb2.ToString(0, sb2.Length - 1);
            }
            else
            {
                return input;
            }
        }

        /// <summary>
        /// 将查询过滤条件解析成键值对
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// filter="AtTime$>=20231101145846,AtTime$<=20231102150314,SubAlarm$0,SzEquipmentName$%设备1%"
        /// ]]></example>
        public static List<KeyValuePair<string, string>> GetFilterKeyValues(string filter = "null")
        {
            List<KeyValuePair<string, string>> keyValuePairs = new List<KeyValuePair<string, string>>();
            //keyValuePairs.Add(new KeyValuePair<string, string>("SzEquipmentType", typeId.ToString()));
            if (filter != "null" && !string.IsNullOrEmpty(filter))
            {
                string[] arr1 = filter.Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in arr1)
                {
                    if (!item.Contains("$")) continue;
                    string[] arr2 = item.Split('$', StringSplitOptions.RemoveEmptyEntries);
                    if (arr2.Length > 1)
                    {
                        //解决内容本身含有$问题
                        string value = arr2.Length == 2 ? arr2[1] : string.Join('$', arr2, 1, arr2.Length - 1);
                        KeyValuePair<string, string> kv = new KeyValuePair<string, string>(arr2[0].Trim(), value);
                        keyValuePairs.Add(kv);
                    }
                    else
                    {
                        KeyValuePair<string, string> kv = new KeyValuePair<string, string>(arr2[0].Trim(), "");
                        keyValuePairs.Add(kv);
                    }
                }
            }
            return keyValuePairs;
        }

        static long _currSeqNumber;
        const long MAX_SEQ_NUMBER = 9999999;
        static object lockObj1 = new object();
        /// <summary>
        /// 获取一个1到9999999之间有序递增的数，超过之后从0重新开始
        /// </summary>
        /// <returns></returns>
        public static long NextSeqNumber()
        {
            long number = System.Threading.Interlocked.Increment(ref _currSeqNumber);
            if (_currSeqNumber > MAX_SEQ_NUMBER)
            {
                lock (lockObj1)
                {
                    if (_currSeqNumber > MAX_SEQ_NUMBER)
                        _currSeqNumber = 0;
                }
            }
            return number;
        }

        /// <summary>
        /// 生成一个不超过32个有字符的有序串
        /// </summary>
        /// <param name="leftStr"></param>
        /// <returns></returns>
        public static string GetTimeSeqNumberString(string leftStr)
        {
            if (string.IsNullOrEmpty(leftStr))
                leftStr = string.Empty;
            DateTime dt = DateTime.Now;
            int yearCode = dt.Year + 30;
            string yearStr = yearCode.ToString().Substring(2);
            long seqNumber = NextSeqNumber();
            //不包括leftStr，结果位数：1+2+2+2+10+7=24
            //8位前导字符串
            string firstStr = leftStr.Length > 8 ? leftStr.Substring(0, 8) : leftStr.PadRight(8, '0');
            return $"{firstStr}.{yearStr}{dt.Month.ToString("D2")}{dt.Day.ToString("D2")}{dt.ToString("HHmmss.fff")}{seqNumber.ToString("D7")}";
        }
    }
}
