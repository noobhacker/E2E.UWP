using E2E.UWP.Services;
using E2E.UWP.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace E2E.UWP.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class KeyboardPage : Page
    {
        KeyboardViewModel vm = new KeyboardViewModel();
        public KeyboardPage()
        {
            this.InitializeComponent();
            this.DataContext = vm;
            CameraService.DirectionProcessed += CameraService_DirectionProcessed;
        }

        string previousDirection = "Middle";
        int repeat = 0;
        private void CameraService_DirectionProcessed(object sender, Objects.LookingDirectionObject e)
        {
            if(e.IsLookingLeft)
            {
                CheckIsRepeat("Left");
                if(vm.SelectionIndex != 0)
                {

                }
            }
        }

        private void CheckIsRepeat(string direction)
        {
            if (previousDirection == direction)
                repeat+=1;
            else
                repeat = 0;
        }
    }
}
