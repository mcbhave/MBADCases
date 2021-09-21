using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
 
namespace Yardillofunctions
{
    public static class Function1
    {
        private static Random _random = new Random();
        [FunctionName("Function1")]
        public static async Task RunAsync([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer, ILogger log)
        {
            var srand = RandomString(5, true);
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri("https://yardillo.azurewebsites.net/V1/case/CallScheduler"),
                Headers =
    {
        { "x-rapidapi-host", "yardillo.p.rapidapi.com" },
        { "x-rapidapi-key", "36d9952f5bmshdde26829370d654p1d5a5cjsnad7db9145569" },
        {"X-RapidAPI-Proxy-Secret","6acc1280-fde1-11eb-b480-3f057f12dc26"},
        { "X-RapidAPI-User", "yardilloapi@gmail.com" },
        { "Y-Auth-Src", "yardillo" },
    },

                Content = new StringContent("{\r\n    \"Casenumber\":" + _random.Next(1, 1000000) + ",\r\n    \"Casetitle\": \"" + DateTime.Now.ToString() + "\",\r\n    \"Casestatus\": \"Open\",\r\n  " +
                " \"Casedescription\": \"Sample case of type mongodb " + DateTime.Now.ToString() + "\",\r\n    \"Fields\": [\r\n     " +
                " {\r\n            \"Fieldid\": \"field one\",\r\n            \"Value\": \"1\"\r\n        },\r\n        {\r\n     " +
                "\"Fieldid\": \"fieldA\",\r\n            \"Value\": \"field value A\"\r\n        },\r\n        {\r\n     " +
                "\"Fieldid\": \"fieldB\",\r\n            \"Value\": \"\"\r\n        }\r\n    ]\r\n}")
                {
                    Headers =
        {
            ContentType = new MediaTypeHeaderValue("application/json")
        }
                }
            };
            using (var response = client.SendAsync(request).GetAwaiter().GetResult())
            {
                response.EnsureSuccessStatusCode();
                var body = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                log.LogInformation(body);
            }
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

        }
        public static string RandomString(int size, bool lowerCase = false)
        {
            var builder = new StringBuilder(size);

            // Unicode/ASCII Letters are divided into two blocks
            // (Letters 65–90 / 97–122):
            // The first group containing the uppercase letters and
            // the second group containing the lowercase.  

            // char is a single Unicode character  
            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26; // A...Z or a..z: length=26  

            for (var i = 0; i < size; i++)
            {
                var @char = (char)_random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }
    }
}
