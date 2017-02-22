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
        static AzureHttpClient httpClient;
        const string endpoint = "https://westus.api.cognitive.microsoft.com/face/v1.0/detect?returnFaceLandmarks=true";
        const string key = "a41afd4e07a24847a9b14eb7bb548f0c";

        static FaceWebService()
        {
            httpClient = new AzureHttpClient(endpoint, key);
        }

        public static async Task<List<FaceObject>> AnalyzeImageAsync(
            InMemoryRandomAccessStream imageStream)
        {
            imageStream.Seek(0);
            var httpContent = new HttpStreamContent(imageStream);
            await httpContent.BufferAllAsync();

            return await httpClient.PostAsync<List<FaceObject>>(
                "", 
                httpContent, 
                "application/octet-stream");
        }

    }
}
