using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;


IPAddress iPAddress = new IPAddress(new byte[] { 127, 0, 0, 1 });
IPAddress iPAddress1 = IPAddress.Parse("127.0.0.1");

IPAddress localAddress = IPAddress.Loopback;

IPEndPoint iPEndPoint = new IPEndPoint(localAddress, 5000);
IPEndPoint local = IPEndPoint.Parse("127.0.0.1:5000");

string uriString = "https://user:password@mysite.ru:400/about/team?manager=Bob&saler=Jim#part2";

Uri uri = new Uri(uriString);

Console.WriteLine($"Absolute uri: {uri.AbsoluteUri}");
Console.WriteLine($"User Info: {uri.UserInfo}");
Console.WriteLine($"Scheme: {uri.Scheme}");
Console.WriteLine($"Authority: {uri.Authority}");
Console.WriteLine($"Host: {uri.Host}");
Console.WriteLine($"Port: {uri.Port}");
Console.WriteLine($"Path and QueryString: {uri.PathAndQuery}");
Console.WriteLine($"Path: {uri.LocalPath}");
Console.WriteLine($"Query: {uri.Query}");
Console.WriteLine($"Segments:");
foreach(var s in uri.Segments)
    Console.WriteLine($"\t: {s}");
Console.WriteLine($"Fragment: {uri.Fragment}");
Console.WriteLine();

//var adresses = Dns.GetHostAddresses(Dns.GetHostName(), System.Net.Sockets.AddressFamily.InterNetwork);
var adresses = Dns.GetHostAddresses("yandex.ru");
foreach (var a in adresses)
    Console.WriteLine(a);

var adapters = NetworkInterface.GetAllNetworkInterfaces();

Socket socketTcp = new(SocketType.Stream, ProtocolType.Tcp);

using (Socket socketUdp = new(SocketType.Dgram, ProtocolType.Udp))
{

}


socketTcp.Close();