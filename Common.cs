using System.Collections.Generic;
using MessagePack;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace c_sharp_serialization_bench
{
    [MessagePackObject]
    public sealed class NodeMeta
    {
        [BsonConstructor("MetaInfo")]
        public NodeMeta(Dictionary<string, string> meta)
        {
            MetaInfo = meta;
        }

        [Key(0)]
        [BsonElementAttribute]
        public Dictionary<string, string> MetaInfo { get; }
    }


    [MessagePackObject]
    public sealed class NodeBase
    {
        [BsonConstructor("Hidden", "Id", "Name", "Meta")]
        public NodeBase(bool hidden, string id, string name, NodeMeta meta)
        {
            Hidden = hidden;
            Id = id;
            Name = name;
            Meta = meta;
        }
        [Key(0)]
        [BsonElementAttribute]
        public bool Hidden { get; }
        [Key(1)]
        [BsonElementAttribute]
        public string Id { get; }
        [Key(2)]
        [BsonElementAttribute]
        public string Name { get; }
        [Key(3)]
        [BsonElementAttribute]
        public NodeMeta Meta { get; }
    }

    [MessagePack.Union(0, typeof(NVar))]
    [MessagePack.Union(1, typeof(NGroup))]
    [BsonDiscriminator(Required = true, RootClass = true)]
    [BsonKnownTypes(typeof(NVar), typeof(NGroup))]
    public abstract class Node {}

    [MessagePackObject]
    public class NVar : Node
    {
        [BsonConstructor("Value")]
        public NVar(double value)
        {
            Value = value;
        }

        [Key(0)]
        [BsonElementAttribute]
        public double Value { get; }
    }

    [MessagePackObject]
    public class NGroup : Node
    {
        [BsonConstructor("Value")]
        public NGroup(NodeGroup value)
        {
            Value = value;
        }

        [Key(0)]
        [BsonElementAttribute]
        public NodeGroup Value { get; }
    }

    [MessagePackObject]
    public sealed class NodeGroup
    {
        [JsonConstructor]
        [BsonConstructor("Base", "Nodes")]
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
        [BsonElementAttribute]
        public NodeBase Base { get; }
        [Key(1)]
        [BsonElementAttribute]
        public List<Node> Nodes { get; }
    }
}