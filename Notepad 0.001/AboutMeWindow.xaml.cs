using System.Windows;
using System.Windows.Input;

namespace Notepad_0._001
{
    public partial class AboutMeWindow : Window
    {
        public AboutMeWindow()
        {
            InitializeComponent();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
