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
        private double xPercent;
        private double yPercent;


        public int Ms { get => ms; set { ms = value; OnPropertyChanged(); } }
        public string CameraStatus { get => cameraStatus; set { cameraStatus = value; OnPropertyChanged(); } }
        public double XPercent { get => xPercent; set { xPercent = value; OnPropertyChanged(); } }
        public double YPercent { get => yPercent; set { yPercent = value; OnPropertyChanged(); } }

    }
}
