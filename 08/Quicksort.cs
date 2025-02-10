namespace _08;

internal static partial class Program
{
    /// <summary>
    /// A quick sort that doesn't allocate any memory.
    /// It rearranges the existing string.
    /// User must ensure the underlying string is not used whilst
    /// this sort is running.
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private static string Quicksort(this string str)
    {
        Quicksort(GetSpan(str), 0, str.Length - 1);
        return str;
    }

    private static void Quicksort(Span<char> list, int low, int high)
    {
        if (low > high)
            return;

        var middle = Split(list, low, high);
        Quicksort(list, low, middle - 1);
        Quicksort(list, middle + 1, high);
    }
    
    private static int Split(Span<char> list, int low, int high)
    {
        var part_element = list[low];

        while(true){
            while (low < high && part_element.CompareTo(list[high]) == 1)
                high--;
            if (low >= high)
                break;
            list[low++] = list[high];

            while(low < high && list[low].CompareTo(part_element) == 1)
                low++;
            if (low >= high)
                break;
            list[high--] = list[low];
        }

        list[high] = part_element;
        return high;
    }
}