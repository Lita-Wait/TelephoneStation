using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using TelephoneStation.Abstract.Interfaces;

namespace TelephoneStation.Abstract.Implementations
{
    public class INotifyLogger : INotifyPropertyChanged, ILogger
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private StringBuilder _textHolder = new StringBuilder();
        public string? LogMessage { get; set; }

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
