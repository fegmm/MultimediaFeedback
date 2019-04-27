using MultimediaFeedback.Controller;
using System.Windows.Controls;
using System.Windows.Input;

namespace MultimediaFeedback
{
    /// <summary>
    /// Interaktionslogik für Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        public Page1()
        {
            InitializeComponent();

            labelHeader.Content = FormController.Instance.LabelHeader;
        }

        private void happy_clicked(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.Navigate(new PageHappy());
        }

        private void happy_mouseEnter(object sender, MouseEventArgs e)
        {
            imageHappy.Opacity = 0.6;
        }

        private void happy_mouseLeave(object sender, MouseEventArgs e)
        {
            imageHappy.Opacity = 1.0;
        }


        private void sad_clicked(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.Navigate(new PageSad());
        }

        private void sad_mouseEnter(object sender, MouseEventArgs e)
        {
            imageSad.Opacity = 0.6;
        }

        private void sad_mouseLeave(object sender, MouseEventArgs e)
        {
            imageSad.Opacity = 1.0;
        }



        private void idea_clicked(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.Navigate(new PageIdea());
        }

        private void idea_mouseEnter(object sender, MouseEventArgs e)
        {
            imageIdea.Opacity = 0.6;
        }

        private void idea_mouseLeave(object sender, MouseEventArgs e)
        {
            imageIdea.Opacity = 1.0;
        }


        private void help_clicked(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.Navigate(new PageHelp1());
        }

        private void help_mouseEnter(object sender, MouseEventArgs e)
        {
            imageHelp.Opacity = 0.6;
        }

        private void help_mouseLeave(object sender, MouseEventArgs e)
        {
            imageHelp.Opacity = 1.0;
        }
    }
}
