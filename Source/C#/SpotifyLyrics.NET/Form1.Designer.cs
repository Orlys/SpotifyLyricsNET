namespace SpotifyLyrics.NET
{
    partial class Form1
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.Panel2 = new System.Windows.Forms.Panel();
            this.lyricsLabel = new System.Windows.Forms.Label();
            this.Panel1 = new System.Windows.Forms.Panel();
            this.authorLabel = new System.Windows.Forms.Label();
            this.titleLabel = new System.Windows.Forms.Label();
            this.timerTitle = new System.Windows.Forms.Timer(this.components);
            this.Panel3 = new System.Windows.Forms.Panel();
            this.prevBtn = new System.Windows.Forms.Button();
            this.countLabel = new System.Windows.Forms.Label();
            this.nextBtn = new System.Windows.Forms.Button();
            this.topmostCheck = new System.Windows.Forms.CheckBox();
            this.versionLabel = new System.Windows.Forms.Label();
            this.separator = new System.Windows.Forms.PictureBox();
            this.Panel2.SuspendLayout();
            this.Panel1.SuspendLayout();
            this.Panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.separator)).BeginInit();
            this.SuspendLayout();
            // 
            // Panel2
            // 
            this.Panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Panel2.AutoScroll = true;
            this.Panel2.Controls.Add(this.lyricsLabel);
            this.Panel2.Location = new System.Drawing.Point(0, 59);
            this.Panel2.Name = "Panel2";
            this.Panel2.Size = new System.Drawing.Size(584, 463);
            this.Panel2.TabIndex = 4;
            // 
            // lyricsLabel
            // 
            this.lyricsLabel.AutoSize = true;
            this.lyricsLabel.Font = new System.Drawing.Font("Segoe UI", 9.25F);
            this.lyricsLabel.Location = new System.Drawing.Point(12, 18);
            this.lyricsLabel.Name = "lyricsLabel";
            this.lyricsLabel.Padding = new System.Windows.Forms.Padding(0, 0, 0, 20);
            this.lyricsLabel.Size = new System.Drawing.Size(233, 37);
            this.lyricsLabel.TabIndex = 1;
            this.lyricsLabel.Text = "Play a song on Spotify to see the lyrics";
            // 
            // Panel1
            // 
            this.Panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Panel1.Controls.Add(this.authorLabel);
            this.Panel1.Controls.Add(this.titleLabel);
            this.Panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.Panel1.Location = new System.Drawing.Point(0, 0);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(584, 59);
            this.Panel1.TabIndex = 3;
            this.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.Panel1_Paint);
            // 
            // authorLabel
            // 
            this.authorLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.authorLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F);
            this.authorLabel.Location = new System.Drawing.Point(12, 36);
            this.authorLabel.Name = "authorLabel";
            this.authorLabel.Size = new System.Drawing.Size(560, 16);
            this.authorLabel.TabIndex = 1;
            this.authorLabel.Text = "by Jakub Stęplowski";
            // 
            // titleLabel
            // 
            this.titleLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.titleLabel.Font = new System.Drawing.Font("Segoe UI", 14.25F);
            this.titleLabel.Location = new System.Drawing.Point(10, 8);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(562, 31);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "Spotify Lyrics .NET";
            // 
            // timerTitle
            // 
            this.timerTitle.Enabled = true;
            this.timerTitle.Interval = 500;
            this.timerTitle.Tick += new System.EventHandler(this.timerTitle_Tick);
            // 
            // Panel3
            // 
            this.Panel3.BackColor = System.Drawing.Color.White;
            this.Panel3.Controls.Add(this.prevBtn);
            this.Panel3.Controls.Add(this.countLabel);
            this.Panel3.Controls.Add(this.nextBtn);
            this.Panel3.Controls.Add(this.topmostCheck);
            this.Panel3.Controls.Add(this.versionLabel);
            this.Panel3.Controls.Add(this.separator);
            this.Panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Panel3.Location = new System.Drawing.Point(0, 520);
            this.Panel3.Name = "Panel3";
            this.Panel3.Size = new System.Drawing.Size(584, 41);
            this.Panel3.TabIndex = 5;
            // 
            // prevBtn
            // 
            this.prevBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.prevBtn.BackgroundImage = global::SpotifyLyrics.NET.Properties.Resources.left;
            this.prevBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.prevBtn.Location = new System.Drawing.Point(414, 8);
            this.prevBtn.Name = "prevBtn";
            this.prevBtn.Size = new System.Drawing.Size(28, 26);
            this.prevBtn.TabIndex = 5;
            this.prevBtn.UseVisualStyleBackColor = true;
            this.prevBtn.Click += new System.EventHandler(this.prevBtn_Click);
            // 
            // countLabel
            // 
            this.countLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.countLabel.AutoSize = true;
            this.countLabel.Location = new System.Drawing.Point(220, 14);
            this.countLabel.Name = "countLabel";
            this.countLabel.Size = new System.Drawing.Size(188, 13);
            this.countLabel.TabIndex = 4;
            this.countLabel.Text = "Wrong Song? Try other lyrics: 0 of 0";
            this.countLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nextBtn
            // 
            this.nextBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.nextBtn.BackgroundImage = global::SpotifyLyrics.NET.Properties.Resources.right;
            this.nextBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.nextBtn.Location = new System.Drawing.Point(448, 8);
            this.nextBtn.Name = "nextBtn";
            this.nextBtn.Size = new System.Drawing.Size(28, 26);
            this.nextBtn.TabIndex = 3;
            this.nextBtn.UseVisualStyleBackColor = true;
            this.nextBtn.Click += new System.EventHandler(this.nextBtn_Click);
            // 
            // topmostCheck
            // 
            this.topmostCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.topmostCheck.AutoSize = true;
            this.topmostCheck.Location = new System.Drawing.Point(482, 13);
            this.topmostCheck.Name = "topmostCheck";
            this.topmostCheck.Size = new System.Drawing.Size(99, 17);
            this.topmostCheck.TabIndex = 2;
            this.topmostCheck.Text = "Always on Top";
            this.topmostCheck.UseVisualStyleBackColor = true;
            this.topmostCheck.CheckedChanged += new System.EventHandler(this.topmostCheck_CheckedChanged);
            // 
            // versionLabel
            // 
            this.versionLabel.AutoSize = true;
            this.versionLabel.Location = new System.Drawing.Point(12, 14);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(0, 13);
            this.versionLabel.TabIndex = 1;
            // 
            // separator
            // 
            this.separator.BackColor = System.Drawing.Color.WhiteSmoke;
            this.separator.Dock = System.Windows.Forms.DockStyle.Top;
            this.separator.Location = new System.Drawing.Point(0, 0);
            this.separator.Name = "separator";
            this.separator.Size = new System.Drawing.Size(584, 3);
            this.separator.TabIndex = 0;
            this.separator.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(584, 561);
            this.Controls.Add(this.Panel2);
            this.Controls.Add(this.Panel1);
            this.Controls.Add(this.Panel3);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(500, 300);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Spotify Lyrics .NET";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.Panel2.ResumeLayout(false);
            this.Panel2.PerformLayout();
            this.Panel1.ResumeLayout(false);
            this.Panel3.ResumeLayout(false);
            this.Panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.separator)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Panel Panel2;
        internal System.Windows.Forms.Label lyricsLabel;
        internal System.Windows.Forms.Panel Panel1;
        internal System.Windows.Forms.Label authorLabel;
        internal System.Windows.Forms.Label titleLabel;
        internal System.Windows.Forms.Timer timerTitle;
        internal System.Windows.Forms.Panel Panel3;
        internal System.Windows.Forms.Button prevBtn;
        internal System.Windows.Forms.Label countLabel;
        internal System.Windows.Forms.Button nextBtn;
        internal System.Windows.Forms.CheckBox topmostCheck;
        internal System.Windows.Forms.Label versionLabel;
        internal System.Windows.Forms.PictureBox separator;
    }
}

