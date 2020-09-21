using System;
using System.Net;
using System.Net.Sockets;

namespace GPVP.HelperClasses
{
    public class IpAddressHelper
    {
        #region Singleton

        private static IpAddressHelper instance;
        public static IpAddressHelper Instance
        {
            get
            {
                if (instance == null)
                    instance = new IpAddressHelper();
                return instance;
            }
        }

        #endregion

        private string ipv4Address;
        public string Ipv4Address
        {
            get
            {
                if (string.IsNullOrEmpty(ipv4Address))
                    ipv4Address = GetLocalIpv4();

                return ipv4Address;
            }
        }

        private string GetLocalIpv4()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }
}
