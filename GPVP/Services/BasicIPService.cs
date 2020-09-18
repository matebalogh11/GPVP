using System;
using System.Net;
using System.Net.Sockets;

namespace GPVP.Services
{
    public class BasicIPService : IAddressService
    {
        public string GetLocalIpv4()
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
