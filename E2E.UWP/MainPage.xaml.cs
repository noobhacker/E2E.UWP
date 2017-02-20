using E2E.UWP.DTOs.FaceDto;
using E2E.UWP.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Media.FaceAnalysis;
using Windows.Media.MediaProperties;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using Windows.Web.Http;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace E2E.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            StartCameraAsync();
        }

        private async Task StartCameraAsync()
        {
            var mediaCapture = new MediaCapture();
            // find front camera
            var allVideoDevices = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
            var frontCamera = allVideoDevices.Where(x => x.EnclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Front);
            var settings = new MediaCaptureInitializationSettings();
            settings.VideoDeviceId = frontCamera.FirstOrDefault().Id;

            //mediaCapture.VideoDeviceController.Brightness += 20;

            await mediaCapture.InitializeAsync(settings);

            // set lowest resolution
            var resolutions = mediaCapture.VideoDeviceController.GetAvailableMediaStreamProperties(MediaStreamType.Photo).Select(x => x as VideoEncodingProperties);
            var minRes = resolutions.OrderBy(x => x.Height * x.Width).FirstOrDefault();
            await mediaCapture.VideoDeviceController.SetMediaStreamPropertiesAsync(MediaStreamType.Photo, minRes);

            preview.Source = mediaCapture;
            await mediaCapture.StartPreviewAsync();

            var sw = new Stopwatch();
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "a41afd4e07a24847a9b14eb7bb548f0c");
            var uri = new Uri($"https://westus.api.cognitive.microsoft.com/face/v1.0/detect?returnFaceId=true&returnFaceLandmarks=true");

            while (true)
            {

                
                sw.Start();
                List<FaceObject> faces = null;
                try
                {
                    using (var capturedPhoto = new InMemoryRandomAccessStream())
                    {
                        await mediaCapture.CapturePhotoToStreamAsync(ImageEncodingProperties.CreatePng(), capturedPhoto);                       
                        capturedPhoto.Seek(0);
                        var httpContent = new HttpStreamContent(capturedPhoto);
                        await httpContent.BufferAllAsync();
                        httpContent.Headers.ContentType = new Windows.Web.Http.Headers.HttpMediaTypeHeaderValue("application/octet-stream");                       
                        var result = await client.PostAsync(uri, httpContent);
                        string jsonString = result.Content.ToString();
                        faces = JsonConvert.DeserializeObject<List<FaceObject>>(jsonString);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }

                sw.Stop();
                Debug.WriteLine($"cloud analyze took {sw.ElapsedMilliseconds.ToString()}ms");
                sw.Reset();

                if (faces.Count() == 0)
                    Debug.WriteLine("no face");
                else if (faces.Count() > 1)
                    Debug.WriteLine("many face");
                else
                {
                    var face = faces[0];

                    sw.Start();
                    var result = await LookingDirectionHelper.GetLookingDirectionAsync(face.faceLandmarks);
                    sw.Stop();
                    Debug.WriteLine($"Analyze took {sw.ElapsedMilliseconds.ToString()}ms");
                    sw.Reset();

                    // draw
                    foreach(var item in positionCanvas.Children.AsEnumerable())
                    {
                        positionCanvas.Children.Remove(item);
                    }

                    var dot = new Ellipse();
                    dot.Fill = new SolidColorBrush(Colors.Green);
                    dot.Height = 10;
                    dot.Width = 10;
                    dot.Margin = new Thickness(positionCanvas.ActualWidth * (result.XPercent > 0.5? result.XPercent * 1.2: result.XPercent/1.2),
                        positionCanvas.ActualHeight * (result.YPercent > 0.5? result.YPercent * 1.3: result.YPercent/1.3), 0, 0);
                    positionCanvas.Children.Add(dot);

                }
            }
        }

    }
}
