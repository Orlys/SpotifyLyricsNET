using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Spotify_Lyrics.NET
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string appVERSION = "v1.0.0-alpha";
        const string appBUILD = "01.01.2019";
        const string appAuthor = "Jakub Stęplowski";
        const string appAuthorWebsite = "https://jakubsteplowski.com";

        private string currentSongTitle = "";
        private int currentLyricsIndx = -1;
        private List<string> lyricsURLs = new List<string>();
        private bool settingsLoaded = false;
        private DispatcherTimer sTimer;

        private SolidColorBrush bgColor = new SolidColorBrush();
        private SolidColorBrush textColor = new SolidColorBrush();
        private SolidColorBrush textColor2 = new SolidColorBrush();

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            SizeChanged += MainWindow_SizeChanged;

            darkTheme.Checked += darkTheme_Checked;
            darkTheme.Unchecked += darkTheme_Unchecked;
            topmostCheck.Checked += topmostCheck_CheckedChanged;
            topmostCheck.Unchecked += topmostCheck_CheckedChanged;
            prevBtn.Click += prevBtn_Click;
            nextBtn.Click += nextBtn_Click;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            versionLabel.Text = appVERSION;

            // Load Settings
            loadTheme(Properties.Settings.Default.theme);
            topmostCheck.IsChecked =Properties.Settings.Default.topMost;
            if (Properties.Settings.Default.theme == 1)
                darkTheme.IsChecked = true;
            this.Topmost = Properties.Settings.Default.topMost;
            if (Properties.Settings.Default.width > 0)
            {
                this.Width =Properties.Settings.Default.width;
                this.Height =Properties.Settings.Default.height;
            }
            if (Properties.Settings.Default.xPos > 0)
            {
                this.Left =Properties.Settings.Default.xPos;
                this.Top =Properties.Settings.Default.yPos;
            }
            this.Opacity =Properties.Settings.Default.opacity;
            settingsLoaded = true;

            // Start Timer
            sTimer = new DispatcherTimer();
            sTimer.Interval = TimeSpan.FromMilliseconds(500);
            sTimer.Tick += timerTitle_Tick;
            sTimer.Start();

            addToLyricsView("Play a song on Spotify to see the lyrics.");
        }

        // Themes
        private void loadTheme(int themeID)
        {
            switch (themeID)
            {
                case 0 // Light
               :
                    {
                        bgColor = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                        textColor = new SolidColorBrush(Color.FromRgb(24, 24, 24));
                        textColor2 = new SolidColorBrush(Color.FromRgb(10, 10, 10));
                        break;
                    }

                case 1 // Dark
         :
                    {
                        bgColor = new SolidColorBrush(Color.FromRgb(24, 24, 24));
                        textColor = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                        textColor2 = new SolidColorBrush(Color.FromRgb(179, 179, 179));
                        break;
                    }
            }

            // Set colors
            mainGrid.Background = bgColor;
            menuGrid.Background = bgColor;
            headerGrid.Background = bgColor;
            bodyGrid.Background = bgColor;
            footerGrid.Background = bgColor;
            songTitleLabel.Foreground = textColor;
            artistLabel.Foreground = textColor2;
            versionLabel.Foreground = textColor2;
            topmostCheck.Foreground = textColor2;
            countLabel.Foreground = textColor2;
            darkTheme.Foreground = textColor2;
            gradient0.Color = bgColor.Color;
            gradient1.Color = Color.FromArgb(0, bgColor.Color.R, bgColor.Color.G, bgColor.Color.B);
            gradient2.Color = Color.FromArgb(0, bgColor.Color.R, bgColor.Color.G, bgColor.Color.B);
            gradient3.Color = bgColor.Color;

            foreach (ListViewItem s in lyricsView.Items)
            {
                Grid g = (Grid)s.Content;
                TextBlock t = (TextBlock)g.Children[0];
                t.Foreground = textColor;
            }

            // Save settings
           Properties.Settings.Default.theme = themeID;
           Properties.Settings.Default.Save();
        }

        private void darkTheme_Checked(object sender, RoutedEventArgs e)
        {
            loadTheme(1);
        }

        private void darkTheme_Unchecked(object sender, RoutedEventArgs e)
        {
            loadTheme(0);
        }

        private void MainWindow_SizeChanged(object sender, EventArgs e)
        {
            // Save window size
            if (settingsLoaded)
            {
                Properties.Settings.Default.width = Convert.ToInt32(this.Width);
                Properties.Settings.Default.height = Convert.ToInt32(this.Height);
                Properties.Settings.Default.Save();

                foreach (ListViewItem s in lyricsView.Items)
                {
                    Grid g = (Grid)s.Content;
                    g.Width = this.ActualWidth - 50;
                }
            }
        }

        private void Form1_LocationChanged(object sender, EventArgs e)
        {
            // Save window position
            if (settingsLoaded)
            {
               Properties.Settings.Default.xPos = Convert.ToInt32(this.Left);
               Properties.Settings.Default.yPos = Convert.ToInt32(this.Top);
               Properties.Settings.Default.Save();
            }
        }

        public void setBtnStatus(bool status)
        {
            nextBtn.IsEnabled = status;
            prevBtn.IsEnabled = status;
        }

        private void topmostCheck_CheckedChanged(object sender, EventArgs e)
        {
            // Save top most status
            if (settingsLoaded)
            {
               this.Topmost = (bool)topmostCheck.IsChecked;
               Properties.Settings.Default.topMost = (bool)topmostCheck.IsChecked;
               Properties.Settings.Default.Save();
            }
        }

        private void nextBtn_Click(object sender, EventArgs e)
        {
            clearLyricsView();
            addToLyricsView("Loading...");
            if (currentLyricsIndx + 1 >= lyricsURLs.Count)
                setLyrics(0);
            else
                setLyrics(currentLyricsIndx + 1);
        }

        private void prevBtn_Click(object sender, EventArgs e)
        {
            clearLyricsView();
            addToLyricsView("Loading...");
            if (currentLyricsIndx - 1 < 0)
                setLyrics(lyricsURLs.Count - 1);
            else
                setLyrics(currentLyricsIndx - 1);
        }

        private void clearLyricsView()
        {
            lyricsView.Items.Clear();
            lyricsView.UpdateLayout();
            ((ScrollViewer)lyricsView.Parent).ScrollToTop();
        }

        private void addToLyricsView(string s)
        {
            ListViewItem lContainer = new ListViewItem();
            lContainer.IsEnabled = false;
            lContainer.HorizontalAlignment = HorizontalAlignment.Center;

            Grid lGrid = new Grid();
            lGrid.Width = this.Width - 50;
            TextBlock lString = new TextBlock();
            lString.FontFamily = new FontFamily("/Spotify_Lyrics.NET;component/Resources/#Circular Book");
            lString.Foreground = textColor;
            lString.FontSize =Properties.Settings.Default.textSize;
            lString.TextAlignment = TextAlignment.Center;
            lString.Text = s;
            lString.TextWrapping = TextWrapping.WrapWithOverflow;
            lString.Padding = new Thickness(20, 0, 20, 0);

            lGrid.Children.Add(lString);
            lContainer.Content = lGrid;
            lyricsView.Items.Add(lContainer);
        }

        private void ListViewScrollViewer_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)((ListView)sender).Parent;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        private void timerTitle_Tick(object sender, EventArgs e)
        {
            Process[] spotifyProcess = Process.GetProcessesByName("Spotify");

            foreach (Process p in spotifyProcess)
            {
                string songTitle = p.MainWindowTitle;
                if (songTitle != "" & songTitle != "Spotify" & songTitle != currentSongTitle)
                    setSong(songTitle);
            }
        }

        private void setSong(string title)
        {
            currentSongTitle = title;

            if (title.Contains(" - "))
            {
                string artist = title.Substring(0, title.IndexOf(" -"));
                string songTitle = title.Replace(artist + " - ", "");

                songTitleLabel.Text = songTitle.Replace("&", "&&");
                artistLabel.Text = artist;
                getLyrics(artist.Trim(), songTitle.Replace("&", "").Trim());
            }
        }

        private void getLyrics(string artist, string song)
        {
            clearLyricsView();
            addToLyricsView("Searching...");

            // Search the song on Musixmatch
            string searchURL = "https://www.musixmatch.com/search/" + Uri.EscapeDataString(artist) + "-" + Uri.EscapeDataString(song) + "/tracks";
            string response = getHTTPSRequest(searchURL);
            response = response.Replace("\"", "'");

            // Save all the valid search results
            lyricsURLs = new List<String>();
            while (response.Contains("href='/lyrics/"))
            {
                try
                {
                    int a = response.IndexOf("href='/lyrics/");
                    int b = response.IndexOf("'>", a);
                    string link = response.Substring(a, b - a).Replace("href='", "");

                    lyricsURLs.Add("https://www.musixmatch.com" + link);
                    response = response.Substring(b, response.Length - b);
                }
                catch (Exception ex)
                {
                    break;
                }
            }

            // Display the first result if found
            if (lyricsURLs.Count > 0)
            {
                if (lyricsURLs.Count > 1)
                    setBtnStatus(true);
                else
                    setBtnStatus(false);

                setLyrics(0);
            }
            else
                // Try to remove useless strings from the song (e.g. "- Radio Edit", " - (Original Mix)") and search it again
                if (song.Contains("-"))
            {
                song = song.Substring(0, song.IndexOf("-")).Trim();
                getLyrics(artist, song);
            }
            else if (song.Contains("("))
            {
                song = song.Substring(0, song.IndexOf("(")).Trim();
                getLyrics(artist, song);
            }
            else
            {
                clearLyricsView();
                addToLyricsView("I can't find the lyrics, sorry. :(");
                setBtnStatus(false);
                countLabel.Text = "0 of 0";
            }
        }

        public void setLyrics(int indx)
        {
            try
            {
                currentLyricsIndx = indx;
                countLabel.Text = (indx + 1) + " of " + lyricsURLs.Count;

                string responseLyrics = getHTTPSRequest(WebUtility.HtmlEncode(lyricsURLs[indx]));
                HtmlAgilityPack.HtmlDocument lyricsDoc = new HtmlAgilityPack.HtmlDocument();
                lyricsDoc.LoadHtml(responseLyrics);

                var nodes = lyricsDoc.DocumentNode.SelectNodes("//p");
                clearLyricsView();
                string lyricsText = "";
                foreach (HtmlNode p in nodes)
                {
                    try
                    {
                        if (p.HasClass("mxm-lyrics__content"))
                            lyricsText += p.InnerText + Environment.NewLine;
                    }
                    catch
                    {
                    }
                }

                if (lyricsText.Trim().Length > 0)
                    addToLyricsView(lyricsText);
                else
                {
                    clearLyricsView();
                    addToLyricsView("I can't find the lyrics, sorry. :(");
                }
            }
            catch (Exception ex)
            {
            }
        }

        public static string getHTTPSRequest(string strRequest)
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
