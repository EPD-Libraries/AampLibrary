namespace AampLibrary.Structures;

public struct AampParameterList
{
    internal const int SIZE = 0xC;

    public uint Name;
    public ushort ListsOffset;
    public ushort ListCount;
    public ushort ObjectsOffset;
    public ushort ObjectCount;
}
