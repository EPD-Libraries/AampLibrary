using AampLibrary.Extensions;
using AampLibrary.Primitives;
using AampLibrary.Structures;
using System.Buffers;
using System.Numerics;
using VYaml.Emitter;

namespace AampLibrary.Yaml;

public class AampYamlWriter
{
    public static void Write(IBufferWriter<byte> writer, Aamp aamp)
    {
        Utf8YamlEmitter emitter = new(writer);

        emitter.Tag("!io");
        emitter.BeginMapping();
        {
            emitter.WriteString(nameof(Aamp.Type));
            emitter.WriteString(aamp.Type);

            emitter.WriteString(nameof(Aamp.Flags));
            emitter.BeginSequence(SequenceStyle.Flow);
            {
                foreach (AampFlags flag in aamp.Flags.EnumerateFlags()) {
                    emitter.WriteString(flag.ToString());
                }
            }
            emitter.EndSequence();

            WriteList(ref emitter, aamp);
        }
        emitter.EndMapping();
    }

    private static void WriteList(ref Utf8YamlEmitter emitter, ParameterList parameterList)
    {
        foreach (var (key, obj) in parameterList.Objects) {
            emitter.WriteUInt32(key);
            emitter.Tag("!obj");
            WriteObject(ref emitter, obj);
        }

        foreach (var (key, list) in parameterList.Lists) {
            emitter.WriteUInt32(key);
            emitter.Tag("!list");
            emitter.BeginMapping();
            {
                WriteList(ref emitter, list);
            }
            emitter.EndMapping();
        }
    }

    private static void WriteObject(ref Utf8YamlEmitter emitter, ParameterObject parameterObject)
    {
        emitter.BeginMapping();
        {
            foreach (var (key, param) in parameterObject) {
                emitter.WriteUInt32(key);
                WriteParameter(ref emitter, param);
            }
        }
        emitter.EndMapping();
    }

