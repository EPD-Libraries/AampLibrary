using AampLibrary.Primitives;
using System.Numerics;

namespace AampLibrary.Runner.Helpers;

public static class TestAampGenerator
{
    public static Aamp CreateAamp()
    {
        return new() {
            Objects = {
                ["TestObject"] = new() {
                    ["Bool"] = CreateBool(),
                    ["Float"] = CreateFloat(),
                    ["Int"] = CreateInt(),
                    ["Vec2"] = CreateVec2(),
                    ["Vec3"] = CreateVec3(),
                    ["Vec4"] = CreateVec4(),
                    ["Color"] = CreateColor(),
                    ["String32"] = CreateString32(),
                    ["String64"] = CreateString64(),
                    ["Curve1"] = CreateCurve1(),
                    ["Curve2"] = CreateCurve2(),
                    ["Curve3"] = CreateCurve3(),
                    ["Curve4"] = CreateCurve4(),
                    ["IntArray"] = CreateIntArray(),
                    ["FloatArray"] = CreateFloatArray(),
                    ["String256"] = CreateString256(),
                    ["Quat"] = CreateQuat(),
                    ["UInt32"] = CreateUInt32(),
                    ["UInt32Array"] = CreateUInt32Array(),
                    ["ByteArray"] = CreateByteArray(),
                    ["StringRef"] = CreateStringRef()
                }
            },
            Lists = new() {
                ["ListObject"] = new() {
                    Objects = new() {
                        ["TestObject2"] = new() {
                            ["Bool"] = true
                        }
                    }
                }
            }
        };
    }

    public static Parameter CreateBool()
    {
        return false;
    }

    public static Parameter CreateFloat()
    {
        return float.Pi;
    }

    public static Parameter CreateInt()
    {
        return int.MaxValue;
    }

    public static Parameter CreateVec2()
    {
        return new Vector2 {
            X = float.Pi,
            Y = float.E
        };
    }

    public static Parameter CreateVec3()
    {
        return new Vector3 {
            X = float.Pi,
            Y = float.E,
            Z = float.Tau
        };
    }

    public static Parameter CreateVec4()
    {
        return new Vector4 {
            X = float.Pi,
            Y = float.E,
            Z = float.Tau,
            W = float.NaN,
        };
    }

    public static Parameter CreateColor()
    {
        return new Color4 {
            A = 1.0f,
            R = 0.4f,
            G = 0.2f,
            B = 0.6f,
        };
    }

    public static Parameter CreateString32()
    {
        return ("32-byte String", AampStringType.String32);
    }

    public static Parameter CreateString64()
    {
        return ("64-byte String (Longer)", AampStringType.String64);
    }

    public static Parameter CreateCurve1()
    {
        Curve a = new() {
            A = uint.MinValue,
            B = uint.MaxValue
        };

        FillCurvePoints(ref a);

        return new Curve1 {
            A = a
        };
    }

    public static Parameter CreateCurve2()
    {
        Curve a = new() {
            A = uint.MinValue,
            B = uint.MaxValue
        };

        Curve b = new() {
            A = uint.MinValue,
            B = uint.MaxValue
        };

        FillCurvePoints(ref a);
        FillCurvePoints(ref b);

        return new Curve2 {
            A = a,
            B = b
        };
    }

    public static Parameter CreateCurve3()
    {
        Curve a = new() {
            A = uint.MinValue,
            B = uint.MaxValue
        };

        Curve b = new() {
            A = uint.MinValue,
            B = uint.MaxValue
        };

        Curve c = new() {
            A = uint.MinValue,
            B = uint.MaxValue
        };

        FillCurvePoints(ref a);
        FillCurvePoints(ref b);
        FillCurvePoints(ref c);

        return new Curve3 {
            A = a,
            B = b,
            C = c
        };
    }

    public static Parameter CreateCurve4()
    {
        Curve a = new() {
            A = uint.MinValue,
            B = uint.MaxValue
        };

        Curve b = new() {
            A = uint.MinValue,
            B = uint.MaxValue
        };

        Curve c = new() {
            A = uint.MinValue,
            B = uint.MaxValue
        };

        Curve d = new() {
            A = uint.MinValue,
            B = uint.MaxValue
        };

        FillCurvePoints(ref a);
        FillCurvePoints(ref b);
        FillCurvePoints(ref c);
        FillCurvePoints(ref d);

        return new Curve4 {
            A = a,
            B = b,
            C = c,
            D = d
        };
    }

    public static Parameter CreateIntArray()
    {
        return new int[] {
            0,
            1,
            2,
            3
        };
    }

    public static Parameter CreateFloatArray()
    {
        return new float[] {
            0.0f,
            1.1f,
            2.2f,
            3.3f
        };
    }

    public static Parameter CreateString256()
    {
        return ("256-byte String (Very Longer)", AampStringType.String64);
    }

    public static Parameter CreateQuat()
    {
        return new Quaternion {
            X = float.Pi,
            Y = float.E,
            Z = float.Tau,
            W = float.NaN
        };
    }

    public static Parameter CreateUInt32()
    {
        return uint.MaxValue;
    }

    public static Parameter CreateUInt32Array()
    {
        return new uint[] {
            uint.MinValue,
            uint.MaxValue
        };
    }

    public static Parameter CreateByteArray()
    {
        return new byte[] {
            byte.MinValue,
            byte.MaxValue
        };
    }

    public static Parameter CreateStringRef()
    {
        return ("String Ref (can't remember what this is)", AampStringType.StringRef);
    }

    private static unsafe void FillCurvePoints(ref Curve curve)
    {
        for (int i = 0; i < 30; i++) {
            curve.Points[i] = i;
        }
    }
}
