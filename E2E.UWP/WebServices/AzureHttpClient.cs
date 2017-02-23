using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace E2E.UWP.WebServices
{
    public class AzureHttpClient
    {
        HttpClient httpClient;
        string endpoint;

        public AzureHttpClient(string endpoint, string key)
        {
            httpClient = new HttpClient();

            this.endpoint = endpoint;
            httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", key);
            
        }

        public async Task<T> PostAsync<T>(string param = "",
            IHttpContent httpContent = null,
            string contentType = "application/json")
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
                return default(T);

            try
            {
                var uri = new Uri(endpoint + param);

                if (httpContent == null)
                    httpContent = new HttpStringContent("");

                httpContent.Headers.ContentType =
                    new Windows.Web.Http.Headers.HttpMediaTypeHeaderValue(contentType);
                
                var result = await httpClient.PostAsync(uri, httpContent);
                result.EnsureSuccessStatusCode();

                var jsonString = result.Content.ToString();
                var jsonObject = JsonConvert.DeserializeObject<T>(jsonString);
                return jsonObject;
            }
            catch
            {
                return default(T);
            }
        }

    }
}
