/**
 * 数据转换类
 * 邓太华 2011.12.28
 * 
 * 本文定义了 
 * #define UseEncrypt
 * 编译器常量，如果不需要加密，可以去掉该定义
 * 例如：
 * #undef UseEncrypt
 */

//是否使用消息加密的编译器申明
#define NoUseEncrypt

using System;
using System.Collections.Generic;
using System.Text;

namespace PWMIS.EnterpriseFramework.Common.Encrypt
{

    public class DataConverter
    {
        /// <summary>
        /// 将字节数组以8位字符编码的格式，转换成字符串
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static string Byte2String(byte[] buffer)
        {
            Encoding _encoding = Encoding.GetEncoding(28591);
            string result = _encoding.GetString(buffer);
            return result;
        }

        /// <summary>
        /// 将8位字符编码的字符串转换成字节数组，不能是UTF-8格式的字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] String2Byte(string input)
        {
            Encoding _encoding = Encoding.GetEncoding(28591);
            return _encoding.GetBytes(input);
        }

        private static int currNumSeed;
        private static int NumSeed
        {
            get {
                int i= int.Parse( DateTime.Now.ToString("yyyyMM"));
                return i;
            }
        }

        private static int[] currConvertNumber;
        private static int[] currDeConvertNumber;

        /// <summary>
        /// 获取反向的数字对照表
        /// </summary>
        public static int[] DeMapNumber
        {
            get {
                if (currConvertNumber == null)
                    currConvertNumber = MapNumber;
                return currDeConvertNumber;
            }
        }

        /// <summary>
        /// 获取数字对照表
        /// </summary>
        public static int[] MapNumber
        {
            get
            {
                if (currNumSeed != NumSeed)
                {
                    currNumSeed = NumSeed;

                    Random rnd = new Random(currNumSeed);
                    int[] arr = new int[256];
                    int index = 0;
                    while (index < 256)
                    {
                        int num = rnd.Next(0, 256);
                        bool find = false;
                        for (int i = 0; i < index; i++)
                        {
                            if (arr[i] == num)
                            {
                                find = true;
                                break;
                            }
                        }
                        if (!find)
                            arr[index++] = num;
                    }

                    currConvertNumber = arr;
                    currDeConvertNumber =new int[arr.Length];

                    for (int i = 0; i < arr.Length; i++)
                        currDeConvertNumber[arr[i]] = i;
                       
                }
                return currConvertNumber;
            }
        }

        /// <summary>
        /// 将UTF8格式的字符串，编码成8位编码格式的新字符串
        /// </summary>
        /// <param name="input">UTF8格式的字符串</param>
        /// <param name="EnMap">编码表</param>
        /// <returns>8位编码格式的字符串</returns>
        public static string Encrypt8bitString(string input, int[] EnMap)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(input);
            byte[] outBuffer = new byte[buffer.Length];

            //加密
            for (int i = 0; i < buffer.Length; i++)
            {
                outBuffer[i] = (byte)EnMap[buffer[i]];
            }

            //8位编码，供传输字符串
            string outString = Byte2String(outBuffer);
            return outString;
        }

        /// <summary>
        /// 将UTF8格式的字符串，编码成8位编码格式的新字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Encrypt8bitString(string input)
        {
#if(UseEncrypt)
            return Encrypt8bitString(input, MapNumber);
#else
            return input;
#endif
        }

        /// <summary>
        /// 8位编码格式的字符串，解码成UTF8格式的新字符串
        /// </summary>
        /// <param name="encryptString">8位编码格式的字符串</param>
        /// <param name="DeMap">解码表</param>
        /// <returns>UTF8格式的新字符串</returns>
        public static string DeEncrypt8bitString(string encryptString, int[] DeMap)
        {
            //8位解码，将传输的字符串处理成字节数组
            byte[] buffer2 = String2Byte(encryptString);

            //解密
            byte[] outBuffer2 = new byte[buffer2.Length];

            for (int i = 0; i < buffer2.Length; i++)
            {
                outBuffer2[i] = (byte)DeMap[buffer2[i]];
            }

            //outBuffer2 == buffer

            string outString2 = Encoding.UTF8.GetString(outBuffer2);
            return outString2;
        }

        /// <summary>
        /// 8位编码格式的字符串，解码成UTF8格式的新字符串
        /// </summary>
        /// <param name="encryptString"></param>
        /// <returns></returns>
        public static string DeEncrypt8bitString(string encryptString)
        {
#if(UseEncrypt)
            return DeEncrypt8bitString(encryptString, DeMapNumber);
#else
            return encryptString;
#endif
        }

    }
}
