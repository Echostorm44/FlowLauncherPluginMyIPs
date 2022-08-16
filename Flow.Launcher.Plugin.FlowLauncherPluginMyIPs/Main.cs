using Flow.Launcher.Plugin;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
// Icon by https://www.freepik.com/  FreePik
namespace Flow.Launcher.Plugin.FlowLauncherPluginMyIPs
{
    public class FlowLauncherPluginMyIPs : IPlugin
    {
        private const string Image = "icon.png";
        private PluginInitContext _context;

        public void Init(PluginInitContext context)
        {
            _context = context;
        }

        public List<Result> Query(Query query)
        {   
            string pubIp = new HttpClient().GetStringAsync("https://api.ipify.org").Result;    
            string localIP;
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = endPoint.Address.ToString();
            }
            var results = new List<Result>();
            results.Add(new Result 
            { 
                Title = pubIp,
                SubTitle = "Public IP",
                IcoPath = Image,
                Action = c =>
                {
                    System.Windows.Clipboard.SetText(pubIp);
                    return true;
                }
            });
            results.Add(new Result
            {
                Title = localIP,
                SubTitle = "Private IP",
                IcoPath = Image,
                Action = c =>
                {
                    System.Windows.Clipboard.SetText(localIP);
                    return true;
                }
            });

            return results;
        }
    }
}