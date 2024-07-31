using System.Runtime.InteropServices;

namespace AampLibrary.Primitives;

public unsafe struct Curve
{
    public uint A, B;
    public fixed float Points[30];
}
