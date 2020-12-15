using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day14 : BaseDay
    {
        public string Mask { get; private set; }

        public Dictionary<long, long> Mem { get; }

        public Day14() : base(2020, 14)
        {
            this.Mem = new Dictionary<long, long>();
        }

        public override long? ExecuteOne()
        {
            var lines = this.Input.Trim().Split("\n");
            foreach (var line in lines)
            {
                var first = line.Split('=').FirstOrDefault().Trim();
                var last = line.Split('=').LastOrDefault().Trim();
                if (first.StartsWith("mask"))
                {
                    this.UpdateMask(last);
                    continue;
                }
                else if (first.StartsWith("mem"))
                {
                    var addressString = first.Replace("mem[", string.Empty).Replace("]", string.Empty);
                    if (!int.TryParse(addressString, out var address))
                    {
                        throw new NotImplementedException();
                    }
                    if (!long.TryParse(last, out var preMaskBaseTen))
                    {
                        throw new NotImplementedException();
                    }

                    var preMaskBaseTwo = this.GetBaseTwo(preMaskBaseTen);
                    var postMaskBaseTwo = this.ApplyMaskOne(preMaskBaseTwo);
                    var result = this.GetBaseTen(postMaskBaseTwo);
                    if (this.Mem.ContainsKey(address))
                    {
                        this.Mem[address] = result;
                    }
                    else
                    {
                        this.Mem.Add(address, result);
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            return this.Mem.Values.Aggregate(0L, (acc, val) => acc + val);
        }

        private void UpdateMask(string newMask)
        {
            this.Mask = newMask;
        }

        private string ApplyMaskOne(string input)
        {
            var result = string.Empty;
            for (var i = 0; i < input.Length; i++)
            {
                var inputBit = input[i];
                var maskBit = this.Mask[i];
                var outputBit = maskBit switch
                {
                    'X' => inputBit,
                    '0' => '0',
                    '1' => '1',
                    _ => throw new NotImplementedException(),
                };
                result += outputBit;
            }
            return result;
        }

        private long GetBaseTen(string input)
        {
            return Convert.ToInt64(input, 2);
        }

        private string GetBaseTwo(long input)
        {
            return this.GetBinary(input, this.Mask.Length);
        }

        private string GetBinary(long input, int length)
        {
            var result = Convert.ToString(input, 2);

            // Pad with zeroes.
            for (var i = result.Length; i < length; i++)
            {
                result = "0" + result;
            }

            return result;
        }

        public override long? ExecuteTwo()
        {
            this.Mem.Clear();

            var lines = this.Input.Trim().Split("\n");
            foreach (var line in lines)
            {
                var first = line.Split('=').FirstOrDefault().Trim();
                var last = line.Split('=').LastOrDefault().Trim();
                if (first.StartsWith("mask"))
                {
                    this.UpdateMask(last);
                    continue;
                }
                else if (first.StartsWith("mem"))
                {
                    if (!long.TryParse(last, out var value))
                    {
                        throw new NotImplementedException("Cannot handle unparseable value");
                    }

                    var addressString = first.Replace("mem[", string.Empty).Replace("]", string.Empty);
                    if (!long.TryParse(addressString, out var rootAddress))
                    {
                        throw new NotImplementedException("Cannot handle unparseable addressString");
                    }

                    var addresses = this.CalculateAddresses(rootAddress);
                    foreach (var address in addresses)
                    {
                        if (this.Mem.ContainsKey(address))
                        {
                            this.Mem[address] = value;
                        }
                        else
                        {
                            this.Mem.Add(address, value);
                        }
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            return this.Mem.Values.Aggregate(0L, (acc, val) => acc + val);
        }

        private List<long> CalculateAddresses (long decimalInput)
        {
            var result = new List<long>();
            var binaryInput = this.GetBaseTwo(decimalInput);
            var maskedInput = this.ApplyMaskTwo(binaryInput, this.Mask);
            var addresses = this.GetPermutations(maskedInput);
            
            foreach (var addressString in addresses)
            {
                var address = this.GetBaseTen(addressString);
                result.Add(address);
            }
            
            return result;
        }

        private List<string> GetPermutations(string valueAsBaseTwo)
        {
            var result = new List<string>
            {
                valueAsBaseTwo
            };

            for (var i = 0; i < valueAsBaseTwo.Length; i++)
            {
                result = this.GetPermutations(result, i);
            }

            return result;
        }

        private List<string> GetPermutations(List<string> permutations, int position)
        {
            if (this.Mask[position] == 'X')
            {
                var result = new List<string>();
                foreach (var permutation in permutations)
                {
                    var zero = permutation.ToArray();
                    var one = permutation.ToArray();

                    zero[position] = '0';
                    one[position] = '1';

                    result.Add(new string(zero));
                    result.Add(new string(one));
                }
                return result;
            }
            return permutations;
        }

        private string ApplyMaskTwo(string valueAsBaseTwo, string mask)
        {
            var result = string.Empty;
            for (var i = 0; i < valueAsBaseTwo.Length; i++)
            {
                var inputBit = valueAsBaseTwo[i];
                var maskBit = mask[i];
                var outputBit = maskBit switch
                {
                    '0' => inputBit,
                    '1' => '1',
                    'X' => 'X',
                    _ => throw new NotImplementedException(),
                };
                result += outputBit;
            }
            return result;
        }
    }
}
