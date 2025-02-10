namespace _08;

internal static partial class Program
{
    /// <summary>
    /// An insertion sort that doesn't allocate any memory.
    /// It rearranges the existing string.
    /// User must ensure the underlying string is not used whilst
    /// this sort is running.
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private static string InsertionSort(this string str)
    {
        InsertionSort(GetSpan(str), str.Length - 1);
        return str;
    }
    
    /// <summary>
    /// Danger: Unsafe creates a mutable string. User must make sure the string is not used outside this span's usage.
    /// </summary>
    /// <param name="str"></param>
    /// <returns>mutable Span&lt;char&gt; of str</returns>
    private static unsafe Span<char> GetSpan(string str)
    {
        if (str.Length == 0)
            throw new Exception();
        
        fixed (char* strPtr = &str.AsSpan()[0])
            return new Span<char>(strPtr, str.Length);
    }
    
    private static void InsertionSort(Span<char> str, int n)
    {
        if (n <= 0) 
            return;
        
        InsertionSort(str, n - 1);
        var x = str[n];
        var j = n - 1;
        while (j >= 0 && str[j] > x)
        {
            str[j + 1] = str[j];
            j--;
        }

        str[j + 1] = x;
    }
}