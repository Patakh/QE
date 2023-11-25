using QE.Context;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

public class Client
{ 
    public static async Task SendMessageAsync(string message,string IpTerminal)
    {
        EqContext eqContext = new EqContext();
        var windows = eqContext.SOfficeWindows.Where(x => x.SOfficeId == eqContext.SOfficeTerminals.First(g => g.IpAddress == IpTerminal).SOfficeId);

         if (windows.Any())
         {
             windows.ToList().ForEach(async x =>
             { 

                 // Подключение к серверу
                 try
                 {
                     using (TcpClient client = new TcpClient())
                     {
                         await client.ConnectAsync(IPAddress.Parse(x.WindowIp), 1234); 
                         using (NetworkStream stream = client.GetStream())
                         {
                             byte[] buffer = Encoding.UTF8.GetBytes(message);
                             await stream.WriteAsync(buffer, 0, buffer.Length);
                         }
                     }
                 }
                 catch (Exception ex)
                 {
                     
                 }

              });
         }
         
    }
}
