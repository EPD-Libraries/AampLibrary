using AampLibrary;
using AampLibrary.IO;
using AampLibrary.Runner.Helpers;
using AampLibrary.Yaml;
using System.Text;

AampKeyProvider keyProvider = new([
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
]);

Aamp aamp = TestAampGenerator.CreateAamp();
string yaml = aamp.ToYaml(keyProvider);

// Console.WriteLine(yaml);

byte[] utf8Yaml = Encoding.UTF8.GetBytes(yaml);
Aamp fromYaml = AampYamlParser.FromYaml(new(utf8Yaml));

Console.WriteLine(fromYaml.ToYaml(keyProvider));
