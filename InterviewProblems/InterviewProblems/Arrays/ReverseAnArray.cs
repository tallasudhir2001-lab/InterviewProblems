using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewProblems.Arrays
{
    internal class ReverseAnArray
    {
        /*
            ReverseAnArray reverseAnArray = new ReverseAnArray();
            Console.WriteLine(string.Join(" ",reverseAnArray.ReverseArray(Console.ReadLine().Split(' ').Select(int.Parse).ToArray())));
       */
        public int[] ReverseArray(int[] array)
        {
            for (int i = 0; i < array.Length / 2; i++)
            {
                int temp = array[i];
                array[i] = array[array.Length - 1];
                array[array.Length - 1] = temp;
            }
            return array;
        }
        public int[] ReverseArrayUsingBuiltIn(int[] array)
        {
           Array.Reverse(array);
           return array;
        }
        public int[] ReverseArrayUsingWhileLoop(int[] array)
        {
            int left = 0, right = array.Length - 1;
            while (left < right)
            {
                int temp = array[left];
                array[left] = array[right];
                array[right] = temp;
            }
            return array;
        }
        public void ReverseArrayUsingRecursion(int[] array, int left, int right)
        {
            if (left >= right) return;

            int temp=array[left];
            array[left] = array[right];
            array[right] = temp;

            ReverseArrayUsingRecursion(array, left+1, right-1);
        }
        public void ReverseArrayUsingRecursionForAnyDataType<T>(T[] array, int left, int right)
        {
            if (left > right) return;

            var temp = array[left];
            array[left] = array[right];
            array[right] = temp;

            ReverseArrayUsingRecursionForAnyDataType(array, left + 1, right - 1);
        }

    }
}
