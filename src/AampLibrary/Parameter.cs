using AampLibrary.Primitives;
using System.Numerics;
using System.Runtime.InteropServices;

namespace AampLibrary;

public enum AampParameterType : byte
{
    Bool,
    Float,
    Int,
    Vec2,
    Vec3,
    Vec4,
    Color,
    String32,
    String64,
    Curve1,
    Curve2,
    Curve3,
    Curve4,
    IntArray,
    FloatArray,
    String256,
    Quat,
    UInt32,
    UInt32Array,
    ByteArray,
    StringRef,
}

public enum AampStringType : ushort
{
    String32 = AampParameterType.String32,
    String64 = AampParameterType.String64,
    String256 = AampParameterType.String256,
    StringRef = AampParameterType.StringRef,
}

public class Parameter
{
    private readonly ParameterValue _value;
    private readonly object? _referenceValue = null;

    public AampParameterType Type { get; set; }

    public static implicit operator Parameter(bool value) => new(value);
    public Parameter(bool value) : this(AampParameterType.Bool)
    {
        _value = new() {
            Bool = value
        };
    }

    public static implicit operator Parameter(float value) => new(value);
    public Parameter(float value) : this(AampParameterType.Float)
    {
        _value = new() {
            Float = value
        };
    }

    public static implicit operator Parameter(int value) => new(value);
    public Parameter(int value) : this(AampParameterType.Int)
    {
        _value = new() {
            Int = value
        };
    }

    public static implicit operator Parameter(uint value) => new(value);
    public Parameter(uint value) : this(AampParameterType.UInt32)
    {
        _value = new() {
            UInt32 = value
        };
    }

    public static implicit operator Parameter(Vector2 value) => new(value);
    public Parameter(Vector2 value) : this(AampParameterType.Vec2)
    {
        _value = new() {
            Vec2 = value
        };
    }

    public static implicit operator Parameter(Vector3 value) => new(value);
    public Parameter(Vector3 value) : this(AampParameterType.Vec3)
    {
        _value = new() {
            Vec3 = value
        };
    }

    public static implicit operator Parameter(Vector4 value) => new(value);
    public Parameter(Vector4 value) : this(AampParameterType.Vec4)
    {
        _value = new() {
            Vec4 = value
        };
    }

    public static implicit operator Parameter(Color4 value) => new(value);
    public Parameter(Color4 value) : this(AampParameterType.Color)
    {
        _value = new() {
            Color = value
        };
    }

    public static implicit operator Parameter(Quaternion value) => new(value);
    public Parameter(Quaternion value) : this(AampParameterType.Quat)
    {
        _value = new() {
            Quat = value
        };
    }

    public static implicit operator Parameter(Curve1 value) => new(value);
    public Parameter(Curve1 value) : this(AampParameterType.Curve1)
    {
        _referenceValue = value;
    }

    public static implicit operator Parameter(Curve2 value) => new(value);
    public Parameter(Curve2 value) : this(AampParameterType.Curve2)
    {
        _referenceValue = value;
    }

    public static implicit operator Parameter(Curve3 value) => new(value);
    public Parameter(Curve3 value) : this(AampParameterType.Curve3)
    {
        _referenceValue = value;
    }

    public static implicit operator Parameter(Curve4 value) => new(value);
    public Parameter(Curve4 value) : this(AampParameterType.Curve4)
    {
        _referenceValue = value;
    }

    public static implicit operator Parameter(int[] value) => new(value);
    public static implicit operator Parameter(List<int> value) => new(value);
    public Parameter(IList<int> value) : this(AampParameterType.IntArray)
    {
        _referenceValue = value;
    }

    public static implicit operator Parameter(float[] value) => new(value);
    public static implicit operator Parameter(List<float> value) => new(value);
    public Parameter(IList<float> value) : this(AampParameterType.FloatArray)
    {
        _referenceValue = value;
    }

    public static implicit operator Parameter(uint[] value) => new(value);
    public static implicit operator Parameter(List<uint> value) => new(value);
    public Parameter(IList<uint> value) : this(AampParameterType.UInt32Array)
    {
        _referenceValue = value;
    }

    public static implicit operator Parameter(byte[] value) => new(value);
    public Parameter(byte[] value) : this(AampParameterType.ByteArray)
    {
        _referenceValue = value;
    }

    public static implicit operator Parameter((string Value, AampStringType Type) stringDefinition) => new(stringDefinition.Value, stringDefinition.Type);
    public Parameter(string value, AampStringType stringType) : this((AampParameterType)stringType)
    {
        _referenceValue = value;
    }

    public Parameter(string value, AampParameterType type) : this(type)
    {
        _referenceValue = value;
    }

    private Parameter(AampParameterType type)
    {
        Type = type;
    }

    public bool GetBool()
    {
        return Type switch {
            AampParameterType.Bool => _value.Bool,
            _ => throw new InvalidOperationException(
                $"Invalid type '{nameof(AampParameterType.Bool)}', expected '{Type}'")
        };
    }

