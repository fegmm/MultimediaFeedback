using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaFeedback.Controller
{
    internal class IsYtStreamOffline
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
        internal static async Task<bool> IsYouTubeStreamOfflineAsync(string url)
        {
            var http = new HttpClient();
            var response = await http.GetByteArrayAsync(url);
            String source = Encoding.GetEncoding("utf-8").GetString(response, 0, response.Length - 1);
            source = WebUtility.HtmlDecode(source);
            HtmlDocument result = new HtmlDocument();
            result.LoadHtml(source);

            var htmlNodes = result.DocumentNode.Descendants().Where (x => (x.Name == "script" && x.InnerHtml.Contains("var ytplayer = ytplayer ||"))).ToList();
        
            if (htmlNodes.Count != 1) { throw new Exception("HTML-Format des Youtube-Livestream hat sich geändert! Offline-Status kann nicht mehr herausgefunden werden"); }

            return htmlNodes.First().InnerHtml.Contains("LIVE_STREAM_OFFLINE");
        }
    }
}
