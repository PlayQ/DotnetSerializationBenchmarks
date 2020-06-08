using System;
using BenchmarkDotNet;
using System.IO;
using BenchmarkDotNet.Attributes;
using System.Collections.Generic;
using Google.Protobuf;
using BenchmarkDotNet.Running;
using Common;
using System.Text;

namespace c_sharp_serialization_bench
{
    public static class Utils 
    {
        public static string RandomString(int size, bool lowerCase = false)  
        {  
            StringBuilder builder = new StringBuilder();  
            Random random = new Random();  
            char ch;  
            for (int i = 0; i < size; i++)  
            {  
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));  
                builder.Append(ch);  
            }  
            if (lowerCase)  
                return builder.ToString().ToLower();  
            return builder.ToString();  
        }

        public static NodeMeta RandomMeta(int size)
        {
            var meta = new NodeMeta();
            for (int i = 0; i < size; i++)
            {
                meta.General.Add(RandomString(10), RandomString(10));
            }
            return meta;
        }

        public static NodeBase RandomNodeBase(int size)
        {
            var rnd = new Random();
            var nodeBase = new NodeBase();
            nodeBase.Hidden = rnd.NextDouble() >= 0.5;
            nodeBase.Id = RandomString(10);
            nodeBase.Name = RandomString(10);
            nodeBase.Meta = RandomMeta(size);
            return nodeBase;
        }

        public static Node RandomChildren(int depth)
        {
            var node = new Node();
            var rnd = new Random();
            var group = new NodeGroup();
            NodeVar nodeVarG = new NodeVar();
            if (rnd.Next(2) == 0)
            {
                group.Base = RandomNodeBase(10);
                NodeVar nodeVar = new NodeVar();
                var t = new NodeVarDouble();
                t.Val = 0.0; 
                nodeVar.Nd = t;
                node.Var = nodeVar;
                var n = new Node();
                n.Var = nodeVar;
                group.Children.Add(n);
                node.Group = group;
            } 
            else
            {
                var t = new NodeVarDouble();
                t.Val = 0.0; 
                nodeVarG.Nd = t;
                node.Var = nodeVarG;
            }
         
            return node;
        }


        //  public struct NodeMeta {
        // public Dictionary<string, string> map;

        // public NodeMeta(int collectionSize)
        // {
        //     var tmpStore = Array.Empty<KeyValuePair<string, string>>();
        //     Array.Fill(tmpStore, KeyValuePair.Create(Utils.RandomString(10), Utils.RandomString(10)));
        //     map = new Dictionary<string, string>(tmpStore);
        // }

        // public sealed class NodeBase {
        //     public NodeBase(bool hidden, string id, string name, NodeMeta meta)
        //     {
        //         Hidden = hidden;
        //         Id = id;
        //         Name = name;
        //         Meta = meta;
        //     }

        //     public bool Hidden { get; }
        //     public string Id { get; }
        //     public string Name { get; } 
        //     public NodeMeta Meta { get; }
        // }
    }

    // public class MsgPackSerializationBenchmark
    // {
    //     [Benchmark]
    //     byte[] encode() {}
    // }


    public class ProtobufSerializationBenchmark {
        [Params(10, 100, 1000)]
        public int N;
        
        public NodeGroup group = new NodeGroup();
        public MemoryStream streamOut = new MemoryStream();

        [GlobalSetup]
        public void GlobalSetup()
        {
            group.Base = Utils.RandomNodeBase(10);
            for (int i = 0; i < N; ++i)
            {
                group.Children.Add(Utils.RandomChildren(10));
            }
            group.WriteTo(streamOut);
        }


        [Benchmark]
        public MemoryStream encodeData() {
            //NodeGroup.
            MemoryStream outBuf = new MemoryStream();
            group.WriteTo(outBuf);
            return outBuf;
        }

        [Benchmark]
        public NodeGroup decodeData() {
            return NodeGroup.Parser.ParseFrom(streamOut);
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            BenchmarkRunner.Run<ProtobufSerializationBenchmark>();
        }
    }
}
