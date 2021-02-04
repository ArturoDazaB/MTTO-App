using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net.Sockets;

using Foundation;
using UIKit;

using MTTO_App.Servicios;

using System.Net.NetworkInformation;

[assembly:Xamarin.Forms.Dependency(typeof(MTTO_App.iOS.IPAddressManager))]

namespace MTTO_App.iOS
{
    class IPAddressManager : IIPAddressManager 
    {
        public string GetIPAddress()
        {
            try
            {
                foreach( var netInterface in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if(netInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                       netInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                    {
                        foreach(var addressinfo in netInterface.GetIPProperties().UnicastAddresses)
                        {
                            if(addressinfo.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                return addressinfo.Address.ToString();
                            }
                        }
                    }
                }
            }
            catch(Exception ex) when (ex is NetworkInformationException)
            {
                return "ERROR";
            }

            return "";
        }
    }
}