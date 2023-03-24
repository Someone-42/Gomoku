using Gomoku;
using Gomoku.Structures;

namespace GomokuTesting;

[TestClass]
public class AccessLinkedListTest
{

    private AccessLinkedList<int> CreateAuto(int size)
    {
        AccessLinkedList<int> a = new AccessLinkedList<int>(size);
        for (int i = 0; i < size; i++)
        {
            a.Values[i] = i;
        }
        return a;
    }

    public bool IsCorrect(int[] arr, AccessLinkedList<int> a, int start = 0, bool reverse = false)
    {
        int step = reverse ? -1 : 1;
        int i = start;
        foreach(int j in a)
        {
            if (arr[i] != a.Values[j])
                return false;
            i += step;
        }
        return true;
    }

    public bool IsCorrectRange(AccessLinkedList<int> a, int start = 0, int step = 1, int? count = null)
    {
        int c = start;
        int total = count ?? a.Count;
        foreach (int j in a)
        {
            if (c != a.Values[j])
                return false;
            c += step;
            if (Math.Abs(c - start) > total)
                break;
        }
        return true;
    }

    public void IsCleared(AccessLinkedList<int> a)
    {
        Assert.IsTrue(a.IsEmpty());
        foreach (int i in a)
        {
            throw new Exception("a should be empty, therefore there should be no iterations");
        }
    }

    [TestMethod]
    public void ManualClear()
    {
        AccessLinkedList<int> a = new AccessLinkedList<int>(5);
        for (int i = 0; i < a.Max; i++)
        {
            a.Pop(i);
        }
        Assert.IsTrue(a.IsEmpty());
    }

    [TestMethod]
    public void Enumerator1()
    {
        AccessLinkedList<int> a = CreateAuto(5);
        int counter = 0;
        foreach (int i in a)
        {
            Assert.AreEqual(counter, i);
            counter++;
        }
    }

    [TestMethod]
    public void Enumerator2()
    {
        AccessLinkedList<int> a = CreateAuto(5);
        int[] results = new int[3] { 0, 1, 3 };
        a.Pop(2); a.Pop(4);
        IsCorrect(results, a);
    }

    [TestMethod]
    public void EmbedEnumerator1()
    {
        AccessLinkedList<int> a = CreateAuto(6);
        int[] results = new int[6 * 5]
        {
            1, 2, 3, 4, 5,  // First line, we popped 0
            0, 2, 3, 4, 5,  // Etc...
            0, 1, 3, 4, 5,
            0, 1, 2, 4, 5,
            0, 1, 2, 3, 5,
            0, 1, 2, 3, 4
        };
        int c = 0;
        foreach (int i in a)
        {
            a.Pop(i);
            IsCorrect(results, a, c);
            c += a.Count;
            a.Push(i);
        }
    }

    [TestMethod]
    public void PopThenPush1()
    {
        AccessLinkedList<int> a = CreateAuto(5);
        a.Pop(0);
        a.Pop(3);
        a.Push(3);
        a.Push(0);
        int c = 0;
        foreach (int i in a)
        {
            Assert.AreEqual(c++, i);
        }
    }

    [TestMethod]
    public void PopThenPush2()
    {
        AccessLinkedList<int> a = CreateAuto(6);
        int[] l = new int[6] { 0, 1, 5, 3, 2, 4 };
        foreach (int i in l)
        {
            a.Pop(i);
        }
        IsCleared(a);
        for (int i = l.Length - 1; i >= 0; i--)
        {
            a.Push(l[i]);
        }
        IsCorrectRange(a);
    }

    [TestMethod]
    public void RecursiveBoard1()
    {
        bool[] v = new bool[5];
        AccessLinkedList<int> a = CreateAuto(5);
        List<int> rec1 = new List<int>(5 * 4 * 3 * 2);
        List<int> rec2 = new List<int>(5 * 4 * 3 * 2);
        RecurseBoardTest1(rec1, v, 0, 5);
        RecurseBoardTest2(rec2, a, v, 0, 5);
        /*int maskFirst = (1 << 16) - 1, maskEnd = maskFirst << 16;
        Console.WriteLine(maskFirst);
        Console.WriteLine(maskEnd);
        string[] depthElement = new string[5]
        {
            ">", "> >", "> > >", "> > > >", "> > > > >"
        };*/
        for (int i = 0; i < rec1.Count(); i++)
        {
            /*
            Console.WriteLine(depthElement[maskFirst & rec1[i]] + " 1 : " + ((rec1[i] & maskEnd) >> 16));
            Console.WriteLine(depthElement[maskFirst & rec2[i]] + " 2 : " + ((rec2[i] & maskEnd) >> 16));
            */
            Assert.AreEqual(rec1[i], rec2[i]);
        }
    }

    private void RecurseBoardTest1(List<int> rec, bool[] v, int depth, int maxDepth)
    {
        if (depth >= maxDepth)
            return;

        for(int i = 0; i < v.Length; i++)
        {
            if (!v[i])
            {
                rec.Add((i << 16) | depth);
                v[i] = true;
                RecurseBoardTest1(rec, v, depth + 1, maxDepth);
                v[i] = false;
            }
        }
    }

    private void RecurseBoardTest2(List<int> rec, AccessLinkedList<int> a, bool[] v, int depth, int maxDepth)
    {
        if (depth >= maxDepth)
            return;
        if (a.IsEmpty())
            return;

        int p = 0;
        foreach (int i in a)
        {
            p = a.Pop(i);
            rec.Add((p << 16) | depth);
            v[p] = true;
            RecurseBoardTest2(rec, a, v, depth + 1, maxDepth);
            v[p] = false;
            a.Push(i);
        }
    }

}