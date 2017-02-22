using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2E.UWP.WebServices
{
    public class NextWordWebService
    {
        static AzureHttpClient httpClient;
        const string endpoint =
            "https://westus.api.cognitive.microsoft.com/text/weblm/v1.0/generateNextWords?model=query&words=";
        const string key = "cb67154570fe4d3794f0530a17c03922";

        static NextWordWebService()
        {
            httpClient = new AzureHttpClient(endpoint, key);
        }

        public static async Task<string> GetNextWordAsync(string input)
            => await httpClient.PostAsync<string>(input);
    }
}
