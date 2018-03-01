Imports System.Diagnostics.Process
Imports System.IO
Imports System.Net
Imports HtmlAgilityPack

Public Class Form1
    Const appVERSION As String = "v0.2.3"
    Const appBUILD As String = "02.03.2018"
    Const appAuthor As String = "Jakub Stęplowski"
    Const appAuthorWebsite As String = "https://jakubsteplowski.com"
    Const LEFTMARGIN As Integer = 24

    Dim currentSongTitle As String = ""
    Dim currentLyricsIndx As Integer = -1
    Dim lyricsURLs As New ArrayList
    Dim settingsLoaded As Boolean = False

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        versionLabel.Text = appVERSION '& " (" & appBUILD & ")"

        'Load Settings
        loadTheme(My.Settings.theme)
        topmostCheck.Checked = My.Settings.topMost
        Me.TopMost = My.Settings.topMost
        If My.Settings.width > 0 Then
            Me.Width = My.Settings.width
            Me.Height = My.Settings.height
        End If
        If My.Settings.xPos > 0 Then
            Me.Left = My.Settings.xPos
            Me.Top = My.Settings.yPos
        End If
        settingsLoaded = True
    End Sub

#Region "UI"
    'Themes
    Private Sub loadTheme(ByVal themeID As Integer)
        Dim bg1, bg2, txt As Color

        Select Case themeID
            Case 0 'Light (Default)
                bg1 = Color.White
                bg2 = Color.WhiteSmoke
                txt = Color.Black
                ChangeThemeToolStripMenuItem.Image = My.Resources.settings
            Case 1 'Dark
                bg1 = Color.FromArgb(14, 14, 14)
                bg2 = Color.FromArgb(10, 10, 10)
                txt = Color.FromArgb(245, 245, 245)
                ChangeThemeToolStripMenuItem.Image = My.Resources.settings_white
        End Select

        'Set colors
        Me.BackColor = bg1
        Panel1.BackColor = bg2
        separator.BackColor = bg2
        titleLabel.ForeColor = txt
        artistLabel.ForeColor = txt
        lyricsLabel.ForeColor = txt
        countLabel.ForeColor = txt
        versionLabel.ForeColor = txt
        topmostCheck.ForeColor = txt

        'Save settings
        My.Settings.theme = themeID
        My.Settings.Save()
    End Sub

    Private Sub LightToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LightToolStripMenuItem.Click
        loadTheme(0)
    End Sub

    Private Sub DarkToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DarkToolStripMenuItem.Click
        loadTheme(1)
    End Sub

    'Fix the width of the Lyrics label
    Private Sub fixLyricsLabelPosition()
        lyricsLabel.MaximumSize = New Point(Me.Width - (LEFTMARGIN * 2), 0)
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs)
        fixLyricsLabelPosition()
    End Sub

    Private Sub Form1_SizeChanged(sender As Object, e As EventArgs) Handles Me.SizeChanged
        fixLyricsLabelPosition()

        'Save window size
        If settingsLoaded Then
            My.Settings.width = Me.Width
            My.Settings.height = Me.Height
            My.Settings.Save()
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
        nextBtn.Enabled = status
        prevBtn.Enabled = status
    End Sub

    Private Sub topmostCheck_CheckedChanged(sender As Object, e As EventArgs) Handles topmostCheck.CheckedChanged
        'Save top most status
        If settingsLoaded Then
            Me.TopMost = CType(sender, CheckBox).Checked
            My.Settings.topMost = CType(sender, CheckBox).Checked
            My.Settings.Save()
        End If
    End Sub

    Private Sub nextBtn_Click(sender As Object, e As EventArgs) Handles nextBtn.Click
        lyricsLabel.Text = "Loading..."
        If currentLyricsIndx + 1 >= lyricsURLs.Count Then
            setLyrics(0)
        Else
            setLyrics(currentLyricsIndx + 1)
        End If
    End Sub

    Private Sub prevBtn_Click(sender As Object, e As EventArgs) Handles prevBtn.Click
        lyricsLabel.Text = "Loading..."
        If currentLyricsIndx - 1 < 0 Then
            setLyrics(lyricsURLs.Count - 1)
        Else
            setLyrics(currentLyricsIndx - 1)
        End If
    End Sub
#End Region

#Region "Set Lyrics"
    Private Sub timerTitle_Tick(sender As Object, e As EventArgs) Handles timerTitle.Tick
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

            titleLabel.Text = songTitle
            artistLabel.Text = artist
            getLyrics(artist, songTitle)
        End If
    End Sub

    Private Sub getLyrics(ByVal artist As String, ByVal song As String)
        lyricsLabel.Text = "Searching..."

        'Search the song on Musixmatch
        Dim searchURL As String = "https://www.musixmatch.com/search/" & artist & "-" & song & "/tracks"
        Dim response As String = getHTTPSRequest(WebUtility.HtmlEncode(searchURL))
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
            setLyrics(0)
            If lyricsURLs.Count > 1 Then
                setBtnStatus(True)
            Else
                setBtnStatus(False)
            End If
        Else
            lyricsLabel.Text = "I can't find the lyrics, sorry. :("
            setBtnStatus(False)
            countLabel.Text = "0 of 0"
        End If
    End Sub

    Public Sub setLyrics(ByVal indx As Integer)
        currentLyricsIndx = indx
        countLabel.Text = (indx + 1) & " of " & lyricsURLs.Count

        Dim responseLyrics As String = getHTTPSRequest(WebUtility.HtmlEncode(lyricsURLs(indx)))
        Dim lyricsDoc As New HtmlAgilityPack.HtmlDocument()
        lyricsDoc.LoadHtml(responseLyrics)

        Dim nodes = lyricsDoc.DocumentNode.SelectNodes("//p")
        lyricsLabel.Text = ""
        For Each p As HtmlNode In nodes
            Try
                If p.Attributes.Item("class").Value.Contains("mxm-lyrics__content") Then
                    lyricsLabel.Text &= p.InnerText & vbCrLf
                End If
            Catch
            End Try
        Next

        If lyricsLabel.Text.Trim.Length = 0 Then
            lyricsLabel.Text = "I can't find the lyrics, sorry. :("
        End If
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
