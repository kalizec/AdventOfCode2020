using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day7 : BaseDay
    {
        public static List<Bag> Bags { get; set; }

        public Day7() : base(2020, 7)
        {
            Day7.Bags = new List<Bag>();

            var lines = this.Input.Trim().Split("\n");
            foreach (var line in lines)
            {
                var bag = new Bag(line);
                Day7.Bags.Add(bag);
            }

            foreach (var bag in Day7.Bags)
            {
                bag.CalculateChildren();
            }

            foreach (var bag in Day7.Bags)
            {
                bag.CalculateParents();
            }
        }

        public override long? ExecuteOne()
        {
            var shinyGold = Day7.Bags.FirstOrDefault(bag => bag.Colour == "shiny gold");
            return shinyGold.GetAncestors().Count;
        }

        public override long? ExecuteTwo()
        {
            var shinyGold = Day7.Bags.FirstOrDefault(bag => bag.Colour == "shiny gold");
            return shinyGold.GetDescendants();
        }

        [System.Diagnostics.DebuggerDisplay("{Colour}")]
        public class Bag
        {
            public string Line { get; }

            public string Colour { get; }

            public Dictionary<Bag, int> Children { get; }

            public Dictionary<Bag, int> Parents { get; }

            public Bag (string line)
            {
                this.Line = line;
                var parts = line.Split(" bags contain ");
                this.Colour = parts[0].Replace(" bags", string.Empty);
                this.Children = new Dictionary<Bag, int>();
                this.Parents = new Dictionary<Bag, int>();
            }

            public void CalculateChildren()
            {
                var parts = Line.Split(" bags contain ");
                var rules = parts[1]
                    .Trim()
                    .Split(",")
                    .Select(part => part
                        .Trim()
                        .Replace(".", string.Empty)
                        .Replace(" bags", string.Empty)
                        .Replace(" bag", string.Empty));

                foreach (var rule in rules)
                {
                    if (rule == "no other")
                    {
                        continue;
                    }

                    var numberString = rule.Substring(0, 1);
                    if (!int.TryParse(numberString, out var number))
                    {
                        throw new InvalidProgramException("No valid number");
                    }
                    var colourString = rule.Substring(2);
                    var otherBag = Day7.Bags.FirstOrDefault(bag => bag.Colour == colourString);
                    if (otherBag == null)
                    {
                        throw new InvalidProgramException("No otherBag");
                    }
                    this.Children.Add(otherBag, number);
                }
            }

            public void CalculateParents()
            {
                var otherBags = Day7.Bags.Where(bag => bag.Colour != this.Colour);
                foreach (var otherBag in otherBags)
                {
                    foreach (var rule in otherBag.Children)
                    {
                        if (rule.Key == this)
                        {
                            this.Parents.Add(otherBag, rule.Value);
                        }
                    }
                }
            }

            public HashSet<Bag> GetAncestors()
            {
                var ancestors = new HashSet<Bag>();
                foreach (var parent in this.Parents)
                {
                    ancestors.Add(parent.Key);

                    var grandParents = parent.Key.GetAncestors();
                    ancestors.UnionWith(grandParents);
                }
                return ancestors;
            }

            public int GetDescendants()
            {
                var count = 0;
                foreach (var child in this.Children)
                {
                    count += child.Value;
                    count += child.Value * child.Key.GetDescendants();
                }
                return count;
            }
        }
    }
}
