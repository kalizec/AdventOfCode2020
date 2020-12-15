using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day11 : BaseDay
    {
        public Dictionary<int, GridPosition[,]> WaitingArea { get; }

        public int XLength { get; }

        public int YLength { get; }

        public Day11() : base(2020, 11)
        {
            this.WaitingArea = new Dictionary<int, GridPosition[,]>();
            var round = this.GetWaitingArea();
            this.WaitingArea.Add(0, round);
            this.XLength = round.GetLength(0);
            this.YLength = round.GetLength(1);
        }

        public override long? ExecuteOne()
        {
            bool hasChanged;
            do
            {
                hasChanged = this.CalculateRound();
            }
            while (hasChanged);

            var lastRound = this.WaitingArea.Keys.LastOrDefault();
            var lastConfiguration = this.WaitingArea[lastRound];
            var occupiedSeats = 0;
            for (var y = 0; y < this.YLength; y++)
            {
                for (var x = 0; x < this.XLength; x++)
                {
                    var position = lastConfiguration[x, y];
                    if (position.HasSeat && position.IsOccupied)
                    {
                        occupiedSeats++;
                    }
                }
            }

            return occupiedSeats;
        }

        public override long? ExecuteTwo()
        {
            this.WaitingArea.Clear();
            var round = this.GetWaitingArea();
            this.WaitingArea.Add(0, round);

            bool hasChanged;
            do
            {
                hasChanged = this.CalculateRoundTwo();
            }
            while (hasChanged);

            var lastRound = this.WaitingArea.Keys.LastOrDefault();
            var lastConfiguration = this.WaitingArea[lastRound];
            var occupiedSeats = 0;
            for (var y = 0; y < this.YLength; y++)
            {
                for (var x = 0; x < this.XLength; x++)
                {
                    var position = lastConfiguration[x, y];
                    if (position.HasSeat && position.IsOccupied)
                    {
                        occupiedSeats++;
                    }
                }
            }

            return occupiedSeats;
        }

        private GridPosition[,] GetWaitingArea()
        {
            var x = 0;
            var y = 0;
            var lines = this.Input.Trim().Split('\n');
            var xLength = lines.FirstOrDefault().Length;
            var yLength = lines.Length;
            var result = new GridPosition[xLength, yLength];
            foreach (var line in lines)
            {
                foreach (var character in line)
                {
                    result[x, y] = new GridPosition(character, x, y);
                    x++;
                }
                x = 0;
                y++;
            }
            return result;
        }

        private bool CalculateRound()
        {
            var noChanges = true;
            var newRound = this.WaitingArea.Keys.LastOrDefault() + 1;
            var previousRound = this.WaitingArea[this.WaitingArea.Keys.LastOrDefault()];
            var result = new GridPosition[this.XLength, this.YLength];

            for (var y = 0; y < this.YLength; y++)
            {
                for (var x = 0; x < this.XLength; x++)
                {
                    var oldPosition = previousRound[x, y];
                    var newPosition = oldPosition.CalculateRound(previousRound);
                    if (noChanges && newPosition != oldPosition)
                    {
                        noChanges = false;
                    }
                    result[x, y] = newPosition;
                }
            }

            this.WaitingArea.Add(newRound, result);
            return !noChanges;
        }

        private bool CalculateRoundTwo()
        {
            var noChanges = true;
            var newRound = this.WaitingArea.Keys.LastOrDefault() + 1;
            var previousRound = this.WaitingArea[this.WaitingArea.Keys.LastOrDefault()];
            var result = new GridPosition[this.XLength, this.YLength];

            for (var y = 0; y < this.YLength; y++)
            {
                for (var x = 0; x < this.XLength; x++)
                {
                    var oldPosition = previousRound[x, y];
                    var newPosition = oldPosition.CalculateRoundTwo(previousRound);
                    if (noChanges && newPosition != oldPosition)
                    {
                        noChanges = false;
                    }
                    result[x, y] = newPosition;
                }
            }

            this.WaitingArea.Add(newRound, result);
            return !noChanges;
        }
    }

    public class GridPosition
    {
        public bool HasSeat { get; }
        public bool IsOccupied { get; }
        public int X { get; }
        public int Y { get; }

        public GridPosition(char c, int x, int y)
         {
            this.HasSeat = c == 'L' || c == '#';
            this.IsOccupied = c == '#';
            this.X = x;
            this.Y = y;
        }

        public List<GridPosition> GetAdjacentPositions(GridPosition[,] seatingArea)
        {
            var xMin = this.X == 0 ? 0 : this.X - 1;
            var yMin = this.Y == 0 ? 0 : this.Y - 1;
            var xMax = this.X == seatingArea.GetLength(0) - 1 ? this.X : this.X + 1;
            var yMax = this.Y == seatingArea.GetLength(1) - 1 ? this.Y : this.Y + 1;

            var result = new List<GridPosition>();
            for (var x = xMin; x <= xMax; x++)
            {
                for (var y = yMin; y <= yMax; y++)
                {
                    if (x == this.X && y == this.Y)
                    {
                        // Skip
                    }
                    else
                    {
                        result.Add(seatingArea[x, y]);
                    }
                }
            }
            return result;
        }

        private List<GridPosition> GetDirectionalPositions(GridPosition[,] seatingArea)
        {
            var result = new List<GridPosition>();

            var n  = this.GetPosition(seatingArea,  0, -1);
            var s  = this.GetPosition(seatingArea,  0,  1);
            var w  = this.GetPosition(seatingArea, -1,  0);
            var e  = this.GetPosition(seatingArea,  1,  0);
            var nw = this.GetPosition(seatingArea, -1, -1);
            var ne = this.GetPosition(seatingArea,  1, -1);
            var sw = this.GetPosition(seatingArea, -1,  1);
            var se = this.GetPosition(seatingArea,  1,  1);

            if (n != null) { result.Add(n); }
            if (s != null) { result.Add(s); }
            if (w != null) { result.Add(w); }
            if (e != null) { result.Add(e); }
            if (nw != null) { result.Add(nw); }
            if (ne != null) { result.Add(ne); }
            if (sw != null) { result.Add(sw); }
            if (se != null) { result.Add(se); }

            return result;
        }

        private GridPosition GetPosition(GridPosition[,] seatingArea, int vX, int vY)
        {
            var x = this.X + vX;
            var y = this.Y + vY;

            while (x >= 0
                && y >= 0
                && x < seatingArea.GetLength(0)
                && y < seatingArea.GetLength(1))
            {
                var position = seatingArea[x, y];
                if (position.HasSeat)
                {
                    return position;
                }
                x += vX;
                y += vY;
            }
            return null;
        }

        public GridPosition CalculateRound(GridPosition[,] seatingArea)
        {
            if (!this.HasSeat)
            {
                return this;
            }

            var adjacentPositions = this.GetAdjacentPositions(seatingArea);
            var adjacentSeats = adjacentPositions.Where(position => position.HasSeat);
            var adjacentOccupiedSeats = adjacentSeats.Where(seat => seat.IsOccupied);
            var occupiedAdjacentSeats = adjacentOccupiedSeats.Count();

            if (!this.IsOccupied && occupiedAdjacentSeats == 0)
            {
                return new GridPosition('#', this.X, this.Y);
            }
            if (this.IsOccupied && occupiedAdjacentSeats > 3)
            {
                return new GridPosition('L', this.X, this.Y);
            }
            return this;
        }

        public GridPosition CalculateRoundTwo(GridPosition[,] seatingArea)
        {
            if (!this.HasSeat)
            {
                return this;
            }

            var directionalPositions = this.GetDirectionalPositions(seatingArea);
            var occupiedSeats = directionalPositions.Count(seat => seat.IsOccupied);

            if (!this.IsOccupied && occupiedSeats == 0)
            {
                return new GridPosition('#', this.X, this.Y);
            }
            if (this.IsOccupied && occupiedSeats > 4)
            {
                return new GridPosition('L', this.X, this.Y);
            }
            return this;
        }
    }
}
