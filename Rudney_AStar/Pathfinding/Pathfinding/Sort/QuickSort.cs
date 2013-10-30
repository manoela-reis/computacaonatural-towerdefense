using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pathfinding
{
    public class QuickSort : Sort
    {
        public void Sort(Tree[] array)
        {
            Quicksort(array,0, array.Length-1);
        }

        void Quicksort(Tree[] array, int left, int right)
        {
            if (left < right)
            {
                int p = Partition(array, left, right);
                Quicksort(array, left, p - 1);
                Quicksort(array, p + 1, right);
            }
        }

        int Partition(Tree[] array, int left, int right)
        {
            Tree s = array[right];
            double p = s.module;
            int i = left;

            for (int j = left; j < right; j++)
            {
                if (array[j].module > p)
                {
                    Tree temp = array[i];
                    array[i] = array[j];
                    array[j] = temp;
                    i++;
                }
            }

            array[right] = array[i];
            array[i] = s;
            return i;
        }
    }
}
