using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day13 : BaseDay
    {
        public long Now { get; }

        public List<Bus> Busses { get; }

        public Dictionary<Bus, int> Criteria { get; }

        public Day13() : base(2020, 13)
        {
            var lines = this.Input.Trim().Split("\n");

            this.Now = this.GetNow(lines.FirstOrDefault());
            this.Busses = this.GetBusses(lines.LastOrDefault());
            this.Criteria = this.GetCriteria(lines.LastOrDefault());
        }

        private long GetNow(string firstLine)
        {
            if (!long.TryParse(firstLine, out var now))
            {
                throw new NotImplementedException();
            }
            return now;
        }

        private List<Bus> GetBusses(string secondLine)
        {
            return secondLine
                .Split(",")
                .Where(token => token != "x")
                .Select(token => new Bus(token))
                .ToList();
        }

        private Dictionary<Bus, int> GetCriteria(string secondLine)
        {
            var offset = -1;
            var challenge = secondLine.Split(",").ToList();
            var result = new Dictionary<Bus, int>();
            foreach (var step in challenge)
            {
                offset++;
                if (step == "x")
                {
                    continue;
                }
                if (!int.TryParse(step, out var timestamp))
                {
                    throw new NotImplementedException();
                }
                result.Add(new Bus(timestamp), offset);
            }
            return result;
        }

        public override long? ExecuteOne()
        {
            var departures = new Dictionary<long, Bus>();
            foreach (var bus in this.Busses)
            {
                var nextDeparture = bus.NextDeparture(this.Now);
                departures.Add(nextDeparture, bus);
            }
            var firstDepartureTime = departures.Keys.OrderBy(key => key).First();
            var firstDepartureBus = departures[firstDepartureTime];
            return (firstDepartureTime - this.Now) * firstDepartureBus.Id;
        }

        public override long? ExecuteTwo()
        {
            var offset = 0L;
            var stepSize = 1L;
            for (var numberOfCriteria = 1; numberOfCriteria <= 9; numberOfCriteria++)
            {
                var criteria = this.Criteria.ToList().Take(numberOfCriteria);
                var found = false;
                var time = offset;
                while (!found)
                {
                    if (this.MeetsCriteria(time, new Dictionary<Bus, int>(criteria)))
                    {
                        found = true;
                        offset = time;
                        var product = criteria
                            .Select(kvp => kvp.Key.Id)
                            .Aggregate(1L, (acc, val) => acc * val);
                        stepSize = product;
                    }
                    else
                    {
                        time += stepSize;
                    }
                }
                offset = time;
                //Console.WriteLine($"Time = {time} meets {i} criteria.");
            }
            return offset;
        }

        private bool MeetsCriteria(long time, Dictionary<Bus, int> criteria)
        {
            foreach (var kvpStep in criteria)
            {
                var offset = kvpStep.Value;
                var bus = kvpStep.Key;
                if (!bus.DepartsOn(time + offset))
                {
                    return false;
                }
            }
            return true;
        }
    }

    public class Bus
    {
        public long Id { get; }

        public Bus (string token)
        {
            if (!long.TryParse(token, out var id))
            {
                throw new NotImplementedException();
            }
            this.Id = id;
        }

        public Bus (long id)
        {
            this.Id = id;
        }

        public long NextDeparture (long now)
        {
            var time = 0L;
            while (time < now)
            {
                time += this.Id;
            }
            return time;
        }

        public bool DepartsOn(long time)
        {
            return time % this.Id == 0;
        }
    }
}
