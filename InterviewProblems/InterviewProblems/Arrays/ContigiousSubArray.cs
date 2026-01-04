using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewProblems.Arrays
{
    internal class ContigiousSubArray//kadane' Algorithm
    {
        public void MaxSumOfSubArray(int[] array)
        {
            int maxSum = array[0], currentSum = array[0];

            for (int i = 1; i < array.Length; i++)
            {
                currentSum = Math.Max(currentSum + array[i], array[i]);
                maxSum= Math.Max(maxSum, currentSum);
            }
            Console.WriteLine(" MaxSum is {maxSum}");
        }
        public (int maxSum,int start, int end) MaxSumAndPrintTheSubArray(int[] array)
        {
            int currentSum = array[0], maxSum = array[0], start = 0, end = array.Length - 1, tempstart = 0;
            for(int i=1; i < array.Length; i++)
            {
                if (array[i] > array[i] + currentSum)
                {
                    currentSum=array[i];
                    tempstart=i;
                }
                else
                {
                    currentSum += array[i];
                }
                if (currentSum > maxSum)
                {
                    maxSum=currentSum;
                    start=tempstart;
                    end=i;
                }
            }
            return (maxSum, start, end);
            //Console.WriteLine($"MaxContgiousSubArraySum is {maxSum}");
            //Console.WriteLine($"SubArrayWithMaxSum is{string.Join(", ", array[start,end])}");
        }

        public (int maxSum, int start, int end) MaxProductAndPrintTheSubArray(int[] array)
        {
            int maxProduct = array[0],currentMax=array[0],start=0,end =array.Length - 1, tempstart = 0;
            for(int i = 1; i < array.Length; i++)
            {
                if (array[i]> array[i] * currentMax)
                {
                    currentMax=array[i];
                    tempstart=i;
                }
                else
                {
                    currentMax*=array[i];
                }
                if(currentMax > maxProduct)
                {
                    maxProduct = currentMax;
                    start=tempstart;
                    end=i;
                }
            }
            return (maxProduct, start, end);
            //Console.WriteLine($"MaxContgiousSubArraySum is {maxProduct}");
            //Console.WriteLine($"SubArrayWithMaxSum is{string.Join(", ", array)}");
        }

    }
}
