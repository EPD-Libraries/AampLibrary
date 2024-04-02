using System.Collections.Frozen;

namespace AampLibrary.IO.Hashing;

public class HashTable : IAampNameResolver
{
    private readonly FrozenDictionary<uint, string> _lookup;

    public HashTable(IDictionary<uint, string> values)
    {
        _lookup = FrozenDictionary.ToFrozenDictionary(values);
    }

    public HashTable(FrozenDictionary<uint, string> values)
    {
        _lookup = values;
    }

    public string GetName(uint hash)
    {
        return _lookup[hash];
    }
}
