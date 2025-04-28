<Query Kind="Program" />

namespace FamilyProjectionSimulation
{
	// 定义家庭成员类
	class FamilyMember
	{
		public string Name { get; set; }
		public string Role { get; set; }
		public string EmotionalState { get; set; }

		public FamilyMember(string name, string role, string emotionalState)
		{
			Name = name;
			Role = role;
			EmotionalState = emotionalState;
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			// 创建父母和子女对象
			var parent = new FamilyMember("Alice", "Parent", "Anxious");
			var child = new FamilyMember("Bob", "Child", "Neutral");

			Console.WriteLine($"初始状态：{parent.Name}（{parent.Role}）的情绪状态：{parent.EmotionalState}");
			Console.WriteLine($"初始状态：{child.Name}（{child.Role}）的情绪状态：{child.EmotionalState}");

			// 模拟家庭投射过程
			ProjectEmotionalState(parent, child);

			Console.WriteLine($"投射后：{parent.Name}（{parent.Role}）的情绪状态：{parent.EmotionalState}");
			Console.WriteLine($"投射后：{child.Name}（{child.Role}）的情绪状态：{child.EmotionalState}");
		}

		// 定义投射情绪状态的方法
		static void ProjectEmotionalState(FamilyMember parent, FamilyMember child)
		{
			// 父母将自身的情绪状态投射到子女身上
			child.EmotionalState = parent.EmotionalState;
			Console.WriteLine($"{parent.Name}将其{parent.EmotionalState}的情绪投射给{child.Name}。");
		}
	}
}

namespace FamilySystem
{
	// Define a FamilyMember class
	class FamilyMember
	{
		public string Name { get; set; }
		public string Role { get; set; }
		public string EmotionalState { get; set; }
		public string Action { get; private set; }

		public FamilyMember(string name, string role, string emotionalState)
		{
			Name = name;
			Role = role;
			EmotionalState = emotionalState;
			Action = "Neutral";
		}

		public void ReactToEmotion(FamilyMember other)
		{
			if (other.EmotionalState == "Unresolved Anger")
			{
				EmotionalState = "Hurt";
				Action = "Distance";
				Console.WriteLine($"{Name} ({Role}) feels betrayed by {other.Name} ({other.Role}). Emotional state: {EmotionalState}. Action: {Action}.");
			}
		}

		public void Betray(FamilyMember parent)
		{
			EmotionalState = "Unresolved Anger";
			Action = "Reject Parent";
			Console.WriteLine($"{Name} ({Role}) betrays {parent.Name} ({parent.Role}). Emotional state: {EmotionalState}. Action: {Action}.");
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			// Create a parent and child
			var parent = new FamilyMember("Alice", "Parent", "Stable");
			var child = new FamilyMember("Bob", "Child", "Neutral");

			Console.WriteLine($"Initial state:\n{parent.Name} ({parent.Role}) - Emotional State: {parent.EmotionalState}\n" +
							  $"{child.Name} ({child.Role}) - Emotional State: {child.EmotionalState}\n");

			// Simulate the betrayal process
			Console.WriteLine("\n--- Betrayal Process Begins ---\n");
			child.Betray(parent);
			parent.ReactToEmotion(child);

			// Final state
			Console.WriteLine("\n--- Final State ---");
			Console.WriteLine($"{parent.Name} ({parent.Role}) - Emotional State: {parent.EmotionalState}, Action: {parent.Action}");
			Console.WriteLine($"{child.Name} ({child.Role}) - Emotional State: {child.EmotionalState}, Action: {child.Action}");
		}
	}
}

namespace BetrayProcess
{
	class Program
	{
		static void Main(string[] args)
		{
			// Create a parent process
			var parent = Process.GetCurrentProcess();
			Console.WriteLine($"Parent Process: {parent.ProcessName} (PID: {parent.Id})");

			// Simulate spawning a "child" process
			Console.WriteLine("\n--- Spawning Child Process ---");
			ProcessStartInfo childInfo = new ProcessStartInfo("dotnet")
			{
				Arguments = "ChildProcess.dll",
				CreateNoWindow = true,
				UseShellExecute = false
			};

			var childProcess = Process.Start(childInfo);

			if (childProcess != null)
			{
				Console.WriteLine($"Child Process Spawned: {childProcess.ProcessName} (PID: {childProcess.Id})");
				Thread.Sleep(3000); // Simulate interaction time

				Console.WriteLine("\n--- Betrayal Begins ---");
				Console.WriteLine($"Child Process {childProcess.Id} attempts to terminate Parent Process {parent.Id}...");
				try
				{
					parent.Kill();
					Console.WriteLine($"Parent Process {parent.Id} terminated by Child Process {childProcess.Id}. Betrayal complete.");
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Failed to terminate Parent Process: {ex.Message}");
				}
			}
			else
			{
				Console.WriteLine("Failed to spawn child process.");
			}

			Console.WriteLine("\n--- End of Process Interaction ---");
		}
	}
}

