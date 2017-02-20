using E2E.UWP.DTOs.FaceDto;
using E2E.UWP.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2E.UWP.Helpers
{
    public static class LookingDirectionHelper
    {
        // accuracy should not less than 0.5
        const double ACCURACY = 0.65;
        const double STARTINGACCURACY = 1 - ACCURACY;

        public static LookingDirectionObject GetLookingDirection(Facelandmarks faceLandmarks)
        {
            var leftXMinPos = faceLandmarks.eyeLeftOuter.x;
            var leftXMaxPos = faceLandmarks.eyeLeftInner.x;
            var leftXPos = faceLandmarks.pupilLeft.x;

            double leftXMaxValue = leftXMaxPos - leftXMinPos;
            double leftXValue = leftXPos - leftXMinPos;

            double xPercent = leftXValue / leftXMaxValue;

            // left Y
            var leftYMinPos = faceLandmarks.eyeLeftTop.y;
            var leftYMaxPos = faceLandmarks.eyeLeftBottom.y;
            var leftYPos = faceLandmarks.pupilLeft.y;

            double leftYMaxValue = leftYMaxPos - leftYMinPos;
            double leftYValue = leftYPos - leftYMinPos;

            double yPercent = leftYValue / leftYMaxValue;

            // generate direction
            var direction = new LookingDirectionObject();

            // mirror direction!
            if (xPercent < STARTINGACCURACY)
                //direction.IsLookingLeft = true;
                direction.IsLookingRight = true;
            else if (xPercent > ACCURACY)
                //direction.IsLookingRight = true;
                direction.IsLookingLeft = true;
            else if (yPercent < (STARTINGACCURACY - 0.1)) //0.1 for calibration, maybe all of the values need calibrate
                direction.IsLookingTop = true;
            else if (yPercent > (ACCURACY - 0.1))
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

        public static async Task<LookingDirectionObject> GetLookingDirectionAsync(Facelandmarks faceLandmarks)
            => await Task.Run(() => GetLookingDirection(faceLandmarks));


    }
}
