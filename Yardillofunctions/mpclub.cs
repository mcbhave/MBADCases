using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Yardillofunctions
{
    public static class mpclub
    {
        [FunctionName("mpclub")]
        public static void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri("https://yardillo.azurewebsites.net/V1/mpclub"),
                Headers =
    {
        { "x-rapidapi-host", "yardillo.p.rapidapi.com" },
        { "x-rapidapi-key", "36d9952f5bmshdde26829370d654p1d5a5cjsnad7db9145569" },
        {"X-RapidAPI-Proxy-Secret","6acc1280-fde1-11eb-b480-3f057f12dc26"},
        { "X-RapidAPI-User", "yardilloapi@gmail.com" },
        { "Y-Auth-Src", "yardillo" },
    },

                Content = new StringContent("{}")
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
    }
}
