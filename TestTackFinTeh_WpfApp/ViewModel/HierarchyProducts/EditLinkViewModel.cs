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
    public class EditLinkViewModel : INotifyPropertyChanged
    {
        public EditLinkViewModel(Context context, Link link, WindowPartView partView)
        {
            _context = context;
            _partView = partView;
            _editingLink = link;

            ProductSelected = link.Product;
            Products = context.GetProductNotHaveLinks();
            Products.Add(link.Product);

            if (link.UpProduct == null)
                ParentProductSelected = ParentProducts[^1];
            else
                ParentProductSelected = link.UpProduct;

            
            GoToBackClick = new RelayCommand(GoToHierarchyProductsView);
            SaveClick = new RelayCommand(SaveProduct);
        }

        private Context _context;
        private WindowPartView _partView;
        private Link _editingLink;
        private string _countProducts = string.Empty;
        private string _ErrorText = string.Empty;
        private Product? _ProductSelected;
        private Product? _ParentProductSelected;
        public string CountProducts { get { return _countProducts; } set { _countProducts = value; OnPropertyChanged(nameof(CountProducts)); } }

        public bool EnableInputCountProducts { get; private set; }
        public List<Product> Products { get; }
        public List<Product?> ParentProducts
        {
            get
            {
                List<Product?> productsInDataBase = _context.GetProductHaveLinks()!;
                productsInDataBase.Remove(_editingLink!.Product);
                productsInDataBase.Add(new Product("Нет вышестоящего", 10));
                return productsInDataBase;
            }
        }
        public Product? ParentProductSelected
        {
            get
            {
                return _ParentProductSelected;
            }
            set
            {
                _ParentProductSelected = value;
                CountProducts = "1";
                EnableInputCountProducts = false;
                OnPropertyChanged(nameof(ParentProductSelected));
            }
        }
        public Product? ProductSelected
        {
            get
            {
                return _ProductSelected;
            }
            set
            {
                _ProductSelected = value;
                OnPropertyChanged(nameof(ProductSelected));
            }
        }
        public string ErrorText
        {
            get
            {
                return _ErrorText;
            }
            set
            {
                _ErrorText = value;
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
                if (ParentProductSelected == null)
                {
                    ErrorText = "Не выбран вышестоящий продукт";
                }
                else if (ProductSelected == null)
                    ErrorText = "Не выбран продукт";
                else if (string.IsNullOrEmpty(CountProducts))
                    ErrorText = "Не введено количество продукции";
                else if (ParentProductSelected.Name == "Нет вышестоящего" && CountProducts != "1")
                    ErrorText = "Количество продукции на верхнем уровне должно быть равным 1";
                else
                {
                    _editingLink = UpdateLinkPropettyWithNewData(_editingLink);

                    UpdateLink(_editingLink);
                    ErrorText = "Ссылка на продукт успешно изменена";
                }
            }
            catch (FormatException)
            {
                ErrorText = "введена некорректное количество продукта";
            }
            catch (ArgumentException ex)
            {
                ErrorText = ex.Message;
            }
        }
        private Link UpdateLinkPropettyWithNewData(Link oldLink)
        {
            if (ParentProductSelected!.Name == "Нет вышестоящего")
                oldLink.UpProduct = null;
            else
                oldLink.UpProduct = ParentProductSelected;
            oldLink.Product = ProductSelected!;
            oldLink.Count = Convert.ToInt32(CountProducts);
            return oldLink;
        }
        private void UpdateLink(Link link)
        {
            _context.UpdateLink(link);
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
