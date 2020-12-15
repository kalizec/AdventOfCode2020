using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day10 : BaseDay
    {
        public List<int> Adapters { get; }

        public Day10() : base(2020, 10)
        {
            this.Adapters = this.GetAdapters();
        }

        public override long? ExecuteOne()
        {
            var chain = this.GetChain(this.GetAdapters());
            var differences = this.GetDifferences(chain.ToArray());
            var ones = differences.Count(differences => differences == 1);
            var threes = differences.Count(differences => differences == 3);
            return ones * threes;
        }

        public override long? ExecuteTwo()
        {
            var chain = this.GetChain(this.GetAdapters());
            var differences = this.GetDifferences(chain.ToArray());
            return this.CountPermutations(differences);
        }

        private List<int> GetAdapters()
        {
            return this.Input.Trim().Split("\n")
                .Select(line => int.Parse(line))
                .ToList();
        }

        private List<int> GetChain(List<int> adapters)
        {
            var outlet = 0;
            var chain = new List<int>() { outlet };
            var available = adapters.OrderBy(value => value);
            chain.AddRange(available);
            var last = chain.LastOrDefault();
            var device = last + 3;
            chain.Add(device);
            return chain;
        }

        private List<int> GetDifferences(int[] adapters)
        {
            var differences = new List<int>();
            for (var i = 1; i < adapters.Length; i++)
            {
                var previous = adapters[i - 1];
                var current = adapters[i];
                var difference = current - previous;
                differences.Add(difference);
            }
            return differences;
        }

        private long? CountPermutations(List<int> differences)
        {
            var diffChain = string.Join(string.Empty, differences);

            var tokens = diffChain.Split("3")
                .Where(token => !string.IsNullOrWhiteSpace(token))
                .Where(token => token != "1")
                .ToList();

            var twoChain = tokens.Count(token => token == "11");
            var threeChain = tokens.Count(token => token == "111");
            var fourChain = tokens.Count(token => token == "1111");

            return LongPow(2, twoChain) * LongPow(4, threeChain) * LongPow(7, fourChain);
        }

        private long LongPow(long x, int power)
        {
            long result = 1;
            while (power != 0)
            {
                if ((power & 1) == 1)
                {
                    result *= x;
                }
                x *= x;
                power >>= 1;
            }
            return result;
        }
    }
}
