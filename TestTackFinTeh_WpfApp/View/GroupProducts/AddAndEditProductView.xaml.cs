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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TestTackFinTeh_WpfApp.ViewModel.GroupProducts;

namespace TestTackFinTeh_WpfApp.View.GroupProducts
{
    /// <summary>
    /// Логика взаимодействия для AddProductView.xaml
    /// </summary>
    public partial class AddAndEditProductView : UserControl
    {
        public AddAndEditProductView(AddAndEditProductViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
