using TestTaskFinTeh_Model;
using System.ComponentModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using TestTaskFinTeh_WpfApp.View;
using System.Windows;
using TestTackFinTeh_WpfApp.ViewModel.HierarchyProducts;
using TestTackFinTeh_WpfApp.View.HierarchyProducts;
using TestTaskFinTeh_Data;
using TestTackFinTeh_WpfApp.Model;

namespace TestTaskFinTeh_WpfApp.ViewModel
{
    public class HierarchyProductsViewModel : INotifyPropertyChanged
    {
        public HierarchyProductsViewModel(Context context, WindowPartView partView)
        {
            _context = context;
            LinkTree linkTree = new LinkTree(context.GetAllLinks());
            IncomingProduct = linkTree.RootProduct;
            _partView = partView;
            _HistoryTransitionsidParentProduct = new List<Link>();

            ButtonAddClick = new RelayCommand(AddLink);
            ButtonEditClick = new RelayCommand(EditLink);
            ButtonRemoveClick = new RelayCommand(RemoveLink);
            ButtonGoToIncomingClick = new RelayCommand(GoToIncomingProducts);
            ButtonGoToHigherClick = new RelayCommand(GoToHigherProduct);
        }
        private HierarchyProductsViewModel(Context context, WindowPartView windowPartView, List<Link> historyTransitionsidParentProduct)
        {
            _context = context;
            LinkTree linkTree = new LinkTree(_context.GetAllLinks(), historyTransitionsidParentProduct[^1].Product!);
            IncomingProduct = linkTree.RootProduct;
            _partView = windowPartView;
            _HistoryTransitionsidParentProduct = historyTransitionsidParentProduct;

            ButtonAddClick = new RelayCommand(AddLink);
            ButtonEditClick = new RelayCommand(EditLink);
            ButtonRemoveClick = new RelayCommand(RemoveLink);
            ButtonGoToIncomingClick = new RelayCommand(GoToIncomingProducts);
            ButtonGoToHigherClick = new RelayCommand(GoToHigherProduct);
        }
        private HierarchyProductsViewModel(Context context, List<LinkNode> linksNode, WindowPartView windowPartView, List<Link> historyTransitionsidParentProduct)
        {   
            _context = context;
            IncomingProduct = linksNode;
            _partView = windowPartView;
            _HistoryTransitionsidParentProduct = historyTransitionsidParentProduct;

            
            
        }
        private WindowPartView _partView;
        private List<Link> _HistoryTransitionsidParentProduct;
        private Context _context;

        public List<LinkNode> IncomingProduct { get; }
        private LinkNode? _itemSelected = null;
        public LinkNode? ItemSelected
        {
            get
            {
                return _itemSelected;
            }
            set 
            {
                _itemSelected = value;
                OnPropertyChanged("isEnabledButtonGoToIncoming");
                OnPropertyChanged("isEnabledButtonRemove");
                OnPropertyChanged("isEnabledButtonEdit");
                OnPropertyChanged("isEnabledButtonGoToHigher");
            }
        }
        public bool isEnabledButtonGoToIncoming { get { return ItemSelected != null; } }
        public bool isEnabledButtonRemove { get { return ItemSelected != null; } }
        public bool isEnabledButtonEdit { get { return ItemSelected != null; } }
        public bool isEnabledButtonGoToHigher { get { return _HistoryTransitionsidParentProduct.Count != 0; } }

        public string HierarchyLevel { get { return $"Уровень иерархии: {_HistoryTransitionsidParentProduct.Count + 1}"; } }

        public ICommand ButtonAddClick { get; }
        public ICommand ButtonEditClick { get; }
        public ICommand ButtonRemoveClick { get; }
        public ICommand ButtonGoToIncomingClick { get; } 
        public ICommand ButtonGoToHigherClick { get; } 
        private void AddLink()
        {
            if(_HistoryTransitionsidParentProduct.Count != 0)
                _partView.LoadView(new AddLinkView(new AddLinkViewModel(_context, _partView, _HistoryTransitionsidParentProduct[^1].Product)));
            else
                _partView.LoadView(new AddLinkView(new AddLinkViewModel(_context, _partView, null)));
        }
        private void EditLink()
        {
            Link link = ItemSelected!.Link;
            _partView.LoadView(new EditLinkView(new EditLinkViewModel(_context, link, _partView)));
        }
        private void RemoveLink()
        {
            MessageBoxResult result = DeletionConfirmation();
            if (result == MessageBoxResult.Yes)
            {
                _context.DeleteLink(ItemSelected!.Link);
                UpdateView();
            }
        }
        private MessageBoxResult DeletionConfirmation()
        {
            string messageBoxText = "Вы действительно хотите удалить данную ссылку?\n Это может привести к удалению связанных ссылок.";
            string caption = "Удаление ссылки";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Question;
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.No);
            return result;
        }
        private void UpdateView()
        {
             _partView.LoadView(new HierarchyProductsView(new HierarchyProductsViewModel(_context, _partView)));
        }
        private void GoToIncomingProducts()
        {
            _HistoryTransitionsidParentProduct.Add(ItemSelected!.Link);

            _partView.LoadView(new HierarchyProductsView(new HierarchyProductsViewModel(_context, _partView, _HistoryTransitionsidParentProduct)));
        }
        private void GoToHigherProduct()
        {
            List<LinkNode> higherLinkNodes = GetHigherLinkNodes();
            _HistoryTransitionsidParentProduct.RemoveAt(_HistoryTransitionsidParentProduct.Count - 1);

            _partView.LoadView(new HierarchyProductsView(new HierarchyProductsViewModel(_context, higherLinkNodes, _partView, _HistoryTransitionsidParentProduct)));
        }
        private List<LinkNode> GetHigherLinkNodes()
        {
            List<Link> links = _context.GetAllLinks();

            Link SelectedLink = _HistoryTransitionsidParentProduct[^1];
            LinkTree productTree;
            if (SelectedLink.UpProduct != null)
                productTree = new LinkTree(links, SelectedLink.UpProduct);
            else
                productTree = new LinkTree(links);
            return productTree.RootProduct;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
