<Query Kind="Statements">
  <NuGetReference>SIPSorcery</NuGetReference>
  <Namespace>System.Net</Namespace>
  <Namespace>SIPSorcery.SIP</Namespace>
  <Namespace>SIPSorcery.Media</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

Console.InputEncoding = System.Text.UTF8Encoding.UTF8;
CancellationTokenSource source = new CancellationTokenSource();
// 被叫方監聽 5060
var sipTransport = new SIPSorcery.SIP.SIPTransport();
sipTransport.AddSIPChannel(new SIPSorcery.SIP.SIPUDPChannel(new System.Net.IPEndPoint(IPAddress.Loopback, 5061)));
var userAgent = new SIPSorcery.SIP.App.SIPUserAgent(sipTransport, null);
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
userAgent.OnIncomingCall += async (ua, req) =>
{
	VoIPMediaSession voipMediaSession = new VoIPMediaSession();
	var uas = ua.AcceptCall(req);
	await ua.Answer(uas, voipMediaSession);
};
userAgent.OnCallHungup += (sIPDialogue) =>
{
	$"📞 Call hung up with {sIPDialogue.CallId}".Dump();
	source.Cancel();
};
Console.WriteLine("等待來電中...");
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
