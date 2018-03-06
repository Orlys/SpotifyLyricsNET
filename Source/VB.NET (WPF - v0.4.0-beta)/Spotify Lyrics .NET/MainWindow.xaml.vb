Imports System.Diagnostics.Process
Imports System.IO
Imports System.Net
Imports System.Windows.Threading
Imports HtmlAgilityPack

Class MainWindow
    Const appVERSION As String = "v0.4.0-beta"
    Const appBUILD As String = "06.03.2018"
    Const appAuthor As String = "Jakub Stęplowski"
    Const appAuthorWebsite As String = "https://jakubsteplowski.com"

    Dim currentSongTitle As String = ""
    Dim currentLyricsIndx As Integer = -1
    Dim lyricsURLs As New ArrayList
    Dim settingsLoaded As Boolean = False
    Private sTimer As DispatcherTimer

    Dim bgColor As New SolidColorBrush
    Dim textColor As New SolidColorBrush
    Dim textColor2 As New SolidColorBrush

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        versionLabel.Text = appVERSION

        'Load Settings
        loadTheme(My.Settings.theme)
        topmostCheck.IsChecked = My.Settings.topMost
        If My.Settings.theme = 1 Then
            darkTheme.IsChecked = True
        End If
        Me.Topmost = My.Settings.topMost
        If My.Settings.width > 0 Then
            Me.Width = My.Settings.width
            Me.Height = My.Settings.height
        End If
        If My.Settings.xPos > 0 Then
            Me.Left = My.Settings.xPos
            Me.Top = My.Settings.yPos
        End If
        Me.Opacity = My.Settings.opacity
        settingsLoaded = True

        'Start Timer
        sTimer = New DispatcherTimer
        sTimer.Interval = TimeSpan.FromMilliseconds(500)
        AddHandler sTimer.Tick, AddressOf timerTitle_Tick
        sTimer.Start()

        addToLyricsView("Play a song on Spotify to see the lyrics.")
    End Sub

