using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day2 : BaseDay
    {
        public List<string> Lines { get; }

        public Day2() : base(2020, 2)
        {
            this.Lines = this.Input.Trim().Split("\n").ToList();
        }

        public override long? ExecuteOne()
        {
            var valid = 0;
            foreach (var line in this.Lines)
            {
                var lineParts = line.Split(":");
                var rule = lineParts[0];
                var pass = lineParts[1];

                var ruleParts = rule.Split(" ");
                var minCount = int.Parse(ruleParts[0].Split("-")[0]);
                var maxCount = int.Parse(ruleParts[0].Split("-")[1]);
                var letter = ruleParts[1].ToArray()[0];

                var count = pass.Where(x => x == letter).Count();
                if (count >= minCount && count <= maxCount)
                {
                    valid++;
                }
            }
            return valid;
        }

        public override long? ExecuteTwo()
        {
            var valid = 0;
            foreach (var line in this.Lines)
            {
                var lineParts = line.Split(":");
                var rule = lineParts[0];
                var pass = lineParts[1].Trim();

                var ruleParts = rule.Split(" ");
                var firstPosition = int.Parse(ruleParts[0].Split("-")[0]) - 1;
                var secondPosition = int.Parse(ruleParts[0].Split("-")[1]) - 1;
                var letter = ruleParts[1].ToArray()[0];

                if (pass[firstPosition] == letter
                    && pass[secondPosition] != letter)
                {
                    valid++;
                }
                else if (pass[firstPosition] != letter
                    && pass[secondPosition] == letter)
                {
                    valid++;
                }
            }
            return valid;
        }
    }
}
