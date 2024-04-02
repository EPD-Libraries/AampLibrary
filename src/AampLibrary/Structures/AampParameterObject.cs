namespace AampLibrary.Structures;

public struct AampParameterObject
{
    internal const int SIZE = 8;

    public uint Name;
    public ushort ParametersOffset;
    public ushort ParameterCount;
}
