using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;

namespace E2E.UWP.Extensions.CameraExtension
{
    public static class CameraExtension
    {
        public static async Task SetFrontCameraAsync(this MediaCapture mediaCapture)
        {
            // find front camera
            var allVideoDevices = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
            var frontCamera = allVideoDevices.Where(x => x.EnclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Front);
            var settings = new MediaCaptureInitializationSettings();
            settings.VideoDeviceId = frontCamera.FirstOrDefault().Id;

            //mediaCapture.VideoDeviceController.Brightness += 20;

            await mediaCapture.InitializeAsync(settings);
        }

        public static async Task SetLowestResolutionAsync(this MediaCapture mediaCapture)
        {
            // set lowest resolution
            var resolutions = mediaCapture.VideoDeviceController.GetAvailableMediaStreamProperties(MediaStreamType.Photo).Select(x => x as VideoEncodingProperties);
            var minRes = resolutions.OrderBy(x => x.Height * x.Width).FirstOrDefault();
            await mediaCapture.VideoDeviceController.SetMediaStreamPropertiesAsync(MediaStreamType.Photo, minRes);
        }
    }
}