    private static void WriteParameter(ref Utf8YamlEmitter emitter, Parameter parameter)
    {
        switch (parameter.Type) {
            case AampParameterType.Bool: {
                emitter.WriteBool(parameter.GetBool());
                break;
            }
            case AampParameterType.Float: {
                emitter.Tag("!f32");
                emitter.WriteFloat(parameter.GetFloat());
                break;
            }
            case AampParameterType.Int: {
                emitter.WriteInt32(parameter.GetInt());
                break;
            }
            case AampParameterType.Vec2: {
                emitter.Tag("!vec2");
                emitter.BeginMapping(MappingStyle.Flow);
                {
                    Vector2 vec2 = parameter.GetVec2();
                    emitter.WriteString(nameof(Vector2.X));
                    emitter.WriteFloat(vec2.X);
                    emitter.WriteString(nameof(Vector2.Y));
                    emitter.WriteFloat(vec2.Y);
                }
                emitter.EndMapping();
                break;
            }
            case AampParameterType.Vec3: {
                emitter.Tag("!vec3");
                emitter.BeginMapping(MappingStyle.Flow);
                {
                    Vector3 vec3 = parameter.GetVec3();
                    emitter.WriteString(nameof(Vector3.X));
                    emitter.WriteFloat(vec3.X);
                    emitter.WriteString(nameof(Vector3.Y));
                    emitter.WriteFloat(vec3.Y);
                    emitter.WriteString(nameof(Vector3.Z));
                    emitter.WriteFloat(vec3.Z);
                }
                emitter.EndMapping();
                break;
            }
            case AampParameterType.Vec4: {
                emitter.Tag("!vec4");
                emitter.BeginMapping(MappingStyle.Flow);
                {
                    Vector4 vec4 = parameter.GetVec4();
                    emitter.WriteString(nameof(Vector4.X));
                    emitter.WriteFloat(vec4.X);
                    emitter.WriteString(nameof(Vector4.Y));
                    emitter.WriteFloat(vec4.Y);
                    emitter.WriteString(nameof(Vector4.Z));
                    emitter.WriteFloat(vec4.Z);
                    emitter.WriteString(nameof(Vector4.W));
                    emitter.WriteFloat(vec4.W);
                }
                emitter.EndMapping();
                break;
            }
            case AampParameterType.Color: {
                emitter.Tag("!color");
                emitter.BeginMapping(MappingStyle.Flow);
                {
                    Color4 color = parameter.GetColor();
                    emitter.WriteString(nameof(Color4.A));
                    emitter.WriteFloat(color.A);
                    emitter.WriteString(nameof(Color4.R));
                    emitter.WriteFloat(color.R);
                    emitter.WriteString(nameof(Color4.G));
                    emitter.WriteFloat(color.G);
                    emitter.WriteString(nameof(Color4.B));
                    emitter.WriteFloat(color.B);
                }
                emitter.EndMapping();
                break;
            }
            case AampParameterType.String32: {
                emitter.Tag("!str32");
                emitter.WriteString(parameter.GetString() ?? string.Empty);
                break;
            }
            case AampParameterType.String64: {
                emitter.Tag("!str64");
                emitter.WriteString(parameter.GetString() ?? string.Empty);
                break;
            }
            case AampParameterType.Curve1: {
                emitter.Tag("!curve1");
                WriteCurve(ref emitter, parameter.GetCurve1().A);
                break;
            }
            case AampParameterType.Curve2: {
                emitter.Tag("!curve2");
                Curve2 curve = parameter.GetCurve2();
                emitter.BeginMapping();
                {
                    emitter.WriteString(nameof(curve.A));
                    WriteCurve(ref emitter, curve.A);

                    emitter.WriteString(nameof(curve.B));
                    WriteCurve(ref emitter, curve.B);
                }
                emitter.EndMapping();
                break;
            }
            case AampParameterType.Curve3: {
                emitter.Tag("!curve3");
                Curve3 curve = parameter.GetCurve3();
                emitter.BeginMapping();
                {
                    emitter.WriteString(nameof(curve.A));
                    WriteCurve(ref emitter, curve.A);

                    emitter.WriteString(nameof(curve.B));
                    WriteCurve(ref emitter, curve.B);

                    emitter.WriteString(nameof(curve.C));
                    WriteCurve(ref emitter, curve.C);
                }
                emitter.EndMapping();
                break;
            }
            case AampParameterType.Curve4: {
                emitter.Tag("!curve4");
                Curve4 curve = parameter.GetCurve4();
                emitter.BeginMapping();
                {
                    emitter.WriteString(nameof(curve.A));
                    WriteCurve(ref emitter, curve.A);

                    emitter.WriteString(nameof(curve.B));
                    WriteCurve(ref emitter, curve.B);

                    emitter.WriteString(nameof(curve.C));
                    WriteCurve(ref emitter, curve.C);

                    emitter.WriteString(nameof(curve.D));
                    WriteCurve(ref emitter, curve.D);
                }
                emitter.EndMapping();
                break;
            }
            case AampParameterType.IntArray: {
                emitter.BeginSequence();
                {
                    foreach (int value in parameter.GetIntArray()) {
                        emitter.WriteInt32(value);
                    }
                }
                emitter.EndSequence();
                break;
            }
            case AampParameterType.FloatArray: {
                emitter.BeginSequence();
                {
                    foreach (float value in parameter.GetFloatArray()) {
                        emitter.Tag("!f32");
                        emitter.WriteFloat(value);
                    }
                }
                emitter.EndSequence();
                break;
            }
            case AampParameterType.String256: {
                emitter.Tag("!str256");
                emitter.WriteString(parameter.GetString() ?? string.Empty);
                break;
            }
            case AampParameterType.Quat: {
                emitter.Tag("!quat");
                emitter.BeginMapping(MappingStyle.Flow);
                {
                    Quaternion quat = parameter.GetQuat();
                    emitter.WriteString(nameof(Quaternion.X));
                    emitter.WriteFloat(quat.X);
                    emitter.WriteString(nameof(Quaternion.Y));
                    emitter.WriteFloat(quat.Y);
                    emitter.WriteString(nameof(Quaternion.Z));
                    emitter.WriteFloat(quat.Z);
                    emitter.WriteString(nameof(Quaternion.W));
                    emitter.WriteFloat(quat.W);
                }
                emitter.EndMapping();
                break;
            }
            case AampParameterType.UInt32: {
                emitter.Tag("!u32");
                emitter.WriteUInt32(parameter.GetUInt32());
                break;
            }
            case AampParameterType.UInt32Array: {
                emitter.BeginSequence();
                {
                    foreach (uint value in parameter.GetUInt32Array()) {
                        emitter.Tag("!u32");
                        emitter.WriteUInt32(value);
                    }
                }
                emitter.EndSequence();
                break;
            }
            case AampParameterType.ByteArray: {
                emitter.Tag("!!binary");
                emitter.WriteString(Convert.ToBase64String(parameter.GetByteArray()));
                break;
            }
            case AampParameterType.StringRef: {
                emitter.WriteString(parameter.GetString() ?? string.Empty);
                break;
            }
        }
    }

    private static unsafe void WriteCurve(ref Utf8YamlEmitter emitter, Curve curve)
    {
        emitter.BeginSequence(SequenceStyle.Flow);
        emitter.WriteUInt32(curve.A);
        emitter.WriteUInt32(curve.B);
        for (int i = 0; i < 30; i++) {
            emitter.WriteFloat(curve.Points[i]);
        }
        emitter.EndSequence();
    }
}
