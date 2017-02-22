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
    public sealed partial class NursingPage : Page
    {
        NursingViewModel vm = new NursingViewModel();
        public NursingPage()
        {
            this.InitializeComponent();
            this.DataContext = vm;
            CameraService.DirectionProcessed += MainPage_DirectionProcessed;
        }

        private void MainPage_DirectionProcessed(object sender, Objects.LookingDirectionObject e)
        {
            if (e.IsLookingTop)
                vm.TopCount += 1;
            if (e.IsLookingLeft)
                vm.LeftCount += 1;
            if (e.IsLookingRight)
                vm.RightCount += 1;
            if (e.IsLookingBottom)
                vm.BottomCount += 1;
        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            vm.TopCount = 0;
            vm.LeftCount = 0;
            vm.RightCount = 0;
            vm.BottomCount = 0;
        }
    }
}
