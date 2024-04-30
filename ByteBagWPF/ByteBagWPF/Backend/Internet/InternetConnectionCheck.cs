using System;
using System.Net.NetworkInformation;

namespace ByteBagWPF.Backend.Internet
{
    public class InternetConnectionCheck
    {
        public static bool IsInternetAvailable()
        {
            try
            {
                // Létrehozunk egy pingelési kérést a google.com webhelyhez
                Ping ping = new Ping();
                PingReply reply = ping.Send("google.com");

                if (reply != null && reply.Status == IPStatus.Success)
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return false;
        }
    }
}
