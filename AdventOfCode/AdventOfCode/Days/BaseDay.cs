using AdventOfCode.Input;
using System;

namespace AdventOfCode.Days
{
    public class BaseDay : IDay
    {
        public string Input { get; private set; }

        public BaseDay(int year, int day)
        {
            this.Input = InputHelper.Get(year, day).Result;
        }

        public virtual long? ExecuteOne()
        {
            throw new NotImplementedException();
        }

        public virtual long? ExecuteTwo()
        {
            throw new NotImplementedException();
        }
    }
}
