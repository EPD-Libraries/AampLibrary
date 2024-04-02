using AampLibrary.IO.Hashing;
using AampLibrary.Primitives;
using AampLibrary.Structures;
using Revrs;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace AampLibrary;

public class ParameterObject : Dictionary<uint, Parameter>, IDictionary<string, Parameter>
{
    public ParameterObject()
    {
    }

    public ParameterObject(int capacity) : base(capacity)
    {
    }

    public ParameterObject(IEnumerable<KeyValuePair<uint, Parameter>> collection) : base(collection)
    {
    }

    public ParameterObject(IDictionary<uint, Parameter> dictionary) : base(dictionary)
    {
    }

    internal ParameterObject(ref ImmutableAamp aamp, ref AampParameterObject parameterObj, IAampNameResolver aampNameResolver) : base(parameterObj.ParameterCount)
    {
        for (int i = 0; i < parameterObj.ParameterCount; i++) {
            ref AampParameter parameter = ref aamp.GetParameter(i, parameterObj.ParametersOffset, out int parameterOffset);

            int dataOffset = (parameterOffset + parameter.DataOffset * 4);
            RevrsReader reader = new(aamp.ParameterData[dataOffset..], Endianness.Little);

            this[parameter.Name] = parameter.Type switch {
                AampParameterType.Bool => reader.Read<uint>() != 0,
                AampParameterType.Float => reader.Read<float>(),
                AampParameterType.Int => reader.Read<int>(),
                AampParameterType.Vec2 => reader.Read<Vector2>(),
                AampParameterType.Vec3 => reader.Read<Vector3>(),
                AampParameterType.Vec4 => reader.Read<Vector4>(),
                AampParameterType.Color => reader.Read<Color4>(),
                AampParameterType.String32 or
                AampParameterType.String64 or
                AampParameterType.String256 or
                AampParameterType.StringRef => new(aamp.ReadString(dataOffset), parameter.Type),
                AampParameterType.Curve1 => reader.Read<Curve1>(),
                AampParameterType.Curve2 => reader.Read<Curve2>(),
                AampParameterType.Curve3 => reader.Read<Curve3>(),
                AampParameterType.Curve4 => reader.Read<Curve4>(),
                AampParameterType.IntArray => ReadArray<int>(ref reader),
                AampParameterType.FloatArray => ReadArray<float>(ref reader),
                AampParameterType.Quat => reader.Read<Quaternion>(),
                AampParameterType.UInt32 => reader.Read<uint>(),
                AampParameterType.UInt32Array => ReadArray<uint>(ref reader),
                AampParameterType.ByteArray => ReadArray<byte>(ref reader),
                _ => throw new InvalidOperationException($"""
                    Invalid or unsupported parameter type: '{parameter.Type}'
                    """)
            };
        }
    }

    public Parameter this[string key] {
        get => base[Crc32.ComputeHash(key)];
        set => base[Crc32.ComputeHash(key)] = value;
    }

    public bool IsReadOnly => false;
    ICollection<Parameter> IDictionary<string, Parameter>.Values => Values;
    ICollection<string> IDictionary<string, Parameter>.Keys
        => throw new NotSupportedException(
            "Accessing string values is not supported on a hashed dictionary.");

    public void Add(string key, Parameter value)
    {
        Add(Crc32.ComputeHash(key), value);
    }

    public void Add(KeyValuePair<string, Parameter> item)
    {
        Add(Crc32.ComputeHash(item.Key), item.Value);
    }

    public bool Contains(KeyValuePair<string, Parameter> item)
    {
        return this[item.Key] == item.Value;
    }

    public bool ContainsKey(string key)
    {
        return ContainsKey(Crc32.ComputeHash(key));
    }

    public void CopyTo(KeyValuePair<string, Parameter>[] array, int arrayIndex)
    {
        foreach (var (key, value) in array.AsSpan()[arrayIndex..]) {
            this[Crc32.ComputeHash(key)] = value;
        }
    }

    public bool Remove(string key)
    {
        return Remove(Crc32.ComputeHash(key));
    }

    public bool Remove(KeyValuePair<string, Parameter> item)
    {
        return Remove(Crc32.ComputeHash(item.Key));
    }

    public bool TryGetValue(string key, [MaybeNullWhen(false)] out Parameter value)
    {
        return TryGetValue(Crc32.ComputeHash(key), out value);
    }

    IEnumerator<KeyValuePair<string, Parameter>> IEnumerable<KeyValuePair<string, Parameter>>.GetEnumerator()
        => throw new NotSupportedException(
            "Accessing string values is not supported on a hashed dictionary.");

    private static T[] ReadArray<T>(ref RevrsReader reader) where T : unmanaged
    {
        reader.Move(-4);
        int count = reader.Read<int>();
        return reader.ReadSpan<T>(count).ToArray();
    }
}
