using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewProblems.Arrays
{
    internal class MissingElement
    {
        public void MissingElementInAnArrayBadApproch(int[] array)
        {
            Array.Sort(array);

            for (int i = 1; i < array.Length ; i++)
            {
                if (array[i] - array[i - 1] != 1)
                {
                    Console.Write(array[i-1] + 1);
                    break;
                }
            }
        }

        public void MissingElementInGivenArray(int[] array)
        {
            int max = array[0], arraySum = array[0];
            for (int i = 1; i < array.Length; i++)
            {
                arraySum += array[i];
                if (array[i] > max)
                {
                    max = array[i];
                }
            }
            int SumUntilMax = max * (max + 1) / 2;
            Console.WriteLine($"Missing Number is {SumUntilMax-arraySum}");
        }
    }
}
