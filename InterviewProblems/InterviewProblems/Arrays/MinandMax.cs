using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewProblems.Arrays
{
    internal class MinandMax
    {
        public void FindMinimum(int[] array)
        {
            int min = array[0];
            for(int i=1;i<array.Length; i++)
            {
                if(array[i] < min)
                {
                    min = array[i];
                }
            }
            Console.WriteLine(min);
        }
        public void FindMaximum(int[] array)
        {
            int max = array[0];
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i] < max)
                {
                    max = array[i];
                }
            }
            Console.WriteLine(max);
        }
        public void FindKthMin(int[] array, int k)
        {
            Array.Sort(array);

            Console.WriteLine(array[k-1]);
        }
        public void FindKthMax(int[] array, int k)
        {
            Array.Sort(array);

            Console.WriteLine(array[array.Length - k]);
        }

        
    }
}
