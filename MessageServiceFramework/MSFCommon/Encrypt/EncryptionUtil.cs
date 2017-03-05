using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace PWMIS.EnterpriseFramework.Common.Encrypt
{
    /// <summary>
    /// 加密，解密功能函数
    /// </summary>
    public class EncryptionUtil
    {
        public EncryptionUtil()
        {
            //
            // TODO: Add constructor logic here
            //

        }

        static private Byte[] m_Key = new Byte[8];
        static private Byte[] m_IV = new Byte[8];

        //为了安全，直接将key写死在文件中，你也可以用一个属性来实现
        public string key = "Test$)1";

        public string GetDefaultKey()
        {
            return "Wcf$Mail_" + System.IO.DriveInfo.GetDrives()[0].VolumeLabel;
        }
        /// <summary>
        /// 加密函数
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="strData"></param>
        /// <returns></returns>
        public string EncryptData(String strKey, String strData)
        {
            string strResult; //Return Result

            //1. 字符大小不能超过 90Kb. 否则, 缓存容易溢出（看第3点）
            if (strData.Length > 92160)
            {
                strResult = "Error. Data String too large. Keep within 90Kb.";
                return strResult;
            }

            //2. 生成key
            if (!InitKey(strKey))
            {
                strResult = "Error. Fail to generate key for encryption";
                return strResult;
            }

            //3. 准备处理的字符串
            //字符串的前5个字节用来存储数据的长度
            //用这个简单的方法来记住数据的初始大小，没有用太复杂的方法
            strData = String.Format("{0,5:00000}" + strData, strData.Length);


            //4. 加密数据
            byte[] rbData = new byte[strData.Length];
            ASCIIEncoding aEnc = new ASCIIEncoding();
            aEnc.GetBytes(strData, 0, strData.Length, rbData, 0);
            //加密功能实现的主要类
            DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();

            ICryptoTransform desEncrypt = descsp.CreateEncryptor(m_Key, m_IV);


            //5. 准备stream
            // mOut是输出流.
            // mStream是输入流
            // cs为转换流
            MemoryStream mStream = new MemoryStream(rbData);
            CryptoStream cs = new CryptoStream(mStream, desEncrypt, CryptoStreamMode.Read);
            MemoryStream mOut = new MemoryStream();

            //6. 开始加密
            int bytesRead;
            byte[] output = new byte[1024];
            do
            {
                bytesRead = cs.Read(output, 0, 1024);
                if (bytesRead != 0)
                    mOut.Write(output, 0, bytesRead);
            } while (bytesRead > 0);

            //7. 返回加密结果
            //因为是一个web项目，在这里转换为base64，因此在http上是不会出错的
            if (mOut.Length == 0)
                strResult = "";
            else
                strResult = Convert.ToBase64String(mOut.GetBuffer(), 0, (int)mOut.Length);

            return strResult;
        }

       /// <summary>
        /// 解密函数
       /// </summary>
       /// <param name="strKey"></param>
       /// <param name="strData"></param>
       /// <returns></returns>
        public string DecryptData(String strKey, String strData)
        {
            string strResult;

            //1. 生成解密key
            if (!InitKey(strKey))
            {
                strResult = "Error. Fail to generate key for decryption";
                return strResult;
            }

            //2. 初始化解密的主要类
            int nReturn = 0;
            DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();
            ICryptoTransform desDecrypt = descsp.CreateDecryptor(m_Key, m_IV);

            //3. 准备stream
            // mOut为输出流
            // cs为转换流
            MemoryStream mOut = new MemoryStream();
            CryptoStream cs = new CryptoStream(mOut, desDecrypt, CryptoStreamMode.Write);

            byte[] bPlain = new byte[strData.Length];
            try
            {
                bPlain = Convert.FromBase64CharArray(strData.ToCharArray(), 0, strData.Length);
            }
            catch (Exception)
            {
                strResult = "Error. Input Data is not base64 encoded.";
                return strResult;
            }

            long lRead = 0;
            long lTotal = strData.Length;

            try
            {
                //5. 完成解密
                while (lTotal >= lRead)
                {
                    cs.Write(bPlain, 0, (int)bPlain.Length);
                    lRead = mOut.Length + Convert.ToUInt32(((bPlain.Length / descsp.BlockSize) * descsp.BlockSize));
                };

                ASCIIEncoding aEnc = new ASCIIEncoding();
                strResult = aEnc.GetString(mOut.GetBuffer(), 0, (int)mOut.Length);

                //6.去处存储长度的前5个字节的数据
                String strLen = strResult.Substring(0, 5);
                int nLen = Convert.ToInt32(strLen);
                strResult = strResult.Substring(5, nLen);
                nReturn = (int)mOut.Length;

                return strResult;
            }
            catch (Exception)
            {
                strResult = "Error. Decryption Failed. Possibly due to incorrect Key or corrputed data";
                return strResult;
            }
        }

        /// <summary>
        /// 生成key的函数
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        static private bool InitKey(String strKey)
        {
            try
            {
                // 转换key为字节流
                byte[] bp = new byte[strKey.Length];
                ASCIIEncoding aEnc = new ASCIIEncoding();
                aEnc.GetBytes(strKey, 0, strKey.Length, bp, 0);

                SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider();
                byte[] bpHash = sha.ComputeHash(bp);

                int i;
                // 生成初始化DESCryptoServiceProvider的参数
                for (i = 0; i < 8; i++)
                    m_Key[i] = bpHash[i];

                for (i = 8; i < 16; i++)
                    m_IV[i - 8] = bpHash[i];

                return true;
            }
            catch (Exception)
            {
                //错误处理
                return false;
            }
        }
    }
}
