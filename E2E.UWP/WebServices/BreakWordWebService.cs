using E2E.UWP.Extensions.HttpExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace E2E.UWP.WebServices
{
    public static class BreakWordWebService
    {
        static AzureHttpClient httpClient;
        const string endpoint =
            "https://westus.api.cognitive.microsoft.com/text/weblm/v1.0/breakIntoWords?model=query&text=";
        const string key= "cb67154570fe4d3794f0530a17c03922";
        static BreakWordWebService()
        {
            httpClient = new AzureHttpClient(endpoint, key);
        }

        public static async Task<string> SplitWordAsync(string input)
            => await httpClient.PostAsync<string>(input);

    }
}
