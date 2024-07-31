using System.Collections.Frozen;
using AampLibrary.IO.Hashing;

namespace AampLibrary.IO;

public class AampNameProvider(IDictionary<uint, string> lookup) : IAampNameProvider
{
    private readonly FrozenDictionary<uint, string> _lookup = lookup.ToFrozenDictionary();

    public AampNameProvider(IEnumerable<string> strings) : this(strings.ToDictionary(x => Crc32.ComputeHash(x), x => x))
    {
    }

    public string? this[uint crcHash] => _lookup.GetValueOrDefault(crcHash);
}
