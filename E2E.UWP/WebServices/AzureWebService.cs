using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace E2E.UWP.WebServices
{
    public static class BaseWebService
    {
        static HttpClient httpClient = new HttpClient();
        static string endpoint;

        static void InitializeAzureService(string _endpoint, string key)
        {
            endpoint = _endpoint;
            httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", key);
        }
        
    }
}
