using System;
using System.Collections.Generic;
using System.Text;

namespace c_sharp_serialization_bench
{
    public static class UtilsCommon
    {
        public static int DefaultMapSize = 5;
        public static int DefaultChildNodesAmount = 3;
        public static int DefaultStrLen = 10;

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

        public static NodeMeta RandomMeta()
        {
            var meta = new Dictionary<string, string>();
            for (int i = 0; i < DefaultMapSize; i++)
            {
                meta.Add(RandomString(DefaultStrLen), RandomString(DefaultStrLen));
            }
            return new NodeMeta(meta);
        }

        public static NodeBase RandomNodeBase()
        {
            var rnd = new Random();
            var rndBool = rnd.NextDouble() >= 0.5;
            var rndStr = RandomString(DefaultStrLen);
            return new NodeBase(rndBool, rndStr, rndStr, RandomMeta());
        }

        public static Node RandomChildren(int depth)
        {
            var rnd = new Random();
            if (depth == 0)
            {
                return new NVar(rnd.NextDouble());
            } 
            else
            {
                var nBase = RandomNodeBase();
                var lst = new List<Node>(DefaultChildNodesAmount);
                for (int i = 0; i < DefaultChildNodesAmount; ++ i)
                {
                    lst.Add(RandomChildren(depth - 1));
                }
                return new NGroup(new NodeGroup(RandomNodeBase(),lst));
            }
        }


        public static NodeGroup RandomData(int nodes, int depth)
        {
            var nBase = RandomNodeBase();
            var lst = new List<Node>(nodes);
            for (int i = 0; i < nodes; ++i)
            {
                lst.Add(RandomChildren(depth));
            }
            return new NodeGroup(RandomNodeBase(),lst);
        }
    }
}