namespace AampLibrary.Primitives;

public unsafe struct Curve1
{
    public Curve A;

    public ref Curve this[int index] {
        get => ref AsSpan()[index];
    }

    public Span<Curve> AsSpan()
    {
        fixed (Curve* ptr = &A) {
            return new(ptr, 1);
        }
    }
}
