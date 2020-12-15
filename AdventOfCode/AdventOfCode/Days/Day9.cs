using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day9 : BaseDay
    {
        public long[] Stream { get; }

        public Day9() : base(2020, 9)
        {
            var stream = new List<long>();
            var lines = this.Input.Trim().Split("\n");
            foreach (var line in lines)
            {
                if (!long.TryParse(line, out var number))
                {
                    throw new NotImplementedException();
                }
                stream.Add(number);
            }
            this.Stream = stream.ToArray();
        }

        public override long? ExecuteOne()
        {
            var preambleLength = 25;
            var streamLength = this.Stream.Length;

            for (var i = preambleLength; i < streamLength; i++)
            {
                var preambleStart = i - preambleLength;
                var previousNumbers = this.Stream.SubArray(preambleStart, preambleLength);
                var number = this.Stream[i];
                var isValid = this.IsValid(number, previousNumbers);
                if (!isValid)
                {
                    return number;
                }
            }
            
            throw new NotImplementedException();
        }

        public override long? ExecuteTwo()
        {
            var targetNumber = this.ExecuteOne();
            var streamLength = this.Stream.Length;

            for (var position = 0; position < streamLength; position++)
            {
                var maxLength = streamLength - position;
                for (var length = 2; length < maxLength; length++)
                {
                    var subArray = this.Stream.SubArray(position, length);
                    var sum = subArray.Sum();
                    if (sum == targetNumber)
                    {
                        Array.Sort(subArray);
                        return subArray[0] + subArray[^1];
                    }
                    else if (sum > targetNumber)
                    {
                        break;
                    }
                    else
                    {
                        // Do nothing, we'll increment and try again
                    }
                }
            }

            throw new NotImplementedException();
        }

        private bool IsValid(long number, long[] preamble)
        {
            foreach (var firstNumber in preamble)
            {
                var otherNumbers = preamble.Where(number => number != firstNumber);
                foreach (var secondNumber in otherNumbers)
                {
                    var validNumber = firstNumber + secondNumber;
                    if (number == validNumber)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }

    public static class ArrayExtensions
    {
        public static T[] SubArray<T>(this T[] array, int offset, int length)
        {
            var result = new T[length];
            Array.Copy(array, offset, result, 0, length);
            return result;
        }
    }
}
