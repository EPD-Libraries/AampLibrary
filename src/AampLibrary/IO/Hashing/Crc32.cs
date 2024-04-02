namespace AampLibrary.IO.Hashing;

public class Crc32
{
    private const uint POLY = 0xEDB88320;
    private static readonly uint[] _table;

    static Crc32()
    {
        _table = new uint[256];

        uint m;
        for (uint i = 0; i < _table.Length; ++i) {
            m = i;
            for (int j = 8; j > 0; --j) {
                m = (m & 1) switch {
                    1 => (m >> 1) ^ POLY,
                    _ => m >> 1
                };
            }

            _table[i] = m;
        }
    }

    public static uint ComputeHash(ReadOnlySpan<char> src)
    {
        uint crc = 0xFFFFFFFF;
        for (int i = 0; i < src.Length; ++i) {
            uint index = ((crc) & 0xFF) ^ src[i];
            crc = (crc >> 8) ^ _table[index];
        }

        return crc;
    }

    public static uint ComputeHash(ReadOnlySpan<byte> src)
    {
        uint crc = 0xFFFFFFFF;
        for (int i = 0; i < src.Length; ++i) {
            uint index = ((crc) & 0xFF) ^ src[i];
            crc = (crc >> 8) ^ _table[index];
        }

        return crc;
    }
}
