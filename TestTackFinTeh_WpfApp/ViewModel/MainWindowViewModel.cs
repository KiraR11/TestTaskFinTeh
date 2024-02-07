using GalaSoft.MvvmLight.Command;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TestTackFinTeh_WpfApp.Model;
using TestTackFinTeh_WpfApp.View;
using TestTaskFinTeh_Data;
using TestTaskFinTeh_WpfApp.View;
using TestTaskFinTeh_WpfApp.ViewModel;

namespace TestTackFinTeh_WpfApp.ViewModel
{
    public class MainWindowViewModel
    {
        public MainWindowViewModel(WindowPartView partView, Action CloseAction) 
        {
            try
            {
                _context = new Context();
                _partView = partView;

                ButtonProductClick = new RelayCommand(GoToProductView);
                ButtonHierarchyProductClick = new RelayCommand(GoToHierarchyProductView);
                ButtonReportClick = new RelayCommand(GoToReportView);
            }
            catch(DbUpdateException ex) 
            {
                MessageBox.Show($"{ex.Message}", "Ошибка соедининия с сервером");
                CloseAction();
            }
        }
        private WindowPartView _partView;

        public UserControl Content { get {return new BootView(); } }
        private Context? _context;
        public ICommand ButtonProductClick { get; }
        public ICommand ButtonHierarchyProductClick { get; }
        public ICommand ButtonReportClick { get; }
        private void GoToProductView()
        {
            if(_context != null)
            {
                _partView.LoadView(new GroupProductsView(new GroupProductsViewModel(_context, _partView)));
            }
        }
        private void GoToHierarchyProductView()
        {
            if(_context != null)
            {
                _partView.LoadView(new HierarchyProductsView(new HierarchyProductsViewModel(_context, _partView)));
            }
            
        }
        private void GoToReportView()
        {
            if (_context != null)
            {
                _partView.LoadView(new CreatingReportView(new CreatingReportViewModel(_context)));
            }
        }
    }
}
