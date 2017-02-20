﻿using E2E.UWP.DTOs.FaceDto;
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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
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
            await mediaCapture.InitializeAsync(settings);

            // set lowest resolution
            var resolutions = mediaCapture.VideoDeviceController.GetAvailableMediaStreamProperties(MediaStreamType.Photo).Select(x => x as VideoEncodingProperties);
            var minRes = resolutions.OrderBy(x => x.Height * x.Width).FirstOrDefault();
            await mediaCapture.VideoDeviceController.SetMediaStreamPropertiesAsync(MediaStreamType.Photo, minRes);
        
            preview.Source = mediaCapture;
            await mediaCapture.StartPreviewAsync();
            while(true)
            {
                var lowLagCapture = await mediaCapture.PrepareLowLagPhotoCaptureAsync(ImageEncodingProperties.CreatePng());

                var capturedPhoto = await lowLagCapture.CaptureAsync();

                await lowLagCapture.FinishAsync();
                //const BitmapPixelFormat faceDetectionPixelFormat = BitmapPixelFormat.Gray8;

                //SoftwareBitmap convertedBitmap;
                //var sourceBitmap = capturedPhoto.Frame.SoftwareBitmap;
                //if (sourceBitmap.BitmapPixelFormat != faceDetectionPixelFormat)
                //{
                //    convertedBitmap = SoftwareBitmap.Convert(sourceBitmap, faceDetectionPixelFormat);
                //}
                //else
                //{
                //    convertedBitmap = sourceBitmap;
                //}
                //// crop the face out
                //var softwareBitmap = convertedBitmap;
                //var faceDetector = await FaceDetector.CreateAsync();
                //var detectedFace = await faceDetector.DetectFacesAsync(softwareBitmap);
                //if (detectedFace.Count() == 0)
                //    Debug.WriteLine("no face");
                //else if (detectedFace.Count() > 1)
                //    Debug.WriteLine("many face");
                //else
                //{
                //    Debug.WriteLine("crop");

                //}

                // preview image for send
    //            if (softwareBitmap.BitmapPixelFormat != BitmapPixelFormat.Bgra8 ||
    //softwareBitmap.BitmapAlphaMode == BitmapAlphaMode.Straight)
    //            {
    //                softwareBitmap = SoftwareBitmap.Convert(softwareBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
    //            }
    //            var bitmapSource = new SoftwareBitmapSource();
    //            await bitmapSource.SetBitmapAsync(softwareBitmap);
                //processPreview.Source = bitmapSource;

                var sw = new Stopwatch();
                sw.Start();
                List<FaceObject> faces = null;
                try
                {

                    //faces = await faceServiceClient.DetectAsync(processPreview.om, true, true, null);

                    var HttpContent = new HttpStreamContent(capturedPhoto.Frame.AsStream().AsInputStream());
                    HttpContent.Headers.ContentType = new Windows.Web.Http.Headers.HttpMediaTypeHeaderValue("application/octet-stream");
                    var client = new HttpClient();
                    var uri = new Uri($"https://westus.api.cognitive.microsoft.com/face/v1.0/detect?returnFaceId=true&returnFaceLandmarks=true");
                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "a41afd4e07a24847a9b14eb7bb548f0c");
                    var result = await client.PostAsync(uri, HttpContent);
                    string jsonString = result.Content.ToString();
                    faces = JsonConvert.DeserializeObject<List<FaceObject>>(jsonString);
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
                }
                
            }
        }
        
    }
}