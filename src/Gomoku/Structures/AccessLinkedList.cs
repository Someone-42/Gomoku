using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;

namespace Gomoku.Structures
{
    /// <summary>
    /// Code is very unsafe, push and pop functions should be used like a stack
    /// </summary>
    /// <typeparam name="T">Type of the values for each node</typeparam>
    public class AccessLinkedList<T> : IEnumerable
    {
        public int Count { get; protected set; }
        public int Max;
        public T[] Values;
        public int First, Last;
        public Position[] nodes;

        public AccessLinkedList(int nodeCount)
        {
            Max = nodeCount;
            Count = Max;
            Values = new T[nodeCount];
            nodes = new Position[nodeCount];
            First = 0;
            Last = nodeCount - 1;
            for (int i = 0; i < nodeCount; i++)
            {
                nodes[i] = new Position(i - 1, i + 1);
            }
        }

        public bool IsEmpty()
        {
            return Count == 0;
        }

        public bool IsFull()
        {
            return Count == Max;
        }

        public T Pop(int index)
        {
            Count--;
            if (Count < 0)
                throw new Exception();
            if (index > First)
                nodes[nodes[index].x].y = nodes[index].y;
            else if (index == First)
                First = nodes[index].y;
            if (index < Last)
                nodes[nodes[index].y].x = nodes[index].x;
            else if (index == Last)
                Last = nodes[index].x;
            return Values[index];
        }

        public void Push(int index)
        {
            Count++;
            if (index > First)
                nodes[nodes[index].x].y = index;
            else if (index < First)
                First = index;
            if (index < Last)
                nodes[nodes[index].y].x = index;
            else if (index > Last)
                Last = index;
        }

        public IEnumerator GetEnumerator()
        {
            return new NodeEnumerator<T>(this);
        }

        public int Next(int index)
        {
            return nodes[index].y;
        }

        public int Previous(int index)
        {
            return nodes[index].x;
        }
    }
}
