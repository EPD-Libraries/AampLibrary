namespace AampLibrary.Extensions;

public static class EnumExtension
{
    public static IEnumerable<T> EnumerateFlags<T>(this T flags) where T : struct, Enum
    {
        T[] values = Enum.GetValues<T>();

        // Check for None
        if (Convert.ToInt32(flags) == 0) {
            yield return values[0];
            yield break;
        }

        // Assume the first value is None (0)
        foreach (T flag in values[1..]) {
            if (flags.HasFlag(flag)) {
                yield return flag;
            }
        }
    }
}
