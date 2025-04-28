<Query Kind="Program">
  <NuGetReference>HtmlAgilityPack</NuGetReference>
  <Namespace>HtmlAgilityPack</Namespace>
</Query>

using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

public static class HtmlMerge
{
	/// <summary>
	/// 兩節點 lazy-merge：相同標籤才往下比對，差異節點直接 yield。
	/// </summary>
	public static IEnumerable<HtmlNode> MergeNodes(HtmlNode left, HtmlNode right)
	{
		if (left == null && right == null) yield break;
		if (left == null) { yield return right; yield break; }
		if (right == null) { yield return left; yield break; }

		// 標籤不同 → 視為差異，各自輸出
		if (!left.Name.Equals(right.Name))
		{
			yield return left;
			yield return right;
			yield break;
		}

		//--------------------------------------------------
		// 1) 建立空殼 <tag></tag>
		//--------------------------------------------------
		var merged = HtmlNode.CreateNode($"<{left.Name}></{left.Name}>");

		//--------------------------------------------------
		// 2) 合併屬性（右邊覆蓋左邊）
		//--------------------------------------------------
		// 2) 合併屬性：右邊覆蓋左邊
		foreach (var attr in left.Attributes)
			merged.SetAttributeValue(attr.Name, attr.Value);   // 先貼左邊

		foreach (var attr in right.Attributes)
			merged.SetAttributeValue(attr.Name, attr.Value);   // 再貼右邊，會自動覆蓋


		//--------------------------------------------------
		// 3) 合併文字（可自行改為 diff 或標記）
		//--------------------------------------------------
		merged.InnerHtml = (left.InnerText ?? string.Empty) +
						   (right.InnerText ?? string.Empty);

		//--------------------------------------------------
		// 4) 遞迴子節點
		//--------------------------------------------------
		var lKids = left.ChildNodes.ToList();
		var rKids = right.ChildNodes.ToList();
		int max = System.Math.Max(lKids.Count, rKids.Count);

		for (int i = 0; i < max; i++)
		{
			var l = i < lKids.Count ? lKids[i] : null;
			var r = i < rKids.Count ? rKids[i] : null;

			foreach (var sub in MergeNodes(l, r))
				merged.AppendChild(sub);
		}

		//--------------------------------------------------
		// 5) 把結果丟回上一層
		//--------------------------------------------------
		yield return merged;
	}
}
