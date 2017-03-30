using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;
using System.IO;
using System.ComponentModel;

namespace Notepad_0._001
{
    public partial class MainWindow : Window
    {
        int count = 1;
        public MainWindow()
        {
            InitializeComponent();
        }


        private void New_Command(object sender, RoutedEventArgs e)
        {
 
            var newTextBox = new TextBox()
            {
                AcceptsReturn = true,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                TextWrapping = TextWrapping.Wrap,
                Name = "NewTextBox" + count,
                FontFamily = new FontFamily("Consolas"),
                FontSize = 11 * 96.0 / 72.0
            };

            var newTab = new TabItem()
            {
                Header = "NewTextDocument" + count,
                Content = newTextBox
            };
            count++;
            TextTab.SelectedItem = newTab;
            TextTab.Items.Add(newTab);
        }

        private void Open_Command(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                Filter = "Text Files(*.txt)|*.txt|All(*.*)|*"
            };
            if (dialog.ShowDialog() == true)
            {
                var newTextBox = new TextBox()
                {
                    AcceptsReturn = true,
                    VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                    HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                    TextWrapping = TextWrapping.Wrap,
                    Name = "NewTextBox" + count,
                    FontFamily = new FontFamily("Consolas"),
                    FontSize = 11 * 96.0 / 72.0
                };
                newTextBox.Text = File.ReadAllText(dialog.FileName);

                var newTab = new TabItem()
                {
                    Header = System.IO.Path.GetFileName(dialog.FileName),
                    Content = newTextBox
                };

                TextTab.SelectedItem = newTab;
                TextTab.Items.Add(newTab);
            }
        }


        public String getSelectedText()
        {
            var currentText = TextTab.SelectedContent as TextBox;
            return currentText.Text;
        }

        private void Save_as_Command(object sender, RoutedEventArgs e)
        {
            if (TextTab.SelectedIndex == -1)
            {
                MessageBox.Show("Open a text document first.", "Error at saving.", MessageBoxButton.OK, MessageBoxImage.Error);
                return;

            }
            var dialog = new SaveFileDialog()
            {
                Filter = "Text Files(*.txt)|*.txt|All(*.*)|*"
            };

            if (dialog.ShowDialog() == true)
            {
                File.WriteAllText(dialog.FileName, getSelectedText());
            }
        }

