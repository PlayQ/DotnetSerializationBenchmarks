using System;
using System.IO;
using BenchmarkDotNet.Attributes;
using Google.Protobuf;
using Codecs.Proto;
using System.Text;
using System.Collections.Generic;

namespace c_sharp_serialization_bench
{
    public class ProtobufSerializationBenchmark 
    {
        public NodeGroupP group;
        // public MemoryStream streamOut = new MemoryStream();
        // public MemoryStream outBuf = new MemoryStream();

        public byte[] encodedData;

        [GlobalSetup]
        public void GlobalSetup()
        {
           group = ProtobufUtils.RandomData(1000, 6);
           encodedData = group.ToByteArray();
           GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
        }

        // [GlobalCleanup]
        // public void Cleanup()
        // {
        //     //outBuf.Seek(0, SeekOrigin.Begin);
        // }


        [Benchmark]
        public byte[] encodeData() {
            // MemoryStream outBuf = new MemoryStream();
            // group.WriteTo(outBuf);
            // return outBuf;
            return group.ToByteArray();
        }

        [Benchmark]
        public NodeGroupP decodeData() {
            return NodeGroupP.Parser.ParseFrom(encodedData);
        }
    }


    // Utilitary stuff
    public static class ProtobufUtils 
    {
        public static NodeMetaP RandomMeta(int size)
        {
            var meta = new NodeMetaP();
            for (int i = 0; i < size; i++)
            {
                meta.MetaInfo.Add(UtilsCommon.RandomString(UtilsCommon.DefaultStrLen), UtilsCommon.RandomString(UtilsCommon.DefaultStrLen));
            }
            return meta;
        }

        public static NodeBaseP RandomNodeBase()
        {
            var rnd = new Random();
            var nodeBase = new NodeBaseP();
            nodeBase.Hidden = rnd.NextDouble() >= 0.5;
            nodeBase.Id = UtilsCommon.RandomString(UtilsCommon.DefaultStrLen);
            nodeBase.Name = UtilsCommon.RandomString(UtilsCommon.DefaultStrLen);
            nodeBase.Meta = RandomMeta(UtilsCommon.DefaultMapSize);
            return nodeBase;
        }

        public static NodeP RandomChildren(int depth)
        {
            var node = new NodeP();
            var rnd = new Random();
            if (depth == 0)
            {
                NVarP nodeVar = new NVarP();
                nodeVar.Value = rnd.NextDouble();
                var n = new NodeP();
                n.Nvar = nodeVar;
            } 
            else
            {
                var ngroup = new NGroupP();
                var group = new NodeGroupP();
                group.Base = RandomNodeBase();
                var lst = new List<NodeP>(UtilsCommon.DefaultChildNodesAmount);
                for (int i = 0; i < lst.Capacity; ++i)
                {
                    lst.Add(RandomChildren(depth - 1));
                }
                group.Children.AddRange(lst);
                ngroup.Group = group;
                node.Ngroup = ngroup;
            }
         
            return node;
        }


        public static NodeGroupP RandomData(int nodes, int depth)
        {
            var group = new NodeGroupP();
            group.Base = RandomNodeBase();
            var lst = new List<NodeP>(nodes);
            for (int i = 0; i < nodes; ++i)
            {
                lst.Add(RandomChildren(depth));
            }
            group.Children.AddRange(lst);
            return group;
        }
    }
}
