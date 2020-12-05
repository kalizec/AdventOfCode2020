using System;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day3 : BaseDay
    {
        public Map Map;

        public Day3() : base(2020, 3)
        {
            this.Map = new Map(this.Input);
        }

        public override long? ExecuteOne()
        {
            return this.Map.GetTreeLine(3,1);
        }

        public override long? ExecuteTwo()
        {
            var result = this.Map.GetTreeLine(1, 1)
                * this.Map.GetTreeLine(3, 1)
                * this.Map.GetTreeLine(5, 1)
                * this.Map.GetTreeLine(7, 1)
                * this.Map.GetTreeLine(1, 2);
            return result;
        }
    }

    public class Map
    {
        public readonly int XLength, YLength;
        public readonly bool[,] map;
         
        public Map (string input)
        {
            var lines = input.Trim().Split("\n").ToList();
            this.XLength = lines.First().Length;
            this.YLength = lines.Count();
            this.map = new bool[this.XLength, this.YLength];

            var y = 0;
            foreach (var line in lines)
            {
                var x = 0;
                foreach (var character in line)
                {
                    this.map[x, y] = character == '#';
                    x++;
                }
                y++;
            }
        }

        public bool HasTree(int x, int y)
        {
            var moduloX = x % this.XLength;
            if (y >= this.YLength)
            {
                throw new InvalidProgramException("Y should never be larger or equal to the size of the map");
            }
            return this.map[moduloX, y];
        }

        public long? GetTreeLine(int vx, int vy)
        {
            var treeCount = 0;
            for (int x = 0, y = 0; y < this.YLength; x += vx, y += vy)
            {
                if (this.HasTree(x, y))
                {
                    treeCount++;
                }
            }
            return treeCount;
        }
    }
}
