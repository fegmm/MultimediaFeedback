using MultimediaFeedback.Controller;
using System;
using System.Collections.Generic;
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

namespace MultimediaFeedback
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Set Date and service type
            FormController.Instance.ToString();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //ParsingAsync(); 
            pageNavigation.NavigationService.Navigate(new Page1());
        }

        private async Task ParsingAsync()
        {
            var result = await IsYtStreamOffline.IsYouTubeStreamOfflineAsync("https://youtu.be/tgbPV9EPbJY");
            var result2 = await IsYtStreamOffline.IsYouTubeStreamOfflineAsync("https://www.youtube.com/watch?v=ovmSF0RIwTQ");

            return;
        }
    }
}
