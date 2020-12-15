using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day12 : BaseDay
    {
        public List<NavigationInstruction> NavigationInstructions { get; }

        public Day12() : base(2020, 12)
        {
            this.NavigationInstructions = this.Input
                .Trim()
                .Split("\n")
                .Select(line => new NavigationInstruction(line))
                .ToList();
        }

        public override long? ExecuteOne()
        {
            var direction = Direction.East;
            var position = (0, 0);

            //Console.WriteLine($"Starting position ({position.Item1},{position.Item2}) facing {direction}");

            foreach (var instruction in this.NavigationInstructions)
            {
                //Console.WriteLine($"Instruction {instruction.Direction} {instruction.Value}");
                (position, direction) = instruction.ApplyShip(instruction, position, direction);
                //Console.WriteLine($"Position ({position.Item1},{position.Item2}) facing {direction}");
            }

            return Math.Abs(position.Item1) + Math.Abs(position.Item2);
        }

        public override long? ExecuteTwo()
        {
            var ship = (0, 0);
            var waypoint = (-1, 10);

            //Console.WriteLine($"Starting positions ship ({ship.Item1},{ship.Item2}), waypoint ({waypoint.Item1}, {waypoint.Item2})");

            foreach (var instruction in this.NavigationInstructions)
            {
                //Console.WriteLine($"Instruction {instruction.Direction} {instruction.Value}");
                (ship, waypoint) = instruction.ApplyWaypoint(instruction, ship, waypoint);
                //Console.WriteLine($"Positions ship ({ship.Item1},{ship.Item2}), waypoint ({waypoint.Item1}, {waypoint.Item2})");
            }

            return Math.Abs(ship.Item1) + Math.Abs(ship.Item2);
        }
    }

    public class NavigationInstruction
    {
        public Direction Direction { get; }
        public int Value { get; }

        public NavigationInstruction (string line)
        {
            var directionPart = line.Substring(0, 1);
            this.Direction = directionPart switch
            {
                "N" => Direction.North,
                "S" => Direction.South,
                "W" => Direction.West,
                "E" => Direction.East,
                "L" => Direction.Left,
                "R" => Direction.Right,
                "F" => Direction.Forward,
                _ => throw new NotImplementedException(),
            };

            var valuePart = line.Substring(1);
            if (!int.TryParse(valuePart, out var value))
            {
                throw new NotImplementedException();
            }
            this.Value = value;
        }

        public ((int, int), Direction) ApplyShip (NavigationInstruction instruction, (int, int) currentPosition, Direction currentDirection)
        {
            return instruction.Direction switch
            {
                Direction.North   => ((currentPosition.Item1, currentPosition.Item2 - instruction.Value), currentDirection),
                Direction.South   => ((currentPosition.Item1, currentPosition.Item2 + instruction.Value), currentDirection),
                Direction.West    => ((currentPosition.Item1 - instruction.Value, currentPosition.Item2), currentDirection),
                Direction.East    => ((currentPosition.Item1 + instruction.Value, currentPosition.Item2), currentDirection),
                Direction.Left    => ((currentPosition.Item1, currentPosition.Item2), this.ApplyShipLeft(currentDirection, instruction.Value)),
                Direction.Right   => ((currentPosition.Item1, currentPosition.Item2), this.ApplyShipRight(currentDirection, instruction.Value)),
                Direction.Forward => (this.ApplyShipForward(currentPosition, currentDirection, instruction.Value), currentDirection),
                _ => throw new NotImplementedException(),
            };
        }

        private Direction ApplyShipLeft(Direction currentDirection, int value)
        {
            if (value == 0)
            {
                return currentDirection;
            }

            var newDirection = currentDirection switch
            {
                Direction.North => Direction.West,
                Direction.East => Direction.North,
                Direction.South => Direction.East,
                Direction.West => Direction.South,
                _ => throw new NotImplementedException(),
            };

            if (value > 90)
            {
                return this.ApplyShipLeft(newDirection, value - 90);
            }
            return newDirection;
        }

        private Direction ApplyShipRight(Direction currentDirection, int value)
        {
            if (value == 0)
            {
                return currentDirection;
            }

            var newDirection = currentDirection switch
            {
                Direction.North => Direction.East,
                Direction.East  => Direction.South,
                Direction.South => Direction.West,
                Direction.West  => Direction.North,
                _ => throw new NotImplementedException(),
            };

            if (value > 90)
            {
                return this.ApplyShipRight(newDirection, value - 90);
            }
            return newDirection;
        }

        private (int, int) ApplyShipForward((int, int) currentPosition, Direction currentDirection, int value)
        {
            return currentDirection switch
            {
                Direction.North => (currentPosition.Item1, currentPosition.Item2 - value),
                Direction.South => (currentPosition.Item1, currentPosition.Item2 + value),
                Direction.West => (currentPosition.Item1 - value, currentPosition.Item2),
                Direction.East => (currentPosition.Item1 + value, currentPosition.Item2),
                _ => throw new NotImplementedException(),
            };
        }

        public ((int, int), (int, int)) ApplyWaypoint(NavigationInstruction instruction, (int, int) ship, (int, int) waypoint)
        {
            return instruction.Direction switch
            {
                Direction.North   => (ship, (waypoint.Item1, waypoint.Item2 - instruction.Value)),
                Direction.South   => (ship, (waypoint.Item1, waypoint.Item2 + instruction.Value)),
                Direction.West    => (ship, (waypoint.Item1 - instruction.Value, waypoint.Item2)),
                Direction.East    => (ship, (waypoint.Item1 + instruction.Value, waypoint.Item2)),
                Direction.Left    => (ship, this.ApplyWaypointLeft(ship, waypoint, instruction.Value)),
                Direction.Right   => (ship, this.ApplyWaypointRight(ship, waypoint, instruction.Value)),
                Direction.Forward => this.ApplyWaypointForward(ship, waypoint, instruction.Value),
                _ => throw new NotImplementedException(),
            };
        }

        private (int, int) ApplyWaypointLeft((int, int) ship, (int, int) waypoint, int value)
        {
            if (value == 0)
            {
                return waypoint;
            }

            var relative = (waypoint.Item1 - ship.Item1, waypoint.Item2 - ship.Item2);
            var rotated = (relative.Item2, -relative.Item1);
            var absolute = (ship.Item1 + rotated.Item1, ship.Item2 + rotated.Item2);

            if (value > 90)
            {
                return this.ApplyWaypointLeft(ship, absolute, value - 90);
            }
            return absolute;
        }

        private (int, int) ApplyWaypointRight((int, int) ship, (int, int) waypoint, int value)
        {
            if (value == 0)
            {
                return waypoint;
            }

            var relative = (waypoint.Item1 - ship.Item1, waypoint.Item2 - ship.Item2);
            var rotated = (-relative.Item2, relative.Item1);
            var absolute = (ship.Item1 + rotated.Item1, ship.Item2 + rotated.Item2);

            if (value > 90)
            {
                return this.ApplyWaypointRight(ship, absolute, value - 90);
            }
            return absolute;
        }

        private ((int, int), (int, int)) ApplyWaypointForward((int, int) ship, (int, int) waypoint, int value)
        {
            if (value == 0)
            {
                return (ship, waypoint);
            }

            var relative = (waypoint.Item1 - ship.Item1, waypoint.Item2 - ship.Item2);

            var newShip = (ship.Item1 + relative.Item1, ship.Item2 + relative.Item2);
            var newWaypoint = (waypoint.Item1 + relative.Item1, waypoint.Item2 + relative.Item2);

            if (value > 1)
            {
                return this.ApplyWaypointForward(newShip, newWaypoint, value - 1);
            }
            return (newShip, newWaypoint);
        }
    }

    public enum Direction
    {
        North,
        South,
        East,
        West,
        Left,
        Right,
        Forward
    }
}