#Region "UI"
    'Themes
    Private Sub loadTheme(ByVal themeID As Integer)
        Select Case themeID
            Case 0 'Light
                bgColor = New SolidColorBrush(Color.FromRgb(255, 255, 255))
                textColor = New SolidColorBrush(Color.FromRgb(24, 24, 24))
                textColor2 = New SolidColorBrush(Color.FromRgb(10, 10, 10))
            Case 1 'Dark
                bgColor = New SolidColorBrush(Color.FromRgb(24, 24, 24))
                textColor = New SolidColorBrush(Color.FromRgb(255, 255, 255))
                textColor2 = New SolidColorBrush(Color.FromRgb(179, 179, 179))
        End Select

        'Set colors
        mainGrid.Background = bgColor
        menuGrid.Background = bgColor
        headerGrid.Background = bgColor
        bodyGrid.Background = bgColor
        footerGrid.Background = bgColor
        songTitleLabel.Foreground = textColor
        artistLabel.Foreground = textColor2
        versionLabel.Foreground = textColor2
        topmostCheck.Foreground = textColor2
        countLabel.Foreground = textColor2
        darkTheme.Foreground = textColor2
        gradient0.Color = bgColor.Color
        gradient1.Color = Color.FromArgb(0, bgColor.Color.R, bgColor.Color.G, bgColor.Color.B)
        gradient2.Color = Color.FromArgb(0, bgColor.Color.R, bgColor.Color.G, bgColor.Color.B)
        gradient3.Color = bgColor.Color

        For Each s As ListViewItem In lyricsView.Items
            Dim g As Grid = s.Content
            Dim t As TextBlock = g.Children(0)
            t.Foreground = textColor
        Next

        'Save settings
        My.Settings.theme = themeID
        My.Settings.Save()
    End Sub

    Private Sub darkTheme_Checked(sender As Object, e As RoutedEventArgs) Handles darkTheme.Checked
        loadTheme(1)
    End Sub

    Private Sub darkTheme_Unchecked(sender As Object, e As RoutedEventArgs) Handles darkTheme.Unchecked
        loadTheme(0)
    End Sub

    Private Sub Form1_SizeChanged(sender As Object, e As EventArgs) Handles Me.SizeChanged
        'Save window size
        If settingsLoaded Then
            My.Settings.width = Me.Width
            My.Settings.height = Me.Height
            My.Settings.Save()

            For Each s As ListViewItem In lyricsView.Items
                Dim g As Grid = s.Content
                g.Width = Me.ActualWidth - 50
            Next
        End If
    End Sub

    Private Sub Form1_LocationChanged(sender As Object, e As EventArgs) Handles Me.LocationChanged
        'Save window position
        If settingsLoaded Then
            My.Settings.xPos = Me.Left
            My.Settings.yPos = Me.Top
            My.Settings.Save()
        End If
    End Sub

    Public Sub setBtnStatus(ByVal status As Boolean)
        nextBtn.IsEnabled = status
        prevBtn.IsEnabled = status
    End Sub

    Private Sub topmostCheck_CheckedChanged(sender As Object, e As EventArgs) Handles topmostCheck.Checked, topmostCheck.Unchecked
        'Save top most status
        If settingsLoaded Then
            Me.Topmost = topmostCheck.IsChecked
            My.Settings.topMost = topmostCheck.IsChecked
            My.Settings.Save()
        End If
    End Sub

    Private Sub nextBtn_Click(sender As Object, e As EventArgs) Handles nextBtn.Click
        clearLyricsView()
        addToLyricsView("Loading...")
        If currentLyricsIndx + 1 >= lyricsURLs.Count Then
            setLyrics(0)
        Else
            setLyrics(currentLyricsIndx + 1)
        End If
    End Sub

    Private Sub prevBtn_Click(sender As Object, e As EventArgs) Handles prevBtn.Click
        clearLyricsView()
        addToLyricsView("Loading...")
        If currentLyricsIndx - 1 < 0 Then
            setLyrics(lyricsURLs.Count - 1)
        Else
            setLyrics(currentLyricsIndx - 1)
        End If
    End Sub

    Private Sub clearLyricsView()
        lyricsView.Items.Clear()
        lyricsView.UpdateLayout()
    End Sub

    Private Sub addToLyricsView(ByVal s As String)
        Dim lContainer As New ListViewItem
        lContainer.IsEnabled = False
        lContainer.HorizontalAlignment = HorizontalAlignment.Center

        Dim lGrid As New Grid
        lGrid.Width = Me.Width - 50
        Dim lString As New TextBlock
        lString.FontFamily = New FontFamily("/Spotify_Lyrics.NET;component/Resources/#Circular Book")
        lString.Foreground = textColor
        lString.FontSize = My.Settings.textSize
        lString.TextAlignment = TextAlignment.Center
        lString.Text = s
        lString.TextWrapping = TextWrapping.WrapWithOverflow
        lString.Padding = New Thickness(20, 0, 20, 0)

        lGrid.Children.Add(lString)
        lContainer.Content = lGrid
        lyricsView.Items.Add(lContainer)
    End Sub

    Private Sub ListViewScrollViewer_PreviewMouseWheel(ByVal sender As Object, ByVal e As System.Windows.Input.MouseWheelEventArgs) Handles lyricsView.PreviewMouseWheel
        Dim scv As ScrollViewer = sender.parent
        scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta)
        e.Handled = True
    End Sub
#End Region

