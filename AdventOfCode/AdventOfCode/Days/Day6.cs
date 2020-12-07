using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day6 : BaseDay
    {
        public List<HashSet<char>> GroupAnswers { get; }
        public List<List<HashSet<char>>> PersonAnswers { get; }

        public Day6() : base(2020, 6)
        {
            this.GroupAnswers = this.GetGroupAnswers();
            this.PersonAnswers = this.GetPersonAnswers();
        }

        public override long? ExecuteOne()
        {
            var count = 0;
            foreach (var group in this.GroupAnswers)
            {
                count += group.Count();
            }
            return count;
        }

        public override long? ExecuteTwo()
        {
            var totalCount = 0;
            foreach (var group in this.PersonAnswers)
            {
                var groupCount = 0;
                foreach (var letter in "abcdefghijklmnopqrstuvwxyz")
                {
                    if (group.All(person => person.Contains(letter)))
                    {
                        groupCount++;
                    }
                }
                totalCount += groupCount;
            }
            return totalCount;
        }

        private List<HashSet<char>> GetGroupAnswers()
        {
            var results = new List<HashSet<char>>();
            var groups = this.Input.Trim().Split("\n\n");
            foreach (var group in groups)
            {
                var groupAnswers = new HashSet<char>();
                var persons = group.Trim().Split("\n");
                foreach (var person in persons)
                {
                    foreach (var answer in person)
                    {
                        groupAnswers.Add(answer);
                    }
                }
                results.Add(groupAnswers);
            }
            return results;
        }

        private List<List<HashSet<char>>> GetPersonAnswers()
        {
            var results = new List<List<HashSet<char>>>();
            var groups = this.Input.Trim().Split("\n\n");
            foreach (var group in groups)
            {
                var groupAnswers = new List<HashSet<char>>();
                var persons = group.Trim().Split("\n");
                foreach (var person in persons)
                {
                    var personAnswers = new HashSet<char>();
                    foreach (var answer in person)
                    {
                        personAnswers.Add(answer);
                    }
                    groupAnswers.Add(personAnswers);
                }
                results.Add(groupAnswers);
            }
            return results;
        }
    }
}
