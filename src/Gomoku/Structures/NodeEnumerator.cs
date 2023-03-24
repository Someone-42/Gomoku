using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomoku.Structures
{
    /// <summary>
    /// An enumerator for every index of existing nodes in an AccessLinkedList
    /// </summary>
    public class NodeEnumerator<T> : IEnumerator<int>
    {

        public int Current { get; protected set; }
        public int Next { get; protected set; }
        public T CurrentValue { get
            {
                return List.Values[Current];
            }
        }

        object IEnumerator.Current => Current;

        private AccessLinkedList<T> List;

        public NodeEnumerator(AccessLinkedList<T> list)
        {
            List = list;
            Reset();
        }

        public void Dispose() { }

        public bool MoveNext()
        {
            if (Current == List.Last)
                return false;
            Current = Next;
            Next = List.nodes[Current].y;
            return true;
        }

        public void Reset()
        {
            Next = List.First;
            Current = -1;
        }
    }
}
