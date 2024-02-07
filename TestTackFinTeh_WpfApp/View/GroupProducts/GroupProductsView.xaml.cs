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
using TestTackFinTeh_WpfApp.ViewModel;

namespace TestTackFinTeh_WpfApp.View
{
    /// <summary>
    /// Логика взаимодействия для GroupProductView.xaml
    /// </summary>
    public partial class GroupProductsView : UserControl
    {
        public GroupProductsView(GroupProductsViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
