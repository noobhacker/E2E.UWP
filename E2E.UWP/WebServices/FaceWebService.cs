using E2E.UWP.DTOs.FaceDto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Web.Http;

namespace E2E.UWP.WebServices
{
    public static class FaceWebService
    {
        static HttpClient httpClient = new HttpClient();
        static Uri endpoint =
            new Uri($"https://westus.api.cognitive.microsoft.com/face/v1.0/detect?returnFaceId=true&returnFaceLandmarks=true");

        static FaceWebService()
        {
            httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key",
                "a41afd4e07a24847a9b14eb7bb548f0c");
        }

        public static async Task<List<FaceObject>> AnalyzeImageAsync(
            InMemoryRandomAccessStream imageStream)
        {
            imageStream.Seek(0);
            var httpContent = new HttpStreamContent(imageStream);
            await httpContent.BufferAllAsync();

            httpContent.Headers.ContentType =
                new Windows.Web.Http.Headers.HttpMediaTypeHeaderValue("application/octet-stream");

            try
            {
                var result = await httpClient.PostAsync(endpoint, httpContent);
                result.EnsureSuccessStatusCode();

                var jsonString = result.Content.ToString();
                var jsonObject = JsonConvert.DeserializeObject<List<FaceObject>>(jsonString);
                return jsonObject;
            }
            catch
            {
                return null;
            }

        }

    }
}
