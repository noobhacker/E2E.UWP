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

        public int Ms { get => ms; set { ms = value; OnPropertyChanged(); } }
    }
}
