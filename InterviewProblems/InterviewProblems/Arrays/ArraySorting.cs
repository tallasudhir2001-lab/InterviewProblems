using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewProblems.Arrays
{
    internal class ArraySorting
    {
        /*
         *0's -> 0-low-1
         *1's -> low->mid-1
         *2's -> mid -> high-1
         *the point of mid it to swap and move forward
         *the point of high is to move 2's to the end and move backwards
         */
        public void Sort012Array(int[] array)
        {
            /*[0,0,1,2]
             * low mid high
             * 0 - 0 to low-1
             * 1 - low to mid -1
             * 2 - mid -high -1
             */
            int low = 0, mid = 0, high = array.Length - 1;
            while (mid <= high)
            {
                if (array[mid] == 0)
                {
                    //first swap then increment or mis swap happens
                    (array[low], array[mid]) = (array[mid], array[low]);
                    low++;
                    mid++;
                }
                else if (array[mid] == 1)
                {
                    mid++;
                }
                else
                {
                    (array[mid], array[high]) = (array[high], array[mid]);
                    high--;
                }
            }
        }
        public void MoveNegativeElementsToOneSideOfArray(int[] array)
        {
            int left=0, right=array.Length-1;
            while (left < right)
            {
                if(array[left] < 0)
                {
                    left++;
                }
                else if (array[right] >= 0)
                {
                    right--;
                }
                else
                {
                    (array[left], array[right]) = (array[right],array[left]);
                }
            }
        }

    }
}
