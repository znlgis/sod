using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace TranstarAuction.Model
{
    public class CommonMethods
    {
        public static string GetStorePath(string filename)
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "优信拍");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            return Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "优信拍"), filename);
        }
    }
}
