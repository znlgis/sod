using System;
using System.Collections.Generic;
using System.Text;
using PWMIS.EnterpriseFramework.Common;
using PWMIS.EnterpriseFramework.Common.Encrypt;

namespace PWMIS.EnterpriseFramework.Service.Client
{
    /// <summary>
    /// 消息转换器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MessageConverter<T>
    {
        /// <summary>
        /// 转换结果
        /// </summary>
        public T Result { get; protected internal set; }
        /// <summary>
        /// 转换是否成功，强烈建议使用前检查该属性
        /// </summary>
        public bool Succeed { get; protected internal set; }
        /// <summary>
        /// 消息的原始文本
        /// </summary>
        public string MessageText { get; protected internal set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; protected internal set; }

        /// <summary>
        /// 获取服务器响应的数据类型
        /// </summary>
        /// <typeparam name="T">最终要转换结果的类型</typeparam>
        /// <returns>服务端消息的类型</returns>
        public static DataType GetResponseDataType()
        {
            DataType resultDataType;
            Type tempType = typeof(T);

            if (tempType == typeof(string))
            {
                resultDataType = DataType.Text;
            }
            else if (tempType == typeof(DateTime)) //由于服务器和客户端时间格式可能不相同，统一用Json方式序列化
            {
                resultDataType = DataType.DateTime;
            }
            else if (tempType == typeof(byte[]))
            {
                resultDataType = DataType.Binary;
            }
            else if (tempType.IsValueType)
            {
                resultDataType = DataType.Text;
            }
            else
            {
                resultDataType = DataType.Json;
            }
            return resultDataType;
        }

        public MessageConverter()
        {

        }

        /// <summary>
        /// 转换当前类型的消息数据并处理转换过程中的异常
        /// </summary>
        /// <param name="messageText">消息的原始文本</param>
        public MessageConverter(string messageText)
            : this(messageText, GetResponseDataType())
        {

        }

        /// <summary>
        /// 转换指定类型的消息数据并处理转换过程中的异常
        /// </summary>
        /// <param name="messageText">消息的原始文本</param>
        /// <param name="resultDataType">消息的数据类型</param>
        public MessageConverter(string messageText, DataType resultDataType)
        {
            T result;
            try
            {
                if (typeof(T) == typeof(byte[]) || resultDataType == DataType.Binary)
                {
                    result = (T)Convert.ChangeType(DataConverter.String2Byte(messageText), typeof(T));
                }
                else
                {
                    messageText = DataConverter.DeEncrypt8bitString(messageText);
                    this.MessageText = messageText;
                    //.IsValueType 如果是值类型，强制进行文本转换，避免调用本程序的外部程序使用不合理的数据类型 DataType
                    if (resultDataType == DataType.DateTime || typeof(T) == typeof(DateTime))
                        //result = (T)Newtonsoft.Json.JsonConvert.DeserializeObject<T>(messageText);
                        result = (T)Convert.ChangeType(new DateTime(long.Parse(messageText)), typeof(T));
                    else if (resultDataType == DataType.Text || typeof(T) == typeof(string))
                        result = (T)Convert.ChangeType(messageText, typeof(T));
                    else if (resultDataType == DataType.Json)
                        result = typeof(T).IsValueType ?
                            (T)Convert.ChangeType(messageText, typeof(T)) :
                            (T)Newtonsoft.Json.JsonConvert.DeserializeObject<T>(messageText);
                    else
                        result = default(T);
                }

                this.Succeed = true;
                this.ErrorMessage = "";
                this.MessageText = messageText;
            }
            catch (Exception ex)
            {
                result = default(T);
                this.Succeed = false;
                this.ErrorMessage = string.Format("消息转换错误，具体错误信息：{0}。 请尝试检查系统时间设定是否准确。", ex.Message);
            }
            this.Result = result;
        }


      

        /// <summary>
        /// 将一个结果值进行序列化
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public string Serialize(T result)
        {
            this.Result = result;
            string strResult = "";
            try
            {
                Type tempType = typeof(T);
                if (tempType == typeof(byte[]))
                {
                    strResult = DataConverter.Byte2String(result as byte[]);
                    this.MessageText = "bytes...";
                }
                else
                {
                    if (tempType == typeof(string))
                        this.MessageText = result as string;
                    else if (tempType == typeof(DateTime))
                        //this.MessageText = Newtonsoft.Json.JsonConvert.SerializeObject(result); 
                        this.MessageText = Convert.ToDateTime(result).Ticks.ToString();
                    else if (tempType.IsValueType)
                        this.MessageText = result.ToString();
                    else
                        this.MessageText = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                    strResult = DataConverter.Encrypt8bitString(this.MessageText);
                }

                this.Succeed = true;
                this.ErrorMessage = "";

            }
            catch (Exception ex)
            {
                this.MessageText = "";
                this.Succeed = false;
                this.ErrorMessage = ex.Message;
            }
            return strResult;
        }
    }
}
