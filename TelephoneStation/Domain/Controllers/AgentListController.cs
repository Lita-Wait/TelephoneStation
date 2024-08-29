using System;
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
    public class AgentListController : INotifyPropertyChanged
    {
        private ILogger _logger;
        public event PropertyChangedEventHandler? PropertyChanged;

        private ObservableCollection<Agent> _agents;
        public ObservableCollection<Agent> Agents { get => _agents; set => SetProperty(ref _agents, value); }

        private string _newAgentName;
        public string NewAgentName { get => _newAgentName; set => SetProperty(ref _newAgentName, value); }

        private Agent _selectedAgent;
        public Agent SelectedAgent { get => _selectedAgent; set => SetProperty(ref _selectedAgent, value); }

        public AgentListController(ILogger logger)
        {
            _logger = logger;

            Agents =
            [
                new Agent() { Name = "Андрей" },
                new Agent() { Name = "Виталий" },
                new Agent() { Name = "Алексей" },
            ];
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

        private RelayCommand removeSelectedAgent;
        public ICommand RemoveSelectedAgent => removeSelectedAgent ??= new RelayCommand(execute: PerformRemoveSelectedAgent, canExecute: CanRemoveAgent);
        private void PerformRemoveSelectedAgent(object commandParameter)
        {
            var name = SelectedAgent.Name;
            Agents.Remove(SelectedAgent);
            _logger.Log($"Оператор {name} удален.");
            //SelectedAgent = null;
        }
        private bool CanRemoveAgent(object commandParameter)
        {
            return SelectedAgent != null;
        }


        private RelayCommand addAgent;
        public ICommand AddAgent => addAgent ??= new RelayCommand(PerformAddAgent, CanAddAgent);
        private void PerformAddAgent(object commandParameter)
        {
            Agents.Add(new Agent { Name = NewAgentName });
            _logger.Log($"Оператор {NewAgentName} добавлен.");
            NewAgentName = string.Empty;
        }
        private bool CanAddAgent(object commandParameter)
        {
            return !string.IsNullOrEmpty(NewAgentName);
        }
    }
}
