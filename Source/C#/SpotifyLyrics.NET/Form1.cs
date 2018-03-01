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
        const String appVERSION = "v0.2.3",
                     appBUILD = "02.03.2018",
                     appAuthor = "Jakub Stęplowski",
                     appAuthorWebsite = "https://jakubsteplowski.com";
        const int LEFTMARGIN = 24;

        String currentSongTitle = "";
        int currentLyricsIndx = -1;
        ArrayList lyricsURLs = new ArrayList();
        bool settingsLoaded = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            versionLabel.Text = appVERSION;

            // Load Settings
            loadTheme(Properties.Settings.Default.theme);
            topmostCheck.Checked = Properties.Settings.Default.topMost;
            this.TopMost = Properties.Settings.Default.topMost;
            if (Properties.Settings.Default.width > 0)
            {
                this.Width = Properties.Settings.Default.width;
                this.Height = Properties.Settings.Default.height;
            }
            if (Properties.Settings.Default.xPos > 0)
            {
                this.Left = Properties.Settings.Default.xPos;
                this.Top = Properties.Settings.Default.yPos;
            }
            settingsLoaded = true;
        }

        #region UI
        // Themes
        private void loadTheme(int themeID)
        {
            Color bg1 = Color.White, bg2 = Color.WhiteSmoke, txt = Color.Black;

            switch (themeID)
            {
                case 1: //Dark
                    bg1 = Color.FromArgb(14, 14, 14);
                    bg2 = Color.FromArgb(10, 10, 10);
                    txt = Color.FromArgb(245, 245, 245);
                    ChangeThemeToolStripMenuItem.Image = Properties.Resources.settings_white;
                    break;
                default:
                    ChangeThemeToolStripMenuItem.Image = Properties.Resources.settings;
                    break;
            }

            // Set colors
            this.BackColor = bg1;
            Panel1.BackColor = bg2;
            separator.BackColor = bg2;
            titleLabel.ForeColor = txt;
            artistLabel.ForeColor = txt;
            lyricsLabel.ForeColor = txt;
            countLabel.ForeColor = txt;
            versionLabel.ForeColor = txt;
            topmostCheck.ForeColor = txt;

            // Save Settings
            Properties.Settings.Default.theme = themeID;
            Properties.Settings.Default.Save();
        }

        private void LightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadTheme(0);
        }

        private void DarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadTheme(1);
        }

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

            // Save window size
            if (settingsLoaded)
            {
                Properties.Settings.Default.width = this.Width;
                Properties.Settings.Default.height = this.Height;
                Properties.Settings.Default.Save();
            }
        }

        private void Form1_LocationChanged(object sender, EventArgs e)
        {
            // Save window position
            if (settingsLoaded)
            {
                Properties.Settings.Default.xPos = this.Left;
                Properties.Settings.Default.yPos = this.Top;
                Properties.Settings.Default.Save();
            }
        }

        public void setBtnStatus(bool status)
        {
            nextBtn.Enabled = status;
            prevBtn.Enabled = status;
        }

        private void topmostCheck_CheckedChanged(object sender, EventArgs e)
        {
            // Save top most status
            if (settingsLoaded)
            {
                this.TopMost = topmostCheck.Checked;
                Properties.Settings.Default.topMost = topmostCheck.Checked;
                Properties.Settings.Default.Save();
            }
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

            if (title.Contains(" - "))
            {
                String artist = title.Substring(0, title.IndexOf(" -"));
                String songTitle = title.Replace(artist + " - ", "");

                titleLabel.Text = songTitle;
                artistLabel.Text = artist;
                getLyrics(artist, songTitle);
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
                countLabel.Text = "0 of 0";
            }
        }

        private void setLyrics(int indx)
        {
            currentLyricsIndx = indx;
            countLabel.Text = (indx + 1) + " of " + lyricsURLs.Count;

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
                        lyricsLabel.Text += p.InnerText + System.Environment.NewLine;
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
