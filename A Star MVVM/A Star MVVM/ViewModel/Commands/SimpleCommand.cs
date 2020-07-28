using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace A_Star_MVVM.ViewModel.Commands
{
    public class SimpleCommand : ICommand
    {
        public ViewModelExample viewModel { get; set; }

        public SimpleCommand(ViewModelExample viewModel)
        {
            this.viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this.viewModel.ExecuteProgram();
        }
    }
}
