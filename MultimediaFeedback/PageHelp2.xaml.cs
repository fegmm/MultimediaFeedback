using MultimediaFeedback.Controller;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MultimediaFeedback
{
    /// <summary>
    /// Interaktionslogik für Page2.xaml
    /// </summary>
    public partial class PageHelp2 : Page
    {
        public PageHelp2()
        {
            InitializeComponent();
        }

        private void livestream_clicked(object sender, MouseButtonEventArgs e)
        {
            HelperController.Instance.DownloadAndStartHelpFile(HelperController.Instance.FilenameLivestreamHelp);
        }

        private void livestream_mouseEnter(object sender, MouseEventArgs e)
        {
            imageLivestream.Opacity = 0.6;
        }

        private void livestream_mouseLeave(object sender, MouseEventArgs e)
        {
           imageLivestream.Opacity = 1.0;
        }


        private void circuits_clicked(object sender, MouseButtonEventArgs e)
        {
            HelperController.Instance.DownloadAndStartHelpFile(HelperController.Instance.FilenameCircuitDiagramsHelp);
        }

        private void circuits_mouseEnter(object sender, MouseEventArgs e)
        {
            imageCircuits.Opacity = 0.6;
        }

        private void circuits_mouseLeave(object sender, MouseEventArgs e)
        {
            imageCircuits.Opacity = 1.0;
        }


        private void pulpit_clicked(object sender, MouseButtonEventArgs e)
        {
            HelperController.Instance.DownloadAndStartHelpFile(HelperController.Instance.FilenamePulpitHelp);
        }

        private void pulpit_mouseEnter(object sender, MouseEventArgs e)
        {
            imagePulpit.Opacity = 0.6;
        }

        private void pulpit_mouseLeave(object sender, MouseEventArgs e)
        {
            imagePulpit.Opacity = 1.0;
        }


        private void back_clicked(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Page1());
        }

        private void buttonPrevPage_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new PageHelp1());
        }
    }
}
