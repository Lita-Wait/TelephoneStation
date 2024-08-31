using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TelephoneStation.Abstract.Implementations;
using TelephoneStation.Abstract.Interfaces;
using TelephoneStation.Domain.Model;
using TelephoneStation.Domain.View;

namespace TelephoneStation.Domain.Controllers
{
    public class CallsListVM : INotifyPropertyChanged
    {
        private ILogger Logger;
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<Call> Calls { get; set; }
        public ConcurrentQueue<Call> CallsQueue { get; set; }
        public CallsListVM(ILogger _logger) 
        {
            Logger = _logger;
            CallsQueue = new ConcurrentQueue<Call>();
            Calls = new ObservableCollection<Call>();
        }

        private RelayCommand addCall;
        public ICommand AddCall => addCall ??= new RelayCommand(PerformAddCall);
        public void PerformAddCall(object? commandParameter)
        {
            var newCall = new Call { Id = Guid.NewGuid(), DurationInSec = RandomNumberGenerator.GetInt32(10, 15) }; //10-15 длительность звонка
            Application.Current.Dispatcher.Invoke(() => {
                Calls.Add(newCall);
            });
            CallsQueue.Enqueue(newCall);
            Logger.Log($"Соеденение {newCall.Id} добавлено.");

        }
    }
}
