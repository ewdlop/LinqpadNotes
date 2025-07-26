<Query Kind="Statements">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

2.Dump();

var t = await Task.WhenAny([new Y(()=>{1.Dump();})]);
if(t is Y y)
{
	2.Dump();
}


public class Y : Task
{
	public Action action2;
	public Y(Action action) : base(action)
	{
	}
}