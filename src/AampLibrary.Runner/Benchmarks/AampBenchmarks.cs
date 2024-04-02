using BenchmarkDotNet.Attributes;
using Revrs;

namespace AampLibrary.Runner.Benchmarks;

[MemoryDiagnoser(true)]
public class AampBenchmarks
{
    private byte[] _buffer = null!;

    [GlobalSetup]
    public void Setup()
    {
        _buffer = File.ReadAllBytes(@"D:\bin\AampLibrary\Armor_006_Upper.bphysics");
    }

    [Benchmark]
    public void ReadAamp()
    {
        Aamp _ = Aamp.FromBinary(_buffer);
    }

    [Benchmark]
    public void ReadImmutableAamp()
    {
        RevrsReader reader = new(_buffer);
        ImmutableAamp _ = new(ref reader);
    }
}
