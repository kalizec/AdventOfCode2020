using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day8 : BaseDay
    {
        public Instruction[] Program { get; }

        public Day8() : base(2020, 8)
        {
            this.Program = this.Input
                .Trim()
                .Split("\n")
                .Select(line => new Instruction(line))
                .ToArray();
        }

        public override long? ExecuteOne()
        {
            _ = this.Run(this.Program, out var exitCode);
            return exitCode;
        }

        public override long? ExecuteTwo()
        {
            for (var position = 0; position < this.Program.Length; position++)
            {
                var newProgram = this.Program.Clone() as Instruction[];
                var instruction = newProgram[position];
                var instructionType = instruction.InstructionType;
                switch (instructionType)
                {
                    case InstructionType.Acc:
                        continue;
                    case InstructionType.Nop:
                        newProgram[position] = new Instruction(InstructionType.Jmp, instruction.Argument);
                        break;
                    case InstructionType.Jmp:
                        newProgram[position] = new Instruction(InstructionType.Nop, instruction.Argument);
                        break;
                    default:
                        throw new NotImplementedException();

                }
                var exitCode = this.Run(newProgram, out var result);
                if (exitCode == 0)
                {
                    return result;
                }
            }
            return 0;
        }

        private long? Run(Instruction[] instructions, out int result)
        {
            var log = new HashSet<Instruction>();
            var counter = 0;
            var accumulator = 0;
            while (true)
            {
                var instruction = instructions[counter];

                if (log.Contains(instruction))
                {
                    result = accumulator;
                    return 1;
                }

                instruction.Execute(ref counter, ref accumulator);

                if (counter == instructions.Length)
                {
                    result = accumulator;
                    return 0;
                }

                log.Add(instruction);
            }
        }
    }

    public class Instruction
    {
        public int Argument { get; }
        public InstructionType InstructionType { get; set; }

        public Instruction(string line)
        {
            var parts = line.Split(" ");
            this.InstructionType = parts[0].Trim() switch
            {
                "nop" => InstructionType.Nop,
                "acc" => InstructionType.Acc,
                "jmp" => InstructionType.Jmp,
                _ => throw new NotImplementedException(),
            };
            if (!int.TryParse(parts[1].Trim(), out var arg))
            {
                throw new InvalidOperationException();
            }
            this.Argument = arg;
        }

        public Instruction(InstructionType instructionType, int argument)
        {
            this.InstructionType = instructionType;
            this.Argument = argument;
        }

        public void Execute(ref int position, ref int accumulator)
        {
            var nextInstruction = position + 1;
            if (this.InstructionType == InstructionType.Acc)
            {
                accumulator += this.Argument;
            }
            else if(this.InstructionType == InstructionType.Jmp)
            {
                nextInstruction = position + this.Argument;
            }
            position = nextInstruction;
        }
    }

    public enum InstructionType
    {
        Nop,
        Acc,
        Jmp
    }
}