        private void Find_Command(object sender, RoutedEventArgs e)
        {
            if (TextTab.SelectedIndex == -1)
            {
                MessageBox.Show("Open a text document first.", "Error at find command.", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var findWindow = new FindWindow();
            if(!findWindow.IsVisible)
                findWindow.Show();

        }


        private void Cut_Command(object sender, RoutedEventArgs e)
        {
            var currentText = TextTab.SelectedContent as TextBox;
            try
            {
                Clipboard.SetText(currentText.SelectedText);
                currentText.SelectedText = "";
            }
            catch (Exception)
            {
                MessageBoxResult warning = MessageBox.Show("Try again after selecting at least one character.", "Error at cutting", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Copy_Command(object sender, RoutedEventArgs e)
        {
            var currentText = TextTab.SelectedContent as TextBox;
            try
            {
                Clipboard.SetText(currentText.SelectedText);
            }
            catch (Exception)
            {
                MessageBoxResult warning = MessageBox.Show("Try again after selecting at least one character.", "Error at copying", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Paste_Command(object sender, RoutedEventArgs e)
        {
            try
            {
                var currentText = TextTab.SelectedContent as TextBox;
                currentText.SelectedText = Clipboard.GetText();
                currentText.CaretIndex = currentText.CaretIndex + Clipboard.GetText().Length;
            }catch(Exception)
            {
                MessageBox.Show("Try again after selecting a valid place to paste text into.", "Error at pasting", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Font(object sender, RoutedEventArgs e)
        {
            if(TextTab.SelectedIndex == -1)
            {
                MessageBox.Show("Open a text document first.", "Error at formating.", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                System.Windows.Forms.FontDialog fd = new System.Windows.Forms.FontDialog();
                var currentText = TextTab.SelectedContent as TextBox;
                fd.Font = new System.Drawing.Font("Consolas", 11);
                var result = fd.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    currentText.FontFamily = new FontFamily(fd.Font.Name);
                    currentText.FontSize = fd.Font.Size * 96.0 / 72.0;
                    currentText.FontWeight = fd.Font.Bold ? FontWeights.Bold : FontWeights.Regular;
                    currentText.FontStyle = fd.Font.Italic ? FontStyles.Italic : FontStyles.Normal;

                    var tdc = new TextDecorationCollection();
                    if (fd.Font.Underline) tdc.Add(TextDecorations.Underline);
                    if (fd.Font.Strikeout) tdc.Add(TextDecorations.Strikethrough);
                    currentText.TextDecorations = tdc;
                }
            }
            catch (Exception e1)
            {
                MessageBox.Show("Error trace: " + e1.Message, "Error at formating.", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

        }

        private void Wrap(object sender, RoutedEventArgs e)
        {
            if (TextTab.SelectedIndex == -1)
            {
                MessageBox.Show("Open a text document first.", "Error at formating.", MessageBoxButton.OK, MessageBoxImage.Error);
                WrapCheckBox.IsChecked = !WrapCheckBox.IsChecked;
                return;

            }
            try
            {
                var currentText = TextTab.SelectedContent as TextBox;
                currentText.Focus();
                if (currentText.TextWrapping == TextWrapping.Wrap)
                {
                    currentText.TextWrapping = TextWrapping.NoWrap;
                }
                else
                {
                    currentText.TextWrapping = TextWrapping.Wrap;
                }
            }
            catch(Exception e1)
            {
                MessageBox.Show("Error trace: "+e1.Message, "Error at formating.", MessageBoxButton.OK, MessageBoxImage.Error);
                WrapCheckBox.IsChecked = !WrapCheckBox.IsChecked;
                return;
            }
        }

        private void Save_Command(object sender, RoutedEventArgs e)
        {

        }

        private void Undo_Command(object sender, RoutedEventArgs e)
        {
            try
            {
                var currentText = TextTab.SelectedContent as TextBox;
                currentText.Undo();
            }
            catch (Exception e1)
            {
                MessageBox.Show("Error trace: " + e1.Message, "Error at undo.", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void Redo_Command(object sender, RoutedEventArgs e)
        {
            try
            {
                var currentText = TextTab.SelectedContent as TextBox;
                currentText.Redo();
            }
            catch (Exception e1)
            {
                MessageBox.Show("Error trace: " + e1.Message, "Error at redo.", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void Close_tab_Command(object sender, RoutedEventArgs e)
        {
            try
            {
                TextTab.Items.RemoveAt(TextTab.SelectedIndex);
            }
            catch (Exception)
            {
                Application.Current.Shutdown();
            }

        }

        private void To_Upper(object sender, RoutedEventArgs e)
        {
            try
            {
                TextBox currentText = TextTab.SelectedContent as TextBox;
                currentText.SelectedText= currentText.SelectedText.ToUpper();
            }catch(Exception e1)
            {
                MessageBox.Show(e1.Message, "Error parsing text.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void To_Lower(object sender, RoutedEventArgs e)
        {
            try
            {
                TextBox currentText = TextTab.SelectedContent as TextBox;
                currentText.SelectedText= currentText.SelectedText.ToLower();
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message, "Error parsing text.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Trim(object sender, RoutedEventArgs e)
        {
            try
            {
                TextBox currentText = TextTab.SelectedContent as TextBox;
                currentText.Text = System.Text.RegularExpressions.Regex.Replace(currentText.Text, @"^\s*$(\n|\r|\r\n)", "", System.Text.RegularExpressions.RegexOptions.Multiline); //dark magic
                currentText.Text = currentText.Text.Trim();
            }catch(Exception e1)
            {
                MessageBox.Show(e1.Message, "Error parsing text", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void About(object sender, RoutedEventArgs e)
        {
            AboutMeWindow aboutMeWindow = new AboutMeWindow();
            aboutMeWindow.Show();
        }
        
        private void Go_To_Line(object sender, RoutedEventArgs e)
        {
            if (TextTab.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a valid text document first.", "Error at request", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
                TextBox currentText = TextTab.SelectedContent as TextBox;
                GoToLineDialog goToLineDialog = new GoToLineDialog();
                goToLineDialog.Show();
        }

        private void On_Close(object sender, CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}