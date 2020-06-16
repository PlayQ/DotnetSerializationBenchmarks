using System;
using System.Linq;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace c_sharp_serialization_bench
{
    public class Program
    {
        static void Main(string[] args)
        {
            // var cfg = DefaultConfig.Instance.With(Job.Default // Adding second job
            //         //.AsBaseline() // It will be marked as baseline
            //         .WithWarmupCount(0) // Disable warm-up stage
            //         .WithIterationCount(1)
            //         .WithInvocationCount(16)
            //         .WithEvaluateOverhead(false)
            //         
            // );
            // Console.Out.WriteLine(cfg.GetJobs().Count());
            // var sum = BenchmarkRunner.Run<ProtobufSerializationBenchmark>(cfg);
            // Console.Out.WriteLine(sum.Table.ToString());

            //BenchmarkRunner.Run<MsgpackSerializationBenchmark>();
            var x = BenchmarkRunner.Run<BsonSerializationBenchmark>();

        }
    }
}