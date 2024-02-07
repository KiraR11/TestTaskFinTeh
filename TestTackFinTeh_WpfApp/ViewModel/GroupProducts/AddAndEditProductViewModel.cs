using GalaSoft.MvvmLight.Command;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Windows.Input;
using TestTackFinTeh_WpfApp.Model;
using TestTackFinTeh_WpfApp.View;
using TestTaskFinTeh_Data;
using TestTaskFinTeh_Model;

namespace TestTackFinTeh_WpfApp.ViewModel.GroupProducts
{
    public class AddAndEditProductViewModel : INotifyPropertyChanged
    {
        public AddAndEditProductViewModel(Context context, WindowPartView partView) 
        {
            _context = context;
            _partView = partView;

            TypeActionText = "Добавление продукта";
            GoToBackClick = new RelayCommand(GoToGroupProductView);
            SaveClick = new RelayCommand(SaveProduct);
        }
        public AddAndEditProductViewModel(Context context, WindowPartView partView, Product product)
        {
            _context = context;
            _partView = partView;

            TypeActionText = "Изменение продукта";
            GoToBackClick = new RelayCommand(GoToGroupProductView);
            SaveClick = new RelayCommand(SaveProduct);

            _editingProduct = product;
            InputName = product.Name;
            InputPrice = product.Price.ToString();
        }
        private WindowPartView _partView;
        private Context _context;
        private Product? _editingProduct;
        private string _errorText = string.Empty;
        public string TypeActionText { get; set; }
        public string? InputName { get; set; }
        public string? InputPrice { get; set; }
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

        private void GoToGroupProductView()
        {
            _partView.LoadView(new GroupProductsView(new GroupProductsViewModel(_context, _partView)));
        }
        private void SaveProduct()
        {
            try
            {
                if(string.IsNullOrEmpty(InputName))
                    ErrorText = "Не введено название продукта";
                else if(string.IsNullOrEmpty(InputPrice))
                    ErrorText = "Не введена цена продукта";
                else if (_editingProduct != null)
                {
                    _editingProduct.Price = Convert.ToDecimal(InputPrice);
                    _editingProduct.Name = InputName;
                    UpdateProduct(_editingProduct);
                    ErrorText = "Продукт успешно изменен";
                }
                else
                {
                    Product addtingProduct = new Product(InputName, Convert.ToDecimal(InputPrice));
                    InsertProduct(addtingProduct);
                    ErrorText = "Продукт успешно добавлен";
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
            catch (InvalidOperationException ex)
            {
                SaveProduct();
            }
            catch (DbUpdateException ex)
            {
                ErrorText = ex.Message;
            }
            catch (Exception)
            {
                ErrorText = "Не удалось произвести операцию";
            }
        }
        private void InsertProduct(Product product)
        {
            _context.AddProduct(product);
        }
        private void UpdateProduct(Product product)
        {
            _context.UpdateProduct(product);
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
