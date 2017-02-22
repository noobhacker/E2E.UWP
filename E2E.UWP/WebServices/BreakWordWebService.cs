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
        static HttpClient httpClient = new HttpClient();
        static string endpoint =
            $"https://westus.api.cognitive.microsoft.com/face/v1.0/detect?returnFaceLandmarks=true");

        static BreakWordWebService()
        {
            httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key",
                "cb67154570fe4d3794f0530a17c03922");
        }

        public static async string SplitWordAsync(string input)
        {

        }

    }
}
