<Query Kind="Statements" />

FindSubarrayIndicesKMP([1,2,3,4,5,6],[1]).Dump();


static List<int> FindSubarrayIndicesKMP(byte[] mainArray, byte[] subArray)
{
    List<int> indices = new List<int>();

    if (subArray.Length == 0) return indices;
    if (subArray.Length > mainArray.Length) return indices;

    // Build failure function
    int[] lps = BuildLPS(subArray);

    int i = 0; // index for mainArray
    int j = 0; // index for subArray

    while (i < mainArray.Length)
    {
        if (mainArray[i] == subArray[j])
        {
            i++;
            j++;
        }

        if (j == subArray.Length)
        {
            indices.Add(i - j); // Add starting index
            j = lps[j - 1]; // Continue searching for overlapping matches
        }
        else if (i < mainArray.Length && mainArray[i] != subArray[j])
        {
            if (j != 0)
                j = lps[j - 1];
            else
                i++;
        }
    }
    return indices;
}

static int[] BuildLPS(byte[] pattern)
{
    int[] lps = new int[pattern.Length];
    int len = 0;
    int i = 1;

    while (i < pattern.Length)
    {
        if (pattern[i] == pattern[len])
        {
            len++;
            lps[i] = len;
            i++;
        }
        else
        {
			if (len != 0)
				len = lps[len - 1];
			else
			{
				lps[i] = 0;
				i++;
			}
		}
	}
	return lps;
}