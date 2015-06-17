using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinAvgWaitingTime
{
    /// <summary>
    /// https://www.hackerrank.com/challenges/minimum-average-waiting-time
    /// </summary>
    class Program
    {
        private static int N;

        public class PriorityQueue<T> where T : IComparable<T>
        {
            private List<T> data;

            public PriorityQueue()
            {
                this.data = new List<T>();
            }

            public void Enqueue(T item)
            {
                data.Add(item);
                int ci = data.Count - 1; // child index; start at end
                while (ci > 0)
                {
                    int pi = (ci - 1) / 2; // parent index
                    if (data[ci].CompareTo(data[pi]) >= 0) break; // child item is larger than (or equal) parent so we're done
                    T tmp = data[ci]; data[ci] = data[pi]; data[pi] = tmp;
                    ci = pi;
                }
            }

            public T Dequeue()
            {
                // assumes pq is not empty; up to calling code
                int li = data.Count - 1; // last index (before removal)
                T frontItem = data[0];   // fetch the front
                data[0] = data[li];
                data.RemoveAt(li);

                --li; // last index (after removal)
                int pi = 0; // parent index. start at front of pq
                while (true)
                {
                    int ci = pi * 2 + 1; // left child index of parent
                    if (ci > li) break;  // no children so done
                    int rc = ci + 1;     // right child
                    if (rc <= li && data[rc].CompareTo(data[ci]) < 0) // if there is a rc (ci + 1), and it is smaller than left child, use the rc instead
                        ci = rc;
                    if (data[pi].CompareTo(data[ci]) <= 0) break; // parent is smaller than (or equal to) smallest child so done
                    T tmp = data[pi]; data[pi] = data[ci]; data[ci] = tmp; // swap parent and child
                    pi = ci;
                }
                return frontItem;
            }

            public T Peek()
            {
                T frontItem = data[0];
                return frontItem;
            }

            public int Count()
            {
                return data.Count;
            }
        } // PriorityQueue

        class Customer : IComparable<Customer>
        {
            public long T { get; set; }
            public long L { get; set; }

            public int CompareTo(Customer other)
            {
                if (this == other) return 0;

                var cmp = this.L.CompareTo(other.L);
                if (cmp != 0) return cmp;

                return 1;
            }
        }

        static void Main(string[] args)
        {
            #region Read input
            N = int.Parse(Console.ReadLine());

            var customers = new List<Customer>();
            for (int i = 0; i < N; i++)
            {
                var values = Console.ReadLine().Split(' ').Select(s => long.Parse(s)).ToList();
                customers.Add(new Customer { T = values[0], L = values[1] });
            }
            #endregion

            customers.Sort((x, y) => x.T.CompareTo(y.T));

            PriorityQueue<Customer> pending = new PriorityQueue<Customer>();
            pending.Enqueue(customers[0]);

            var time = 0L;
            var SL = 0L;
            var SCL = 0L;
            var pos = 1;

            while (pending.Count() > 0)
            {
                var customer = pending.Dequeue();

                var min = customer.T;
                var deadTime = min > time ? min - time : 0L;

                var temp = customer.L + deadTime;
                SCL += temp;
                SL += SCL;
                time += temp;

                while (pos < customers.Count && customers[pos].T < time)
                {
                    pending.Enqueue(customers[pos]);
                    pos++;
                }
            }

            var SW = SL - customers.Sum(c => c.T);
            Console.WriteLine(SW / N);
        }
    }
}
