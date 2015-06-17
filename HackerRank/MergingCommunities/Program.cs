using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MergingCommunities
{
    /// <summary>
    /// https://www.hackerrank.com/challenges/merging-communities
    /// </summary>
    class Program
    {
        // WIP
        public class UnionFindNode
        {
            private UnionFindNode _parent;
            private List<UnionFindNode> _children;
            private uint _rank;

            public int Count { get { return _children.Distinct().Count(); } }

            /// <summary>
            /// Creates a new disjoint node, representative of a set containing only the new node.
            /// </summary>
            public UnionFindNode()
            {
                _parent = this;
                _children = new List<UnionFindNode>();
                _children.Add(this);
            }

            /// <summary>
            /// Returns the current representative of the set this node is in.
            /// Note that the representative is only accurate untl the next Union operation.
            /// </summary>
            public UnionFindNode Find()
            {
                if (!ReferenceEquals(_parent, this))
                {
                    var newParent = _parent.Find();
                    if (_parent != newParent)
                    {
                        newParent._children.AddRange(_parent._children);
                        _parent = newParent;
                    }                    
                }
                return _parent;
            }

            /// <summary>
            /// Determines whether or not this node and the other node are in the same set.
            /// </summary>
            public bool IsUnionedWith(UnionFindNode other)
            {
                if (other == null) throw new ArgumentNullException("other");
                return ReferenceEquals(Find(), other.Find());
            }

            /// <summary>
            /// Merges the sets represented by this node and the other node into a single set.
            /// Returns whether or not the nodes were disjoint before the union operation (i.e. if the operation had an effect).
            /// </summary>
            /// <returns>True when the union had an effect, false when the nodes were already in the same set.</returns>
            public bool Union(UnionFindNode other)
            {
                if (other == null) throw new ArgumentNullException("other");
                var root1 = this.Find();
                var root2 = other.Find();
                if (ReferenceEquals(root1, root2)) return false;

                if (root1._rank < root2._rank)
                {
                    var oldParent = root1._parent;
                    root1._parent = root2;
                    root2._children.AddRange(oldParent._children);
                }
                else if (root1._rank > root2._rank)
                {
                    var oldParent = root2._parent;
                    root2._parent = root1;
                    root1._children.AddRange(oldParent._children);
                }
                else
                {
                    var oldParent = root2._parent;
                    root2._parent = root1;
                    root1._children.AddRange(oldParent._children);
                    root1._rank++;
                }
                return true;
            }

            
        }

        private static int N, Q;
        private static List<UnionFindNode> communities;
      
        static void Main(string[] args)
        {
            var tokens = Console.ReadLine().Split(' ').Select(s => int.Parse(s)).ToArray();
            N = tokens[0];
            Q = tokens[1];

            communities = new List<UnionFindNode>();
            for (int i = 0; i < N; i++)
            {
                communities.Add(new UnionFindNode());
            }

            //Debugger.Launch();

            for (int i = 0; i < Q; i++)
            {
                var stokens = Console.ReadLine().Split(' ').ToArray();
                if (stokens[0] == "Q")
                {
                    QueryQ(int.Parse(stokens[1]));
                }
                else
                {
                    QueryM(int.Parse(stokens[1]), int.Parse(stokens[2]));
                }
            }
        }

        private static void QueryQ(int i)
        {
            Console.WriteLine(communities[i - 1].Find().Count);
        }

        private static void QueryM(int i, int j)
        {
            communities[i-1].Union(communities[j-1]);
        }
    }
}
