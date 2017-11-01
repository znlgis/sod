using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace PWMIS.EnterpriseFramework.Common.ZIP
{
    public class ZipClass
    {
        public static string ZipString(string unCompressedString)
        {
            //ISO8859-1 字符串，8位，只有这种可以完整保留二进制
            Encoding _encoding = Encoding.GetEncoding(28591);
            //byte[] bytData = System.Text.Encoding.UTF8.GetBytes(unCompressedString);
            byte[] bytData = _encoding.GetBytes(unCompressedString);
            MemoryStream ms = new MemoryStream();
            Stream s = new GZipStream(ms, CompressionMode.Compress);
            s.Write(bytData, 0, bytData.Length);
            s.Close();
            byte[] compressedData = (byte[])ms.ToArray();
            return System.Convert.ToBase64String(compressedData, 0, compressedData.Length);
        }

        public static string UnzipString(string unCompressedString)
        {
            System.Text.StringBuilder uncompressedString = new System.Text.StringBuilder();
            byte[] writeData = new byte[4096];

            byte[] bytData = System.Convert.FromBase64String(unCompressedString);
            int totalLength = 0;
            int size = 0;

            //ISO8859-1 字符串，8位，只有这种可以完整保留二进制
            Encoding _encoding = Encoding.GetEncoding(28591);

            Stream s = new GZipStream(new MemoryStream(bytData), CompressionMode.Decompress);
            while (true)
            {
                size = s.Read(writeData, 0, writeData.Length);
                if (size > 0)
                {
                    totalLength += size;
                    //uncompressedString.Append(System.Text.Encoding.UTF8.GetString(writeData, 0, size));
                    uncompressedString.Append(_encoding.GetString(writeData, 0, size));
                }
                else
                {
                    break;
                }
            }
            s.Close();
            return uncompressedString.ToString();
        }
    }
}
