using System;
using System.Windows;
using System.Windows.Controls;

namespace Notepad_0._001
{
    public partial class FindWindow : Window
    {
        public FindWindow()
        {
            InitializeComponent();
            ToSearchFor.Focus();
        }
        TextBox currentText = ((MainWindow)Application.Current.MainWindow).TextTab.SelectedContent as TextBox;

        int lastCaret = -1;
        int changedCaret = -1;
        int position;
        private void Find_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (changedCaret != currentText.CaretIndex)
                {
                    changedCaret = currentText.CaretIndex;
                    lastCaret = -1;
                }
                if (lastCaret == -1)
                {
                    if (MatchCase.IsChecked == false)
                    {
                        position = currentText.Text.IndexOf(ToSearchFor.Text, currentText.CaretIndex);
                    }else
                        position = currentText.Text.ToLower().IndexOf(ToSearchFor.Text.ToLower(), currentText.CaretIndex);
                    lastCaret = currentText.CaretIndex + ToSearchFor.Text.Length;
                }
                else
                {
                    if (MatchCase.IsChecked == false)
                    {
                        position = currentText.Text.IndexOf(ToSearchFor.Text, lastCaret);
                    }
                    else
                        position = currentText.Text.ToLower().IndexOf(ToSearchFor.Text.ToLower(), lastCaret);
                    lastCaret = lastCaret + 2 * ToSearchFor.Text.Length;
                }
                currentText.Focus();
                currentText.Select(position, ToSearchFor.Text.Length);
            }
            catch(Exception)
            {
                MessageBox.Show("No results found!", "EOF Reached", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            ReplaceButton.IsEnabled = true;
            ToReplace.IsEnabled = true;
        }


        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Replace(object sender, RoutedEventArgs e)
        {
            currentText.SelectedText = ToReplace.Text;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
