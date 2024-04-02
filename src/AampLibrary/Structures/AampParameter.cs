using System.Runtime.InteropServices;

namespace AampLibrary.Structures;

[StructLayout(LayoutKind.Explicit)]
public struct AampParameter
{
    internal const int SIZE = 8;

    [FieldOffset(0)]
    public uint Name;

    [FieldOffset(4)]
    private readonly int _value;

    public readonly int DataOffset {
        get => _value & 0xFFFFFF;
    }

    public readonly AampParameterType Type {
        get => (AampParameterType)((_value >> 24) & 0xFF);
    }
}
