<Query Kind="Statements">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

System.Threading.Thread.CurrentThread.Dump();
await Task.Delay(1000);
System.Threading.Thread.CurrentThread.Dump();