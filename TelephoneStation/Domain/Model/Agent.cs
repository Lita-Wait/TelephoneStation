
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TelephoneStation.Domain.Model
{
    public class Agent : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public required string Name { get; set; }
        public bool isBusy { get; set; }
        public override string ToString() => Name;

    }
}
