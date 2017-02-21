using E2E.UWP.DTOs.FaceDto;
using E2E.UWP.Extensions;
using E2E.UWP.Extensions.CameraExtension;
using E2E.UWP.Extensions.DotOnCanvasExtension;
using E2E.UWP.Helpers;
using E2E.UWP.ViewModels;
using E2E.UWP.WebServices;
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
        MainViewModel vm = new MainViewModel();
        public MainPage()
        {
            this.InitializeComponent();
            this.DataContext = vm;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await InitializeServiceAsync();
        }

        MediaCapture mediaCapture = new MediaCapture();
        private async Task InitializeServiceAsync()
        {
            await mediaCapture.SetFrontCameraAsync();
            await mediaCapture.SetLowestResolutionAsync();

            preview.Source = mediaCapture;
            await mediaCapture.StartPreviewAsync();

            await StartServiceAsync();
        }

        private async Task StartServiceAsync()
        {
            var sw = new Stopwatch();
            sw.Start();
            while (true)
            {
                List<FaceObject> faces = null;
                try
                {
                    using (var imageStream = new InMemoryRandomAccessStream())
                    {
                        await mediaCapture.CapturePhotoToStreamAsync(ImageEncodingProperties.CreatePng(), imageStream);
                        faces = await FaceWebService.AnalyzeImageAsync(imageStream);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    continue;
                }

                vm.CameraStatus = CameraMessageHelper.GetCameraMessage(faces);

                if (faces == null)
                    continue;

                if (faces.Count() == 1)
                {
                
                    var face = faces.FirstOrDefault();

                    var result = await LookingDirectionHelper.GetLookingDirectionAsync(face.faceLandmarks);

                    if (positionCanvas.Children.Count() > 10)
                        positionCanvas.RemoveDots();
                    positionCanvas.DrawDotByPercent(result.XPercent, result.YPercent);

                }
                // write ms
                vm.Ms = Convert.ToInt32(sw.ElapsedMilliseconds);
                sw.Reset();
            }
        }

    }
}
