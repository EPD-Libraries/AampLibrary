using AampLibrary.Primitives;
using System.Buffers;
using System.Numerics;
using VYaml.Parser;

namespace AampLibrary.Yaml;

public static class AampYamlParser
{
    public static Aamp FromYaml(ReadOnlySequence<byte> yaml)
    {
        Aamp result = new();

        YamlParser parser = new(yaml);
        parser.SkipAfter(ParseEventType.DocumentStart);

        if (!parser.TryGetCurrentTag(out Tag tag) || tag.Suffix is not "io") {
            throw new ArgumentException("Invalid AAMP yaml. Missing !io tag.", nameof(yaml));
        }

        parser.SkipAfter(ParseEventType.MappingStart);
        parser.Read();

        if (parser.TryReadScalarAsString(out string? typeKey) && typeKey is "Type") {
            result.Type = parser.ReadScalarAsString()
                ?? "xml";
        }

        if (parser.TryReadScalarAsString(out string? flagsKey) && flagsKey is "Flags") {
            result.Flags = AampFlags.None;
            parser.SkipAfter(ParseEventType.SequenceStart);
            while (parser.CurrentEventType is not ParseEventType.SequenceEnd) {
                if (Enum.TryParse(parser.ReadScalarAsString(), out AampFlags flag)) {
                    result.Flags |= flag;
                }
            }
            parser.SkipAfter(ParseEventType.SequenceEnd);
        }

        ParseList(ref parser, result);
        return result;
    }

    public static ParameterObject ParseObjects(ref YamlParser parser)
    {
        ParameterObject result = [];
        parser.SkipAfter(ParseEventType.MappingStart);

        while (parser.CurrentEventType is not ParseEventType.MappingEnd) {
            if (parser.TryReadScalarAsUInt32(out uint hashedKey)) {
                parser.TryGetCurrentTag(out Tag tag);
                result[hashedKey] = ParseParameter(ref parser, tag);
                continue;
            }

            if (parser.TryReadScalarAsString(out string? key) && key is not null) {
                parser.TryGetCurrentTag(out Tag tag);
                result[key] = ParseParameter(ref parser, tag);
                continue;
            }
        }

        return result;
    }

    public static ParameterList ParseList(ref YamlParser parser)
    {
        ParameterList list = new();
        ParseList(ref parser, list);
        return list;
    }

    public static void ParseList(ref YamlParser parser, ParameterList list)
    {
        while (parser.CurrentEventType is not ParseEventType.MappingEnd) {
            // Needs to run before TryGetCurrentTag
            bool isHashed = parser.TryReadScalarAsUInt32(out uint hashedKey);
            bool isString = parser.TryReadScalarAsString(out string? key);

            if (!parser.TryGetCurrentTag(out Tag tag)) {
                throw new InvalidDataException(
                    message: "Invalid AAMP yaml. No !obj or !list tag was present."
                );
            }

            if (isHashed) {
                switch (tag.Suffix) {
                    case "obj":
                        list.Objects[hashedKey] = ParseObjects(ref parser);
                        break;
                    case "list":
                        list.Lists[hashedKey] = ParseList(ref parser);
                        break;
                }

                continue;
            }

            if (isString && key is not null) {
                switch (tag.Suffix) {
                    case "obj":
                        list.Objects[key] = ParseObjects(ref parser);
                        break;
                    case "list":
                        list.Lists[key] = ParseList(ref parser);
                        break;
                }

                continue;
            }
        }
    }

    public static Parameter ParseParameter(ref YamlParser parser, Tag? tag)
    {
        if (tag is null) {
            return ParseSimpleParameter(ref parser);
        }

        return tag.Suffix switch {
            "f32" => parser.ReadScalarAsFloat(),
            "vec2" => ReadVector2(ref parser),
            "vec3" => ReadVector3(ref parser),
            "vec4" => ReadVector4(ref parser),
            "color" => ReadColor(ref parser),
            "str32" => (parser.ReadScalarAsString() ?? string.Empty, AampStringType.String32),
            "str64" => (parser.ReadScalarAsString() ?? string.Empty, AampStringType.String64),
            "curve1" => ReadCurve1(ref parser),
            "curve2" => ReadCurve2(ref parser),
            "curve3" => ReadCurve3(ref parser),
            "curve4" => ReadCurve4(ref parser),
            "string256" => (parser.ReadScalarAsString() ?? string.Empty, AampStringType.String256),
            "quat" => ReadQuat(ref parser),
            "u32" => parser.ReadScalarAsUInt32(),
            "binary" => Convert.FromBase64String(parser.ReadScalarAsString() ?? string.Empty),
            _ => throw new Exception(
                message: $"Unknwon field type: '{tag.Suffix}'"
            )
        };
    }

