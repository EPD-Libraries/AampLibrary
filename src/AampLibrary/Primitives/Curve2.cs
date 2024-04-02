namespace AampLibrary.Primitives;

public unsafe struct Curve2
{
    public Curve A;
    public Curve B;

    public ref Curve this[int index] {
        get => ref AsSpan()[index];
    }

    public Span<Curve> AsSpan()
    {
        fixed (Curve* ptr = &A) {
            return new(ptr, 2);
        }
    }
}
