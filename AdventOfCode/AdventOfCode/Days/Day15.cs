using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day15 : BaseDay
    {
        public List<int> Numbers { get; }

        public Day15() : base(2020, 15)
        {
            this.Numbers = this.Input.Trim().Split(",").Select(token => int.Parse(token)).ToList();
        }

        /// <summary>
        /// The following implementation was deemed too slow.
        /// </summary>
        /// <returns></returns>
        public long? ExecuteZero()
        {
            while (this.Numbers.Count < 10)
            {
                int newNumber;
                var turn = this.Numbers.Count;
                var last = this.Numbers.Last();
                var others = this.Numbers.SkipLast(1).ToList();
                if (!others.Contains(last))
                {
                    newNumber = 0;
                }
                else
                {
                    var index = others.LastIndexOf(last);
                    newNumber = turn - index - 1;
                }
                this.Numbers.Add(newNumber);
            }
            return this.Numbers.LastOrDefault();
        }

        public override long? ExecuteOne()
        {
            var others = new Dictionary<long, int>();
            var otherNumbers = this.Numbers.SkipLast(1).ToArray();
            var position = 1;
            foreach (var otherNumber in otherNumbers)
            {
                others[otherNumber] = position;
                position++;
            }
            var previous = (long)this.Numbers.Last();
            var start = this.Numbers.Count() + 1;
            var end = 2020;

            return this.Execute(others, previous, start, end);
        }

        public override long? ExecuteTwo()
        {
            var others = new Dictionary<long, int>();
            var otherNumbers = this.Numbers.SkipLast(1).ToArray();
            var position = 1;
            foreach (var otherNumber in otherNumbers)
            {
                others[otherNumber] = position;
                position++;
            }
            var previous = (long)this.Numbers.Last();
            var start = this.Numbers.Count() + 1;
            var end = 30000000;

            return this.Execute(others, previous, start, end);
        }

        private long? Execute(Dictionary<long, int> others, long previous, int turnStart, int turnEnd)
        {
            for (var turn = turnStart; turn <= turnEnd; turn++)
            {
                var newNumber = 0;
                if (others.ContainsKey(previous))
                {
                    newNumber = (turn - 1) - others[previous];
                }
                others[previous] = (turn - 1);
                previous = newNumber;
            }
            return previous;
        }
    }
}
