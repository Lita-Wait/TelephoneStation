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
using TelephoneStation.Abstract.Interfaces;
using TelephoneStation.Domain.Model;
using TelephoneStation.Domain.View;

namespace TelephoneStation.Domain.Controllers
{
    public class MainWindowController : INotifyPropertyChanged
    {
        public ILogger LogConsole { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public AgentListVM AgentsVM { get; set; }
        public CallsListVM CallListVM { get; set; }

        public MainWindowController()
        {
            LogConsole = new INotifyLogger();

            AgentsVM = new AgentListVM(LogConsole);
            CallListVM = new CallsListVM(LogConsole);


            ThreadPool.QueueUserWorkItem(GenerateCalls);
            ThreadPool.QueueUserWorkItem(HandleCalls);

        }

        private void GenerateCalls(object? state)
        {
            while (true)
            {
                CallListVM.PerformAddCall(state);
                //Задержка добавление рнд звонка
                Thread.Sleep(10000);

            }
        }
        private void HandleCalls(object? state)
        {
            while (true)
            {
                foreach (var handler in AgentsVM.Agents)
                {
                    ThreadPool.QueueUserWorkItem(SimulateHandlerWork);  
                }
                //Задержка для проверки свободных операторов
                Thread.Sleep(3000);
            }
        }

        private void SimulateHandlerWork(object? state)
        {
            var handler = AgentsVM.Agents.FirstOrDefault(x => !x.isBusy);
            if (handler != null)
            {
                lock (handler)
                {
                    if (CallListVM.CallsQueue.TryDequeue(out var call))
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            CallListVM.Calls.Remove(call);
                        });
                        handler.isBusy = true;
                        LogConsole.Log($"Оператор {handler} началал работу со звонком номер \n {call}");
                        Thread.Sleep(call.DurationInSec * 1000);
                        LogConsole.Log($"Оператор {handler} закончил работу со звонком номер \n {call}");
                        handler.isBusy = false;
                        //Задержка потока для наглядности индикаторов
                        Thread.Sleep(2000);
                    }
                }
            }
        }
    }
}
