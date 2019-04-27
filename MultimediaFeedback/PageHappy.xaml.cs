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
    /// Interaktionslogik für PageHappy.xaml
    /// </summary>
    public partial class PageHappy : Page
    {
        public PageHappy()
        {
            InitializeComponent();

            labelHeader.Content = FormController.Instance.LabelHeader;
            inputName.Text = FormController.GetUsername();
        }

        private void submit_clicked(object sender, RoutedEventArgs e)
        {
            FormController.Instance.SetPage(this);
            var errorText = FormController.Instance.CheckForm(Model.FeedbackType.Happy);

            labelError.Content = errorText;

            if (errorText.Length < 1)
            {
                FormController.Instance.SaveData(Model.FeedbackType.Happy, "Alles gut");
                this.NavigationService.Navigate(new PageSubmitted("Feedback erfolgreich gespeichert.\nDanke."));
            }
        }

        private void back_clicked(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Page1());
        }
    }
}
