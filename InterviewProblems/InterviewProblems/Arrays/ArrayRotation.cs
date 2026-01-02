using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace InterviewProblems.Arrays
{
    internal class ArrayRotation
    {
        public void RightSideRotationByKNotOptimized(int[] array, int k)
        {
            for (int w = 0; w < k; w++)
            {
                int element = array[array.Length - 1];
                int i = array.Length - 2;
                while (i >= 0)
                {
                    array[i + 1] = array[i];
                    i--;
                }
                array[0] = element;
            }
            Console.WriteLine(string.Join(" ", array));
        }
        public void RightSideRotationByK(int[] array, int k,bool isClockWiseRotation)
        {
            /*
             * when K is 100 you dont have to rotate 100 times, if array size is 6 then k=k/array size=16
             */
            int arrayLength=array.Length;
            if (k > arrayLength)
            {
                k = k % arrayLength;
            }
            if (isClockWiseRotation)
            {
                Reverse(array, 0, arrayLength - 1);
                Reverse(array, 0, k - 1);
                Reverse(array, k, arrayLength - 1);
            }
            else
            {
                Reverse(array, 0, k - 1);
                Reverse(array, k, arrayLength - 1);
                Reverse(array, 0, arrayLength - 1);
            }
            Console.WriteLine(string.Join(" ", array));
        }
        public void Reverse(int[] array, int left, int right)
        {
            while(left < right)
            {
                (array[left], array[right]) = (array[right], array[left]);
                left++;
                right--;
            }
        }

    }
}
