using GalaSoft.MvvmLight;
using System;

namespace GPVP.HelperClasses
{
    public class Tag : ObservableObject
    {
        public string Name { get; set; }

        private bool activated;
        public bool Activated
        {
            get { return activated; }
            set
            {
                if ( value != activated )
                {
                    activated = value;
                    ActivateEvent?.Invoke(this, null);
                    RaisePropertyChanged(nameof(Activated));
                }
            }
        }

        public delegate void TagActivity(object sender, EventArgs e);
        public event TagActivity ActivateEvent;
    }
}