#Region "Set Lyrics"
    Private Sub timerTitle_Tick(sender As Object, e As EventArgs)
        Dim spotifyProcess As Process() = GetProcessesByName("Spotify")

        For Each p As Process In spotifyProcess
            Dim songTitle As String = p.MainWindowTitle
            If songTitle <> "" And songTitle <> "Spotify" And songTitle <> currentSongTitle Then
                setSong(songTitle)
            End If
        Next
    End Sub

    Private Sub setSong(ByVal title As String)
        currentSongTitle = title

        If title.Contains(" - ") Then
            Dim artist As String = title.Substring(0, title.IndexOf(" -"))
            Dim songTitle As String = title.Replace(artist & " - ", "")

            songTitleLabel.Text = songTitle.Replace("&", "&&")
            artistLabel.Text = artist
            getLyrics(artist.Trim, songTitle.Replace("&", "").Trim)
        End If
    End Sub

    Private Sub getLyrics(ByVal artist As String, ByVal song As String)
        clearLyricsView()
        addToLyricsView("Searching...")

        'Search the song on Musixmatch
        Dim searchURL As String = "https://www.musixmatch.com/search/" & Uri.EscapeDataString(artist) & "-" & Uri.EscapeDataString(song) & "/tracks"
        Dim response As String = getHTTPSRequest(searchURL)
        response = response.Replace("""", "'")

        'Save all the valid search results
        lyricsURLs = New ArrayList
        While response.Contains("href='/lyrics/")
            Try
                Dim a As Integer = response.IndexOf("href='/lyrics/")
                Dim b As Integer = response.IndexOf("'>", a)
                Dim link As String = response.Substring(a, b - a).Replace("href='", "")

                lyricsURLs.Add("https://www.musixmatch.com" & link)
                response = response.Substring(b, response.Length - b)
            Catch ex As Exception
                Exit While
            End Try
        End While

        'Display the first result if found
        If lyricsURLs.Count > 0 Then
            If lyricsURLs.Count > 1 Then
                setBtnStatus(True)
            Else
                setBtnStatus(False)
            End If

            setLyrics(0)
        Else
            clearLyricsView()
            addToLyricsView("I can't find the lyrics, sorry. :(")
            setBtnStatus(False)
            countLabel.Text = "0 of 0"
        End If
    End Sub

    Public Sub setLyrics(ByVal indx As Integer)
        Try
            currentLyricsIndx = indx
            countLabel.Text = (indx + 1) & " of " & lyricsURLs.Count

            Dim responseLyrics As String = getHTTPSRequest(WebUtility.HtmlEncode(lyricsURLs(indx)))
            Dim lyricsDoc As New HtmlAgilityPack.HtmlDocument()
            lyricsDoc.LoadHtml(responseLyrics)

            Dim nodes = lyricsDoc.DocumentNode.SelectNodes("//p")
            clearLyricsView()
            Dim lyricsText As String = ""
            For Each p As HtmlNode In nodes
                Try
                    If p.Attributes.Item("class").Value.Contains("mxm-lyrics__content") Then
                        lyricsText &= p.InnerText & vbNewLine
                    End If
                Catch
                End Try
            Next

            If lyricsText.Trim.Length > 0 Then
                addToLyricsView(lyricsText)
            Else
                clearLyricsView()
                addToLyricsView("I can't find the lyrics, sorry. :(")
            End If
        Catch ex As Exception
        End Try
    End Sub
#End Region

#Region "Web Requests"
    Public Shared Function getHTTPSRequest(ByVal strRequest As String) As String
        Try
            Dim ThisRequest As WebRequest = WebRequest.Create(strRequest)
            ThisRequest.ContentType = "application/x-www-form-urlencoded"
            ThisRequest.Method = "GET"

            Dim Encoder As New System.Text.ASCIIEncoding
            Dim BytesToSend As Byte() = Encoder.GetBytes(strRequest)
            ServicePointManager.Expect100Continue = True
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

            Dim TheirResponse As HttpWebResponse = ThisRequest.GetResponse

            Dim sr As New StreamReader(TheirResponse.GetResponseStream)
            Dim strResponse As String = sr.ReadToEnd
            sr.Close()

            Return strResponse
        Catch ex As Exception
            Return ""
        End Try
    End Function
#End Region
End Class
