using GalaSoft.MvvmLight.Command;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using TestTaskFinTeh_Model;
using TestTackFinTeh_WpfApp.View;
using TestTackFinTeh_WpfApp.ViewModel.GroupProducts;
using TestTackFinTeh_WpfApp.View.GroupProducts;
using TestTaskFinTeh_Data;
using TestTackFinTeh_WpfApp.Model;

namespace TestTackFinTeh_WpfApp.ViewModel
{
    public class GroupProductsViewModel : INotifyPropertyChanged
    {
        public GroupProductsViewModel(Context context, WindowPartView partView)
        {
            _context = context;
            _partView = partView;
            Products = _context.GetAllProducts();

            ButtonAddClick = new RelayCommand(GoToAddProductView);
            ButtonEditClick = new RelayCommand(GoToEditProductView);
            ButtonRemoveClick = new RelayCommand(RemoveProduct);
        }
        private WindowPartView _partView;
        private Context _context;
        public List<Product> Products { get; }
        
        private Product? _itemSelected = null;
        public Product? ItemSelected
        {
            get
            {
                return _itemSelected;
            }
            set
            {
                _itemSelected = value;
                OnPropertyChanged("isEnabledButtonRemove");
                OnPropertyChanged("isEnabledButtonEdit");
            }
        }
        public bool isEnabledButtonRemove { get { return ItemSelected != null; } }
        public bool isEnabledButtonEdit { get { return ItemSelected != null; } }
        public ICommand ButtonAddClick { get; }
        public ICommand ButtonEditClick { get; }
        public ICommand ButtonRemoveClick { get; }
        private void GoToAddProductView()
        {
            _partView.LoadView(new AddAndEditProductView(new AddAndEditProductViewModel(_context, _partView)));
        }
        private void GoToEditProductView()
        {
            _partView.LoadView(new AddAndEditProductView(new AddAndEditProductViewModel(_context, _partView, ItemSelected!)));
        }
        private void RemoveProduct()
        {
            MessageBoxResult result = DeletionConfirmation();
            if (result == MessageBoxResult.Yes && _context != null)
            {
                _context.DeleteProduct(ItemSelected!);
                UpdateView();
            }
        }
        private MessageBoxResult DeletionConfirmation()
        {
            string messageBoxText = "Вы действительно хотите удалить данный тип продукта?\n Это может привести к удалению связанных ссылок.";
            string caption = "Удаление типа продукта";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Question;
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.No);
            return result;
        }
        private void UpdateView()
        {
            if (_context != null)
            {
                _partView.LoadView(new GroupProductsView(new GroupProductsViewModel(_context, _partView)));
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
