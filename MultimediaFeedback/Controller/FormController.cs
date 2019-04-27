using MultimediaFeedback.Model;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.DirectoryServices.AccountManagement;
using System.Collections.Generic;
using System.Net.Mime;
using Microsoft.Win32;

namespace MultimediaFeedback.Controller
{
    public class FormController
    {
        private static FormController _instance;
        public static FormController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new FormController();
                }

                return _instance;
            }
        }

        public static readonly string FilenameNextTicketNr = "NextTicketNr.ini";

        public string LogFilePath { get; private set; }
        public string MailAlways { get; private set; }
        public string[] MailIssues { get; private set; } = new string[2];

        public string ServiceString { get { return FeedbackData.TypeOfService.ToFriendlyString(); } }
        public string LabelHeader { get { return "Dein Feedback (" + ServiceString + ")"; } }

        private Page             CurrentPage;
        private FeedbackData     FeedbackData;

        private FormController()
        {
            if(!ReadIni()) { Application.Current.Shutdown(); }

            // Sets time and service type
            FeedbackData = new FeedbackData();
        }

        public void SetPage(Page page)
        {
            CurrentPage = page;
        }

        private bool ReadIni(string filename = "Settings.ini")
        {
            if (!File.Exists(filename))
            {
                MessageBox.Show(filename + " - Datei konnte nicht gelesen werden!", "Settings-Datei nicht gefunden",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var iniContent = File.ReadAllLines(filename, Encoding.UTF8).Where(x => !x.StartsWith("#") && x.Length > 6);
            if (iniContent.Count() < 7 && iniContent.Count() > 9)
            {
                MessageBox.Show(filename + " - Datei enthält weniger / mehr als 2 Einträge!", "Settings-Datei ungültig",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            LogFilePath = iniContent.First();

            // HelpDirectory wird woanders dynamisch gesetzt
            HelperController.Instance.HelpRemotePath = iniContent.ElementAt(1).Replace(" ", "%20");
            HelperController.Instance.FilenameSongbeamerHelp = iniContent.ElementAt(2).Replace(" ", "%20");
            HelperController.Instance.FilenameMultimediaHelp = iniContent.ElementAt(3).Replace(" ", "%20");
            HelperController.Instance.FilenameSolvingToolsHelp = iniContent.ElementAt(4).Replace(" ", "%20");
            HelperController.Instance.FilenameLivestreamHelp = iniContent.ElementAt(5).Replace(" ", "%20");
            HelperController.Instance.FilenameCircuitDiagramsHelp = iniContent.ElementAt(6).Replace(" ", "%20");
            HelperController.Instance.FilenamePulpitHelp = iniContent.ElementAt(7).Replace(" ", "%20");

            MailAlways = iniContent.ElementAt(8);

            if (!IsValidEmail(MailAlways))
            {
                MessageBox.Show(filename + " - Datei: 1. Mail-Adresse ist ungültig!", "Settings-Datei ungültig",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        public string CheckForm(FeedbackType feedbackType)
        {
            var inputName    = (TextBox) CurrentPage.FindName("inputName");
            var inputComment = (TextBox) CurrentPage.FindName("inputComment");
            var inputMail    = (TextBox) CurrentPage.FindName("inputMail");

            if (inputName?.Text.Length < 8) { return "Bitte ganzen Namen eingeben."; }
            if (feedbackType != FeedbackType.Happy)
            {
                if (inputMail.Text.Length < 7) { return "Bitte eine E-Mail-Adresse eingeben."; }
                if (!IsValidEmail(inputMail.Text)) { return "Bitte eine gültige E-Mail-Adresse eingeben, damit wir auf dich zurückkommen können."; }
                if (inputComment.Text.Length < 10) { return "Bitte einen aussagekräftigen Kommentar eingeben."; }
            }

            return "";
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email && !email.Contains("Test") && !email.Contains("test");
            }
            catch
            {
                return false;
            }
        }

        public bool SaveData(FeedbackType feedbackType, string selectionName, string attachment = "")
        {
            // Collect Data
            var inputName    = (TextBox)CurrentPage.FindName("inputName");
            var inputComment = (TextBox)CurrentPage.FindName("inputComment");
            var inputMail    = (TextBox)CurrentPage.FindName("inputMail");


            FeedbackData.TypeOfFeedback  = feedbackType;
            FeedbackData.Name            = inputName.Text;
            FeedbackData.Mail            = inputMail?.Text ?? ""; 
            FeedbackData.Comment         = inputComment.Text;
            FeedbackData.Selection       = selectionName;
            FeedbackData.Attachment      = attachment;

            // Set Culture of Application to english (for CSV file to work properly)
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CreateSpecificCulture("de-GE");

            while (true)
            {
                // Save To File
                try
                {
                    var fileExists = File.Exists(LogFilePath);
                    var file = new StreamWriter(LogFilePath, append: true);

                    if (!fileExists)
                    {
                        file.WriteLine(FeedbackData.HeaderString());
                    }
                    file.WriteLine(FeedbackData.ToString());

                    // Close file
                    file.Flush();
                    file.Close();
                    file.Dispose();

                    return true;
                }
                catch (Exception ex)
                {
                    if (MessageBox.Show("Daten konnte nicht gespeichert werden. Nochmal versuchen?\nEventuell ist die Datei gerade geöffnet\n\n(" + ex.ToString() + ")", "Fehler beim Speichern",
                                        MessageBoxButton.YesNoCancel, MessageBoxImage.Error) != MessageBoxResult.Yes)
                    {
                        return false;
                    }
                }
            }
        }

        private IEnumerable<string> GetSubscriptionData(string filename)
        {
            var iniContent = File.ReadAllLines(filename, Encoding.UTF8).Where(x => !x.StartsWith("#") && x.Length > 7);

            if (iniContent.Count() < 2) return new List<string>();
            return iniContent;
        }

        private void AddMailRecipientsDynamically(ref MailMessage mail)
        {
            // Allow up to 10 mail feeds, where you can subscribe to get some error reports by e-mail
            for (int i=1; i < 10; i++)
            {
                if (!File.Exists($"Mail{i}Feed.ini")) { break; }
                var subscriptionLines = File.ReadAllLines($"Mail{i}Feed.ini", Encoding.UTF8).Where(x => !x.StartsWith("#") && x.Length > 3);

                if (subscriptionLines.Count() < 2) { continue; }
                var mailTo = subscriptionLines.First();
                subscriptionLines = subscriptionLines.Skip(1);

                if (subscriptionLines.Where(x => x.Equals(FeedbackData.Selection)).Any() && IsValidEmail(mailTo))
                {
                    mail.To.Add(mailTo);
                }
            }
        }

        private int ReadNextTicketNr()
        {
            var ticketNr = -1;
            if (File.Exists(FilenameNextTicketNr))
            {
                var lines = File.ReadAllLines(FilenameNextTicketNr, Encoding.UTF8).Where(x => !x.StartsWith("#") && x.Length > 0);
                if (lines.Count() == 1)
                {
                    if (!int.TryParse(lines.First(), out ticketNr)) return -1;
                }
              }

            return ticketNr;
        }

        private void UpdateNextTicketNr(int nextNr)
        {
            string[] lines = { "# Nächste Ticket-Nummer. Für den E-Mail-Versand. Muss eine Zahl in genau einer Zeile sein. Wird automatisch hochgezählt (Encoding: UTF8).", "" + nextNr };

            try
            {
                File.WriteAllLines(FilenameNextTicketNr, lines, Encoding.UTF8);
            }
            catch
            {
                MessageBox.Show("Die Ticket-Nummer kann nicht aktualisiert werden. Bitte Teamleiter benachrichtigen.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public bool SendMail()
        {
            var ticketNr = ReadNextTicketNr();

            while (true)
            {
                try
                {
                    /*Mailserver:
                   Server: mail.feg.de SMTP-Port 25
                   Benutzername: kool @mmserver.feg.de
                   Passwort: mail4kool
                   Absenderadresse: belieig*/

                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("mail.feg.de");

                    mail.From = new MailAddress("Medientechnik-Feedback<noreply-feedback@feg.de>");
                    mail.To.Add(MailAlways);
                    mail.ReplyToList.Add(FeedbackData.Mail);
                    AddMailRecipientsDynamically(ref mail);

                    mail.Subject = $"Medientechnik-Feedback (#{ticketNr}): {FeedbackData.TypeOfFeedback.ToFriendlyString()} - {FeedbackData.Selection}";
                    mail.Body = $"Von: {FeedbackData.Name} ({FeedbackData.Mail}) aus Godi: {FeedbackData.TypeOfService.ToFriendlyString()}\n\n-----------------------------------------------\n\nKommentar:\n{FeedbackData.Comment}";
                    if (!string.IsNullOrEmpty(FeedbackData.Attachment)) { mail.Attachments.Add(new Attachment(FeedbackData.Attachment, MediaTypeNames.Application.Octet)); }

                    // todo change
                    var pwd = new byte[] { 80, 0, 84, 0, 76, 0, 99, 0 };

                    SmtpServer.Port = 25; //587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("kool@mmserver.feg.de", "mail4kool");//Encoding.Unicode.GetString(pwd));
                    SmtpServer.EnableSsl = true;

                    SmtpServer.Send(mail);

                    UpdateNextTicketNr(ticketNr+1);

                    return true;
                }
                catch (Exception ex)
                {
                    if (MessageBox.Show("E-Mail konnte nicht gesendet werden. Nochmal versuchen?\n\n(" + ex.ToString() + ")", "Fehler beim E-Mail senden",
                                        MessageBoxButton.YesNoCancel, MessageBoxImage.Error) != MessageBoxResult.Yes)
                    {
                        return false;
                    }
                }
            }
        }

        public static string GetFilenamePathWithDialog()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.CheckPathExists = true;
            dialog.Multiselect = false;
            //dialog.FileName = "FixationSensitive.csv"; // Settings.Instance.GazeDataDirectory; // Default file name
            dialog.DefaultExt = ".txt | .jpg | .png | .zip";
            dialog.Filter = "Textdateien (.txt)|*.txt|Compressed picture (.jpg)|*.jpg|Graphics with alpha channel (.png)|*.png|Zip-Ordner|*.zip";
            dialog.Title = "Datei auswählen";
            var result = dialog.ShowDialog();

            return result.HasValue && result.Value ? dialog.FileName : "";
        }


        public static string GetUsername()
        {
            try
            {
                return UserPrincipal.Current.DisplayName;
            }
            catch 
            {
                return Environment.UserName;
            }
        }

        public static string GetUserMail()
        {
            try
            {
                return UserPrincipal.Current.EmailAddress;
            }
            catch
            {
                return "";
            }
        }
    }
}
