Imports System.Diagnostics.Process
Imports System.IO
Imports System.Net
Imports HtmlAgilityPack

Public Class Form1
    Const appVERSION As String = "v0.1.0"
    Const appBUILD As String = "28.02.2018"
    Const appAuthor As String = "Jakub Stęplowski"
    Const appAuthorWebsite As String = "https://jakubsteplowski.com"
    Const LEFTMARGIN As Integer = 24

    Dim currentSongTitle As String = ""
    Dim currentLyricsIndx As Integer = -1
    Dim lyricsURLs As New ArrayList

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        versionLabel.Text = appVERSION & " (" & appBUILD & ")"
    End Sub

#Region "UI"
    'Fix the width of the Lyrics label
    Private Sub fixLyricsLabelPosition()
        lyricsLabel.MaximumSize = New Point(Me.Width - (LEFTMARGIN * 2), 0)
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs)
        fixLyricsLabelPosition()
    End Sub

    Private Sub Form1_SizeChanged(sender As Object, e As EventArgs) Handles Me.SizeChanged
        fixLyricsLabelPosition()
    End Sub

    Public Sub setBtnStatus(ByVal status As Boolean)
        nextBtn.Enabled = status
        prevBtn.Enabled = status
    End Sub

    Private Sub topmostCheck_CheckedChanged(sender As Object, e As EventArgs) Handles topmostCheck.CheckedChanged
        Me.TopMost = CType(sender, CheckBox).Checked
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

        Dim titleParser As New ArrayList
        titleParser.AddRange(title.Split("-"))

        If titleParser.Count > 1 Then
            titleLabel.Text = titleParser(1).ToString.Trim
            authorLabel.Text = titleParser(0).ToString.Trim
            getLyrics(titleParser(0).ToString.Trim, titleParser(1).ToString.Trim)
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
            countLabel.Text = "Wrong Song? Try other lyrics: 0 of 0"
        End If
    End Sub

    Public Sub setLyrics(ByVal indx As Integer)
        currentLyricsIndx = indx
        countLabel.Text = "Wrong Song? Try other lyrics: " & (indx + 1) & " of " & lyricsURLs.Count

        Dim responseLyrics As String = getHTTPSRequest(WebUtility.HtmlEncode(lyricsURLs(indx)))
        Dim lyricsDoc As New HtmlAgilityPack.HtmlDocument()
        lyricsDoc.LoadHtml(responseLyrics)

        Dim nodes = lyricsDoc.DocumentNode.SelectNodes("//p")
        lyricsLabel.Text = ""
        For Each p As HtmlNode In nodes
            Try
                If p.Attributes.Item("class").Value.Contains("mxm-lyrics__content") Then
                    lyricsLabel.Text &= p.InnerText
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
