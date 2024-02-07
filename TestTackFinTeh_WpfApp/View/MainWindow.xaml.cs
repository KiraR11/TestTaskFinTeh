using System.Text;
using System.Windows;
using TestTaskFinTeh_WpfApp.View;
using TestTaskFinTeh_WpfApp.ViewModel;
using TestTaskFinTeh_Model;
using TestTackFinTeh_WpfApp.ViewModel;
using TestTackFinTeh_WpfApp.Model;

namespace TestTaskFinTeh_WpfApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            WindowPartView partView = new WindowPartView(ContentControl);
            DataContext = new MainWindowViewModel(partView, new Action(() => this.Close()));
        }
    }
}