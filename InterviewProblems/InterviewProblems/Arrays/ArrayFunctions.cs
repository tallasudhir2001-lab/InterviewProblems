using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewProblems.Arrays
{
    internal class ArrayFunctions
    {
        public void ReadArrayInput()
        {
            int[] array = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
        }

    }
}
