using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LargestRectangle
{
    class Program
    {
        class Local
        {
            public int X { get; set; }
            public int Min { get; set; }
            public long Value { get; set; }

            public Local(int x, int min)
            {
                X = x;
                Min = min;
                Value = min * x;
            }

            public override string ToString()
            {
                return X + "x" + Min;
            }
        }

        /// <summary>
        /// https://www.hackerrank.com/challenges/largest-rectangle
        /// </summary>
        static void Main(string[] args)
        {
            var N = int.Parse(Console.ReadLine());

            var h = Console.ReadLine().Split(' ').Select(s => int.Parse(s)).ToList();
            int maxArea = 0;
            Stack<int> stack = new Stack<int>();
            h.Add(0);

            // copied from https://github.com/rickytan/LeetCode/blob/master/largest-rectangle-in-histogram/main.cpp
            for (int i = 0; i < h.Count; i++)
            {
                if (!stack.Any() || h[i] > h[stack.Peek()])
                    stack.Push(i);
                else {
                    int tmp = stack.Pop();
                    int area = h[tmp] * (!stack.Any() ? i : (i-stack.Peek() - 1));
                    maxArea = Math.Max(maxArea, area);
                    i--;
                }
            }

            Console.WriteLine(maxArea);
        }

        private static Local Combine(Local h1, Local h2)
        {
            return new Local(h1.X + h2.X, Math.Min(h1.Min, h2.Min));
        }
    }
}
