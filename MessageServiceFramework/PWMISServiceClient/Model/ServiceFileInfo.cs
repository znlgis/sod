using System;
using System.Collections.Generic;
using System.Text;

namespace PWMIS.EnterpriseFramework.Service.Client.Model
{
    public class ServiceFileInfo
    {
        public string Name { get; set; }
        public DateTime LastWriteTime { get; set; }
        public long Length { get; set; }
    }
}
