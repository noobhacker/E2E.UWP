using E2E.UWP.DTOs.FaceDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2E.UWP.Helpers
{
    public static class CameraMessageHelper
    {
        public static string GetCameraMessage(List<FaceObject> faces)
        {
            if (faces == null)
                return "Internet failure";

            if (faces.Count() == 0)
                return "No face detected";

            if (faces.Count() > 1)
                return "More than 1 face detected";

            if(faces.Count == 1)
                return "OK";

            // unlikely
            return "";
        }
    }
}
