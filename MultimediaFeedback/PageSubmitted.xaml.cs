using MultimediaFeedback.Controller;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MultimediaFeedback
{
    /// <summary>
    /// Interaktionslogik für PageSubmitted.xaml
    /// </summary>
    public partial class PageSubmitted : Page
    {
        public PageSubmitted(string message)
        {
            InitializeComponent();
            labelMessage.Content = message;

            labelHeader.Content = FormController.Instance.LabelHeader;
        }

        private void again_clicked(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Page1());
        }

        private void close_clicked(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
