using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace HrmsBe.Helper
{
    public static class DeviceHelper
    {
        public static (string, string, string,string, string ) GetAdditionalDetails()
        {
            string deviceName = Environment.MachineName;
            string hostName = Dns.GetHostName();
            string ip = Dns.GetHostByName(hostName).AddressList[0].ToString();
            string deviceModel = GetDeviceModelName();
            var (mac,ipv6) = GetMacAndIPv6Address();
            return (deviceName, deviceModel, mac,ipv6,ip);
        }
        static string GetDeviceModelName()
        {
            string model = "Unknown";

            try
            {
                using ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");
                foreach (ManagementObject obj in searcher.Get())
                {
                    model = obj["Model"]?.ToString() ?? "Unknown";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving the device model: " + ex.Message);
            }

            return model;
        }
        static (string macAddress, string ipv6Address) GetMacAndIPv6Address()
        {
            string macAddress = string.Empty;
            string ipv6Address = string.Empty;

            try
            {
                var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

                foreach (var networkInterface in networkInterfaces)
                {
                    // Ensure the network interface is up and is either Ethernet or Wireless
                    if (networkInterface.OperationalStatus == OperationalStatus.Up &&
                        (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                         networkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211))
                    {
                        // Get the MAC address
                        if (string.IsNullOrEmpty(macAddress))
                        {
                            macAddress = string.Join(":", networkInterface.GetPhysicalAddress()
                                                            .GetAddressBytes()
                                                            .Select(b => b.ToString("X2")));
                        }

                        // Get the IPv6 address
                        foreach (var address in networkInterface.GetIPProperties().UnicastAddresses)
                        {
                            if (address.Address.AddressFamily == AddressFamily.InterNetworkV6)
                            {
                                ipv6Address = address.Address.ToString();
                                break;
                            }
                        }

                        // If both MAC and IPv6 are found, we can exit the loop
                        if (!string.IsNullOrEmpty(macAddress) && !string.IsNullOrEmpty(ipv6Address))
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving the MAC and IPv6 addresses: " + ex.Message);
            }

            return (macAddress, ipv6Address);
        }
    }
}
