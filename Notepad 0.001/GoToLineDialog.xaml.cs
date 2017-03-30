using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.RegularExpressions;
namespace Notepad_0._001
{
    public partial class GoToLineDialog : Window
    {
        public GoToLineDialog()
        {
            InitializeComponent();
        }

        private void NumberValidation(object sender, TextCompositionEventArgs e)
        {
            /*
            *   The Event handler is preview text input. Here regular expression 
            *   matches the text input only if it is not a number, then it is not made 
            *   to entry textbox. If you want only alphabets then replace the regular 
            *   expression as [^a-zA-Z].
            */
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void GoToLine(object sender, RoutedEventArgs e)
        {
            try
            {
                var currentText = ((MainWindow)Application.Current.MainWindow).TextTab.SelectedContent as TextBox;
                var lineNumber = Int32.Parse( TextBox1.Text);
                currentText.SelectionStart = currentText.GetCharacterIndexFromLineIndex(lineNumber - 1);
                currentText.SelectionLength = currentText.GetLineLength(lineNumber - 1);
                currentText.CaretIndex = currentText.SelectionStart;
                currentText.ScrollToLine(lineNumber - 1);
                currentText.Focus();
            }catch(Exception e1)
            {
                MessageBox.Show(e1.Message, "Error parsing text", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
