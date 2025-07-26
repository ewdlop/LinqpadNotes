<Query Kind="Statements">
  <Namespace>System.Collections.Concurrent</Namespace>
</Query>

System.Collections.Concurrent.ConcurrentBag<int> x = new System.Collections.Concurrent.ConcurrentBag<int>();
x.Add(1);
x.Add(2);
x.Add(3);
x.Add(4);
x.Add(5);
x.Add(6);
x.Dump();

List<int> y = new List<int>();
y.Add(1);
y.Add(2);
y.Dump();