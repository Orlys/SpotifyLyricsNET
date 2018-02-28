<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Richiesto da Progettazione Windows Form
    Private components As System.ComponentModel.IContainer

    'NOTA: la procedura che segue è richiesta da Progettazione Windows Form
    'Può essere modificata in Progettazione Windows Form.  
    'Non modificarla mediante l'editor del codice.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.authorLabel = New System.Windows.Forms.Label()
        Me.titleLabel = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.lyricsLabel = New System.Windows.Forms.Label()
        Me.timerTitle = New System.Windows.Forms.Timer(Me.components)
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.countLabel = New System.Windows.Forms.Label()
        Me.topmostCheck = New System.Windows.Forms.CheckBox()
        Me.versionLabel = New System.Windows.Forms.Label()
        Me.prevBtn = New System.Windows.Forms.Button()
        Me.nextBtn = New System.Windows.Forms.Button()
        Me.separator = New System.Windows.Forms.PictureBox()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel3.SuspendLayout()
        CType(Me.separator, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Panel1.Controls.Add(Me.authorLabel)
        Me.Panel1.Controls.Add(Me.titleLabel)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(584, 59)
        Me.Panel1.TabIndex = 0
        '
        'authorLabel
        '
        Me.authorLabel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.authorLabel.Font = New System.Drawing.Font("Segoe UI Semibold", 8.25!)
        Me.authorLabel.Location = New System.Drawing.Point(12, 36)
        Me.authorLabel.Name = "authorLabel"
        Me.authorLabel.Size = New System.Drawing.Size(560, 16)
        Me.authorLabel.TabIndex = 1
        Me.authorLabel.Text = "by Jakub Stęplowski"
        '
        'titleLabel
        '
        Me.titleLabel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.titleLabel.Font = New System.Drawing.Font("Segoe UI", 14.25!)
        Me.titleLabel.Location = New System.Drawing.Point(10, 8)
        Me.titleLabel.Name = "titleLabel"
        Me.titleLabel.Size = New System.Drawing.Size(562, 31)
        Me.titleLabel.TabIndex = 0
        Me.titleLabel.Text = "Spotify Lyrics .NET"
        '
        'Panel2
        '
        Me.Panel2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel2.AutoScroll = True
        Me.Panel2.Controls.Add(Me.lyricsLabel)
        Me.Panel2.Location = New System.Drawing.Point(0, 59)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(584, 463)
        Me.Panel2.TabIndex = 1
        '
        'lyricsLabel
        '
        Me.lyricsLabel.AutoSize = True
        Me.lyricsLabel.Font = New System.Drawing.Font("Segoe UI", 9.25!)
        Me.lyricsLabel.Location = New System.Drawing.Point(12, 18)
        Me.lyricsLabel.Name = "lyricsLabel"
        Me.lyricsLabel.Padding = New System.Windows.Forms.Padding(0, 0, 0, 20)
        Me.lyricsLabel.Size = New System.Drawing.Size(233, 37)
        Me.lyricsLabel.TabIndex = 1
        Me.lyricsLabel.Text = "Play a song on Spotify to see the lyrics"
        '
        'timerTitle
        '
        Me.timerTitle.Enabled = True
        Me.timerTitle.Interval = 500
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.Color.White
        Me.Panel3.Controls.Add(Me.prevBtn)
        Me.Panel3.Controls.Add(Me.countLabel)
        Me.Panel3.Controls.Add(Me.nextBtn)
        Me.Panel3.Controls.Add(Me.topmostCheck)
        Me.Panel3.Controls.Add(Me.versionLabel)
        Me.Panel3.Controls.Add(Me.separator)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel3.Location = New System.Drawing.Point(0, 520)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(584, 41)
        Me.Panel3.TabIndex = 2
        '
        'countLabel
        '
        Me.countLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.countLabel.AutoSize = True
        Me.countLabel.Location = New System.Drawing.Point(220, 14)
        Me.countLabel.Name = "countLabel"
        Me.countLabel.Size = New System.Drawing.Size(188, 13)
        Me.countLabel.TabIndex = 4
        Me.countLabel.Text = "Wrong Song? Try other lyrics: 0 of 0"
        Me.countLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'topmostCheck
        '
        Me.topmostCheck.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.topmostCheck.AutoSize = True
        Me.topmostCheck.Location = New System.Drawing.Point(482, 13)
        Me.topmostCheck.Name = "topmostCheck"
        Me.topmostCheck.Size = New System.Drawing.Size(99, 17)
        Me.topmostCheck.TabIndex = 2
        Me.topmostCheck.Text = "Always on Top"
        Me.topmostCheck.UseVisualStyleBackColor = True
        '
        'versionLabel
        '
        Me.versionLabel.AutoSize = True
        Me.versionLabel.Location = New System.Drawing.Point(12, 14)
        Me.versionLabel.Name = "versionLabel"
        Me.versionLabel.Size = New System.Drawing.Size(0, 13)
        Me.versionLabel.TabIndex = 1
        '
        'prevBtn
        '
        Me.prevBtn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.prevBtn.BackgroundImage = Global.SpotifyLyrics.NET.My.Resources.Resources.left
        Me.prevBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.prevBtn.Location = New System.Drawing.Point(414, 8)
        Me.prevBtn.Name = "prevBtn"
        Me.prevBtn.Size = New System.Drawing.Size(28, 26)
        Me.prevBtn.TabIndex = 5
        Me.prevBtn.UseVisualStyleBackColor = True
        '
        'nextBtn
        '
        Me.nextBtn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.nextBtn.BackgroundImage = Global.SpotifyLyrics.NET.My.Resources.Resources.right
        Me.nextBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.nextBtn.Location = New System.Drawing.Point(448, 8)
        Me.nextBtn.Name = "nextBtn"
        Me.nextBtn.Size = New System.Drawing.Size(28, 26)
        Me.nextBtn.TabIndex = 3
        Me.nextBtn.UseVisualStyleBackColor = True
        '
        'separator
        '
        Me.separator.BackColor = System.Drawing.Color.WhiteSmoke
        Me.separator.Dock = System.Windows.Forms.DockStyle.Top
        Me.separator.Location = New System.Drawing.Point(0, 0)
        Me.separator.Name = "separator"
        Me.separator.Size = New System.Drawing.Size(584, 2)
        Me.separator.TabIndex = 0
        Me.separator.TabStop = False
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(584, 561)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(500, 300)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Spotify Lyrics .NET"
        Me.Panel1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        CType(Me.separator, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents titleLabel As Label
    Friend WithEvents Panel2 As Panel
    Friend WithEvents lyricsLabel As Label
    Friend WithEvents timerTitle As Timer
    Friend WithEvents authorLabel As Label
    Friend WithEvents Panel3 As Panel
    Friend WithEvents versionLabel As Label
    Friend WithEvents separator As PictureBox
    Friend WithEvents topmostCheck As CheckBox
    Friend WithEvents nextBtn As Button
    Friend WithEvents countLabel As Label
    Friend WithEvents prevBtn As Button
End Class
