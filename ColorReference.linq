<Query Kind="Program">
  <Namespace>System.Drawing</Namespace>
</Query>

public class ColorSpanGenerator
{
	public string GenerateColoredSpan(Color color, string text)
	{
		string hexColor = ColorToHex(color);
		return $"<span style=\"color: {hexColor}\">{text}</span>";
	}

	private string ColorToHex(Color color)
	{
		return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
	}

	public string GenerateAllColorSpans()
	{
		var sb = new StringBuilder();
		sb.AppendLine("<div style=\"font-family: Arial, sans-serif;\">");

		// Get all KnownColor values
		var allColors = Enum.GetValues(typeof(KnownColor))
			.Cast<KnownColor>()
			.Select(k => Color.FromKnownColor(k))
			.Where(c => !c.IsSystemColor) // Exclude system colors
			.OrderBy(c => c.GetHue()) // Sort by hue
			.ThenBy(c => c.GetSaturation())
			.ThenBy(c => c.GetBrightness());

		foreach (var color in allColors)
		{
			string colorName = color.Name;
			string hexCode = ColorToHex(color);

			// Create a color swatch with the name and hex code
			sb.AppendLine($"<div style=\"margin: 5px; padding: 5px; border: 1px solid #ccc;\">");
			sb.AppendLine($"  <div style=\"display: inline-block; width: 20px; height: 20px; background-color: {hexCode}; vertical-align: middle; margin-right: 10px;\"></div>");
			sb.AppendLine($"  {GenerateColoredSpan(color, colorName)}");
			sb.AppendLine($"  <span style=\"color: #666; margin-left: 10px;\">{hexCode}</span>");
			sb.AppendLine("</div>");
		}

		sb.AppendLine("</div>");
		return sb.ToString();
	}

	public string GenerateColorPalette()
	{
		var sb = new StringBuilder();
		sb.AppendLine("<div style=\"display: flex; flex-wrap: wrap; gap: 10px; padding: 20px;\">");

		// Get all web colors (excluding system colors)
		var webColors = Enum.GetValues(typeof(KnownColor))
			.Cast<KnownColor>()
			.Select(k => Color.FromKnownColor(k))
			.Where(c => !c.IsSystemColor)
			.OrderBy(c => c.GetHue())
			.ThenBy(c => c.GetBrightness());

		// Group colors by hue ranges
		var colorGroups = webColors.GroupBy(c =>
		{
			double hue = c.GetHue();
			if (hue >= 330 || hue < 30) return "Reds";
			if (hue < 90) return "Yellows";
			if (hue < 150) return "Greens";
			if (hue < 210) return "Cyans";
			if (hue < 270) return "Blues";
			return "Purples";
		});

		foreach (var group in colorGroups)
		{
			sb.AppendLine($"<div style=\"flex: 1; min-width: 200px;\">");
			sb.AppendLine($"<h3>{group.Key}</h3>");

			foreach (var color in group)
			{
				string hexCode = ColorToHex(color);
				string textColor = color.GetBrightness() > 0.5 ? "#000000" : "#FFFFFF";

				sb.AppendLine($"<div style=\"background-color: {hexCode}; color: {textColor}; padding: 8px; margin: 4px; border-radius: 4px;\">");
				sb.AppendLine($"  {color.Name}<br/>");
				sb.AppendLine($"  <small>{hexCode}</small>");
				sb.AppendLine("</div>");
			}

			sb.AppendLine("</div>");
		}

		sb.AppendLine("</div>");
		return sb.ToString();
	}

	// HTML generator for a complete color reference page
	public string GenerateColorReferencePage()
	{
		var sb = new StringBuilder();
		sb.AppendLine("<!DOCTYPE html>");
		sb.AppendLine("<html>");
		sb.AppendLine("<head>");
		sb.AppendLine("  <title>C# Color Reference</title>");
		sb.AppendLine("  <style>");
		sb.AppendLine("    body { font-family: Arial, sans-serif; margin: 20px; }");
		sb.AppendLine("    .color-group { margin-bottom: 30px; }");
		sb.AppendLine("    .color-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(200px, 1fr)); gap: 10px; }");
		sb.AppendLine("    .color-item { border: 1px solid #ddd; padding: 10px; border-radius: 4px; }");
		sb.AppendLine("    .color-swatch { height: 100px; border-radius: 4px; margin-bottom: 10px; }");
		sb.AppendLine("    .color-info { font-size: 14px; }");
		sb.AppendLine("    .code-sample { background: #f5f5f5; padding: 5px; font-family: monospace; }");
		sb.AppendLine("  </style>");
		sb.AppendLine("</head>");
		sb.AppendLine("<body>");

		// Add color palette
		sb.AppendLine("<h2>Color Palette</h2>");
		sb.AppendLine(GenerateColorPalette());

		// Add all individual colors
		sb.AppendLine("<h2>All Colors</h2>");
		sb.AppendLine(GenerateAllColorSpans());

		sb.AppendLine("</body>");
		sb.AppendLine("</html>");

		return sb.ToString();
	}
}

class Program
{
	static void Main(string[] args)
	{
		var generator = new ColorSpanGenerator();

		// Generate complete color reference
		string colorReference = generator.GenerateColorReferencePage();

		// Save to file
		System.IO.File.WriteAllText(@"C:\Users\Owner\OneDrive\Documents\LINQPad Queries\ColorReference.html", colorReference);

		Console.WriteLine("Color reference has been generated and saved to ColorReference.html");

		// You can also generate individual components:
		// Console.WriteLine(generator.GenerateAllColorSpans());
		// Console.WriteLine(generator.GenerateColorPalette());
	}
}