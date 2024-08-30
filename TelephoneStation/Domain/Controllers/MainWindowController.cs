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
using System.Windows;
using System.Windows.Input;
using TelephoneStation.Abstract.Implementations;
using TelephoneStation.Domain.Model;

namespace TelephoneStation.Domain.Controllers
{
    public class MainWindowController : INotifyPropertyChanged
    {
        public INotifyLogger LogConsole { get; set; } = new INotifyLogger();
        public event PropertyChangedEventHandler? PropertyChanged;

        private ConcurrentQueue<Call> _calls;
        public ConcurrentQueue<Call> Calls { get => _calls; set => SetProperty(ref _calls, value); }
        private ObservableCollection<Call> _callsFV;
        public ObservableCollection<Call> CallsFV { get => _callsFV; set => SetProperty(ref _callsFV, value); }
        public AgentListVM AgentsVM { get; set; }
        public MainWindowController()
        {

            Calls = new ConcurrentQueue<Call>();
            CallsFV = new ObservableCollection<Call>();


            AgentsVM = new AgentListVM(LogConsole);


            ThreadPool.QueueUserWorkItem(GenerateCalls);
            ThreadPool.QueueUserWorkItem(HandleCalls);

        }

        private RelayCommand addCall;
        public ICommand AddCall => addCall ??= new RelayCommand(PerformAddCall);
        private void PerformAddCall(object commandParameter)
        {
            var newCall = new Call { Id = Guid.NewGuid(), DurationInSec = RandomNumberGenerator.GetInt32(10, 15) };
            Application.Current.Dispatcher.Invoke(() => {
                CallsFV.Add(newCall);
            });
            Calls.Enqueue(newCall);
            LogConsole.Log($"Соеденение {newCall.Id} добавлено.");
     
        }

        private void GenerateCalls(object? state)
        {
            while (true)
            {
                PerformAddCall(state);
                Thread.Sleep(100000);

            }
        }
        private async void HandleCalls(object? state)
        {
            while (true)
            {
                foreach (var handler in AgentsVM.Agents)
                {
                    ThreadPool.QueueUserWorkItem(SimulateHandlerWork);
                }
                Thread.Sleep(3000);
            }
        }

        private void SimulateHandlerWork(object state)
        {
            var handler = AgentsVM.Agents.FirstOrDefault(x => !x.isBusy);
            if (handler != null)
            {
                lock (handler)
                {
                    if (Calls.TryDequeue(out var call))
                    {
                        Application.Current.Dispatcher.Invoke(() => {
                            CallsFV.Remove(call);
                        });
                        handler.isBusy = true;
                        LogConsole.Log($"Оператор {handler} началал работу со звонком номер \n {call}");
                        Thread.Sleep(call.DurationInSec * 1000);
                        LogConsole.Log($"Оператор {handler} закончил работу со звонком номер \n {call}");
                        handler.isBusy = false;
                        Thread.Sleep(2000);
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


    }
}
