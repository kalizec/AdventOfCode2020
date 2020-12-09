using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace AdventOfCode.Input
{
    public static class InputHelper
    {
        public const string EndPoint = "https://adventofcode.com";

        public static async Task<string> Get(int year, int day)
        {
            if (InputHelper.Exists(year, day))
            {
                return await InputHelper.Read(year, day);
            }

            var input = await InputHelper.Get($"/{year}/day/{day}/input");
            if (input == null)
            {
                throw new NotImplementedException();
            }
            InputHelper.Write(year, day, input);
            return input;
        }

        private static bool Exists(int year, int day)
        {
            return File.Exists(InputHelper.GetFileName(year, day));
        }

        private static void Write(int year, int day, string input)
        {
            File.WriteAllText(InputHelper.GetFileName(year, day), input);
        }

        private static Task<string> Read(int year, int day)
        {
            return File.ReadAllTextAsync(InputHelper.GetFileName(year, day));
        }

        private static string GetFileName(int year, int day)
        {
            return $"../../../Input/input-{year}-{day}";
        }

        private static async Task<string> Get(string path)
        {
            var baseAddress = new Uri(InputHelper.EndPoint);
            var cookieContainer = new CookieContainer();

            using var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
            using var client = new HttpClient(handler) { BaseAddress = baseAddress };

            cookieContainer.Add(baseAddress, new Cookie("session", Program.Session));
            var response = await client.GetAsync(path);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
