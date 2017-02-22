using E2E.UWP.DTOs.FaceDto;
using E2E.UWP.Extensions.CameraExtension;
using E2E.UWP.Helpers;
using E2E.UWP.Objects;
using E2E.UWP.ViewModels;
using E2E.UWP.WebServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage.Streams;
using Windows.System.Display;

namespace E2E.UWP.Services
{
    public static class CameraService
    {
        // output message view model
        static MainViewModel vm = MainPage.vm;

        public static MediaCapture mediaCapture = new MediaCapture();
        static DisplayRequest displayRequest = new DisplayRequest();
        public static async Task InitializeServiceAsync()
        {
            await mediaCapture.SetFrontCameraAsync();
            await mediaCapture.SetLowestResolutionAsync();
            
            displayRequest.RequestActive();
        }

        // infinity loop! dont await
        public static async void StartServiceAsync()
        {
            await mediaCapture.StartPreviewAsync();
            var sw = new Stopwatch();

            while (true)
            {
                sw.Start();
                List<FaceObject> faces = null;
                var properties = GetCompressedProperties();
                using (var imageStream = new InMemoryRandomAccessStream())
                {
                    try
                    {
                        await mediaCapture.CapturePhotoToStreamAsync(properties, imageStream);
                        faces = await FaceWebService.AnalyzeImageAsync(imageStream);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        vm.CameraStatus = "Camera permission denied";
                        await Task.Delay(5000);
                        continue;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                        continue;
                    }
                }

                vm.CameraStatus = CameraMessageHelper.GetCameraMessage(faces);

                // something wrong during post
                if (faces == null)
                    continue;

                if (faces.Count() == 1)
                {
                    var face = faces.FirstOrDefault();
                    var result = await LookingDirectionAlgorithm.GetLookingDirectionAsync(face.faceLandmarks);
                    DirectionProcessed(null, result);
                }

                // write ms
                vm.Ms = Convert.ToInt32(sw.ElapsedMilliseconds);
                sw.Reset();
            }
        }

        public static event EventHandler<LookingDirectionObject> DirectionProcessed;

        private static ImageEncodingProperties GetCompressedProperties()
        {
            var encodingQuality = ImageEncodingProperties.CreatePng();
            //encodingQuality.Height = encodingQuality.Height / 2;
            //encodingQuality.Width = encodingQuality.Width / 2;
            //encodingQuality.Subtype = "GIF";
            return encodingQuality;
        }

    }
}
