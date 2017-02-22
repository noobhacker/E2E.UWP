using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2E.UWP.ViewModels
{
    public class NursingViewModel : BaseViewModel
    {
        private int topCount;
        private int leftCount;
        private int rightCount;
        private int bottomCount;

        public int TopCount { get => topCount; set { topCount = value; OnPropertyChanged(); } }
        public int LeftCount { get => leftCount; set { leftCount = value; OnPropertyChanged(); } }
        public int RightCount { get => rightCount; set { rightCount = value; OnPropertyChanged(); } }
        public int BottomCount { get => bottomCount; set { bottomCount = value; OnPropertyChanged(); } }

    }
}
