using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace PWMIS.EnterpriseFramework.Common.Encrypt
{
    /// <summary>
    /// 对字符串进行加密的功能类
    /// </summary>
    public class EncryptHelper
    {
        #region DES加密密钥
        /// <summary>
        /// DES加密密钥
        /// </summary>
        private const string ENCRYPTKEY = "auto@#$&";
        #endregion

        #region DES方式加密字符串的方法
        /// <summary>
        /// DES方式加密字符串的方法
        /// </summary>
        /// <param name="s">要进行加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string DesEncrypt(string s)
        {
            byte[] byKey = null;
            byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            byKey = Encoding.Default.GetBytes(ENCRYPTKEY.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.Default.GetBytes(s);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray());
        }
        #endregion

        #region DES方式解密字符串的方法
        /// <summary>
        /// DES方式解密字符串的方法
        /// </summary>
        /// <param name="s">要进行解密的字符串</param>
        /// <returns>解密后的字符串</returns>
        public static string DesDecrypt(string s)
        {
            byte[] byKey = null;
            byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            byte[] inputByteArray = new Byte[s.Length];
            byKey = Encoding.Default.GetBytes(ENCRYPTKEY.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            inputByteArray = Convert.FromBase64String(s);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.Default.GetString(ms.ToArray());
        }
        #endregion

        #region MD5方式加密字符串的方法
        /// <summary>
        /// MD5方式加密字符串的方法
        /// </summary>
        /// <param name="s">要进行加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string MD5Encrypt(string s)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes = md5.ComputeHash(Encoding.Default.GetBytes(s));
            string result = string.Empty;
            foreach (byte b in bytes)
            {
                result += b.ToString("x");
            }
            return result;
        }
        #endregion

        #region SHA1方式加密字符串的方法
        /// <summary>
        /// SHA1方式加密字符串的方法
        /// </summary>
        /// <param name="s">要进行加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string SHA1Encrypt(string s)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] bytes = sha1.ComputeHash(Encoding.Default.GetBytes(s));
            string result = string.Empty;
            foreach (byte b in bytes)
            {
                result += b.ToString("x");
            }
            return result;
        }
        #endregion
    }
}
