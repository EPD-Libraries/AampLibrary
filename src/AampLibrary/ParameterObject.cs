using AampLibrary.Primitives;
using AampLibrary.Structures;
using Revrs;
using System.Numerics;

namespace AampLibrary;

public class ParameterObject : Dictionary<string, Parameter>
{
    public ParameterObject()
    {
    }

    public ParameterObject(int capacity) : base(capacity)
    {
    }

    public ParameterObject(IEnumerable<KeyValuePair<string, Parameter>> collection) : base(collection)
    {
    }

    public ParameterObject(IDictionary<string, Parameter> dictionary) : base(dictionary)
    {
    }

    internal ParameterObject(ref ImmutableAamp aamp, ref AampParameterObject parameterObj, IAampNameResolver aampNameResolver) : base(parameterObj.ParameterCount)
    {
        for (int i = 0; i < parameterObj.ParameterCount; i++) {
            ref AampParameter parameter = ref aamp.GetParameter(i, parameterObj.ParametersOffset, out int parameterOffset);

            int dataOffset = (parameterOffset + parameter.DataOffset * 4);
            RevrsReader reader = new(aamp.ParameterData[dataOffset..], Endianness.Little);

            this[aampNameResolver.GetName(parameter.Name)] = parameter.Type switch {
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

    private static T[] ReadArray<T>(ref RevrsReader reader) where T : unmanaged
    {
        reader.Move(-4);
        int count = reader.Read<int>();
        return reader.ReadSpan<T>(count).ToArray();
    }
}
