using System.Buffers.Binary;

namespace AampLibrary.IO;

public class BinaryUInt32Comparer : IComparer<uint>
{
    public static readonly BinaryUInt32Comparer Instance = new();

    public int Compare(uint x, uint y)
    {
        uint binaryX = BinaryPrimitives.ReverseEndianness(x);
        uint binaryY = BinaryPrimitives.ReverseEndianness(y);

        if (binaryX > binaryY) {
            return 1;
        }

        if (binaryX < binaryY) {
            return -1;
        }

        return 0;
    }
}