    public float GetFloat()
    {
        return Type switch {
            AampParameterType.Float => _value.Float,
            _ => throw new InvalidOperationException(
                $"Invalid type '{nameof(AampParameterType.Float)}', expected '{Type}'")
        };
    }

    public int GetInt()
    {
        return Type switch {
            AampParameterType.Int => _value.Int,
            _ => throw new InvalidOperationException(
                $"Invalid type '{nameof(AampParameterType.Int)}', expected '{Type}'")
        };
    }

    public uint GetUInt32()
    {
        return Type switch {
            AampParameterType.UInt32 => _value.UInt32,
            _ => throw new InvalidOperationException(
                $"Invalid type '{nameof(AampParameterType.UInt32)}', expected '{Type}'")
        };
    }

    public Vector2 GetVec2()
    {
        return Type switch {
            AampParameterType.Vec2 => _value.Vec2,
            _ => throw new InvalidOperationException(
                $"Invalid type '{nameof(AampParameterType.Vec2)}', expected '{Type}'")
        };
    }

    public Vector3 GetVec3()
    {
        return Type switch {
            AampParameterType.Vec3 => _value.Vec3,
            _ => throw new InvalidOperationException(
                $"Invalid type '{nameof(AampParameterType.Vec3)}', expected '{Type}'")
        };
    }

    public Vector4 GetVec4()
    {
        return Type switch {
            AampParameterType.Vec4 => _value.Vec4,
            _ => throw new InvalidOperationException(
                $"Invalid type '{nameof(AampParameterType.Vec4)}', expected '{Type}'")
        };
    }

    public Color4 GetColor()
    {
        return Type switch {
            AampParameterType.Color => _value.Color,
            _ => throw new InvalidOperationException(
                $"Invalid type '{nameof(AampParameterType.Color)}', expected '{Type}'")
        };
    }

    public Quaternion GetQuat()
    {
        return Type switch {
            AampParameterType.Quat => _value.Quat,
            _ => throw new InvalidOperationException(
                $"Invalid type '{nameof(AampParameterType.Quat)}', expected '{Type}'")
        };
    }

    public string? GetString()
    {
        return _referenceValue switch {
            string => (string)_referenceValue,
            _ => throw new InvalidOperationException(
                $"Invalid type '{typeof(string)}', expected '{Type}'")
        };
    }

    public Curve1 GetCurve1()
    {
        return _referenceValue switch {
            Curve1 => (Curve1)_referenceValue,
            _ => throw new InvalidOperationException(
                $"Invalid type '{typeof(Curve1)}', expected '{Type}'")
        };
    }

    public Curve2 GetCurve2()
    {
        return _referenceValue switch {
            Curve2 => (Curve2)_referenceValue,
            _ => throw new InvalidOperationException(
                $"Invalid type '{typeof(Curve2)}', expected '{Type}'")
        };
    }

    public Curve3 GetCurve3()
    {
        return _referenceValue switch {
            Curve3 => (Curve3)_referenceValue,
            _ => throw new InvalidOperationException(
                $"Invalid type '{typeof(Curve3)}', expected '{Type}'")
        };
    }

    public Curve4 GetCurve4()
    {
        return _referenceValue switch {
            Curve4 => (Curve4)_referenceValue,
            _ => throw new InvalidOperationException(
                $"Invalid type '{typeof(Curve4)}', expected '{Type}'")
        };
    }

    public IList<int> GetIntArray()
    {
        return _referenceValue switch {
            IList<int> result => result,
            _ => throw new InvalidOperationException(
                $"Invalid type '{typeof(int[])}', expected '{Type}'")
        };
    }

    public IList<float> GetFloatArray()
    {
        return _referenceValue switch {
            IList<float> result => result,
            _ => throw new InvalidOperationException(
                $"Invalid type '{typeof(float[])}', expected '{Type}'")
        };
    }

    public IList<uint> GetUInt32Array()
    {
        return _referenceValue switch {
            IList<uint> result => result,
            _ => throw new InvalidOperationException(
                $"Invalid type '{typeof(uint[])}', expected '{Type}'")
        };
    }

    public byte[] GetByteArray()
    {
        return _referenceValue switch {
            byte[] => (byte[])_referenceValue,
            _ => throw new InvalidOperationException(
                $"Invalid type '{typeof(byte[])}', expected '{Type}'")
        };
    }

    [StructLayout(LayoutKind.Explicit, Pack = 4, Size = 16)]
    private struct ParameterValue
    {
        [FieldOffset(0)]
        public bool Bool;

        [FieldOffset(0)]
        public float Float;

        [FieldOffset(0)]
        public int Int;

        [FieldOffset(0)]
        public uint UInt32;

        [FieldOffset(0)]
        public Vector2 Vec2;

        [FieldOffset(0)]
        public Vector3 Vec3;

        [FieldOffset(0)]
        public Vector4 Vec4;

        [FieldOffset(0)]
        public Color4 Color;

        [FieldOffset(0)]
        public Quaternion Quat;
    }
}
