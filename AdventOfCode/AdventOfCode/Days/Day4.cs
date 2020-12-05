using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Days
{
    public class Day4 : BaseDay
    {
        public List<Dictionary<string, string>> Passports { get; }

        public Dictionary<string, string> RequiredFields = new Dictionary<string, string>()
        {
            { "byr", "Birth Year"},
            { "iyr", "Issue Year"},
            { "eyr", "Expiration Year"},
            { "hgt", "Height"},
            { "hcl", "Hair Color"},
            { "ecl", "Eye Color"},
            { "pid", "Passport ID"},
            //{ "cid", "Country ID"},
        };

        public List<string> ValidEyeColours = new List<string>()
        {
            "amb",
            "blu",
            "brn",
            "gry",
            "grn",
            "hzl",
            "oth",
        };

        public Day4() : base(2020, 4)
        {
            this.Passports = this.GetPassports();
        }

        public override long? ExecuteOne()
        {
            return this.Passports.Where(passport => this.IsValidPassport(passport, false)).Count();
        }

        public override long? ExecuteTwo()
        {
            return this.Passports.Where(passport => this.IsValidPassport(passport, true)).Count();
        }

        private List<Dictionary<string, string>> GetPassports()
        {
            var passportStrings = this.Input.Split("\n\n")
                .Select(p => p.Trim().Replace("\n", " "))
                .ToList();
            var passports = new List<Dictionary<string, string>>();

            foreach (var passportString in passportStrings)
            {
                var fields = passportString.Split(" ");
                var passport = new Dictionary<string, string>();
                foreach (var field in fields)
                {
                    var fieldKvp = field.Split(":");
                    passport.Add(fieldKvp[0], fieldKvp[1]);
                }
                passports.Add(passport);
            }

            return passports;
        }

        private bool IsValidPassport (Dictionary<string, string> passport, bool deep)
        {
            foreach (var key in this.RequiredFields.Keys)
            {
                if (!passport.Keys.Contains(key))
                {
                    return false; // Found a missing required field.
                }
                var value = passport[key];
                if (string.IsNullOrWhiteSpace(value))
                {
                    return false;
                }
                if (!deep)
                {
                    continue; // Puzzle 1 for this day only needs to code above.
                }

                switch (key)
                {
                    case "byr":
                        if (!this.IsValidBirthYear(value)) { return false; }
                        break;
                    case "iyr":
                        if (!this.IsValidIssueYear(value)) { return false; }
                        break;
                    case "eyr":
                        if (!this.IsValidExpirationYear(value)) { return false; }
                        break;
                    case "hgt":
                        if (!this.IsValidHeight(value)) { return false; }
                        break;
                    case "hcl":
                        if (!this.IsValidHairColor(value)) { return false; }
                        break;
                    case "ecl":
                        if (!this.IsValidEyeColor(value)) { return false; }
                        break;
                    case "pid":
                        if (!this.IsValidPassportId(value)) { return false; }
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            return true;
        }

        private bool IsValidBirthYear (string value)
        {
            return this.IsValidNumberRange(value, 1920, 2002);
        }

        private bool IsValidIssueYear(string value)
        {
            return this.IsValidNumberRange(value, 2010, 2020);
        }

        private bool IsValidExpirationYear(string value)
        {
            return this.IsValidNumberRange(value, 2020, 2030);
        }

        private bool IsValidHeight(string value)
        {
            if (value.EndsWith("cm"))
            {
                return this.IsValidNumberRange(value.Replace("cm", string.Empty), 150, 193);
            }
            else if (value.EndsWith("in"))
            {
                return this.IsValidNumberRange(value.Replace("in", string.Empty), 59, 76);
            }
            return false;
        }

        private bool IsValidNumberRange(string value, int min, int max)
        {
            return int.TryParse(value, out var number)
                && number >= min
                && number <= max;
        }

        private bool IsValidHairColor(string value)
        {
            return Regex.IsMatch(value, "^[#]{1}[0-9a-f]{6}$");
        }

        private bool IsValidEyeColor(string value)
        {
            return this.ValidEyeColours.Contains(value);
        }

        private bool IsValidPassportId(string value)
        {
            return Regex.IsMatch(value, "^[0-9]{9}$");
        }
    }
}
