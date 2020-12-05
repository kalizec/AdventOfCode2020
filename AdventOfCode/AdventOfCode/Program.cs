using AdventOfCode.Days;
using System;
using System.Linq;

namespace AdventOfCode
{
    class Program
    {
        public const string Session = "53616c7465645f5f5ab8a516ba4ff92b9468d3a1996d234f7c947cb84057888d7959f428c2263e2a972941df1078afaa";

        static void Main()
        {
            Console.WriteLine("Advent of Code");

            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(IDay).IsAssignableFrom(p));

            foreach (var type in types)
            {
                if (type == typeof(BaseDay))
                {
                    continue;
                }

                try
                {
                    var ctor = type.GetConstructor(Type.EmptyTypes);
                    if (ctor == null)
                    {
                        continue;
                    }
                    object instance = ctor.Invoke(new object[] { });
                    var day = instance as IDay;

                    var answer1 = day.ExecuteOne();
                    Console.WriteLine($"Answer 1 for {type}: {answer1}");

                    var answer2 = day.ExecuteTwo();
                    Console.WriteLine($"Answer 2 for {type}: {answer2}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex}");
                }
            }

            Console.WriteLine("Happy Christmas!");
        }
    }
}
