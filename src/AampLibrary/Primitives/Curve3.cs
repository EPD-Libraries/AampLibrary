namespace AampLibrary.Primitives;

public unsafe struct Curve3
{
    public Curve A;
    public Curve B;
    public Curve C;

    public ref Curve this[int index] {
        get => ref AsSpan()[index];
    }

    public Span<Curve> AsSpan()
    {
        fixed (Curve* ptr = &A) {
            return new(ptr, 3);
        }
    }
}
