/// <summary>
/// Copyright(c) 2008-2009北京玖富时代投资顾问有限公司, All Rights Reserved.
///
/// 该类的作用是用于获取硬盘信息
///
/// 版本        Ver 1.0
/// 作者		邓太华      时间		2009-02-10
/// 备注：本程序使用网上下载的资源
/// </summary>
/*获取硬盘物理序列号的方式
 * 相关程序来自网上，并经过程序
 * 邓太华 2008.10.30
 * 使用举例：
 * 
 * static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("WMI 方式 硬盘序列号 : " + WMI_HD.GetHdId());

                //API 方式
                Console.WriteLine("API 方式 硬盘序信息 : ");
                HardDiskInfo hdd = AtapiDevice.GetHddInfo(0); // 第一个硬盘
                Console.WriteLine("Module Number: {0}", hdd.ModuleNumber);
                Console.WriteLine("Serial Number: {0}", hdd.SerialNumber);
                Console.WriteLine("Firmware: {0}", hdd.Firmware);
                Console.WriteLine("Capacity: {0} M", hdd.Capacity);
                //
                Console.WriteLine("WMI 方式 优盘序列号 : " + WMI_USB.GetUSBId());
                Console.WriteLine("OK");
                Console.Read();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.Read();
            }
           
        }
 * 
 * 
 * 
 */
using System;
using System.Management;

namespace PWMIS.EnterpriseFramework.Common.Hardware
{
    /// <summary>
    /// 使用ＷＭＩ方式获取硬盘信息
    /// </summary>
    public class WMI_HD
    {
       /// <summary>
       /// 获取硬盘唯一序列号（不是卷标号），可能需要以管理员身份运行程序
       /// </summary>
       /// <returns></returns>
        public static string GetHdId()
        {
            ManagementObjectSearcher wmiSearcher = new ManagementObjectSearcher();
            /*
             * PNPDeviceID   的数据是由四部分组成的：   
  1、接口，通常有   IDE，ATA，SCSI；   
  2、型号   
  3、（可能）驱动版本号   
  4、（可能）硬盘的出厂序列号   
             * 
             * 
             */


            //signature 需要程序以管理员身份运行（经过测试，2003系统上非管理员身份也可以运行，查相关资料说，可能在2000系统上获取的值为空）

            wmiSearcher.Query = new SelectQuery(
            "Win32_DiskDrive",
            "",
            new string[] { "PNPDeviceID", "signature" }
            );
            ManagementObjectCollection myCollection = wmiSearcher.Get();
            ManagementObjectCollection.ManagementObjectEnumerator em =
            myCollection.GetEnumerator();
            em.MoveNext();
            ManagementBaseObject mo = em.Current;
            //string id = mo.Properties["PNPDeviceID"].Value.ToString().Trim();
            string id="";
            try
            {
                //首先使用signature，ＳＣＳＩ硬盘可能没有该属性
                if (mo.Properties["signature"] != null && mo.Properties["signature"].Value != null)
                    id = mo.Properties["signature"].Value.ToString();
                else if (mo.Properties["PNPDeviceID"] != null && mo.Properties["PNPDeviceID"].Value != null)//防止意外
                    id = mo.Properties["PNPDeviceID"].Value.ToString();
            }
            catch
            {
                if (mo.Properties["PNPDeviceID"] != null && mo.Properties["PNPDeviceID"].Value != null)//防止意外
                    id = mo.Properties["PNPDeviceID"].Value.ToString();
            }
            
            return id;
        }
    }

    public class HardDiskSN
    {
        static string _serialNumber=string.Empty;
        /// <summary>
        /// 获取硬盘号
        /// </summary>
        public static string SerialNumber
        {
            get {
                if (_serialNumber == string.Empty)
                {
                    try
                    {
                        HardDiskInfo hdd = AtapiDevice.GetHddInfo(0); // 第一个硬盘
                        _serialNumber ="API"+ hdd.SerialNumber;
                    }
                    catch
                    {
                        try
                        {
                            _serialNumber ="WMI"+ WMI_HD.GetHdId();
                        }
                        catch
                        {
                            _serialNumber = "FAIL111111";
                        }
                       
                    }
                   
                }
                return _serialNumber;
            }
        }
    }
}
