using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2E.UWP.DTOs.FaceDto
{
    public class FaceObject
    {
        public string faceId { get; set; }
        public Facerectangle faceRectangle { get; set; }
        public Facelandmarks faceLandmarks { get; set; }
        public Faceattributes faceAttributes { get; set; }
    }

    public class Facerectangle
    {
        public int width { get; set; }
        public int height { get; set; }
        public int left { get; set; }
        public int top { get; set; }
    }

    public class Facelandmarks
    {
        public Position pupilLeft { get; set; }
        public Position pupilRight { get; set; }
        public Position noseTip { get; set; }
        public Position mouthLeft { get; set; }
        public Position mouthRight { get; set; }
        public Position eyebrowLeftOuter { get; set; }
        public Position eyebrowLeftInner { get; set; }
        public Position eyeLeftOuter { get; set; }
        public Position eyeLeftTop { get; set; }
        public Position eyeLeftBottom { get; set; }
        public Position eyeLeftInner { get; set; }
        public Position eyebrowRightInner { get; set; }
        public Position eyebrowRightOuter { get; set; }
        public Position eyeRightInner { get; set; }
        public Position eyeRightTop { get; set; }
        public Position eyeRightBottom { get; set; }
        public Position eyeRightOuter { get; set; }
        public Position noseRootLeft { get; set; }
        public Position noseRootRight { get; set; }
        public Position noseLeftAlarTop { get; set; }
        public Position noseRightAlarTop { get; set; }
        public Position noseLeftAlarOutTip { get; set; }
        public Position noseRightAlarOutTip { get; set; }
        public Position upperLipTop { get; set; }
        public Position upperLipBottom { get; set; }
        public Position underLipTop { get; set; }
        public Position underLipBottom { get; set; }
    }

    public class Position
    {
        public float x { get; set; }
        public float y { get; set; }
    }

    public class Faceattributes
    {
        public float age { get; set; }
        public string gender { get; set; }
        public Headpose headPose { get; set; }
        public int smile { get; set; }
        public Facialhair facialHair { get; set; }
        public string glasses { get; set; }
    }

    public class Headpose
    {
        public float roll { get; set; }
        public float yaw { get; set; }
        public int pitch { get; set; }
    }

    public class Facialhair
    {
        public float moustache { get; set; }
        public float beard { get; set; }
        public float sideburns { get; set; }
    }
}
