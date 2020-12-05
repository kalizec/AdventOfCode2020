using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day1 : BaseDay
    {
        public HashSet<int> Numbers { get; }

        public Day1() : base(2020, 1)
        {
            this.Numbers = this.Input.Trim().Split("\n").Select(x => int.Parse(x)).ToHashSet();
        }

        public override long? ExecuteOne()
        {
            foreach (var number in this.Numbers)
            {
                var otherNumber = 2020 - number;
                if (this.Numbers.Contains(otherNumber))
                {
                    return number * otherNumber;
                }
            }
            return null;
        }

        public override long? ExecuteTwo()
        {
            foreach (var first in this.Numbers)
            {
                var secondAndThird = 2020 - first;
                foreach (var second in this.Numbers)
                {
                    if (first == second)
                    {
                        continue;
                    }

                    var third = 2020 - first - second;
                    if (this.Numbers.Contains(third))
                    {
                        return first * second * third;
                    }
                }
            }
            return null;
        }
    }
}
