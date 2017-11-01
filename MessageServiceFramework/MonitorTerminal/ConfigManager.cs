using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MonitorTerminal.CustomEntities;
using System.Xml.Serialization;

namespace MonitorTerminal
{
    public class ConfigManager
    {
        protected string ConfigFileName { get; set; }

        public ConfigManager()
        {
            ConfigFileName = Path.Combine(Environment.CurrentDirectory, "MonitoerServiceItems.config");
        }
        public void SerializeServiceItems(List<Service> list)
        {
            FileStream fs = null;
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(List<Service>));
                fs = new FileStream(ConfigFileName, FileMode.Create, FileAccess.Write, FileShare.None);
                xs.Serialize(fs, list.ToList());
                fs.Close();
            }
            catch
            {
                if (fs != null)
                    fs.Close();
            }
        }

        public List<Service> DeserializeServiceItems()
        {
            string path = ConfigFileName;
            if (!File.Exists(path)) return null;
            FileStream fs = null;
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(List<Service>));
                fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                var list = (List<Service>)xs.Deserialize(fs);
                fs.Close();
                return list;
            }
            catch
            {
                if (fs != null)
                    fs.Close();
            }
            return null;
        }
    }
}
