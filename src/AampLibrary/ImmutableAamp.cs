using AampLibrary.Structures;
using Revrs;
using Revrs.Extensions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;

namespace AampLibrary;

public ref struct ImmutableAamp
{
    public AampHeader Header;
    public Span<byte> Lists;
    public Span<byte> Objects;
    public Span<byte> ParameterData;

    public readonly ref AampParameterList IO {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref Lists[0..].Read<AampParameterList>();
    }

    public ImmutableAamp(ref RevrsReader reader)
    {
        reader.Endianness = Endianness.Little;
        Header = reader.Read<AampHeader>();

        if (Header.Magic is not AampHeader.MAGIC) {
            throw new InvalidDataException("""
                Invalid magic.
                """);
        }

        if (!Header.Flags.HasFlag(AampFlags.IsLittleEndian)) {
            throw new InvalidDataException("""
                Only little endian parameter archives are supported.
                """);
        }

        if (!Header.Flags.HasFlag(AampFlags.IsUtf8)) {
            throw new InvalidDataException("""
                Only UTF-8 parameter archives are supported.
                """);
        }

        reader.Move(Header.ParameterIOOffset);
        Lists = reader.Data[reader.Position..];
        Objects = reader.Data[reader.Position..];

        reader.Move(Header.ListCount * AampParameterList.SIZE);
        ParameterData = reader.Data[reader.Position..];
    }

    public readonly ref AampParameterList GetList(int index, int offsetRelativeToLists, out int relativeListOffset)
    {
        return ref Lists[(relativeListOffset = offsetRelativeToLists + index * AampParameterList.SIZE)..]
            .Read<AampParameterList>();
    }

    public readonly ref AampParameterObject GetObject(int index, int offsetRelativeToObjects, out int relativeObjectOffset)
    {
        return ref Objects[(relativeObjectOffset = offsetRelativeToObjects + index * AampParameterObject.SIZE)..]
            .Read<AampParameterObject>();
    }

    public readonly ref AampParameter GetParameter(int index, int offsetRelativeToObjects, out int relativeParameterOffset)
    {
        return ref ParameterData[(relativeParameterOffset = offsetRelativeToObjects + index * AampParameter.SIZE)..]
            .Read<AampParameter>();
    }

    public unsafe string ReadString(int offsetRelativeToParameter)
    {
        fixed (byte* ptr = &ParameterData[offsetRelativeToParameter]) {
            return Utf8StringMarshaller.ConvertToManaged(ptr)
                ?? string.Empty;
        }
    }
}
