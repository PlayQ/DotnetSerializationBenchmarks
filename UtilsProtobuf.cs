using System;
using System.Collections.Generic;
using Codecs.Proto;

namespace c_sharp_serialization_bench
{
    public static class ProtobufUtils 
    {
        private static Random rnd = new Random();

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