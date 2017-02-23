using E2E.UWP.DTOs.BreakWordDto;
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
        {
            var result = await httpClient.PostAsync<BreakWordObject>(input);

            if (result.candidates.Count() > 0)
                return result.candidates[0].words;
            else
                return "";
        }

    }
}
