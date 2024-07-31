using AampLibrary.Structures;
using AampLibrary.Yaml;
using Revrs;
using System.Buffers;
using System.Runtime.InteropServices.Marshalling;
using System.Text;

namespace AampLibrary;

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

    public string ToYaml()
    {
        ArrayBufferWriter<byte> writer = new();
        AampYamlWriter.Write(writer, this);
        return Encoding.UTF8.GetString(writer.WrittenSpan);
    }

    public void ToYaml(IBufferWriter<byte> writer)
    {
        AampYamlWriter.Write(writer, this);
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
