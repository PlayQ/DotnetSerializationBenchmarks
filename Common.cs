using System.Collections.Generic;
using MessagePack;
using Newtonsoft.Json;


namespace c_sharp_serialization_bench
{
    [MessagePackObject]
    public sealed class NodeMeta
    {
        public NodeMeta(Dictionary<string, string> meta)
        {
            MetaInfo = meta;
        }

        [Key(0)]
        public Dictionary<string, string> MetaInfo { get; }
    }


    [MessagePackObject]
    public sealed class NodeBase
    {
        public NodeBase(bool hidden, string id, string name, NodeMeta meta)
        {
            Hidden = hidden;
            Id = id;
            Name = name;
            Meta = meta;
        }
        [Key(0)]
        public bool Hidden { get; }
        [Key(1)]
        public string Id { get; }
        [Key(2)]
        public string Name { get; }
        [Key(3)]
        public NodeMeta Meta { get; }
    }

    [MessagePack.Union(0, typeof(NVar))]
    [MessagePack.Union(1, typeof(NGroup))]
    // [BsonDiscriminator(Required = true, RootClass = true)]
    // [BsonKnownTypes(typeof(NVar), typeof(NGroup))]
    public abstract class Node {}

    [MessagePackObject]
    public class NVar : Node
    {
        public NVar(double value)
        {
            Value = value;
        }

        [Key(0)]
        public double Value { get; }
    }

    [MessagePackObject]
    public class NGroup : Node
    {
        public NGroup(NodeGroup value)
        {
            Value = value;
        }

        [Key(0)]
        public NodeGroup Value { get; }
    }

    [MessagePackObject]
    public sealed class NodeGroup
    {
        [JsonConstructor]
        public NodeGroup(NodeBase nodeBase, List<Node> nodes)
        {
            Base = nodeBase;
            Nodes = nodes;
        }

        public NodeGroup(NodeGroup other)
        {
            Base = other.Base;
            Nodes = other.Nodes;
        }

        [Key(0)]
        public NodeBase Base { get; }
        [Key(1)]
        public List<Node> Nodes { get; }
    }
}