using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewProblems.Arrays
{
    internal class UnionAndIntersection
    {
        //arrays must be sorted first or assume the given arrays are sorted
        public void ArrayIntersection(int[] array1, int[] array2)
        {
            Array.Sort(array1);
            Array.Sort(array2);
            List<int> intersection = new List<int>();
            int i=0; int j=0;
            while(i < array1.Length && j < array2.Length)
            {
                if(array1[i] < array2[j])
                {
                    i++;
                }
                else if(array1[i] > array2[j])
                {
                    j++;
                }
                else
                {
                    intersection.Add(array1[i]);
                    i++;
                    j++;
                }
            }
            Console.WriteLine(string.Join(" ",intersection));
        }
        public void ArrayUnion(int[] array1, int[] array2)
        {
            List<int> union = new List<int>();
            int i = 0, j = 0;
            while(i < array1.Length &&j < array2.Length)
            {
                if(array1[i] < array2[j])
                {
                    union.Add(array1[i]);
                    i++;
                }
                else if(array1[i] > array2[j]){ 
                    union.Add(array2[j]);
                }
                else
                {
                    union.Add(array1[i]);
                    i++;
                    j++;
                }
            }
            while (i < array1.Length)
            {
                union.Add(array1[i]);
                i++;
            }
            while(j < array2.Length)
            {
                union.Add(array2[j]);
                j++;
            }
            Console.WriteLine(string.Join(" ", union));
        }
    }
}
