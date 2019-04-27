using MultimediaFeedback.Controller;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MultimediaFeedback
{
    /// <summary>
    /// Interaktionslogik für Page1.xaml
    /// </summary>
    public partial class PageHelp1 : Page
    {
        public string HelpDirectory {get; private set; }

        public PageHelp1()
        {
            InitializeComponent();

#if DEBUG
            HelpDirectory = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(Environment.CurrentDirectory)), "help");
#else
            HelpDirectory = Path.Combine(Environment.CurrentDirectory, "help");
#endif

            //labelHeader.Content = FormController.Instance.LabelHeader;
        }

        private void manual_clicked(object sender, MouseButtonEventArgs e)
        {
            HelperController.Instance.DownloadAndStartHelpFile(HelperController.Instance.FilenameMultimediaHelp);
        }

        private void manual_mouseEnter(object sender, MouseEventArgs e)
        {
            imageManual.Opacity = 0.6;
        }

        private void manual_mouseLeave(object sender, MouseEventArgs e)
        {
            imageManual.Opacity = 1.0;
        }


        private void songbeamer_clicked(object sender, MouseButtonEventArgs e)
        {
            HelperController.Instance.DownloadAndStartHelpFile(HelperController.Instance.FilenameSongbeamerHelp);
        }

        private void songbeamer_mouseEnter(object sender, MouseEventArgs e)
        {
            imageSongbeamer.Opacity = 0.6;
        }

        private void songbeamer_mouseLeave(object sender, MouseEventArgs e)
        {
            imageSongbeamer.Opacity = 1.0;
        }


        private void solvingTools_clicked(object sender, MouseButtonEventArgs e)
        {
            HelperController.Instance.DownloadAndStartHelpFile(HelperController.Instance.FilenameSolvingToolsHelp);
        }

        private void tools_mouseEnter(object sender, MouseEventArgs e)
        {
            imageSolvingTools.Opacity = 0.6;
        }

        private void tools_mouseLeave(object sender, MouseEventArgs e)
        {
            imageSolvingTools.Opacity = 1.0;
        }


        private void back_clicked(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Page1());
        }

        private void buttonNextPage_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new PageHelp2());
        }
    }
}
