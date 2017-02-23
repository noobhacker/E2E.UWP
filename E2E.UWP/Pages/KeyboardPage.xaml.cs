using E2E.UWP.Dictionaries;
using E2E.UWP.Models;
using E2E.UWP.Services;
using E2E.UWP.ViewModels;
using E2E.UWP.WebServices;
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
            CameraService.DirectionProcessed += CameraService_DirectionProcessedAsync;
        }

        DatabaseContext db = new DatabaseContext();

        string previousDirection = "Middle";
        int repeat = 1;
        private async void CameraService_DirectionProcessedAsync(object sender, Objects.LookingDirectionObject e)
        {
            var selection = vm.Selections[vm.SelectionIndex];

            var splittedSentence = await BreakWordWebService.SplitWordAsync(vm.InputText);
            var words = splittedSentence.Split(' ');
            var lastword = words[words.Count()];

            if (e.IsLookingLeft)
            {
                CheckIsRepeat("Left");
                if (vm.SelectionIndex < repeat)
                    vm.SelectionIndex -= repeat;
                else
                    vm.SelectionIndex = 0;
            }

            if (e.IsLookingRight)
            {
                CheckIsRepeat("Right");
                if (vm.SelectionIndex > repeat)
                    vm.SelectionIndex += repeat;
                else
                    vm.SelectionIndex = vm.Selections.Count;
            }

            if (e.IsLookingTop)
            {
                CheckIsRepeat("Top");

                if (selection == "Back")
                    this.Frame.GoBack();
                else if (selection == "Send")
                    Frame.Navigate(typeof(MainPage)); // next page! ask to auto correct and spacing
                else
                {
                    vm.InputText += selection;

                    // generate auto suggestion
                    var wordFromDb = db.WordFrequencies
                        .FirstOrDefault(x => x.Word.StartsWith(lastword))
                        .Word;

                    if(wordFromDb != null)
                    {
                        vm.AutoCompleteText = wordFromDb;
                    }
                    else
                    {
                        var wordFromList = EnglishDictionary.EnglishWords
                            .Where(x => x.StartsWith(lastword))
                            .FirstOrDefault();

                        if (wordFromList == null)
                            vm.AutoCompleteText = "";
                        else
                            vm.AutoCompleteText = wordFromList;
                    }

                }
            }

            if (e.IsLookingBottom)
            {
                CheckIsRepeat("Bottom");
                vm.InputText = vm.InputText.Substring(0,vm.InputText.Count() - lastword.Count());

                // don't want double spacing
                if (!vm.InputText.EndsWith(" "))
                    vm.InputText += " ";

                vm.InputText += selection + " ";
                vm.AutoCompleteText = await NextWordWebService.GetNextWordAsync(vm.AutoCompleteText);

                vm.AutoCompleteText = "";
            }

            if (e.IsLookingCenter)
                CheckIsRepeat("Center");

        }

        private void CheckIsRepeat(string direction)
        {
            if (previousDirection == direction)
                repeat += 1;
            else
                repeat = 1;

            previousDirection = direction;
        }
    }
}
