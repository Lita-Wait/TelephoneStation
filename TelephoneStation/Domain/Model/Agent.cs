
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TelephoneStation.Domain.Model
{
    public class Agent : INotifyPropertyChanged
    {
        public required string Name { get; set; }

        private bool _isBusy = false;
        public bool isBusy { get => _isBusy; set => SetProperty(ref _isBusy, value); }
        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = "")
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }

            return false;
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        public override string ToString() => Name;

    }
}