    public static Parameter ParseSimpleParameter(ref YamlParser parser)
    {
        if (parser.TryReadScalarAsBool(out bool boolean)) {
            return boolean;
        }

        if (parser.TryReadScalarAsInt32(out int int32)) {
            return int32;
        }

        if (parser.TryReadScalarAsString(out string? str)) {
            return (str ?? string.Empty, AampStringType.StringRef);
        }

        if (parser.CurrentEventType is ParseEventType.SequenceStart) {
            return ParseArray(ref parser);
        }

        throw new InvalidDataException(
            message: "Invalid AAMP YAML. Unkown scalar type."
        );
    }

    public static Parameter ParseArray(ref YamlParser parser)
    {
        parser.SkipAfter(ParseEventType.SequenceStart);

        if (!parser.TryGetCurrentTag(out Tag? tag)) {
            return ParseIntArray(ref parser);
        }

        return tag.Suffix switch {
            "f32" => ParseFloatArray(ref parser),
            "u32" => ParseUIntArray(ref parser),
            _ => throw new Exception(
                message: $"Unknwon array field type: '{tag.Suffix}'"
            )
        };
    }

    public static Parameter ParseIntArray(ref YamlParser parser)
    {
        List<int> result = [];
        while (parser.CurrentEventType is not ParseEventType.SequenceEnd) {
            result.Add(parser.ReadScalarAsInt32());
        }

        parser.SkipAfter(ParseEventType.SequenceEnd);
        return result;
    }

    public static Parameter ParseFloatArray(ref YamlParser parser)
    {
        List<float> result = [];
        while (parser.CurrentEventType is not ParseEventType.SequenceEnd) {
            result.Add(parser.ReadScalarAsFloat());
        }

        parser.SkipAfter(ParseEventType.SequenceEnd);
        return result;
    }

    public static Parameter ParseUIntArray(ref YamlParser parser)
    {
        List<uint> result = [];
        while (parser.CurrentEventType is not ParseEventType.SequenceEnd) {
            result.Add(parser.ReadScalarAsUInt32());
        }

        parser.SkipAfter(ParseEventType.SequenceEnd);
        return result;
    }

    public static Parameter ReadVector2(ref YamlParser parser)
    {
        parser.SkipAfter(ParseEventType.MappingStart);
        Vector2 vec = new();

        while (parser.CurrentEventType is not ParseEventType.MappingEnd) {
            if (parser.TryReadScalarAsString(out string? keyX) && keyX is nameof(vec.X)) {
                vec.X = parser.ReadScalarAsFloat();
            }

            if (parser.TryReadScalarAsString(out string? keyY) && keyY is nameof(vec.Y)) {
                vec.Y = parser.ReadScalarAsFloat();
            }
        }

        parser.SkipAfter(ParseEventType.MappingEnd);
        return vec;
    }

    public static Parameter ReadVector3(ref YamlParser parser)
    {
        parser.SkipAfter(ParseEventType.MappingStart);
        Vector3 vec = new();

        while (parser.CurrentEventType is not ParseEventType.MappingEnd) {
            if (parser.TryReadScalarAsString(out string? keyX) && keyX is nameof(vec.X)) {
                vec.X = parser.ReadScalarAsFloat();
            }

            if (parser.TryReadScalarAsString(out string? keyY) && keyY is nameof(vec.Y)) {
                vec.Y = parser.ReadScalarAsFloat();
            }

            if (parser.TryReadScalarAsString(out string? keyZ) && keyZ is nameof(vec.Z)) {
                vec.Z = parser.ReadScalarAsFloat();
            }
        }

        parser.SkipAfter(ParseEventType.MappingEnd);
        return vec;
    }

    public static Parameter ReadVector4(ref YamlParser parser)
    {
        parser.SkipAfter(ParseEventType.MappingStart);
        Vector4 vec = new();

        while (parser.CurrentEventType is not ParseEventType.MappingEnd) {
            if (parser.TryReadScalarAsString(out string? keyX) && keyX is nameof(vec.X)) {
                vec.X = parser.ReadScalarAsFloat();
            }

            if (parser.TryReadScalarAsString(out string? keyY) && keyY is nameof(vec.Y)) {
                vec.Y = parser.ReadScalarAsFloat();
            }

            if (parser.TryReadScalarAsString(out string? keyZ) && keyZ is nameof(vec.Z)) {
                vec.Z = parser.ReadScalarAsFloat();
            }

            if (parser.TryReadScalarAsString(out string? keyW) && keyW is nameof(vec.W)) {
                vec.W = parser.ReadScalarAsFloat();
            }
        }

        parser.SkipAfter(ParseEventType.MappingEnd);
        return vec;
    }

