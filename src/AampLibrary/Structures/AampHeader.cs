namespace AampLibrary.Structures;

[Flags]
public enum AampFlags : int
{
    None = 0,
    IsLittleEndian = 1 << 0,
    IsUtf8 = 1 << 1,
}

public struct AampHeader
{
    public const uint MAGIC = 0x504D4141;
    public const int SIZE = 0x30;

    public uint Magic;
    public uint Version;
    public AampFlags Flags;
    public uint FileSize;
    public uint ParameterIOVersion;
    public int ParameterIOOffset;
    public int ListCount;
    public int ObjectCount;
    public int ParameterCount;
    public int DataSectionSize;
    public int StringSectionSize;
    public int UnknownSectionSize;
}
