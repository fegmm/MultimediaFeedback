using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaFeedback.Controller
{
    public class CookieAwareWebClient : WebClient
    {
        private CookieContainer cookie = new CookieContainer();

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);
            if (request is HttpWebRequest)
            {
                (request as HttpWebRequest).CookieContainer = cookie;
            }
            return request;
        }
    }


    public class LoginBeamer
    {
        /*
            // Beispielaufruf:
            private async Task ParsingAsync()
            {
                // FEG-Livestream
                var result = await IsYtStreamOffline.IsYouTubeStreamOfflineAsync("https://youtu.be/tgbPV9EPbJY");
                // Mister Been
                var result2 = await IsYtStreamOffline.IsYouTubeStreamOfflineAsync("https://www.youtube.com/watch?v=ovmSF0RIwTQ");

                return;
            }
        */

        public static bool Test2(string url)
        {
            return true;
        }

        public static bool Login(string url)
        {
            Test2("https://fegmm.elvanto.eu/admin/people/#v:all");

            var client = new CookieAwareWebClient();
            client.BaseAddress = @"https://nextcloud.fegmm.de/";
            var loginData = new NameValueCollection();
            loginData.Add("login", "sylupp_j");
            loginData.Add("password", "Pe3.Likan");
            client.UploadValues("login.php/login", "POST", loginData);

            //Now you are logged in and can request pages    
            string htmlSource = client.DownloadString("https://nextcloud.fegmm.de/index.php/apps/file");//"index.php");

            //Test(url);

            return true;
        }

        public static bool Test(string url)
        {
            string formUrl = "http://www.mmoinn.com/index.do?PageModule=UsersAction&Action=UsersLogin"; // NOTE: This is the URL the form POSTs to, not the URL of the form (you can find this in the "action" attribute of the HTML's form tag
            string formParams = string.Format("email_address={0}&password={1}", "sylupp_j", "Pe3.Likan");
            string cookieHeader;
            WebRequest req = WebRequest.Create(formUrl);
            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";
            byte[] bytes = Encoding.ASCII.GetBytes(formParams);
            req.ContentLength = bytes.Length;
            using (Stream os = req.GetRequestStream())
            {
                os.Write(bytes, 0, bytes.Length);
            }
            WebResponse resp = req.GetResponse();
            cookieHeader = resp.Headers["Set-cookie"];

            string pageSource;
            string getUrl = "https://nextcloud.fegmm.de/index.php/apps/files";
            WebRequest getRequest = WebRequest.Create(getUrl);
            getRequest.Headers.Add("Cookie", cookieHeader);
            WebResponse getResponse = getRequest.GetResponse();
            using (StreamReader sr = new StreamReader(getResponse.GetResponseStream()))
            {
                pageSource = sr.ReadToEnd();
            }

            return true;
        }
    }
}
