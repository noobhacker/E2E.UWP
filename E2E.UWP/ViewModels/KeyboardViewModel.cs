using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2E.UWP.ViewModels
{
    public class KeyboardViewModel : BaseViewModel
    {
        public ObservableCollection<string> Selections { get; set; }
      
        private string inputText;
        private string autoCompleteText;
        private int selectionIndex;

        public string InputText { get => inputText; set { inputText = value; OnPropertyChanged(); } }
        public string AutoCompleteText { get => autoCompleteText; set { autoCompleteText = value; OnPropertyChanged(); } }

        public int SelectionIndex { get => selectionIndex; set { selectionIndex = value; OnPropertyChanged(); } }


        const string alphabets = "abcdefghijklmnopqrstuvwxyz";
        public KeyboardViewModel()
        {
            Selections = new ObservableCollection<string>();
            Selections.Add("Back");
            foreach (var item in alphabets)
                Selections.Add(item.ToString());
            Selections.Add("Send");

            SelectionIndex = 1;
        }

    }
}
