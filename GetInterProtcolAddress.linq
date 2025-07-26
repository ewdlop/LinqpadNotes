<Query Kind="Statements">
  <Namespace>System.Net</Namespace>
  <Namespace>System.Net.Sockets</Namespace>
</Query>

string localIp = Dns.GetHostEntry(Dns.GetHostName())
	.AddressList
	.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork)?
	.ToString();

Dns.GetHostEntry(Dns.GetHostName())
	.AddressList.Dump();
	
localIp.Dump();