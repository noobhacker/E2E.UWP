using E2E.UWP.DTOs.FaceDto;
using E2E.UWP.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2E.UWP.Helpers
{
    public static class LookingDirectionAlgorithm
    {
        // accuracy should not less than 0.5
        const double ACCURACY = 0.6;
        const double STARTINGACCURACY = 1 - ACCURACY;

        public static LookingDirectionObject GetLookingDirection(Facelandmarks faceLandmarks)
        {
            // left X
            var leftXMinPos = faceLandmarks.eyeLeftOuter.x;
            var leftXMaxPos = faceLandmarks.eyeLeftInner.x;
            var leftXPos = faceLandmarks.pupilLeft.x;

            double leftXMaxValue = leftXMaxPos - leftXMinPos;
            double leftXValue = leftXPos - leftXMinPos;

            double leftXPercent = leftXValue / leftXMaxValue;

            // left Y
            var leftYMinPos = faceLandmarks.eyeLeftTop.y;
            var leftYMaxPos = faceLandmarks.eyeLeftBottom.y;
            var leftYPos = faceLandmarks.pupilLeft.y;

            double leftYMaxValue = leftYMaxPos - leftYMinPos;
            double leftYValue = leftYPos - leftYMinPos;

            double leftYPercent = leftYValue / leftYMaxValue;

            // right X
            var rightXMinPos = faceLandmarks.eyeRightInner.x;
            var rightXMaxPos = faceLandmarks.eyeRightOuter.x;
            var rightXPos = faceLandmarks.pupilRight.x;

            double rightXMaxValue = rightXMaxPos - rightXMinPos;
            double rightXValue = rightXPos - rightXMinPos;

            double rightXPercent = rightXValue / rightXMaxValue;

            // right Y
            var rightYMinPos = faceLandmarks.eyeRightTop.y;
            var rightYMaxPos = faceLandmarks.eyeRightBottom.y;
            var rightYPos = faceLandmarks.pupilRight.y;

            double rightYMaxValue = rightYMaxPos - rightYMinPos;
            double rightYValue = rightYPos - rightYMinPos;

            double rightYPercent = rightYValue / rightYMaxValue;

            // average, xPercent mirrored thus 1 - value
            double xPercent = 1 - ((leftXPercent + rightXPercent) / 2);
            double yPercent = (leftYPercent + rightYPercent) / 2;


            // generate direction
            var direction = new LookingDirectionObject();
                     
            if (xPercent < STARTINGACCURACY)
                direction.IsLookingLeft = true;
            else if (xPercent > ACCURACY)
                direction.IsLookingRight = true;
            else if (yPercent < (STARTINGACCURACY)) 
                direction.IsLookingTop = true;
            else if (yPercent > (ACCURACY))
                direction.IsLookingBottom = true;

            if (!direction.IsLookingLeft &&
                !direction.IsLookingRight &&
                !direction.IsLookingTop &&
                !direction.IsLookingBottom)
                direction.IsLookingCenter = true;

            direction.XPercent = xPercent;
            direction.YPercent = yPercent;

            Debug.WriteLine($"XPercent {xPercent} YPercent {yPercent}");
            return direction;
        }

        public static async Task<LookingDirectionObject> GetLookingDirectionAsync(Facelandmarks faceLandmarks)
            => await Task.Run(() => GetLookingDirection(faceLandmarks));

    }
}
