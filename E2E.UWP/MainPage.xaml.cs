using E2E.UWP.Helpers;
using Microsoft.ProjectOxford.Face;
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
using Windows.Media.MediaProperties;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

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
        private readonly IFaceServiceClient faceServiceClient = new FaceServiceClient("a41afd4e07a24847a9b14eb7bb548f0c");

        private async Task StartCameraAsync()
        {
            var mediaCapture = new MediaCapture();
            // find front camera
            var allVideoDevices = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
            var frontCamera = allVideoDevices.Where(x => x.EnclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Front);
            var settings = new MediaCaptureInitializationSettings();
            settings.VideoDeviceId = frontCamera.FirstOrDefault().Id;            
            await mediaCapture.InitializeAsync(settings);

            // set lowest resolution
            var resolutions = mediaCapture.VideoDeviceController.GetAvailableMediaStreamProperties(MediaStreamType.Photo).Select(x => x as VideoEncodingProperties);
            var minRes = resolutions.OrderBy(x => x.Height * x.Width).FirstOrDefault();
            await mediaCapture.VideoDeviceController.SetMediaStreamPropertiesAsync(MediaStreamType.Photo, minRes);
        
            // preview.Source = mediaCapture;
            //await mediaCapture.StartPreviewAsync();
            while(true)
            {
                var lowLagCapture = await mediaCapture.PrepareLowLagPhotoCaptureAsync(ImageEncodingProperties.CreateUncompressed(MediaPixelFormat.Bgra8));
                
                var capturedPhoto = await lowLagCapture.CaptureAsync();

                await lowLagCapture.FinishAsync();

    //            var softwareBitmap = capturedPhoto.Frame.SoftwareBitmap;
    //            if (softwareBitmap.BitmapPixelFormat != BitmapPixelFormat.Bgra8 ||
    //softwareBitmap.BitmapAlphaMode == BitmapAlphaMode.Straight)
    //            {
    //                softwareBitmap = SoftwareBitmap.Convert(softwareBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
    //            }
    //            var bitmapSource = new SoftwareBitmapSource();
    //            await bitmapSource.SetBitmapAsync(softwareBitmap);
    //            preview.Source = bitmapSource;

                var sw = new Stopwatch();
                sw.Start();
                var faces = await faceServiceClient.DetectAsync(capturedPhoto.Frame.AsStream());
                sw.Stop();
                Debug.Write($"cloud analyze took {sw.ElapsedMilliseconds.ToString()}ms");
                sw.Reset();

                if (faces.Count() == 0)
                    Debug.Write("no face");
                else if (faces.Count() > 1)
                    Debug.Write("many face");
                else
                {
                    var face = faces[0];
                 
                    sw.Start();
                    var result = await LookingDirectionHelper.GetLookingDirectionAsync(face.FaceLandmarks);
                    sw.Stop();
                    Debug.Write($"Analyze took {sw.ElapsedMilliseconds.ToString()}ms");
                }
                
                await Task.Delay(1000);
            }
        }
        
    }
}
