using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TelephoneStation.Abstract.Implementations;
using TelephoneStation.Abstract.Interfaces;

namespace TelephoneStation.Domain.Controllers
{
    public class CallsVM
    {
        public ILogger Logger { get; set; }
        public CallsVM(ILogger _logger)
        {
            Logger = _logger;
        }



        private RelayCommand addCall;
        public ICommand AddCall => addCall ??= new RelayCommand(PerformAddCall);
        private void PerformAddCall(object commandParameter)
        {

        }
    }
}
