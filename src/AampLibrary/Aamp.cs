using AampLibrary.IO.Hashing;
using AampLibrary.Structures;
using Revrs;

namespace AampLibrary;

public class Aamp : ParameterList
{
    public AampFlags Flags { get; set; } = AampFlags.IsLittleEndian | AampFlags.IsUtf8;

    public static Aamp FromBinary(Span<byte> data, IAampNameResolver? aampNameResolver = null)
    {
        RevrsReader reader = new(data);
        ImmutableAamp aamp = new(ref reader);
        return FromImmutable(ref aamp, aampNameResolver);
    }

    public static Aamp FromImmutable(ref ImmutableAamp aamp, IAampNameResolver? aampNameResolver = null)
    {
        return new(ref aamp, aampNameResolver);
    }

    public Aamp()
    {

    }

    internal Aamp(ref ImmutableAamp aamp, IAampNameResolver? aampNameResolver = null)
        : base(ref aamp, ref aamp.IO, aampNameResolver ?? HashNameResolver.Shared)
    {

    }
}
