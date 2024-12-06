using BugBot.Objects.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBot.Managers
{
    
    internal static class Trie
    {
        private static Node root = new Node();
        public static void Insert(string input)
        {
            root.Insert(input);
        }

        public static string Find(string input)
        {
            return root.Find(input);
        }


    }
}
