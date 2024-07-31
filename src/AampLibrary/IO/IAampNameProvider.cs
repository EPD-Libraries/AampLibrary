namespace AampLibrary.IO;

public interface IAampNameProvider
{
    public string? this[uint crcHash] { get; }
}
