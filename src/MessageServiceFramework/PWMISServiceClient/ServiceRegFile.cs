using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using PWMIS.EnterpriseFramework.Service.Client.Model;

namespace PWMIS.EnterpriseFramework.Service.Client
{
    public class ServiceRegFile
    {
        public void Save(List<ServiceRegModel> regInfos, string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            XmlSerializer ser = new XmlSerializer(typeof(List<ServiceRegModel>));
            ser.Serialize(fs, regInfos);
            fs.Close();
            fs.Dispose();
        }

        public List<ServiceRegModel> Load(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            XmlSerializer ser = new XmlSerializer(typeof(List<ServiceRegModel>));
            List<ServiceRegModel> result = (List<ServiceRegModel>)ser.Deserialize(fs);
            fs.Close();
            fs.Dispose();
            return result;
        }
    }
}
