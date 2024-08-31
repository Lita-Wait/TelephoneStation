using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TelephoneStation.Abstract.Implementations;
using TelephoneStation.Abstract.Interfaces;
using TelephoneStation.Domain.Model;

namespace TelephoneStation.Domain.Controllers
{
    public class AgentListVM : INotifyPropertyChanged
    {
        private ILogger Logger;
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<Agent> Agents {  get; set; }
        public string NewAgentName { get; set; }
        public Agent SelectedAgent { get; set; }


        public AgentListVM(ILogger _logger)
        {
            Logger = _logger;

            Agents =
            [
                new Agent() { Name = "Андрей" },
                new Agent() { Name = "Виталий" },
                new Agent() { Name = "Алексей" },
                new Agent() { Name = "Наталья" },
                new Agent() { Name = "Иван" }
            ];
        }

        private RelayCommand removeSelectedAgent;
        public ICommand RemoveSelectedAgent => removeSelectedAgent ??= new RelayCommand(execute: PerformRemoveSelectedAgent, canExecute: CanRemoveAgent);
        private void PerformRemoveSelectedAgent(object commandParameter)
        {
            var name = SelectedAgent.Name;
            Agents.Remove(SelectedAgent);
            Logger.Log($"Оператор {name} удален.");
        }
        private bool CanRemoveAgent(object commandParameter)
        {
            return SelectedAgent != null && SelectedAgent.isBusy == false ;
        }


        private RelayCommand addAgent;
        public ICommand AddAgent => addAgent ??= new RelayCommand(PerformAddAgent, CanAddAgent);
        private void PerformAddAgent(object commandParameter)
        {
            Agents.Add(new Agent { Name = NewAgentName });
            Logger.Log($"Оператор {NewAgentName} добавлен.");
            NewAgentName = string.Empty;
        }
        private bool CanAddAgent(object commandParameter)
        {
            return !string.IsNullOrEmpty(NewAgentName);
        }
    }
}
