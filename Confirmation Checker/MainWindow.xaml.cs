using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;

namespace Confirmation_Checker
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isEnglish = false;
        public MainWindow()
        {
            InitializeComponent();

            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\ConfirmationChecker\Confirmed";
            FileInfo fileInfo = new FileInfo(path);
            if (fileInfo.Exists)
            {
                Application.Current.Shutdown();
            }

            var titleText = this.FindName("TitleText") as TextBlock;
            if (titleText == null) return;
            var notesText = this.FindName("NotesText") as TextBlock;
            if (notesText == null) return;
            var confirmButton = this.FindName("ConfirmButton") as Button;
            if (confirmButton == null) return;
            var languageSwitch = this.FindName("LanguageSwitch") as Button;
            if (languageSwitch == null) return;

            if (ConfigurationManager.AppSettings["can_change_language"] == "false")
            {
                languageSwitch.Visibility = Visibility.Hidden;
            }

            titleText.Text = ConfigurationManager.AppSettings["title_ja"];
            notesText.Text = ConfigurationManager.AppSettings["notes_ja"];
            confirmButton.Content = ConfigurationManager.AppSettings["confirmMessage_ja"];
            languageSwitch.Content = "English";

            Activated += (s, e) => {
                var scrollViewer = this.FindName("ScrollViewer") as ScrollViewer;
                if (scrollViewer == null) return;
                Debug.Print(scrollViewer.ScrollableHeight + "");
                if (scrollViewer.ScrollableHeight > 0)
                {
                    confirmButton.IsEnabled = false;
                }
            };

            this.WindowStyle = WindowStyle.None;
            this.WindowState = WindowState.Maximized;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\ConfirmationChecker\Confirmed";
            FileInfo fileInfo = new FileInfo(path);
            if (fileInfo.Directory == null) return;
            if (!fileInfo.Directory.Exists)
            {
                fileInfo.Directory.Create();
            }
            FileStream fileStream = fileInfo.Create();
            Application.Current.Shutdown();
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var scrollViewer = (ScrollViewer)sender;
            if (e.VerticalOffset / scrollViewer.ScrollableHeight > .999f)
            {
                var confirmButton = this.FindName("ConfirmButton") as Button;
                if (confirmButton == null) return;
                confirmButton.IsEnabled = true;
            }
        }

        private void LanguageSwitch_Click(object sender, RoutedEventArgs e)
        {
            isEnglish = !isEnglish;
            var titleText = this.FindName("TitleText") as TextBlock;
            if (titleText == null) return;
            var notesText = this.FindName("NotesText") as TextBlock;
            if (notesText == null) return;
            var confirmButton = this.FindName("ConfirmButton") as Button;
            if (confirmButton == null) return;
            var languageSwitch = this.FindName("LanguageSwitch") as Button;
            if (languageSwitch == null) return;

            var scrollViewer = this.FindName("ScrollViewer") as ScrollViewer;
            if (scrollViewer == null) return;

            if (scrollViewer.ScrollableHeight > 0)
            {
                confirmButton.IsEnabled = false;
            }

            scrollViewer.ScrollToTop();

            if (isEnglish)
            {
                titleText.Text = ConfigurationManager.AppSettings["title_en"];
                notesText.Text = ConfigurationManager.AppSettings["notes_en"];
                confirmButton.Content = ConfigurationManager.AppSettings["confirmMessage_en"];
                languageSwitch.Content = "日本語";
            }
            else
            {
                titleText.Text = ConfigurationManager.AppSettings["title_ja"];
                notesText.Text = ConfigurationManager.AppSettings["notes_ja"];
                confirmButton.Content = ConfigurationManager.AppSettings["confirmMessage_ja"];
                languageSwitch.Content = "English";
            }
        }
    }
}
