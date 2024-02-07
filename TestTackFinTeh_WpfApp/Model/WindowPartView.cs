using System.Windows.Controls;

namespace TestTackFinTeh_WpfApp.Model
{
    public class WindowPartView
    {
        public WindowPartView(ContentControl contentControl)
        {
            _contentControl = contentControl;
        }
        private ContentControl _contentControl;

        public void LoadView(UserControl userControl)
        {
            _contentControl.Content = userControl;
        }
    }
}
