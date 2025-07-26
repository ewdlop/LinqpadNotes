<Query Kind="Statements">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

CancellationTokenSource source = new CancellationTokenSource();
Task t = Task.Run(async ()=> {
	await Task.Delay(10000, source.Token);
},source.Token);
try
{
	_ = Task.Run(() =>
	{
		source.Cancel();
	});
	await t;
	
}
catch(OperationCanceledException)
{
	"Cancel".Dump();

	t.IsFaulted.Dump();
	t.IsCompleted.Dump();
	t.IsCompletedSuccessfully.Dump();
}
catch(Exception e)
{

	t.IsFaulted.Dump();
	t.IsCompleted.Dump();
	t.IsCompletedSuccessfully.Dump();
}