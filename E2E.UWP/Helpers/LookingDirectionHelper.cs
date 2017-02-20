using E2E.UWP.Objects;
using Microsoft.ProjectOxford.Face.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2E.UWP.Helpers
{
    public static class LookingDirectionHelper
    {
        const double ACCURACY = 0.8;
        const double STARTINGACCURACY = 1 - ACCURACY;

        public static LookingDirectionObject GetLookingDirection(FaceLandmarks faceLandmarks)
        {
            var leftXMinPos = faceLandmarks.EyeLeftOuter.X;
            var leftXMaxPos = faceLandmarks.EyeLeftInner.X;
            var leftXPos = faceLandmarks.PupilLeft.X;

            double leftXMaxValue = leftXMaxPos - leftXMinPos;
            double leftXValue = leftXPos - leftXMinPos;

            double xPercent = leftXValue / leftXMaxValue;

            // left Y
            var leftYMinPos = faceLandmarks.EyeLeftTop.Y;
            var leftYMaxPos = faceLandmarks.EyeLeftBottom.Y;
            var leftYPos = faceLandmarks.PupilLeft.Y;

            double leftYMaxValue = leftYMaxPos - leftYMinPos;
            double leftYValue = leftYPos - leftYMinPos;

            double yPercent = leftYValue / leftYMaxValue;

            // generate direction
            var direction = new LookingDirectionObject();

            if (xPercent < STARTINGACCURACY)
                direction.IsLookingLeft = true;
            else if (xPercent > ACCURACY)
                direction.IsLookingRight = true;
            else if (yPercent < STARTINGACCURACY)
                direction.IsLookingTop = true;
            else if (yPercent > ACCURACY)
                direction.IsLookingBottom = true;

            if (!direction.IsLookingLeft &&
                !direction.IsLookingRight &&
                !direction.IsLookingTop &&
                !direction.IsLookingBottom)
                direction.IsLookingCenter = true;

            direction.XPercent = xPercent;
            direction.YPercent = yPercent;

            return direction;
        }

        public static async Task<LookingDirectionObject> GetLookingDirectionAsync(FaceLandmarks faceLandmarks)
            => await Task.Run(() => GetLookingDirection(faceLandmarks));


    }
}
