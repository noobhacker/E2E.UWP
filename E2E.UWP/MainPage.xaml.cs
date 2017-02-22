using E2E.UWP.DTOs.FaceDto;
using E2E.UWP.Extensions;
using E2E.UWP.Extensions.CameraExtension;
using E2E.UWP.Extensions.DotOnCanvasExtension;
using E2E.UWP.Helpers;
using E2E.UWP.Objects;
using E2E.UWP.Pages;
using E2E.UWP.Services;
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
using Windows.System.Display;
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
        public static MainViewModel vm = new MainViewModel();
        public MainPage()
        {
            this.InitializeComponent();
            this.DataContext = vm;
            CameraService.DirectionProcessed += this.CameraService_DirectionProcessed;      
        }

        private void CameraService_DirectionProcessed(object sender, LookingDirectionObject e)
        {
            positionCanvas.RemoveDots();
            positionCanvas.DrawDotByPercent(e.XPercent, e.YPercent);
            vm.XPercent = e.XPercent;
            vm.YPercent = e.YPercent;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            await CameraService.InitializeServiceAsync();
            preview.Source = CameraService.mediaCapture;
            CameraService.StartServiceAsync();

            frame.Navigate(typeof(KeyboardPage));
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // displayrequest.requestrelease
            // mediacapture.dispose
        }        

     
    }
}
