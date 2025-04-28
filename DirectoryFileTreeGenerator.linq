<Query Kind="Statements" />

string baseDirectory = 
"""
C:\Games\ewdlop\Bots
""";

// Define the directory structure and files
Dictionary<string, string[]> structure = new Dictionary<string, string[]>
{
	{"src/bot", new[] {"TaskManagerBot.js"}},
	{"src/bot/dialogs", new[] {"index.js", "taskDialog.js"}},
	{"src/cards", new[] {"taskCard.json", "reminderCard.json"}},
	{"src/services", new[] {
		"googleCalendarService.js",
		"outlookService.js",
		"notificationService.js"
	}},
	{"src/utils", new[] {"logger.js", "dateHelper.js"}},
	{"src", new[] {"app.js"}},
	{"tests", new[] {"bot.test.js", "taskDialog.test.js"}},
	{"", new[] {".env", "package.json", "README.md", "index.js"}}
};

try
{
	// Create base directory if it doesn't exist
	if (Directory.Exists(baseDirectory))
	{
		Console.WriteLine($"Warning: Directory '{baseDirectory}' already exists. Continuing anyway...");
	}
	else
	{
		Directory.CreateDirectory(baseDirectory);
		Console.WriteLine($"Created base directory: {baseDirectory}");
	}

	// Create directories and files
	foreach (var entry in structure)
	{
		string fullPath = Path.Combine(baseDirectory, entry.Key);

		// Create directory if it's not empty
		if (!string.IsNullOrEmpty(entry.Key))
		{
			Directory.CreateDirectory(fullPath);
			Console.WriteLine($"Created directory: {fullPath}");
		}

		// Create files
		foreach (string file in entry.Value)
		{
			string filePath = Path.Combine(fullPath, file);
			if (!File.Exists(filePath))
			{
				File.Create(filePath).Close();
				Console.WriteLine($"Created file: {filePath}");
			}
			else
			{
				Console.WriteLine($"Warning: File already exists: {filePath}");
			}
		}
	}

	Console.WriteLine("\nDirectory structure created successfully!");
}
catch (Exception ex)
{
	Console.WriteLine($"Error: {ex.Message}");
}