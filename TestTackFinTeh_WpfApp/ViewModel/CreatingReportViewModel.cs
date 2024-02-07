using GalaSoft.MvvmLight.Command;
using System.ComponentModel;
using System.Windows.Input;
using TestTackFinTeh_WpfApp.Model;
using TestTaskFinTeh_Data;
using TestTaskFinTeh_Model;

namespace TestTackFinTeh_WpfApp.ViewModel
{
    public class CreatingReportViewModel : INotifyPropertyChanged
    {
        public CreatingReportViewModel(Context context) 
        {
            _context = context;

            List<Link> links = _context.GetAllLinks();
            LinkTree linkTree = new LinkTree(links);
            int maxLevel = linkTree.MaxLevelHierarchy;
            LevelsHierarchy = CreatingСonsecutiveNumbers(1, maxLevel);
            _linkTree = linkTree;



            CreatClick = new RelayCommand(CreatReport);
        }
        private Context _context;
        private LinkTree _linkTree;
        private string? _itemSelected;
        private bool _enableSaveButton = false;
        public List<string> LevelsHierarchy { get; set;}

        public string? ItemSelected 
        {
            get { return _itemSelected; }
            set
            {
                _itemSelected = value;
                EnableSaveButton = true;
            }
        }

        public bool EnableSaveButton { get { return _enableSaveButton; } set { _enableSaveButton = value; OnPropertyChanged(nameof(EnableSaveButton)); } }

        private int MaxLevelHierarchy { get { return Convert.ToInt32(ItemSelected); } }
        public ICommand CreatClick { get; }
        private void CreatReport()
        {
            HierarchicalReportGenerator reportGenerator = new HierarchicalReportGenerator(_linkTree, MaxLevelHierarchy);
            reportGenerator.Generate();
        }
        
        private List<string> CreatingСonsecutiveNumbers(int startNumber, int endNumber)
        {
            List<string> result = new List<string>();

            for (int i = startNumber;i <= endNumber; i++)
            {
                result.Add(Convert.ToString(i));
            }
            return result;
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
