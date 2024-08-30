using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TelephoneStation.Abstract.Implementations;
using TelephoneStation.Abstract.Interfaces;
using TelephoneStation.Domain.Model;

namespace TelephoneStation.Domain.Controllers
{
    public class CallsVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private ConcurrentQueue<Call> _calls;
        public ConcurrentQueue<Call> Calls { get => _calls; set => SetProperty(ref _calls, value); }

        public ILogger Logger { get; set; }
        public CallsVM(ILogger _logger)
        {
            Logger = _logger;
            Calls = new ConcurrentQueue<Call>();
           

            ThreadPool.QueueUserWorkItem(GenerateCalls);
            ThreadPool.QueueUserWorkItem(HandleCalls);
            
        }

        private void GenerateCalls(object? state)
        {
            while(true)
            {
                var newCall = new Call { Id = Guid.NewGuid(), DurationInSec = 1 * RandomNumberGenerator.GetInt32(1, 2) };
                Calls.Enqueue(newCall);
                Logger.Log($"Соеденение {newCall.Id} добавлено.");
                Thread.Sleep(10000 * RandomNumberGenerator.GetInt32(1, 2));
                
            }
        }
        private async void HandleCalls(object? state)
        {
            while (true)
            {
                foreach (var handler in AgentListVM.Agents)
                {
                    ThreadPool.QueueUserWorkItem(SimulateHandlerWork);
                }
                Thread.Sleep(1000);
            }
        }

        private void SimulateHandlerWork(object state)
        {
            var handler = AgentListVM.Agents.FirstOrDefault(x => !x.isBusy);
            if (handler != null) 
            {
                if (Calls.TryDequeue(out var call))
                {
                    lock (handler)
                    {
                        handler.isBusy = true;
                        Logger.Log($"Оператор {handler} началал работу со звонком номер \n {call}");
                        Thread.Sleep(call.DurationInSec * 1000);
                        Logger.Log($"Оператор {handler} закончил работу со звонком номер \n {call}");
                        handler.isBusy = false;
                        
                    }
                }
            }         
        }

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

        private RelayCommand addCall;
        public ICommand AddCall => addCall ??= new RelayCommand(PerformAddCall);
        private void PerformAddCall(object commandParameter)
        {
            var newCall = new Call { Id = Guid.NewGuid(), DurationInSec = 1000 * RandomNumberGenerator.GetInt32(1, 2) };
            Calls.Enqueue(newCall);
            Logger.Log($"Соеденение {newCall.Id} добавлено.");
           
        }
    }
}
