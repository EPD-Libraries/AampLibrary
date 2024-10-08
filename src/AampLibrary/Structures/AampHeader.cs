﻿namespace AampLibrary.Structures;

public struct AampHeader
{
    public const uint MAGIC = 0x504D4141;
    public const int SIZE = 0x30;

    public uint Magic;
    public int Version;
    public AampFlags Flags;
    public int FileSize;
    public int ParameterIOVersion;
    public int ParameterIOOffset;
    public int ListCount;
    public int ObjectCount;
    public int ParameterCount;
    public int DataSectionSize;
    public int StringSectionSize;
    public int UnknownSectionSize;
}
