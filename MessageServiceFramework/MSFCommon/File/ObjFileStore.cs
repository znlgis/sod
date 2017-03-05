using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Xml.Serialization;
using System.Xml;

namespace PWMIS.EnterpriseFramework.Common
{
    public class ObjFileStore
    {
        const string keyStr = "PdfNet.EF";  //加密的KEY字符，用MD5码生成128位KEY密钥

        const string ivStr = "comm@#$&";  //加密的IV字符，用MD5码生成128位IV密钥
        public ObjFileStore()
        { }

        /// <summary>
        /// 将对象保存为文件
        /// </summary>
        /// <param name="uFilename">文件名及地址</param>
        /// <param name="uTarget">保存对象，需[Serializable]</param>
        static public void SaveObj(string uFilename, object uTarget)
        {
            Rijndael rijn = Rijndael.Create();
            MD5 md5 = MD5.Create();
            byte[] Key = md5.ComputeHash(Encoding.ASCII.GetBytes(keyStr));
            byte[] IV = md5.ComputeHash(Encoding.ASCII.GetBytes(ivStr));
            FileStream fStream = null;
            CryptoStream cStream = null;
            IFormatter formatter = new BinaryFormatter();
            try
            {
                fStream = new FileStream(uFilename, FileMode.Create, FileAccess.Write, FileShare.None);
                cStream = new CryptoStream(fStream, rijn.CreateEncryptor(Key, IV), CryptoStreamMode.Write);
                formatter.Serialize(cStream, uTarget);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                if(cStream !=null)
                    cStream.Close();
                if(fStream!=null)
                    fStream.Close();
            }
        }

        /// <summary>
        /// 从文件读取对象
        /// </summary>
        /// <param name="uFilename">文件名及地址</param>
        /// <returns>反序列化后的对象，类型为object</returns>
        static public object LoadObj(string uFilename)
        {
            if (!File.Exists(uFilename)) return null;
            IFormatter formatter = new BinaryFormatter();
            Rijndael rijn = Rijndael.Create();
            MD5 md5 = MD5.Create();
            byte[] Key = md5.ComputeHash(Encoding.ASCII.GetBytes(keyStr));
            byte[] IV = md5.ComputeHash(Encoding.ASCII.GetBytes(ivStr));
            FileStream fStream = null;
            CryptoStream cStream = null;
            object result = null;
            try
            {
                fStream = new FileStream(uFilename, FileMode.Open, FileAccess.Read, FileShare.Read);
                cStream = new CryptoStream(fStream, rijn.CreateDecryptor(Key, IV), CryptoStreamMode.Read);
                result = formatter.Deserialize(cStream);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cStream != null)
                    cStream.Close();
                if (fStream != null)
                    fStream.Close();
            }
            return result;
        }

    }
}