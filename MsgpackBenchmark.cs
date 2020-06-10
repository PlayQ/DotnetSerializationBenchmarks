using BenchmarkDotNet.Attributes;


namespace c_sharp_serialization_bench
{
    public class MsgpackSerializationBenchmark
    {
        public NodeGroup data;
        public byte[] encodedData;

        [GlobalSetup]
        public void GlobalSetup()
        {
           data = UtilsCommon.RandomData(1000, 6);
           encodedData = MessagePack.MessagePackSerializer.Serialize(data);
        }

        [Benchmark]
        public byte[] encodeNodes()
        {
            return MessagePack.MessagePackSerializer.Serialize(data);
        }

        [Benchmark]
        public NodeGroup decodeNodes()
        {
            return MessagePack.MessagePackSerializer.Deserialize<NodeGroup>(encodedData);
        }
    }
}