    public static Parameter ReadColor(ref YamlParser parser)
    {
        parser.SkipAfter(ParseEventType.MappingStart);
        Color4 color = new();

        while (parser.CurrentEventType is not ParseEventType.MappingEnd) {
            if (parser.TryReadScalarAsString(out string? keyA) && keyA is nameof(color.A)) {
                color.A = parser.ReadScalarAsFloat();
            }

            if (parser.TryReadScalarAsString(out string? keyR) && keyR is nameof(color.R)) {
                color.R = parser.ReadScalarAsFloat();
            }

            if (parser.TryReadScalarAsString(out string? keyG) && keyG is nameof(color.G)) {
                color.G = parser.ReadScalarAsFloat();
            }

            if (parser.TryReadScalarAsString(out string? keyB) && keyB is nameof(color.B)) {
                color.B = parser.ReadScalarAsFloat();
            }
        }

        parser.SkipAfter(ParseEventType.MappingEnd);
        return color;
    }

    public static unsafe Parameter ReadCurve1(ref YamlParser parser)
    {
        return new Curve1 {
            A = ReadCurve(ref parser)
        };
    }

    public static unsafe Parameter ReadCurve2(ref YamlParser parser)
    {
        parser.SkipAfter(ParseEventType.MappingStart);
        Curve2 curve = new();

        while (parser.CurrentEventType is not ParseEventType.MappingEnd) {
            if (parser.TryReadScalarAsString(out string? keyA) && keyA is nameof(curve.A)) {
                curve.A = ReadCurve(ref parser);
            }

            if (parser.TryReadScalarAsString(out string? keyB) && keyB is nameof(curve.B)) {
                curve.B = ReadCurve(ref parser);
            }
        }

        parser.SkipAfter(ParseEventType.MappingEnd);
        return curve;
    }

    public static unsafe Parameter ReadCurve3(ref YamlParser parser)
    {
        parser.SkipAfter(ParseEventType.MappingStart);
        Curve3 curve = new();

        while (parser.CurrentEventType is not ParseEventType.MappingEnd) {
            if (parser.TryReadScalarAsString(out string? keyA) && keyA is nameof(curve.A)) {
                curve.A = ReadCurve(ref parser);
            }

            if (parser.TryReadScalarAsString(out string? keyB) && keyB is nameof(curve.B)) {
                curve.B = ReadCurve(ref parser);
            }

            if (parser.TryReadScalarAsString(out string? keyC) && keyC is nameof(curve.C)) {
                curve.C = ReadCurve(ref parser);
            }
        }

        parser.SkipAfter(ParseEventType.MappingEnd);
        return curve;
    }

    public static unsafe Parameter ReadCurve4(ref YamlParser parser)
    {
        parser.SkipAfter(ParseEventType.MappingStart);
        Curve4 curve = new();

        while (parser.CurrentEventType is not ParseEventType.MappingEnd) {
            if (parser.TryReadScalarAsString(out string? keyA) && keyA is nameof(curve.A)) {
                curve.A = ReadCurve(ref parser);
            }

            if (parser.TryReadScalarAsString(out string? keyB) && keyB is nameof(curve.B)) {
                curve.B = ReadCurve(ref parser);
            }

            if (parser.TryReadScalarAsString(out string? keyC) && keyC is nameof(curve.C)) {
                curve.C = ReadCurve(ref parser);
            }

            if (parser.TryReadScalarAsString(out string? keyD) && keyD is nameof(curve.D)) {
                curve.D = ReadCurve(ref parser);
            }
        }

        parser.SkipAfter(ParseEventType.MappingEnd);
        return curve;
    }

    public static unsafe Curve ReadCurve(ref YamlParser parser)
    {
        parser.SkipAfter(ParseEventType.SequenceStart);
        Curve curve = new() {
            A = parser.ReadScalarAsUInt32(),
            B = parser.ReadScalarAsUInt32(),
        };

        for (int i = 0; i < 30; i++) {
            curve.Points[i] = parser.ReadScalarAsFloat();
        }

        parser.SkipAfter(ParseEventType.SequenceEnd);
        return curve;
    }

    public static Parameter ReadQuat(ref YamlParser parser)
    {
        parser.SkipAfter(ParseEventType.MappingStart);
        Quaternion quat = new();

        while (parser.CurrentEventType is not ParseEventType.MappingEnd) {
            if (parser.TryReadScalarAsString(out string? keyX) && keyX is nameof(quat.X)) {
                quat.X = parser.ReadScalarAsFloat();
            }

            if (parser.TryReadScalarAsString(out string? keyY) && keyY is nameof(quat.Y)) {
                quat.Y = parser.ReadScalarAsFloat();
            }

            if (parser.TryReadScalarAsString(out string? keyZ) && keyZ is nameof(quat.Z)) {
                quat.Z = parser.ReadScalarAsFloat();
            }

            if (parser.TryReadScalarAsString(out string? keyW) && keyW is nameof(quat.W)) {
                quat.W = parser.ReadScalarAsFloat();
            }
        }

        parser.SkipAfter(ParseEventType.MappingEnd);
        return quat;
    }
}
