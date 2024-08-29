using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TelephoneStation.Abstract.Implementations;
using TelephoneStation.Domain.Model;

namespace TelephoneStation.Domain.Controllers
{
    public class MainWindowController
    {
        public INotifyLogger LogConsole { get; set; } = new INotifyLogger();
        public AgentListController AgentsVM { get; set; }
        public MainWindowController()
        {
            AgentsVM = new AgentListController(LogConsole);

        }

        private RelayCommand addTextToConsole;
        public ICommand AddTextToConsole => addTextToConsole ??= new RelayCommand(execute: PerformAddTextToConsole, canExecute: CanAddTextToConsole);
        private void PerformAddTextToConsole(object commandParameter)
        {
            LogConsole.Log("1231231231");
        }
        private bool CanAddTextToConsole(object parameter)
        {
            return true;
        }

    }
}
