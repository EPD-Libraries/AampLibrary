namespace AampLibrary.IO;

public interface IAampKeyProvider
{
    public string? this[uint crcHash] { get; }
}
