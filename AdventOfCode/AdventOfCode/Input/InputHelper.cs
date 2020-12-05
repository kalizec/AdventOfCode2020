using System;
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
            return await InputHelper.Get($"/{year}/day/{day}/input");
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
