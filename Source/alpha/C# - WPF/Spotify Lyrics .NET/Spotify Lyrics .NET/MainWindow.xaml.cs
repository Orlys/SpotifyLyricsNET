using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Spotify_Lyrics.NET.API;
using System.Windows.Media.Imaging;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Spotify_Lyrics.NET
{
    /// <summary>
    /// Main View
    /// </summary>
    public partial class MainWindow : Window
    {
        const string appVERSION = "v1.5.0-alpha";
        const string appBUILD = "20.08.2019"; // DD.MM.YYYY
        const string appAuthor = "Jakub Stęplowski";
        const string appAuthorWebsite = "https://jakubsteplowski.com";

        const int fontSizeMIN = 8;
        const int fontSizeMAX = 42;

        public struct lyricsURL
        {
            public string title;
            public string artist;
            public string img;
            public string url;
            public string id;
            public string source;
        }

        private string currentSongTitle = "";
        private int currentLyricsIndx = -1;
        public List<lyricsURL> lyricsURLs = new List<lyricsURL>();
        private bool settingsLoaded = false;
        private DispatcherTimer sTimer;
        public string lyricsText = "";
        public string lyricsTextTemp = "";
        private bool isDownloading = false;
        private bool isMarked = false; // local
        private bool isMarkedFlag = false; // runtime

        private SolidColorBrush bgColor = new SolidColorBrush();
        private SolidColorBrush bgColor2 = new SolidColorBrush();
        private SolidColorBrush textColor = new SolidColorBrush();
        private SolidColorBrush textColor2 = new SolidColorBrush();
        private SolidColorBrush spotifyGreen = new SolidColorBrush(Color.FromRgb(57, 184, 91));

        private UpdateHelper updateH;
        private FileSystemHelper filesysH = new FileSystemHelper();
        private MusixmatchAPI mmAPI = new MusixmatchAPI();
        private GeniusAPI geniusAPI;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            SizeChanged += MainWindow_SizeChanged;
            LocationChanged += MainView_LocationChanged;
            prevBtn.Click += prevBtn_Click;
            nextBtn.Click += nextBtn_Click;
            lyricsView.PreviewMouseWheel += ListViewScrollViewer_PreviewMouseWheel;

            var me = this;
            updateH = new UpdateHelper(ref me, appVERSION);
            geniusAPI = new GeniusAPI(ref me);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            versionLabel.Inlines.Add(appVERSION);
            versionLabel.Inlines.Add(new LineBreak());
            versionLabel.Inlines.Add(appBUILD);

            // Load Settings
            loadTheme(Properties.Settings.Default.theme);
            this.Topmost = Properties.Settings.Default.topMost;
            if (Properties.Settings.Default.topMost)
            {
                topModeBtnText.Foreground = spotifyGreen;
                topModeBtnFlag.Visibility = Visibility.Visible;
                topModeBtn.ToolTip = "Disable \"Always on Top\"";
            }
            if (Properties.Settings.Default.theme == 1)
            {
                darkModeBtnText.Foreground = spotifyGreen;
                darkModeBtnFlag.Visibility = Visibility.Visible;
                darkModeBtn.ToolTip = "Disable \"Dark mode\"";
            }
            if (Properties.Settings.Default.boldFont)
            {
                boldFontBtnText.Foreground = spotifyGreen;
                boldFontBtnFlag.Visibility = Visibility.Visible;
                boldFontBtn.ToolTip = "Disable \"Bold font\"";
            }
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
            if (Properties.Settings.Default.textSize < fontSizeMIN)
                Properties.Settings.Default.textSize = fontSizeMIN;
            else if (Properties.Settings.Default.textSize > fontSizeMAX)
                Properties.Settings.Default.textSize = fontSizeMAX;
            fontSizeText.Text = Properties.Settings.Default.textSize + "px";
            biggerFontBtn.IsEnabled = (Properties.Settings.Default.textSize != fontSizeMAX);
            smallerFontBtn.IsEnabled = (Properties.Settings.Default.textSize != fontSizeMIN);

            this.Opacity = Properties.Settings.Default.opacity;
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
                case 0: // Light
                    {
                        bgColor = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                        bgColor2 = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                        textColor = new SolidColorBrush(Color.FromRgb(24, 24, 24));
                        textColor2 = new SolidColorBrush(Color.FromRgb(10, 10, 10));
                        break;
                    }
                case 1: // Dark
                    {
                        bgColor = new SolidColorBrush(Color.FromRgb(24, 24, 24));
                        bgColor2 = new SolidColorBrush(Color.FromRgb(61, 61, 61));
                        textColor = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                        textColor2 = new SolidColorBrush(Color.FromRgb(179, 179, 179));
                        break;
                    }
            }

            // Set colors
            mainGrid.Background = bgColor;
            menuGrid.Background = bgColor;
            bodyGrid.Background = bgColor;
            footerGrid.Background = bgColor;
            songTitleLabel.Foreground = textColor;
            artistLabel.Foreground = textColor2;
            if (correctMarkBtnFlag.Visibility == Visibility.Collapsed) correctMarkBtnText.Foreground = textColor2;
            correctMarkDescriptionLabel.Foreground = textColor;
            versionLabel.Foreground = textColor2;
            smallerFontBtnText.Foreground = textColor2;
            biggerFontBtnText.Foreground = textColor2;
            fontSizeText.Foreground = textColor2;
            if (!Properties.Settings.Default.boldFont) boldFontBtnText.Foreground = textColor2;
            if (themeID == 0) darkModeBtnText.Foreground = textColor2;
            if (!Properties.Settings.Default.topMost) topModeBtnText.Foreground = textColor2;
            countLabel.Foreground = textColor2;
            gradient0.Color = bgColor.Color;
            gradient1.Color = Color.FromArgb(0, bgColor.Color.R, bgColor.Color.G, bgColor.Color.B);
            gradient2.Color = Color.FromArgb(0, bgColor.Color.R, bgColor.Color.G, bgColor.Color.B);
            gradient3.Color = bgColor.Color;
            gradient4.Color = bgColor2.Color;
            gradient5.Color = bgColor.Color;

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

        private void MainView_LocationChanged(object sender, EventArgs e)
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

        private void nextBtn_Click(object sender, EventArgs e)
        {
            if (!isDownloading)
            {
                clearLyricsView();
                addToLyricsView("Loading...");
                if (currentLyricsIndx + 1 >= lyricsURLs.Count)
                    setLyrics(0);
                else
                    setLyrics(currentLyricsIndx + 1);
            }
        }

        private void prevBtn_Click(object sender, EventArgs e)
        {
            if (!isDownloading)
            {
                clearLyricsView();
                addToLyricsView("Loading...");
                if (currentLyricsIndx - 1 < 0)
                    setLyrics(lyricsURLs.Count - 1);
                else
                    setLyrics(currentLyricsIndx - 1);
            }
        }

        private void clearLyricsView()
        {
            lyricsView.Items.Clear();
            lyricsView.UpdateLayout();
            ((ScrollViewer)lyricsView.Parent).ScrollToTop();
        }

        private void addToLyricsView(string s, bool error = false)
        {
            ListViewItem lContainer = new ListViewItem();
            lContainer.IsEnabled = false;
            lContainer.HorizontalAlignment = HorizontalAlignment.Center;

            Grid lGrid = new Grid();
            lGrid.Width = this.Width - 50;
            TextBlock lString = new TextBlock();
            lString.Style = Properties.Settings.Default.boldFont ? (Style)Application.Current.FindResource("BoldFont") : (Style)Application.Current.FindResource("BookFont");
            lString.Foreground = textColor;
            lString.FontSize = Properties.Settings.Default.textSize;
            lString.FontStretch = FontStretches.UltraExpanded;
            lString.LineHeight = 15;
            lString.TextAlignment = TextAlignment.Center;
            lString.Text = s;
            lString.TextWrapping = TextWrapping.WrapWithOverflow;
            lString.Padding = new Thickness(20, 0, 20, 0);

            if (error)
            {
                lString.Text = "I can't find the lyrics, sorry.";
                lString.LineHeight = 30;
                Run emoji = new Run();
                emoji.Style = (Style)Application.Current.FindResource("IconFont");
                emoji.FontSize = Properties.Settings.Default.textSize + 40;
                emoji.Text = ""; // Sad1: , Sad2: 
                lString.Inlines.Add(new LineBreak());
                lString.Inlines.Add(emoji);
            }

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
                if (songTitle != "" && songTitle != "Spotify" && songTitle != currentSongTitle)
                    setSong(songTitle);
            }
        }

        private async void setSong(string title, bool bypass = false)
        {
            if ((!title.Contains("Spotify") && title != currentSongTitle) || (bypass))
            {
                currentSongTitle = title;

                if (title.Contains(" - "))
                {
                    string artist = title.Substring(0, title.IndexOf(" -"));
                    string songTitle = title.Replace(artist + " - ", "");

                    isMarked = false;
                    correctMarkDescription.Visibility = Visibility.Collapsed;
                    navigationGrid.Visibility = Visibility.Visible;
                    correctMarkBtnText.Foreground = textColor2;
                    correctMarkBtnFlag.Visibility = Visibility.Collapsed;
                    correctMarkBtn.ToolTip = "Mark as \"Correct Lyrics\"";

                    songTitleLabel.Text = songTitle; //.Replace("&", "&&");
                    songTitleLabel.ToolTip = songTitle;
                    artistLabel.Text = artist;

                    if (!bypass)
                    {
                        try
                        {
                            List<string> currentSongLyrics = filesysH.getLyrics(currentSongTitle);
                            string currentSongLyricsId = currentSongLyrics[0],
                                   currentSongLyricsCoverImg = currentSongLyrics[1],
                                   currentSongLyricsUrl = currentSongLyrics[2];

                            if (currentSongLyricsUrl.Length > 0 && Uri.IsWellFormedUriString(currentSongLyricsUrl, UriKind.Absolute))
                            {
                                isMarked = true;

                                setBtnStatus(false);
                                clearLyricsView();
                                coverImage.Visibility = Visibility.Collapsed;
                                sourceLabel.Text = "";
                                countLabel.Text = "...";
                                addToLyricsView("Searching...");

                                lyricsURLs = new List<lyricsURL>();

                                correctMarkDescription.Visibility = Visibility.Visible;
                                navigationGrid.Visibility = Visibility.Collapsed;
                                correctMarkBtnText.Foreground = spotifyGreen;
                                correctMarkBtnFlag.Visibility = Visibility.Visible;
                                correctMarkBtn.ToolTip = "Remove \"Correct Lyrics\" mark";

                                if (currentSongLyricsUrl.Contains("musixmatch"))
                                {
                                    mmAPI.getLyrics("", "", ref lyricsURLs, currentSongLyricsUrl);
                                }
                                else
                                {
                                    await geniusAPI.getLyrics("", "", currentSongLyricsUrl, currentSongLyricsCoverImg, currentSongLyricsId);
                                }

                                if (lyricsURLs.Count > 0)
                                {
                                    setLyrics(0);
                                }
                                else
                                {
                                    clearLyricsView();
                                    sourceLabel.Text = "";
                                    coverImage.Visibility = Visibility.Collapsed;
                                    addToLyricsView("", true); // Error: Can't find the lyrics
                                    countLabel.Text = "0 of 0";
                                }
                            }
                        }
                        catch (Exception ex) { }
                    }

                    if (!isMarked)
                        getLyrics(artist.Trim(), songTitle.Replace("&", "").Trim());
                }
            }
        }

        private async void getLyrics(string artist, string song)
        {
            setBtnStatus(false);
            clearLyricsView();
            coverImage.Visibility = Visibility.Collapsed;
            sourceLabel.Text = "";
            countLabel.Text = "...";
            addToLyricsView("Searching...");

            lyricsURLs = new List<lyricsURL>();

            // Search the song on Musixmatch
            mmAPI.getLyrics(artist, song, ref lyricsURLs);

            // Search the song on Genius
            await geniusAPI.getLyrics(artist, song);

            // Display the first result if found
            await findFirstLyrics();

            if (lyricsURLs.Count > 0)
            {
                if (lyricsURLs.Count > 1)
                    setBtnStatus(true);

                setLyrics(0);

                // Check all the next lyrics in background
                //checkLyrics(lyricsURLs.Count, true);
            }
            else if (song.Contains("-"))
            {
                // Try to remove useless strings from the song (e.g. "- Radio Edit", " - (Original Mix)") and search it again
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
                sourceLabel.Text = "";
                coverImage.Visibility = Visibility.Collapsed;
                addToLyricsView("", true); // Error: Can't find the lyrics
                countLabel.Text = "0 of 0";
            }
        }

        public async Task findFirstLyrics()
        {
            clearLyricsView();
            addToLyricsView("Checking...");

            string img = "";
            List<lyricsURL> toRemove = new List<lyricsURL>();
            for (int indx = 0; indx < lyricsURLs.Count; indx++)
            {
                lyricsTextTemp = "";

                switch (lyricsURLs[indx].source)
                {
                    case "Musixmatch":
                        lyricsTextTemp = mmAPI.setLyrics(indx, ref lyricsURLs, ref img);
                        break;
                    case "Genius":
                        await geniusAPI.setLyrics(indx, true);
                        break;
                }

                if (lyricsTextTemp.Trim().Length == 0)
                    toRemove.Add(lyricsURLs[indx]);
                else
                    break;
            }

            foreach (lyricsURL ly in toRemove)
            {
                lyricsURLs.Remove(ly);
            }
        }

        public async Task checkLyrics(int count, bool inBackground = false)
        {
            try
            {
                if (!inBackground)
                {
                    clearLyricsView();
                    addToLyricsView("Checking...");
                }
                else
                {
                    countLabel.Text = (currentLyricsIndx + 1) + " of ...";
                    setBtnStatus(false);
                }

                if (count > lyricsURLs.Count)
                {
                    count = lyricsURLs.Count;
                }

                string img = "";
                List<lyricsURL> toRemove = new List<lyricsURL>();
                for (int indx = 0; indx < count; indx++)
                {
                    lyricsTextTemp = "";

                    switch (lyricsURLs[indx].source)
                    {
                        case "Musixmatch":
                            lyricsTextTemp = mmAPI.setLyrics(indx, ref lyricsURLs, ref img);
                            break;
                        case "Genius":
                            await geniusAPI.setLyrics(indx, true);
                            break;
                    }

                    if (lyricsTextTemp.Trim().Length == 0)
                    {
                        toRemove.Add(lyricsURLs[indx]);
                    }
                }

                foreach (lyricsURL ly in toRemove)
                {
                    lyricsURLs.Remove(ly);
                }

                countLabel.Text = (currentLyricsIndx + 1) + " of " + lyricsURLs.Count;
                if (lyricsURLs.Count > 1 && inBackground)
                    setBtnStatus(true);
            }
            catch (Exception ex) { }
        }

        public async void setLyrics(int indx)
        {
            try
            {
                if (!isDownloading)
                {
                    if (lyricsURLs.Count > 1) setBtnStatus(false);
                    isDownloading = true;

                    try
                    {
                        currentLyricsIndx = indx;
                        countLabel.Text = (indx + 1) + " of " + lyricsURLs.Count;

                        clearLyricsView();

                        lyricsText = "";

                        addToLyricsView("Downloading...");
                        sourceLabel.Text = "Lyrics from ";

                        Hyperlink sourceLink = new Hyperlink();
                        sourceLink.Inlines.Add(lyricsURLs[indx].source);
                        sourceLink.Foreground = new SolidColorBrush(Colors.Gray);
                        sourceLink.NavigateUri = new Uri(lyricsURLs[indx].url);
                        sourceLink.RequestNavigate += Hyperlink_RequestNavigate;
                        sourceLabel.Inlines.Add(sourceLink);

                        var coverImg = "";
                        switch (lyricsURLs[indx].source)
                        {
                            case "Musixmatch":
                                lyricsText = mmAPI.setLyrics(indx, ref lyricsURLs, ref coverImg);
                                break;
                            case "Genius":
                                await geniusAPI.setLyrics(indx);
                                coverImg = lyricsURLs[indx].img;
                                break;
                        }

                        if (coverImg != "")
                        {
                            BitmapImage cover = new BitmapImage();
                            cover.BeginInit();
                            cover.UriSource = new Uri(coverImg);
                            cover.EndInit();
                            coverImage.Source = cover;
                            coverImage.Visibility = Visibility.Visible;
                        }
                        else if (coverImage.Visibility == Visibility.Visible)
                        {
                            coverImage.Visibility = Visibility.Collapsed;
                        }

                        if (lyricsText.Trim().Length > 0)
                        {
                            clearLyricsView();
                            addToLyricsView(lyricsText);
                        }
                        else
                        {
                            clearLyricsView();
                            sourceLabel.Text = "";
                            coverImage.Visibility = Visibility.Collapsed;
                            addToLyricsView("", true); // Error: Can't find the lyrics
                        }
                    }
                    catch (Exception ex)
                    {
                        clearLyricsView();
                        sourceLabel.Text = "";
                        coverImage.Visibility = Visibility.Collapsed;
                        addToLyricsView("", true); // Error: Can't find the lyrics
                    }

                    isDownloading = false;
                    if (lyricsURLs.Count > 1) setBtnStatus(true);
                }
            }
            catch (Exception ex) { }
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

        private static void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void BoldFontBtn_Click(object sender, RoutedEventArgs e)
        {
            // Save bold font status
            if (settingsLoaded)
            {
                if (Properties.Settings.Default.boldFont)
                {
                    boldFontBtnText.Foreground = textColor2;
                    boldFontBtnFlag.Visibility = Visibility.Collapsed;
                    boldFontBtn.ToolTip = "Enable \"Bold font\"";
                    Properties.Settings.Default.boldFont = false;
                }
                else
                {
                    boldFontBtnText.Foreground = spotifyGreen;
                    boldFontBtnFlag.Visibility = Visibility.Visible;
                    boldFontBtn.ToolTip = "Disable \"Bold font\"";
                    Properties.Settings.Default.boldFont = true;
                }
                Properties.Settings.Default.Save();

                UpdateFont();
            }
        }

        private void UpdateFont()
        {
            foreach (ListViewItem s in lyricsView.Items)
            {
                Grid g = (Grid)s.Content;
                TextBlock t = (TextBlock)g.Children[0];
                t.Style = Properties.Settings.Default.boldFont ? (Style)Application.Current.FindResource("BoldFont") : (Style)Application.Current.FindResource("BookFont");
                t.FontSize = Properties.Settings.Default.textSize;
            }
        }

        private void BiggerFontBtn_Click(object sender, RoutedEventArgs e)
        {
            // Set bigger font
            if (settingsLoaded && Properties.Settings.Default.textSize < fontSizeMAX)
            {

                Properties.Settings.Default.textSize++;
                Properties.Settings.Default.Save();

                if (!smallerFontBtn.IsEnabled && fontSizeMIN != fontSizeMAX)
                    smallerFontBtn.IsEnabled = true;
                if (Properties.Settings.Default.textSize == fontSizeMAX)
                    biggerFontBtn.IsEnabled = false;

                fontSizeText.Text = Properties.Settings.Default.textSize + "px";

                UpdateFont();
            }
        }

        private void SmallerFontBtn_Click(object sender, RoutedEventArgs e)
        {
            // Set smaller font
            if (settingsLoaded && Properties.Settings.Default.textSize > fontSizeMIN)
            {
                Properties.Settings.Default.textSize--;
                Properties.Settings.Default.Save();

                if (!biggerFontBtn.IsEnabled && fontSizeMIN != fontSizeMAX)
                    biggerFontBtn.IsEnabled = true;
                if (Properties.Settings.Default.textSize == fontSizeMIN)
                    smallerFontBtn.IsEnabled = false;

                fontSizeText.Text = Properties.Settings.Default.textSize + "px";

                UpdateFont();
            }
        }

        private void DarkModeBtn_Click(object sender, RoutedEventArgs e)
        {
            // Save dark mode status
            if (settingsLoaded)
            {
                if (Properties.Settings.Default.theme == 1)
                {
                    darkModeBtnText.Foreground = textColor2;
                    darkModeBtnFlag.Visibility = Visibility.Collapsed;
                    darkModeBtn.ToolTip = "Enable \"Dark mode\"";
                    Properties.Settings.Default.theme = 0;
                }
                else
                {
                    darkModeBtnText.Foreground = spotifyGreen;
                    darkModeBtnFlag.Visibility = Visibility.Visible;
                    darkModeBtn.ToolTip = "Disable \"Dark mode\"";
                    Properties.Settings.Default.theme = 1;
                }
                Properties.Settings.Default.Save();

                loadTheme(Properties.Settings.Default.theme);
            }
        }

        private void TopModeBtn_Click(object sender, RoutedEventArgs e)
        {
            // Save top most status
            if (settingsLoaded)
            {
                if (Properties.Settings.Default.topMost)
                {
                    topModeBtnText.Foreground = textColor2;
                    topModeBtnFlag.Visibility = Visibility.Collapsed;
                    topModeBtn.ToolTip = "Enable \"Always on Top\"";
                    Properties.Settings.Default.topMost = false;
                }
                else
                {
                    topModeBtnText.Foreground = spotifyGreen;
                    topModeBtnFlag.Visibility = Visibility.Visible;
                    topModeBtn.ToolTip = "Disable \"Always on Top\"";
                    Properties.Settings.Default.topMost = true;
                }
                Properties.Settings.Default.Save();

                this.Topmost = Properties.Settings.Default.topMost;
            }
        }

        private void CorrectMarkBtn_Click(object sender, RoutedEventArgs e)
        {
            if (isMarked)
            {
                filesysH.removeLyrics(currentSongTitle);
                setSong(currentSongTitle, true);
            }
            else
            {
                if (isMarkedFlag)
                {
                    if (filesysH.removeLyrics(currentSongTitle))
                    {
                        correctMarkDescription.Visibility = Visibility.Collapsed;
                        navigationGrid.Visibility = Visibility.Visible;
                        correctMarkBtnText.Foreground = textColor2;
                        correctMarkBtnFlag.Visibility = Visibility.Collapsed;
                        correctMarkBtn.ToolTip = "Mark as \"Correct Lyrics\"";
                        isMarkedFlag = false;
                    }
                }
                else if (lyricsURLs.Count > 0)
                {
                    if (filesysH.saveLyrics(currentSongTitle, lyricsURLs[currentLyricsIndx].id, lyricsURLs[currentLyricsIndx].img, lyricsURLs[currentLyricsIndx].url))
                    {
                        correctMarkDescription.Visibility = Visibility.Visible;
                        navigationGrid.Visibility = Visibility.Collapsed;
                        correctMarkBtnText.Foreground = spotifyGreen;
                        correctMarkBtnFlag.Visibility = Visibility.Visible;
                        correctMarkBtn.ToolTip = "Remove \"Correct Lyrics\" mark";
                        isMarkedFlag = true;
                    }
                }
            }
        }
    }
}
