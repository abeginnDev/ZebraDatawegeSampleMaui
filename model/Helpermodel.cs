using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace scannerwerWithIntent.model
{
    public sealed class Helpermodel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static Helpermodel _instance;
        public static Helpermodel GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Helpermodel();
            }
            return _instance;
        }

        private String scannedText = "Test";


        public void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public string ScannedText
        {
            get => scannedText;
            set
            {
                scannedText = value;
                OnPropertyChanged(nameof(ScannedText));
            }
        }

        public Helpermodel()
        {

        }
    }
}
