using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2E.UWP.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private int ms;
        private string cameraStatus;
        private string userId;
        private double xPercent;
        private double yPercent;

        private int topCount;
        private int leftCount;
        private int rightCount;
        private int bottomCount;

        public int Ms { get => ms; set { ms = value; OnPropertyChanged(); } }
        public string CameraStatus { get => cameraStatus; set { cameraStatus = value; OnPropertyChanged(); } }
        public string UserId { get => userId; set { userId = value; OnPropertyChanged(); } }
        public double XPercent { get => xPercent; set { xPercent = value; OnPropertyChanged(); } }
        public double YPercent { get => yPercent; set { yPercent = value; OnPropertyChanged(); } }

        public int TopCount { get => topCount; set { topCount = value; OnPropertyChanged(); } }
        public int LeftCount { get => leftCount; set { leftCount = value; OnPropertyChanged(); } }
        public int RightCount { get => rightCount; set { rightCount = value; OnPropertyChanged(); } }
        public int BottomCount { get => bottomCount; set { bottomCount = value; OnPropertyChanged(); } }
    }
}
