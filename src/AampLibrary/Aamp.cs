using AampLibrary.IO;
using AampLibrary.Yaml;
using Revrs;
using Revrs.Buffers;
using System.Buffers;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AampLibrary;

[Flags]
public enum AampFlags : int
{
    None = 0,
    IsLittleEndian = 1 << 0,
    IsUtf8 = 1 << 1,
}

public class Aamp : ParameterList
{
    public AampFlags Flags { get; set; } = AampFlags.IsLittleEndian | AampFlags.IsUtf8;
    public string Type { get; set; } = "xml";

    public static Aamp FromBinary(Span<byte> data)
    {
        RevrsReader reader = new(data);
        ImmutableAamp aamp = new(ref reader);
        return FromImmutable(ref aamp);
    }

    public static Aamp FromImmutable(ref ImmutableAamp aamp)
    {
        return new(ref aamp);
    }

    public static Aamp FromYaml(string yaml)
    {
        int size = Encoding.UTF8.GetByteCount(yaml);
        using ArraySegmentOwner<byte> data = ArraySegmentOwner<byte>.Allocate(size);
        Encoding.UTF8.GetBytes(yaml, data.Segment);
        return AampYamlParser.FromYaml(new(data.Segment));
    }

    public static Aamp FromYaml(ArraySegment<byte> utf8)
    {
        return AampYamlParser.FromYaml(new(utf8));
    }

    public string ToYaml(IAampKeyProvider? keyProvider = null)
    {
        ArrayBufferWriter<byte> writer = new();
        AampYamlWriter.Write(writer, this, keyProvider);
        return Encoding.UTF8.GetString(writer.WrittenSpan);
    }

    public void ToYaml(IBufferWriter<byte> writer, IAampKeyProvider? keyProvider = null)
    {
        AampYamlWriter.Write(writer, this, keyProvider);
    }

    public Aamp()
    {
    }

    internal unsafe Aamp(ref ImmutableAamp aamp)
        : base(ref aamp, ref aamp.IO, 0)
    {
        fixed (byte* ptr = aamp.Type) {
            Type = Utf8StringMarshaller.ConvertToManaged(ptr)
                ?? string.Empty;
        }
    }
}
