using GalaSoft.MvvmLight.Command;
using System.ComponentModel;
using System.Windows.Input;
using TestTackFinTeh_WpfApp.Model;
using TestTaskFinTeh_Data;
using TestTaskFinTeh_Model;
using TestTaskFinTeh_WpfApp.View;
using TestTaskFinTeh_WpfApp.ViewModel;

namespace TestTackFinTeh_WpfApp.ViewModel.HierarchyProducts
{
    public class AddLinkViewModel : INotifyPropertyChanged
    {
        public AddLinkViewModel(Context context, WindowPartView partView, Product? upProduct) 
        {
            _context = context;
            _partView = partView;

            UpProduct = upProduct;
            ProductsForChice = context.GetProductNotHaveLinks();

            GoToBackClick = new RelayCommand(GoToHierarchyProductsView);
            SaveClick = new RelayCommand(SaveProduct);


            if (upProduct == null)
            {
                CountProducts = "1";
                EnableInputCountProducts = false;
            }
            else
                EnableInputCountProducts = true;
            if (ProductsForChice.Count == 0)
                ErrorText = "Нет продуктов на которые можно добавить ссылки";
        }
        
        private Context _context;
        private WindowPartView _partView;
        private string _countProducts = string.Empty;
        private string _errorText = string.Empty;
        private Product? _productSelected;
        private Product? UpProduct { get; set; }
        public string UpProductName 
        {
            get 
            {
                if (UpProduct == null)
                    return "Нет вышестоящего";
                else
                    return UpProduct.Name;
            }
        }
        public string CountProducts { get { return _countProducts; } set { _countProducts = value; } }
        public bool EnableInputCountProducts { get; set; }

        public List<Product> ProductsForChice { get; }
        
        public Product? ProductSelected
        {
            get
            {
                return _productSelected;
            }
            set
            {
                _productSelected = value;
                OnPropertyChanged(nameof(ProductSelected));
            }
        }
        public string ErrorText
        {
            get
            {
                return _errorText;
            }
            set
            {
                _errorText = value;
                OnPropertyChanged(nameof(ErrorText));
            }
        }
        public ICommand GoToBackClick { get; }
        public ICommand SaveClick { get; }

        private void GoToHierarchyProductsView()
        {
            _partView.LoadView(new HierarchyProductsView(new HierarchyProductsViewModel(_context, _partView)));
        }
        private void SaveProduct()
        {
            try
            {
                if (ProductSelected == null)
                {
                    ErrorText = "Не выбран тип продукции";
                }
                else if (string.IsNullOrEmpty(CountProducts))
                    ErrorText = "Не введено колоичество продукции";
                else
                {
                    Link link = new Link(UpProduct, ProductSelected, Convert.ToInt32(CountProducts));
                    InsertProduct(link);
                    ErrorText = "Ссылка на продукт успешно добавлена";
                }
            }
            catch (FormatException)
            {
                ErrorText = "введена некорректная цена продукта";
            }
            catch (ArgumentException ex)
            {
                ErrorText = ex.Message;
            }
        }
        private void InsertProduct(Link link)
        {
            _context.AddLink(link);
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

