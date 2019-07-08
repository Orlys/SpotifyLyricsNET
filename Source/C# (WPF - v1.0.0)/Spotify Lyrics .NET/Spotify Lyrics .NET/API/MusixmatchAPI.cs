using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Spotify_Lyrics.NET.API
{
    class MusixmatchAPI
    {
        public void getLyrics(string artist, string song, ref List<MainWindow.lyricsURL> lyricsURLs)
        {
            // Search the song on Musixmatch
            string searchURL = "https://www.musixmatch.com/search/" + Uri.EscapeDataString(artist) + "-" + Uri.EscapeDataString(song) + "/tracks";
            string response = getHTTPSRequest(searchURL);
            response = response.Replace("\"", "'");

            // Save all the valid search results
            while (response.Contains("href='/lyrics/"))
            {
                try
                {
                    int a = response.IndexOf("href='/lyrics/");
                    int b = response.IndexOf("'>", a);
                    string link = response.Substring(a, b - a).Replace("href='", "");

                    MainWindow.lyricsURL lyricsObj = new MainWindow.lyricsURL();
                    lyricsObj.url = "https://www.musixmatch.com" + link;
                    lyricsObj.source = "Musixmatch";

                    lyricsURLs.Add(lyricsObj);
                    response = response.Substring(b, response.Length - b);
                }
                catch (Exception ex)
                {
                    break;
                }
            }
        }

        public string setLyrics(int indx, ref List<MainWindow.lyricsURL> lyricsURLs)
        {
            string responseLyrics = getHTTPSRequest(WebUtility.HtmlEncode(lyricsURLs[indx].url));
            HtmlAgilityPack.HtmlDocument lyricsDoc = new HtmlAgilityPack.HtmlDocument();
            lyricsDoc.LoadHtml(responseLyrics);

            var nodes = lyricsDoc.DocumentNode.SelectNodes("//p");
            string lyricsText = "";
            foreach (HtmlNode p in nodes)
            {
                try
                {
                    if (p.HasClass("mxm-lyrics__content"))
                        lyricsText += p.InnerText + Environment.NewLine;
                }
                catch { }
            }
            return lyricsText;
        }

        private static string getHTTPSRequest(string strRequest)
        {
            try
            {
                WebRequest ThisRequest = WebRequest.Create(strRequest);
                ThisRequest.ContentType = "application/x-www-form-urlencoded";
                ThisRequest.Method = "GET";

                System.Text.ASCIIEncoding Encoder = new System.Text.ASCIIEncoding();
                byte[] BytesToSend = Encoder.GetBytes(strRequest);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                HttpWebResponse TheirResponse = (HttpWebResponse)ThisRequest.GetResponse();

                StreamReader sr = new StreamReader(TheirResponse.GetResponseStream());
                string strResponse = sr.ReadToEnd();
                sr.Close();

                return strResponse;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
