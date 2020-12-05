using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day5 : BaseDay
    {
        public List<Seat> Seats { get; }

        public Day5() : base(2020, 5)
        {
            this.Seats = this.GetSeats();
        }

        public override long? ExecuteOne()
        {
            return this.Seats
                .OrderByDescending(seat => seat.ID)
                .FirstOrDefault()
                .ID;
        }

        public override long? ExecuteTwo()
        {
            var highestId = this.ExecuteOne();
            var lowestId = this.Seats
                .OrderByDescending(seat => seat.ID)
                .LastOrDefault()
                .ID;
            var seatIds = this.Seats.Select(seat => seat.ID);

            for (var i = lowestId; i <= highestId; i++)
            {
                if (!seatIds.Contains(i))
                {
                    return i;
                }
            }
            return null;
        }

        private List<Seat> GetSeats()
        {
            var result = new List<Seat>();
            var lines = this.Input.Trim().Split("\n");
            foreach (var line in lines)
            {
                result.Add(new Seat(line));
            }
            return result;
        }
    }

    public class Seat
    {
        public string Line { get; }

        public int Row { get; }

        public int Column { get; }

        public int ID => 8 * Row + Column;

        public Seat (string line)
        {
            this.Line = line;
            var row = line.Substring(0, 7);
            var col = line.Substring(7, 3);

            this.Row
                = (row[0] == 'B' ? 64 : 0)
                + (row[1] == 'B' ? 32 : 0)
                + (row[2] == 'B' ? 16 : 0)
                + (row[3] == 'B' ? 8 : 0)
                + (row[4] == 'B' ? 4 : 0)
                + (row[5] == 'B' ? 2 : 0)
                + (row[6] == 'B' ? 1 : 0);

            this.Column
                = (col[0] == 'R' ? 4 : 0)
                + (col[1] == 'R' ? 2 : 0)
                + (col[2] == 'R' ? 1 : 0);
        }
    }
}
