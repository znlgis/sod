using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace PWMIS.EnterpriseFramework.Common.ZIP
{
    public class SampleCompress
    {
        public static void Compress(string filePath, string zipPath)
        {
            FileStream sourceFile = File.OpenRead(filePath);
            FileStream destinationFile = File.Create(zipPath);
            byte[] buffer = new byte[sourceFile.Length];
            GZipStream zip = null;
            try
            {
                sourceFile.Read(buffer, 0, buffer.Length);
                zip = new GZipStream(destinationFile, CompressionMode.Compress);
                zip.Write(buffer, 0, buffer.Length);
            }
            catch
            {
                throw;
            }
            finally
            {
                zip.Close();
                sourceFile.Close();
                destinationFile.Close();
            }
        }

        public static void Decompress(string zipPath, string filePath)
        {
            FileStream sourceFile = File.OpenRead(zipPath);

            string path = filePath.Replace(Path.GetFileName(filePath), "");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            FileStream destinationFile = File.Create(filePath);
            GZipStream unzip = null;
            byte[] buffer = new byte[sourceFile.Length];
            try
            {
                unzip = new GZipStream(sourceFile, CompressionMode.Decompress, true);
                int numberOfBytes = unzip.Read(buffer, 0, buffer.Length);

                destinationFile.Write(buffer, 0, numberOfBytes);
            }
            catch
            {
                throw;
            }
            finally
            {
                sourceFile.Close();
                destinationFile.Close();
                unzip.Close();
            }
        }
    }
}
