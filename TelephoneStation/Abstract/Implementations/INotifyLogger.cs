using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using TelephoneStation.Abstract.Interfaces;

namespace TelephoneStation.Abstract.Implementations
{
    public class INotifyLogger : INotifyPropertyChanged, ILogger
    {
        private StringBuilder _textHolder = new StringBuilder();

        public event PropertyChangedEventHandler? PropertyChanged;

        private string _logMessage;
        public string LogMessage
        {
            get => _logMessage;
            set
            {
                _logMessage = value;
                NotifyPropertyChanged();
            }
        }

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Log(string message)
        {
            try
            {
               
                 _textHolder.AppendLine(message);
                 LogMessage = _textHolder.ToString();
                 
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }
        }
    }
}
