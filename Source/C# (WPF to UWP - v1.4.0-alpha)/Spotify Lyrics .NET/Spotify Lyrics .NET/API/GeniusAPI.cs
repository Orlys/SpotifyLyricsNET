using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Genius.Clients;
using Genius;
using Genius.Models;

namespace Spotify_Lyrics.NET.API
{
    class GeniusAPI
    {
        GeniusClient geniusClient;
        MainWindow mainW;

        public GeniusAPI(ref MainWindow mw)
        {
            mainW = mw;
            geniusClient = new GeniusClient("W0knrrPmfODbCT-Oe26Uimx8GJSqszwKyh34soM0oQuNRSppLmlOuHffrO8YD0iL");
        }

        public async Task getLyrics(string artist, string song)
        {
            // Search the song on Genius
            var searchResult = await geniusClient.SearchClient.Search(TextFormat.Dom, Uri.EscapeDataString(artist) + "-" + Uri.EscapeDataString(song));

            // Save all the valid search results
            for (var r = 0; r < searchResult.Response.Count(); r++)
            {
                Hit h = searchResult.Response[r];
                string[] hContent = h.Result.ToString().Replace('"', '\'').Split('\n');

                MainWindow.lyricsURL lyricsObj = new MainWindow.lyricsURL();
                lyricsObj.source = "Genius";
                for (var x = 0; x < hContent.Count(); x++)
                {
                    var ln = hContent[x];
                    if (ln.Contains("'id':"))
                    {
                        lyricsObj.id = ln.Replace("  'id': ", "").Replace(",\r", "");
                        if (lyricsObj.img != null) break;
                    }
                    else if (ln.Contains("'api_path':"))
                    {
                        lyricsObj.url = "https://genius.com" + ln.Replace("  'api_path': ", "").Replace(",\r", "").Replace("'", "");
                    }
                    else if (ln.Contains("'header_image_thumbnail_url':"))
                    {
                        lyricsObj.img = ln.Replace("  'header_image_thumbnail_url': '", "").Replace("',\r", "");
                        if (lyricsObj.id != null) break;
                    }
                }

                mainW.lyricsURLs.Add(lyricsObj);
            }
        }

        public async Task setLyrics(int indx, bool test = false)
        {
            string lyricsText = "";

            var songResult = await geniusClient.SongsClient.GetSong(TextFormat.Html, mainW.lyricsURLs[indx].id);
            string responseLyrics = getHTTPSRequest(WebUtility.HtmlEncode(songResult.Response.Url));
            HtmlAgilityPack.HtmlDocument lyricsDoc = new HtmlAgilityPack.HtmlDocument();
            lyricsDoc.LoadHtml(responseLyrics);

            var nodes = lyricsDoc.DocumentNode.SelectNodes("//p");
            if (nodes != null)
            {
                foreach (HtmlNode p in nodes)
                {
                    try
                    {
                        lyricsText += p.InnerText.Replace("&amp;", "&") + Environment.NewLine;
                    }
                    catch (Exception ex)
                    {
                        mainW.lyricsText = "";
                    }
                }
            }

            if (!test)
            {
                mainW.lyricsText = lyricsText;
            }
            else
            {
                mainW.lyricsTextTemp = lyricsText;
            }
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
