<Query Kind="Statements">
  <NuGetReference>SIPSorcery</NuGetReference>
  <Namespace>SIPSorcery.SIP</Namespace>
  <Namespace>System.Net</Namespace>
</Query>

SIPEndPoint endpoint = new SIPEndPoint(new IPEndPoint(IPAddress.Any, 5060));
endpoint.Dump();

SIPURI.TryParse("sip:alice@192.168.1.88", out SIPURI uri);
endpoint = new SIPEndPoint(uri);
endpoint.Dump();