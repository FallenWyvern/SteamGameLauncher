namespace WindowsFormsApplication1
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.steamGameURL = new System.Windows.Forms.TextBox();
            this.steamGameURLLabel = new System.Windows.Forms.Label();
            this.loadEXE = new System.Windows.Forms.Button();
            this.exePath = new System.Windows.Forms.TextBox();
            this.readSteamGame = new System.Windows.Forms.Button();
            this.createGameButton = new System.Windows.Forms.Button();
            this.findSnapDirectory = new System.Windows.Forms.Button();
            this.snapDirectory = new System.Windows.Forms.TextBox();
            this.findVideoDirectory = new System.Windows.Forms.Button();
            this.videoDirectory = new System.Windows.Forms.TextBox();
            this.screenshotsList = new System.Windows.Forms.ListBox();
            this.videosList = new System.Windows.Forms.ListBox();
            this.screenshotImage = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.videoPreview = new System.Windows.Forms.PictureBox();
            this.bannerImage = new System.Windows.Forms.PictureBox();
            this.stopVideoPlayback = new System.Windows.Forms.Button();
            this.hdMovieCheckbox = new System.Windows.Forms.CheckBox();
            this.doesNothingButton = new System.Windows.Forms.Button();
            this.titleDirectory = new System.Windows.Forms.TextBox();
            this.gameTitle = new System.Windows.Forms.TextBox();
            this.gameTitleLabel = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.processProfile = new System.Windows.Forms.Button();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.grabProfileGames = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox7 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.screenshotImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.videoPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bannerImage)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // steamGameURL
            // 
            this.steamGameURL.Location = new System.Drawing.Point(12, 22);
            this.steamGameURL.Name = "steamGameURL";
            this.steamGameURL.Size = new System.Drawing.Size(405, 20);
            this.steamGameURL.TabIndex = 0;
            // 
            // steamGameURLLabel
            // 
            this.steamGameURLLabel.AutoSize = true;
            this.steamGameURLLabel.Location = new System.Drawing.Point(12, 6);
            this.steamGameURLLabel.Name = "steamGameURLLabel";
            this.steamGameURLLabel.Size = new System.Drawing.Size(93, 13);
            this.steamGameURLLabel.TabIndex = 1;
            this.steamGameURLLabel.Text = "Steam Game URL";
            // 
            // loadEXE
            // 
            this.loadEXE.Location = new System.Drawing.Point(12, 93);
            this.loadEXE.Name = "loadEXE";
            this.loadEXE.Size = new System.Drawing.Size(93, 23);
            this.loadEXE.TabIndex = 2;
            this.loadEXE.Text = "EXE";
            this.loadEXE.UseVisualStyleBackColor = true;
            this.loadEXE.Click += new System.EventHandler(this.findExe_Click);
            // 
            // exePath
            // 
            this.exePath.Location = new System.Drawing.Point(111, 95);
            this.exePath.Name = "exePath";
            this.exePath.Size = new System.Drawing.Size(387, 20);
            this.exePath.TabIndex = 3;
            // 
            // readSteamGame
            // 
            this.readSteamGame.Location = new System.Drawing.Point(423, 20);
            this.readSteamGame.Name = "readSteamGame";
            this.readSteamGame.Size = new System.Drawing.Size(75, 23);
            this.readSteamGame.TabIndex = 4;
            this.readSteamGame.Text = "Read";
            this.readSteamGame.UseVisualStyleBackColor = true;
            this.readSteamGame.Click += new System.EventHandler(this.readSteamGame_Click);
            // 
            // createGameButton
            // 
            this.createGameButton.Location = new System.Drawing.Point(3, 366);
            this.createGameButton.Name = "createGameButton";
            this.createGameButton.Size = new System.Drawing.Size(486, 23);
            this.createGameButton.TabIndex = 5;
            this.createGameButton.Text = "Create Game File";
            this.createGameButton.UseVisualStyleBackColor = true;
            this.createGameButton.Click += new System.EventHandler(this.createGameFile_Click);
            // 
            // findSnapDirectory
            // 
            this.findSnapDirectory.Location = new System.Drawing.Point(12, 151);
            this.findSnapDirectory.Name = "findSnapDirectory";
            this.findSnapDirectory.Size = new System.Drawing.Size(93, 23);
            this.findSnapDirectory.TabIndex = 6;
            this.findSnapDirectory.Text = "Snaps Dir";
            this.findSnapDirectory.UseVisualStyleBackColor = true;
            this.findSnapDirectory.Click += new System.EventHandler(this.pickScreenshotDirectory_Click);
            // 
            // snapDirectory
            // 
            this.snapDirectory.Location = new System.Drawing.Point(111, 153);
            this.snapDirectory.Name = "snapDirectory";
            this.snapDirectory.Size = new System.Drawing.Size(387, 20);
            this.snapDirectory.TabIndex = 7;
            // 
            // findVideoDirectory
            // 
            this.findVideoDirectory.Location = new System.Drawing.Point(12, 180);
            this.findVideoDirectory.Name = "findVideoDirectory";
            this.findVideoDirectory.Size = new System.Drawing.Size(93, 23);
            this.findVideoDirectory.TabIndex = 8;
            this.findVideoDirectory.Text = "Video Dir";
            this.findVideoDirectory.UseVisualStyleBackColor = true;
            this.findVideoDirectory.Click += new System.EventHandler(this.pickVideoDirectory_Click);
            // 
            // videoDirectory
            // 
            this.videoDirectory.Location = new System.Drawing.Point(111, 182);
            this.videoDirectory.Name = "videoDirectory";
            this.videoDirectory.Size = new System.Drawing.Size(387, 20);
            this.videoDirectory.TabIndex = 9;
            // 
            // screenshotsList
            // 
            this.screenshotsList.FormattingEnabled = true;
            this.screenshotsList.Location = new System.Drawing.Point(3, 86);
            this.screenshotsList.Name = "screenshotsList";
            this.screenshotsList.Size = new System.Drawing.Size(280, 134);
            this.screenshotsList.TabIndex = 10;
            this.screenshotsList.Click += new System.EventHandler(this.selectScreenshot_Click);
            // 
            // videosList
            // 
            this.videosList.FormattingEnabled = true;
            this.videosList.Location = new System.Drawing.Point(3, 226);
            this.videosList.Name = "videosList";
            this.videosList.Size = new System.Drawing.Size(280, 134);
            this.videosList.TabIndex = 11;
            this.videosList.SelectedIndexChanged += new System.EventHandler(this.selectVideo_Click);
            // 
            // screenshotImage
            // 
            this.screenshotImage.Location = new System.Drawing.Point(289, 89);
            this.screenshotImage.Name = "screenshotImage";
            this.screenshotImage.Size = new System.Drawing.Size(200, 131);
            this.screenshotImage.TabIndex = 12;
            this.screenshotImage.TabStop = false;
            this.screenshotImage.Click += new System.EventHandler(this.popOutVideoPreview_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(285, 52);
            this.label2.TabIndex = 13;
            this.label2.Text = resources.GetString("label2.Text");
            // 
            // videoPreview
            // 
            this.videoPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.videoPreview.Location = new System.Drawing.Point(0, 0);
            this.videoPreview.Name = "videoPreview";
            this.videoPreview.Size = new System.Drawing.Size(200, 108);
            this.videoPreview.TabIndex = 14;
            this.videoPreview.TabStop = false;
            this.videoPreview.Click += new System.EventHandler(this.popOutVideoPreview_Click);
            // 
            // bannerImage
            // 
            this.bannerImage.Location = new System.Drawing.Point(289, 7);
            this.bannerImage.Name = "bannerImage";
            this.bannerImage.Size = new System.Drawing.Size(200, 76);
            this.bannerImage.TabIndex = 15;
            this.bannerImage.TabStop = false;
            // 
            // stopVideoPlayback
            // 
            this.stopVideoPlayback.Location = new System.Drawing.Point(349, 337);
            this.stopVideoPlayback.Name = "stopVideoPlayback";
            this.stopVideoPlayback.Size = new System.Drawing.Size(75, 23);
            this.stopVideoPlayback.TabIndex = 16;
            this.stopVideoPlayback.Text = "Stop";
            this.stopVideoPlayback.UseVisualStyleBackColor = true;
            this.stopVideoPlayback.Click += new System.EventHandler(this.stopVideoPlayback_Click);
            // 
            // hdMovieCheckbox
            // 
            this.hdMovieCheckbox.AutoSize = true;
            this.hdMovieCheckbox.Location = new System.Drawing.Point(6, 66);
            this.hdMovieCheckbox.Name = "hdMovieCheckbox";
            this.hdMovieCheckbox.Size = new System.Drawing.Size(79, 17);
            this.hdMovieCheckbox.TabIndex = 17;
            this.hdMovieCheckbox.Text = "HD Movies";
            this.hdMovieCheckbox.UseVisualStyleBackColor = true;
            // 
            // doesNothingButton
            // 
            this.doesNothingButton.Location = new System.Drawing.Point(12, 122);
            this.doesNothingButton.Name = "doesNothingButton";
            this.doesNothingButton.Size = new System.Drawing.Size(93, 23);
            this.doesNothingButton.TabIndex = 18;
            this.doesNothingButton.Text = "Title Dir";
            this.doesNothingButton.UseVisualStyleBackColor = true;
            this.doesNothingButton.Click += new System.EventHandler(this.pickBannerDirectory_Click);
            // 
            // titleDirectory
            // 
            this.titleDirectory.Location = new System.Drawing.Point(111, 124);
            this.titleDirectory.Name = "titleDirectory";
            this.titleDirectory.Size = new System.Drawing.Size(387, 20);
            this.titleDirectory.TabIndex = 19;
            // 
            // gameTitle
            // 
            this.gameTitle.Location = new System.Drawing.Point(111, 69);
            this.gameTitle.Name = "gameTitle";
            this.gameTitle.Size = new System.Drawing.Size(387, 20);
            this.gameTitle.TabIndex = 20;
            // 
            // gameTitleLabel
            // 
            this.gameTitleLabel.AutoSize = true;
            this.gameTitleLabel.Location = new System.Drawing.Point(26, 72);
            this.gameTitleLabel.Name = "gameTitleLabel";
            this.gameTitleLabel.Size = new System.Drawing.Size(58, 13);
            this.gameTitleLabel.TabIndex = 21;
            this.gameTitleLabel.Text = "Game Title";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tabControl1.Location = new System.Drawing.Point(0, 209);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(510, 423);
            this.tabControl1.TabIndex = 22;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.createGameButton);
            this.tabPage1.Controls.Add(this.screenshotsList);
            this.tabPage1.Controls.Add(this.videosList);
            this.tabPage1.Controls.Add(this.screenshotImage);
            this.tabPage1.Controls.Add(this.hdMovieCheckbox);
            this.tabPage1.Controls.Add(this.stopVideoPlayback);
            this.tabPage1.Controls.Add(this.bannerImage);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(502, 397);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Single";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.videoPreview);
            this.panel1.Location = new System.Drawing.Point(289, 226);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 108);
            this.panel1.TabIndex = 18;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.processProfile);
            this.tabPage2.Controls.Add(this.checkedListBox1);
            this.tabPage2.Controls.Add(this.grabProfileGames);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.textBox7);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(502, 397);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Batch";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // processProfile
            // 
            this.processProfile.Location = new System.Drawing.Point(390, 46);
            this.processProfile.Name = "processProfile";
            this.processProfile.Size = new System.Drawing.Size(104, 23);
            this.processProfile.TabIndex = 4;
            this.processProfile.Text = "Process Selected";
            this.processProfile.UseVisualStyleBackColor = true;
            this.processProfile.Click += new System.EventHandler(this.processProfile_Click);
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(3, 75);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(496, 319);
            this.checkedListBox1.TabIndex = 3;
            // 
            // grabProfileGames
            // 
            this.grabProfileGames.Location = new System.Drawing.Point(419, 4);
            this.grabProfileGames.Name = "grabProfileGames";
            this.grabProfileGames.Size = new System.Drawing.Size(75, 23);
            this.grabProfileGames.TabIndex = 2;
            this.grabProfileGames.Text = "Go";
            this.grabProfileGames.UseVisualStyleBackColor = true;
            this.grabProfileGames.Click += new System.EventHandler(this.grabAllProfileGames_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "SteamID";
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(60, 6);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(353, 20);
            this.textBox7.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 632);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.gameTitleLabel);
            this.Controls.Add(this.gameTitle);
            this.Controls.Add(this.titleDirectory);
            this.Controls.Add(this.doesNothingButton);
            this.Controls.Add(this.videoDirectory);
            this.Controls.Add(this.findVideoDirectory);
            this.Controls.Add(this.snapDirectory);
            this.Controls.Add(this.findSnapDirectory);
            this.Controls.Add(this.readSteamGame);
            this.Controls.Add(this.exePath);
            this.Controls.Add(this.loadEXE);
            this.Controls.Add(this.steamGameURLLabel);
            this.Controls.Add(this.steamGameURL);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "SteamGameLauncher";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.screenshotImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.videoPreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bannerImage)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox steamGameURL;
        private System.Windows.Forms.Label steamGameURLLabel;
        private System.Windows.Forms.Button loadEXE;
        private System.Windows.Forms.TextBox exePath;
        private System.Windows.Forms.Button readSteamGame;
        private System.Windows.Forms.Button createGameButton;
        private System.Windows.Forms.Button findSnapDirectory;
        private System.Windows.Forms.TextBox snapDirectory;
        private System.Windows.Forms.Button findVideoDirectory;
        private System.Windows.Forms.TextBox videoDirectory;
        private System.Windows.Forms.ListBox screenshotsList;
        private System.Windows.Forms.ListBox videosList;
        private System.Windows.Forms.PictureBox screenshotImage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox videoPreview;
        private System.Windows.Forms.PictureBox bannerImage;
        private System.Windows.Forms.Button stopVideoPlayback;
        private System.Windows.Forms.CheckBox hdMovieCheckbox;
        private System.Windows.Forms.Button doesNothingButton;
        private System.Windows.Forms.TextBox titleDirectory;
        private System.Windows.Forms.TextBox gameTitle;
        private System.Windows.Forms.Label gameTitleLabel;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button grabProfileGames;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Button processProfile;
        private System.Windows.Forms.Panel panel1;
    }
}

