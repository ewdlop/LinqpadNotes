<Query Kind="Statements">
  <NuGetReference>SIPSorcery</NuGetReference>
  <Namespace>SIPSorcery.SIP</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>SIPSorcery.SIP.App</Namespace>
  <Namespace>SIPSorcery.Media</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

Console.InputEncoding = System.Text.UTF8Encoding.UTF8;
CancellationTokenSource source = new CancellationTokenSource();
// 主叫方監聽 5062
var sipTransport = new SIPTransport();
sipTransport.AddSIPChannel(new SIPUDPChannel(new IPEndPoint(IPAddress.Loopback, 5069)));

var userAgent = new SIPUserAgent(sipTransport, null);
sipTransport.SIPTransportRequestReceived += async (lep, rep, req) =>
{
	lep.Dump();
	rep.Dump();
	req.Dump();
};
sipTransport.SIPTransportResponseReceived += async (lep, rep, req) =>
{
	lep.Dump();
	rep.Dump();
	req.Dump();
};
userAgent.OnCallHungup += (sIPDialogue) =>
{
	$"📞 Call hung up with {sIPDialogue.CallId}".Dump();
	source.Cancel();
};
VoIPMediaSession voipMediaSession = new VoIPMediaSession();

bool callResult = await userAgent.Call("sip:ray@127.0.0.1:5061", null, null, voipMediaSession);
Console.WriteLine($"撥號結果: {callResult}");
Task<string> callTask = Util.ReadLineAsync(cancelToken: source.Token);
try
{
	await callTask;
	userAgent.Hangup();
	Console.WriteLine("關閉");
	sipTransport.Shutdown();
	return;
}
catch (OperationCanceledException)
{
	userAgent.Hangup();
	Console.WriteLine("關閉");
	sipTransport.Shutdown();
	return;
}
