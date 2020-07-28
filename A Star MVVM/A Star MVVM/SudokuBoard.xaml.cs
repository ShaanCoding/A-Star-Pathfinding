using A_Star_MVVM.Model.Enums;
using A_Star_MVVM.Model.Structures;
using A_Star_MVVM.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace A_Star_MVVM
{
    /// <summary>
    /// Interaction logic for SudokuBoard.xaml
    /// </summary>
    public partial class SudokuBoard : UserControl
    {
        private readonly ViewModelExample viewModel = new ViewModelExample();

        public SudokuBoard()
        {
            InitializeComponent();
            MainList.DataContext = viewModel;
        }
    }
}
