using BenchmarkDotNet.Running;

namespace c_sharp_serialization_bench
{
    public class Program
    {
        static void Main(string[] args)
        {
             BenchmarkRunner.Run<BsonSerializationBenchmark>();
        }
    }
}