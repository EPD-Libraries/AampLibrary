namespace AampLibrary.IO.Hashing;

public class HashNameResolver : IAampNameResolver
{
    public static HashNameResolver Shared { get; set; } = new();

    public string GetName(uint hash)
    {
        return $"0x{hash:x8}";
    }
}
