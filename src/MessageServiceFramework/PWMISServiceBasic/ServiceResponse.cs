using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using PWMIS.EnterpriseFramework.Common;
using PWMIS.EnterpriseFramework.Common.Encrypt;

namespace PWMIS.EnterpriseFramework.Service.Basic
{
    /// <summary>
    /// 服务输出响应
    /// </summary>
    public class ServiceResponse
    {
        /// <summary>
        /// 获取响应的数据流
        /// </summary>
        public Stream ResponseStream { get; private set; }

        /// <summary>
        /// 是否结束输出响应
        /// </summary>
        public bool IsEndResponse { get; private set; }

        private string _allText = null;
        /// <summary>
        /// 获取流中所有的文本，如果流是字节数组，那么文本将是8位二进制形式的字符串，在接收端需要做相应的处理
        /// </summary>
        public string AllText
        {
            get
            {
                if (_allText == null)
                {
                    if (this.ResultType != null && this.ResultType == typeof(byte[]))
                    {
                        _allText = DataConverter.Byte2String(this.GetAllBytes());
                    }
                    else
                    {
                        _allText = DataConverter.Encrypt8bitString(this.GetAllText());
                    }
                }

                return _allText;
            }
        }

        /// <summary>
        /// 写入流中的结果类型
        /// </summary>
        public Type ResultType { get; set; }

        public ServiceResponse(Stream stream)
        {
            this.ResponseStream = stream;
            this.IsEndResponse = false;
        }
        /// <summary>
        /// 将文本写入输出流
        /// </summary>
        /// <param name="text"></param>
        public void Write(string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            this.ResponseStream.Write(buffer, 0, buffer.Length);
        }

        public void Write(byte[] buffer)
        {
            this.ResponseStream.Write(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// 将对象以Json字符串的方式写入输出流
        /// </summary>
        /// <param name="source"></param>
        public void WriteJsonString(object source)
        {
            string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(source);
            this.Write(jsonString);
        }

        public void WriteDateTime(DateTime data)
        {
             string strTemp = data.Ticks.ToString();
             this.Write(strTemp);
        }

        /// <summary>
        /// 从当前流的起始位置，读取所有字符串文本。
        /// </summary>
        /// <returns></returns>
        private string GetAllText()
        {
            this.ResponseStream.Flush();
            this.ResponseStream.Position = 0;
            System.IO.TextReader reader = new System.IO.StreamReader(this.ResponseStream);
            return reader.ReadToEnd();
        }

        private byte[] GetAllBytes()
        {
            this.ResponseStream.Flush();
            this.ResponseStream.Position = 0;
            System.IO.BinaryReader reader = new System.IO.BinaryReader(this.ResponseStream);
            return reader.ReadBytes((int)this.ResponseStream.Length);
        }

        //private string Byte2String(byte[] buffer)
        //{
        //    Encoding _encoding = Encoding.GetEncoding(28591);
        //    string result = _encoding.GetString(buffer);
        //    return result;
        //}

        /// <summary>
        /// 从流中读取所有内容，并关闭流。
        /// </summary>
        public void End()
        {
            this.IsEndResponse = true;
            string temp = this.AllText;
            this.ResponseStream.Close();
        }

        /// <summary>
        /// 清除当前文本，以便需要时重新从流读取
        /// </summary>
        public void Clear()
        {
            _allText = null;
        }
    }
}
