using System.Runtime.InteropServices;

namespace AampLibrary.Primitives;

[StructLayout(LayoutKind.Sequential, Size = 0x80)]
public unsafe struct Curve
{
    private const int FLOAT_COUNT = 30;

    public uint A, B;
    private readonly float _float0;

    public readonly Span<byte> Floats {
        get {
            fixed (float* ptr = &_float0) {
                return new(ptr, 30);
            }
        }
    }
}
