using AampLibrary;
using AampLibrary.IO;
using AampLibrary.Runner.Helpers;

Aamp aamp = TestAampGenerator.CreateAamp();
string yaml = aamp.ToYaml(new AampKeyProvider([
    "TestObject",
    "ListObject",
    "TestObject2",
    "Bool",
    "Float",
    "Int",
    "Vec2",
    "Vec3",
    "Vec4",
    "Color",
    "String32",
    "String64",
    "Curve1",
    "Curve2",
    "Curve3",
    "Curve4",
    "IntArray",
    "FloatArray",
    "String256",
    "Quat",
    "UInt32",
    "UInt32Array",
    "ByteArray",
    "StringRef",
]));
Console.WriteLine(yaml);
