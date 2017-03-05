using System;
using System.Collections.Generic;
using System.Text;

namespace PWMIS.EnterpriseFramework.Common
{
    /// <summary>
    /// IP信息类，判断IP是否为内网IP
    /// </summary>
    public class IpUtility
    {
        //判断IP地址是否为内网IP地址

        public bool IsInner(String ip)
        {

            //私有IP：

            //A类  10.0.0.0 ~ 10.255.255.255

            //B类  172.16.0.0 ~ 172.31.255.255

            //C类  192.168.0.0 ~ 192.168.255.255

            //以及127这个网段是环回地址

            return IsInner(ip, "10.0.0.0", "10.255.255.255")

                || IsInner(ip, "172.16.0.0", "172.31.255.255")

                || IsInner(ip, "192.168.0.0", "192.168.255.255")

                || ip.Equals("127.0.0.1");

        }



        //判断IP是否落在指定范围内

        private bool IsInner(string ip, string bottom_ip, string top_ip)
        {

            return (GetIpNum(ip) >= GetIpNum(bottom_ip))

                && (GetIpNum(ip) <= GetIpNum(top_ip));

        }



        /// 把IP地址转换为Long型数字

        private long GetIpNum(String ipAddress)
        {

            String[] ip = ipAddress.Split('.');

            long a = int.Parse(ip[0]) << 24;

            long b = int.Parse(ip[1]) << 16;

            long c = int.Parse(ip[2]) << 8;

            long d = int.Parse(ip[3]);



            return a + b + c + d;

        }
    }
}
