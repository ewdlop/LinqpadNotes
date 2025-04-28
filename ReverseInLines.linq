<Query Kind="Statements" />

string s =
"""
循环处理每一行：我们使用一个循环来处理每一行。对于每一行，我们计算出当前行的开始和结束索引，并提取出对应的子字符串。如果子字符串长度不足 n，则用空格填充
""";
int lineLength = 10;

Console.WriteLine(Reverse(s,10));

var result = ToCharArray(s,10);
result = result.Dump();
//Rotate(result);

Console.Write(result);

string Reverse(string s, int lineLength)
{
	int remainder = s.Length % lineLength;
	int times = (s.Length - remainder) / lineLength;
	return string.Join(Environment.NewLine, Enumerable.Range(0, times + 1).Select(i =>
	{
		var ce = s.Substring(i * lineLength, Math.Min(s.Length - i * lineLength, lineLength));
		if (ce.Length < lineLength) ce = $"{ce}{new string(Enumerable.Repeat(' ', lineLength - ce.Length).ToArray())}";
		return new string(ce.Reverse().ToArray());
	}));
}

static char[][] ToCharArray(string s, int lineLength)
{
	int remainder = s.Length % lineLength;
	int times = (s.Length - remainder) / lineLength;
	char[][] result = new char[times + 1][];
	for (int i = 0; i < times; i++)
	{
		result[i] = [.. s.Substring(i * lineLength, lineLength)];
	}
	result[times] = [.. s[^remainder..]];
	return result;
}

//only wokrs for n by n
void Rotate(char[][] matrix)
{
	int n = matrix.Length;
	for (int i = 0; i < (n + 1) / 2; i++)
	{
		for (int j = 0; j < n / 2; j++)
		{
			(matrix[n - j - 1][i], matrix[n - i - 1][n - j - 1], matrix[j][n - i - 1], matrix[i][j]) = (
				matrix[n - i - 1][n - j - 1],
				matrix[j][n - i - 1],
				matrix[i][j],
				matrix[n - 1 - j][i]
			);
		}
	}
}

public class Program
{
	public static void Main()
	{
		string s = @"
循环处理每一行：我们使用一个循环来处理每一行。
对于每一行，我们计算出当前行的开始和结束索引，并提取出对应的子字符串。
如果子字符串长度不足 n，则用空格填充
";
		int lineLength = 10;
		Console.WriteLine(ReverseInLines(s, lineLength));
	}

	public static string ReverseInLines(string s, int lineLength)
	{
		if (string.IsNullOrEmpty(s)) return string.Empty;

		int length = s.Length;
		int fullChunks = length / lineLength;
		int remainder = length % lineLength;

		// 預估容量，省去StringBuilder的頻繁擴容
		// (滿行數 + 可能的最後一行) * (lineLength + 換行符號)
		var sb = new StringBuilder((fullChunks + (remainder > 0 ? 1 : 0)) * (lineLength + Environment.NewLine.Length));

		// 處理前面滿行的部分
		for (int i = 0; i < fullChunks; i++)
		{
			int start = i * lineLength;
			// 直接使用 Substring 取得定長區段
			string chunk = s.Substring(start, lineLength);
			// 反轉
			for (int j = lineLength - 1; j >= 0; j--)
			{
				sb.Append(chunk[j]);
			}
			sb.Append('\n');
		}

		// 處理最後不足一行的部分（若有）
		if (remainder > 0)
		{
			int start = fullChunks * lineLength;
			string chunk = s.Substring(start, remainder);

			// 先反轉再補空格
			for (int j = remainder - 1; j >= 0; j--)
			{
				sb.Append(chunk[j]);
			}

			// 補足至 lineLength 長度
			for (int k = 0; k < lineLength - remainder; k++)
			{
				sb.Append(' ');
			}
			sb.Append('\n');
		}

		return sb.ToString();
	}
}
