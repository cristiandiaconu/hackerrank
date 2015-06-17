using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckTour
{
    class Program
    {
        class Pump
        {
            public long Amount { get; set; }
            public long DistanceToNext { get; set; }
            public int Index { get; set; }
        }

        /// <summary>
        /// https://www.hackerrank.com/challenges/truck-tour
        /// </summary>
        static void Main(string[] args)
        {
            var N = int.Parse(Console.ReadLine());
            Queue<Pump> pumps = new Queue<Pump>();
            for (int i = 0; i < N; i++)
            {
                var values = Console.ReadLine().Split(' ').Select(s => long.Parse(s)).ToArray();
                pumps.Enqueue(new Pump { Amount = values[0], DistanceToNext = values[1], Index = i });
            }

            Queue<Pump> order = new Queue<Pump>();
            var totalPetrol = 0L;
            var totalDistance = 0L;

            while (pumps.Any())
            {
                var crt = pumps.Dequeue();
                if (!pumps.Any())
                {
                    break;
                }

                order.Enqueue(crt);
                totalPetrol += crt.Amount;
                totalDistance += crt.DistanceToNext;

                while (order.Any() && totalPetrol < totalDistance)
                {
                    var start = order.Dequeue();
                    totalPetrol -= start.Amount;
                    totalDistance -= start.DistanceToNext;
                    pumps.Enqueue(start);
                }                
            }

            Console.WriteLine(order.Peek().Index);
        }
    }
}
