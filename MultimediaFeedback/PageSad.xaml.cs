using Microsoft.Win32;
using MultimediaFeedback.Controller;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MultimediaFeedback
{
    /// <summary>
    /// Interaktionslogik für PageSad.xaml
    /// </summary>
    public partial class PageSad : Page
    {
        private string CurrentRadioButton = "";
        private string AttachmentPath = "";

        private void CreateRadioButtons()
        {
            var lines = File.ReadAllLines($"Complaints.ini", Encoding.UTF8).Where(x => !x.StartsWith("#") && x.Length > 3);
            if (lines.Count() > 9)
            {
                lines = lines.Take(9);  // max 9 lines
            }

            var count = 0;
            var padding = lines.Count() > 6 ? 25 : 40;
            foreach (var line in lines)
            {
                RadioButton rb = new RadioButton()
                {
                    GroupName = "complaint",
                    Content = line,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    FontSize = 14,
                    IsChecked = count == lines.Count() - 1,
                    Margin = new Thickness(250 + (count % 3) * 160, 210 + (int)((count / 3)) * padding, 0, 0) // "250",205,0,0"
                };

                rb.Checked += (sender, args) =>
                {
                    CurrentRadioButton = (string)((RadioButton)(sender)).Content;
                };
                myGrid.Children.Add(rb);
                count++;
            }

            CurrentRadioButton = lines.LastOrDefault();
        }

        public PageSad()
        {
            InitializeComponent();

            CreateRadioButtons();

            labelHeader.Content = FormController.Instance.LabelHeader;
            inputName.Text = FormController.GetUsername();
            inputMail.Text = FormController.GetUserMail();
        }

        private void submit_clicked(object sender, RoutedEventArgs e)
        {
            FormController.Instance.SetPage(this);
            var errorText = FormController.Instance.CheckForm(Model.FeedbackType.Sad);

            labelError.Content = errorText;

            if (errorText.Length < 1)
            {
                FormController.Instance.SaveData(Model.FeedbackType.Sad, CurrentRadioButton, AttachmentPath);
                FormController.Instance.SendMail();
                this.NavigationService.Navigate(new PageSubmitted("Problem erfolgreich an Teamleiter übermittelt.\nDanke."));
            }
        }

        private void back_clicked(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Page1());
        }

        private void attachment_clicked(object sender, MouseButtonEventArgs e)
        {
            var attachmentPath = FormController.GetFilenamePathWithDialog();
            if (!string.IsNullOrEmpty(attachmentPath))
            {
                AttachmentPath = attachmentPath;
                buttonDeleteAttachment.Visibility = Visibility.Visible;
                imageAttachment.Opacity = 1.0;
            }
        }

        private void attachment_mouseEnter(object sender, MouseEventArgs e)
        {
            imageAttachment.Opacity = 0.8;
        }

        private void attachment_mouseLeave(object sender, MouseEventArgs e)
        {
            imageAttachment.Opacity = string.IsNullOrEmpty(AttachmentPath) ? 0.6 : 1.0;
        }

        private void deleteAttachment_clicked(object sender, RoutedEventArgs e)
        {
            AttachmentPath = "";
            buttonDeleteAttachment.Visibility = Visibility.Hidden;
            imageAttachment.Opacity = 0.6;
        }
    }
}
