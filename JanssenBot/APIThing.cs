using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JanssenBot
{
    public class APIThing
    {
        public static async void GetInfo(string channel)
        {
            string page = $"https://osu.ppy.sh/api/get_match?k={MainWindow.apiKey}&mp={channel}";

            // ... Use HttpClient.
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(page))
            using (HttpContent content = response.Content)
            // ... Read the string.
            {
                string result = await content.ReadAsStringAsync();
                string[] splitResult = SplitRequestOne(result);
            }
        }

        private static string[] SplitRequestOne(string result)
        {
            string[] splitRes;
            splitRes = result.Split('{', '}');
            return splitRes;
        }
    }
}
