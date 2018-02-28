using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Collections;
using HtmlAgilityPack;
using System.Drawing;

namespace SpotifyLyrics.NET
{
    public partial class Form1 : Form
    {
        const String appVERSION = "v0.1.0",
                     appBUILD = "28.02.2018",
                     appAuthor = "Jakub Stęplowski",
                     appAuthorWebsite = "https://jakubsteplowski.com";
        const int LEFTMARGIN = 24;

        String currentSongTitle = "";
        int currentLyricsIndx = -1;
        ArrayList lyricsURLs = new ArrayList();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            versionLabel.Text = appVERSION + " (" + appBUILD + ")";
        }

        #region UI
        private void fixLyricsLabelPosition()
        {
            lyricsLabel.MaximumSize = new Size(this.Width - (LEFTMARGIN * 2), 0);
        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {
            fixLyricsLabelPosition();
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            fixLyricsLabelPosition();
        }

        public void setBtnStatus(bool status)
        {
            nextBtn.Enabled = status;
            prevBtn.Enabled = status;
        }

        private void topmostCheck_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = topmostCheck.Checked;
        }

        private void nextBtn_Click(object sender, EventArgs e)
        {
            lyricsLabel.Text = "Loading...";
            if (currentLyricsIndx + 1 >= lyricsURLs.Count)
            {
                setLyrics(0);
            }
            else
            {
                setLyrics(currentLyricsIndx + 1);
            }
        }

        private void prevBtn_Click(object sender, EventArgs e)
        {
            lyricsLabel.Text = "Loading...";
            if (currentLyricsIndx - 1 < 0)
            {
                setLyrics(lyricsURLs.Count - 1);
            }
            else
            {
                setLyrics(currentLyricsIndx - 1);
            }
        }
        #endregion

        #region Set Lyrics
        private void timerTitle_Tick(object sender, EventArgs e)
        {
            Process[] spotifyProcess = Process.GetProcessesByName("Spotify");

            foreach (Process p in spotifyProcess)
            {
                String songTitle = p.MainWindowTitle;

                if (songTitle != "" && songTitle != "Spotify" && songTitle != currentSongTitle)
                {
                    setSong(songTitle);
                }
            }
        }

        private void setSong(String title)
        {
            currentSongTitle = title;

            ArrayList titleParser = new ArrayList();
            titleParser.AddRange(title.Split('-'));

            if (titleParser.Count > 1)
            {
                titleLabel.Text = (String)titleParser[1];
                titleLabel.Text = titleLabel.Text.Trim(' ');

                authorLabel.Text = (String)titleParser[0];
                authorLabel.Text = authorLabel.Text.Trim(' ');

                getLyrics(authorLabel.Text, titleLabel.Text);
            }
        }

        private void getLyrics(String artist, String song)
        {
            lyricsLabel.Text = "Searching...";

            // Search the song on Musixmatch
            String searchURL = "https://www.musixmatch.com/search/" + artist + "-" + song + "/tracks";
            String response = getHTTPSRequest(WebUtility.HtmlEncode(searchURL));
            response = response.Replace("\"", "'");

            // Save all the valid search results
            lyricsURLs = new ArrayList();

            while (response.Contains("href='/lyrics/"))
            {
                try
                {
                    int a = response.IndexOf("href='/lyrics/"),
                        b = response.IndexOf("'>", a);
                    String link = response.Substring(a, b - a).Replace("href='", "");

                    lyricsURLs.Add("https://www.musixmatch.com" + link);
                    response = response.Substring(b, response.Length - b);
                } catch (Exception ex)
                {
                    break;
                }
            }

            // Display the first result if found
            if (lyricsURLs.Count > 0)
            {
                setLyrics(0);
                setBtnStatus(lyricsURLs.Count > 1);
            } else
            {
                lyricsLabel.Text = "I can't find the lyrics, sorry. :(";
                setBtnStatus(false);
                countLabel.Text = "Wrong Song? Try other lyrics: 0 of 0";
            }
        }

        private void setLyrics(int indx)
        {
            currentLyricsIndx = indx;
            countLabel.Text = "Wrong Song? Try other lyrics: " + (indx + 1) + " of " + lyricsURLs.Count;

            String responseLyrics = getHTTPSRequest(WebUtility.HtmlEncode((String)lyricsURLs[indx]));
            HtmlAgilityPack.HtmlDocument lyricsDoc = new HtmlAgilityPack.HtmlDocument();
            lyricsDoc.LoadHtml(responseLyrics);

            HtmlNodeCollection nodes = lyricsDoc.DocumentNode.SelectNodes("//p");
            lyricsLabel.Text = "";

            foreach (HtmlNode p in nodes)
            {
                try
                {
                    if (p.Attributes["class"].Value.Contains("mxm-lyrics__content"))
                    {
                        lyricsLabel.Text += p.InnerText;
                    }
                } catch (Exception ex)
                {
                }
            }

            if (lyricsLabel.Text.Trim(' ') == "")
            {
                lyricsLabel.Text = "I can't find the lyrics, sorry. :(";
            }
        }
        #endregion

        #region Web Requests
        public string getHTTPSRequest(String strRequest)
        {
            try
            {
                WebRequest ThisRequest = WebRequest.Create(strRequest);
                ThisRequest.ContentType = "application/x-www-form-urlencoded";
                ThisRequest.Method = "GET";

                System.Text.ASCIIEncoding Encoder = new System.Text.ASCIIEncoding();
                Byte[] BytesToSend = Encoder.GetBytes(strRequest);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                WebResponse TheirResponse = ThisRequest.GetResponse();

                StreamReader sr = new StreamReader(TheirResponse.GetResponseStream());
                String strResponse = sr.ReadToEnd();
                sr.Close();

                return strResponse;
            } catch (Exception ex)
            {
                return "";
            }
        }
        #endregion
    }
}
