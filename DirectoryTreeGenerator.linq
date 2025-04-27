<Query Kind="Program" />

public class DirectoryTreeGenerator
{
	public class TreeNode
	{
		public string Name { get; set; }
		public string Comment { get; set; }
		public bool IsFile { get; set; }
		public List<TreeNode> Children { get; set; }

		public TreeNode(string name, string comment = "", bool isFile = false)
		{
			Name = name;
			Comment = comment;
			IsFile = isFile;
			Children = new List<TreeNode>();
		}
	}

	public void GenerateFromText(string treeText, string baseDirectory)
	{
		var lines = treeText.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
		var root = ParseTreeText(lines);
		CreateDirectoryStructure(root, baseDirectory);
	}

	private TreeNode ParseTreeText(string[] lines)
	{
		var root = new TreeNode("root");
		var currentPath = new Stack<TreeNode>();
		currentPath.Push(root);
		int previousIndentLevel = -1;

		foreach (var line in lines)
		{
			if (string.IsNullOrWhiteSpace(line)) continue;

			var indentMatch = Regex.Match(line, @"^[\s│├└─]*");
			int currentIndentLevel = indentMatch.Length;

			// Adjust stack based on indent level
			while (currentPath.Count > 1 && previousIndentLevel >= currentIndentLevel)
			{
				currentPath.Pop();
				previousIndentLevel -= 2;
			}

			// Parse the line
			var cleanLine = line.TrimStart(' ', '│', '├', '└', '─');
			var parts = cleanLine.Split(new[] { '#' }, 2);
			var name = parts[0].Trim();
			var comment = parts.Length > 1 ? parts[1].Trim() : "";

			// Create node
			var node = new TreeNode(name, comment, !name.EndsWith("/"));
			currentPath.Peek().Children.Add(node);

			if (!node.IsFile)
			{
				currentPath.Push(node);
				previousIndentLevel = currentIndentLevel;
			}
		}

		return root;
	}

	private void CreateDirectoryStructure(TreeNode node, string currentPath)
	{
		foreach (var child in node.Children)
		{
			var newPath = Path.Combine(currentPath, child.Name);

			try
			{
				if (child.IsFile)
				{
					if (!File.Exists(newPath))
					{
						File.Create(newPath).Close();
						Console.WriteLine($"Created file: {newPath}");

						// If there's a comment, add it as first line in the file
						if (!string.IsNullOrEmpty(child.Comment))
						{
							File.WriteAllText(newPath, $"// {child.Comment}\n");
						}
					}
					else
					{
						Console.WriteLine($"File already exists: {newPath}");
					}
				}
				else
				{
					if (!Directory.Exists(newPath))
					{
						Directory.CreateDirectory(newPath);
						Console.WriteLine($"Created directory: {newPath}");
					}
					CreateDirectoryStructure(child, newPath);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error creating {(child.IsFile ? "file" : "directory")} {newPath}: {ex.Message}");
			}
		}
	}
}

class Program
{
	static void Main(string[] args)
	{
		Console.WriteLine("Enter base directory name:");
		//string baseDir = Util.ReadLine();
		string baseDir = args.Any() ? args[0] : Console.ReadLine();
		
		//Console.WriteLine("Enter tree structure (press Ctrl+Z on Windows or Ctrl+D on Unix and Enter when done):");
		
		string treeText = args.Length > 1 ? args[1] :
"""
Tetris3D/
├── .gitignore
├── CMakeLists.txt
├── README.md
├── LICENSE
│
├── src/                    # Source files
│   ├── main.cpp
│   ├── Game.cpp
│   ├── Graphics.cpp
│   ├── Audio.cpp
│   ├── Input.cpp
│   ├── UI.cpp
│   ├── ParticleSystem.cpp
│   ├── Camera.cpp
│   ├── DebugRenderer.cpp
│   └── ProfilerSystem.cpp
│
├── include/               # Header files
│   ├── Game.h
│   ├── Graphics.h
│   ├── Audio.h
│   ├── Input.h
│   ├── UI.h
│   ├── ParticleSystem.h
│   ├── Camera.h
│   ├── DebugRenderer.h
│   └── ProfilerSystem.h
│
├── shaders/              # HLSL shader files
│   ├── VertexShader.hlsl
│   ├── PixelShader.hlsl
│   └── ParticleShader.hlsl
│
├── assets/               # Game assets
│   ├── textures/
│   ├── sounds/
│   ├── fonts/
│   └── models/
│
├── tests/                # Test files
│   ├── CMakeLists.txt
│   ├── TestGame.cpp
│   ├── TestGraphics.cpp
│   └── TestAudio.cpp
│
├── docs/                 # Documentation
│   ├── api/
│   ├── design/
│   └── README.md
│
├── scripts/              # Build/deployment scripts
│   ├── build.bat
│   ├── package.bat
│   └── deploy.bat
│
├── extern/               # External dependencies
│   └── README.md
│
└── build/               # Build output (git ignored)
    ├── bin/
    ├── lib/
    └── obj/
"""
		;
		DirectoryTreeGenerator generator = new DirectoryTreeGenerator();
		try
		{
			generator.GenerateFromText(treeText, baseDir);
			Console.WriteLine("\nDirectory structure created successfully!");
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error: {ex.Message}");
		}
	}
}