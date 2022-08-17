using Flow.Launcher.Plugin;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
// Icon by https://www.freepik.com/  FreePik
namespace Flow.Launcher.Plugin.FlowLauncherPluginMyIPs
{
    public class FlowLauncherPluginMyIPs : IAsyncPlugin
    {
        private const string Image = "icon.png";
        private PluginInitContext _context;

        public async Task InitAsync(PluginInitContext context)
        {
            _context = context;
        }
        public async Task<List<Result>> QueryAsync(Query query, CancellationToken token)
        {
            var results = new List<Result>();
            token.ThrowIfCancellationRequested();
            string pubIp = await new HttpClient().GetStringAsync("https://api.ipify.org", token);
            string localIP;
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = endPoint.Address.ToString();
            }
            
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