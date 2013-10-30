using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pathfinding
{
    public class HeapSort : Sort
    {
        public void Sort(Tree[] array)
        {
            Heapsort(array);
        }

        public void Heapsort(Tree[] v)
        {
            buildMaxHeap(v);
            int n = v.Length;

            for (int i = v.Length - 1; i > 0; i--)
            {
                swap(v, i, 0);
                maxHeapify(v, 0, --n);
            }
        }

        private void buildMaxHeap(Tree[] v)
        {
            for (int i = v.Length / 2 - 1; i >= 0; i--)
                maxHeapify(v, i, v.Length);
        }

        private void maxHeapify(Tree[] v, int pos, int n)
        {
            int max = 2 * pos + 1, right = max + 1;
            if (max < n)
            {
                if (right < n && v[max].module > v[right].module)
                    max = right;

                if (v[max].module < v[pos].module)
                {
                    swap(v, max, pos);
                    maxHeapify(v, max, n);
                }
            }
        }

        public static void swap(Tree[] v, int j, int aposJ)
        {
            Tree aux = v[j];
            v[j] = v[aposJ];
            v[aposJ] = aux;
        }
    }
}
