using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MultimediaFeedback.Controller
{
    // Info: Pfade werden durch FormController initialisiert
    public class HelperController
    {
        public string HelpDirectory { get; private set; }
        public string HelpRemotePath { get; set; }
        public string FilenameSongbeamerHelp { get; set; }
        public string FilenameMultimediaHelp { get; set; }
        public string FilenameSolvingToolsHelp { get; set; }
        public string FilenameLivestreamHelp { get; set; }
        public string FilenameCircuitDiagramsHelp { get; set; }
        public string FilenamePulpitHelp { get; set; }

        private static HelperController _instance;
        public static HelperController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new HelperController();
                }

                return _instance;
            }
        }

        private HelperController()
        {
#if DEBUG
            HelpDirectory = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(Environment.CurrentDirectory)), "help");
#else
        HelpDirectory = Path.Combine(Environment.CurrentDirectory, "help");
#endif
        }


        public void DownloadAndStartHelpFile(string remoteName)
        {
            var localPath = Path.Combine(HelpDirectory, remoteName);
            var remotePath = @"https://nextcloud.fegmm.de/remote.php/webdav/Anleitungen%20Medienteam/" + remoteName;

            if (!Directory.Exists(HelpDirectory))
            {
                Directory.CreateDirectory(HelpDirectory);
            }

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(remotePath);
                request.Credentials = new NetworkCredential("Media", "M3di3n!");
                request.PreAuthenticate = true;

                int bufferSize = 1024;
                byte[] buffer = new byte[bufferSize];
                int bytesRead = 0;
                var httpResponseStream = request.GetResponse().GetResponseStream();

                // Read from response and write to file
                var fileStream = File.Create(localPath);
                while ((bytesRead = httpResponseStream.Read(buffer, 0, bufferSize)) != 0)
                {
                    fileStream.Write(buffer, 0, bytesRead);
                }

                // Clean up
                fileStream.Flush();
                fileStream.Close();
                fileStream.Dispose();
                httpResponseStream.Close();
            }
            catch
            {
            }

            try
            {
                Process.Start(localPath);
            }
            catch
            {
                MessageBox.Show("Leider kann die Datei entweder nicht aus dem Internet aktualisiert oder geöffnet werden.\nBitte kontaktieren Sie leitung-medientechnik@fegmm.de", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
