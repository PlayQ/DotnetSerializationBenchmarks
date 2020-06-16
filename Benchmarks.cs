using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BenchmarkDotNet.Attributes;
using Codecs.Proto;
using Google.Protobuf;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace c_sharp_serialization_bench
{
    public class BsonSerializationBenchmark
    {
        public List<byte[]> encodedBsonData = new List<byte[]>();
        public List<byte[]> encodedJsonData = new List<byte[]>();
        public List<byte[]> encodedMsgPackData = new List<byte[]>();
        public List<byte[]> encodedPbData = new List<byte[]>();
        public List<NodeGroup> data = new List<NodeGroup>();
        public List<NodeGroupP> pbData = new List<NodeGroupP>();

        private long idx = 0;
        private int samples = 1000;

        [GlobalSetup]
        public void Setup()
        {
            foreach (var index in Enumerable.Range(0, samples))
            {
                var d = UtilsCommon.RandomData(1000, 6);
                data.Append(d);
                
                encodedBsonData.Append(DoBsonEncodeData(d));
                encodedMsgPackData.Append(DoMsgPackEncodeData(d));
                encodedJsonData.Append(DoJsonEncodeData(d));
                
                var pbd = ProtobufUtils.RandomData(1000, 6);
                pbData.Append(pbd);
                encodedPbData.Append(pbd.ToByteArray());
            }
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);            
        }

        [Benchmark]
        public byte[] pbEncode()
        {
            var cidx = (int) ((idx++) % samples);
            return pbData[cidx].ToByteArray();
        }


        [Benchmark]
        public NodeGroupP pbDecode()
        {
            var cidx = (int) ((idx++) % samples);
            return NodeGroupP.Parser.ParseFrom(encodedPbData[cidx]);
        }
        
        [Benchmark]
        public byte[] bsonEncode()
        {
            var cidx = (int) ((idx++) % samples);
            return DoBsonEncodeData(data[cidx]);
        }


        [Benchmark]
        public NodeGroup bsonDecode()
        {
            var cidx = (int) ((idx++) % samples);
            return DoBsonDecodeData(encodedBsonData[cidx]);
        }

        [Benchmark]
        public byte[] jsonEncode()
        {
            var cidx = (int) ((idx++) % samples);
            return DoJsonEncodeData(data[cidx]);
        }

        [Benchmark]
        public NodeGroup jsonDecode()
        {
            var cidx = (int) ((idx++) % samples);
            return DoJsonDecodeData(encodedJsonData[cidx]);
        }

        [Benchmark]
        public byte[] msgPackEncode()
        {
            var cidx = (int) ((idx++) % samples);
            return DoMsgPackEncodeData(data[cidx]);
        }

        [Benchmark]
        public NodeGroup msgPackDecode()
        {
            var cidx = (int) ((idx++) % samples);
            return DoMsgPackDecodeData(encodedMsgPackData[cidx]);
        }

        
        private NodeGroup DoBsonDecodeData(byte[] encodedData)
        {
            MemoryStream ms = new MemoryStream(encodedData);
            using (BsonReader reader = new BsonReader(ms))
            {
                JsonSerializer serializer = new JsonSerializer();

                return serializer.Deserialize<NodeGroup>(reader);
            }
        }

        private byte[] DoBsonEncodeData(NodeGroup data)
        {
            MemoryStream ms = new MemoryStream();
            using (BsonWriter writer = new BsonWriter(ms))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(writer, data);
            }

            return ms.ToArray();
        }

        private NodeGroup DoJsonDecodeData(byte[] encodedData)
        {
            MemoryStream ms = new MemoryStream(encodedData);
            using (StreamReader sr = new StreamReader(ms))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                JsonSerializer serializer = new JsonSerializer();

                return serializer.Deserialize<NodeGroup>(reader);
            }
        }

        private byte[] DoJsonEncodeData(NodeGroup data)
        {
            MemoryStream ms = new MemoryStream();

            using (StreamWriter sw = new StreamWriter(ms))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(writer, data);
            }

            return ms.ToArray();
        }
        
        private NodeGroup DoMsgPackDecodeData(byte[] encodedData)
        {
            return MessagePack.MessagePackSerializer.Deserialize<NodeGroup>(encodedData);
        }

        private byte[] DoMsgPackEncodeData(NodeGroup data)
        {
            return MessagePack.MessagePackSerializer.Serialize(data);
        }
    }
}