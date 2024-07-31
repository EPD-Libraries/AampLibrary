using AampLibrary;
using AampLibrary.Runner.Helpers;
using AampLibrary.Yaml;
using System.Buffers;
using System.Text;

Aamp aamp = TestAampGenerator.CreateAamp();

ArrayBufferWriter<byte> writer = new();
AampYamlWriter.Write(writer, aamp);

string yaml = Encoding.UTF8.GetString(writer.WrittenSpan);
Console.WriteLine(yaml);
