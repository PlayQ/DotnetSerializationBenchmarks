using BenchmarkDotNet.Attributes;

using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Builders;

namespace c_sharp_serialization_bench
{
    public class BsonSerializationBenchmark
    {
        public BsonClassMap codec;
        public byte[] encodedData;
        public NodeGroup data;

        [GlobalSetup]
        public void Setup()
        {
            data = UtilsCommon.RandomData(1000, 6);
            encodedData = data.ToBson();
        }

        [Benchmark]
        public byte[] encodeData()
        {
            return data.ToBson();
        }

        [Benchmark]
        public NodeGroup decodeData()
        {
            return BsonSerializer.Deserialize<NodeGroup>(encodedData);
        }

    }
}