using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Diagnostics;
using Newtonsoft.Json;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        OpenFileDialog dialog = new OpenFileDialog();
        Form popOut = new Form();
        Process mplayer = new Process();
        SteamAPI api;
        RegistryKey key;
        string gameInfo = "";
        string clicked = "";
        string steamPath = "";
        string id = "0";

        public Form1()
        {
            InitializeComponent();
            dialog.RestoreDirectory = true;
            screenshotImage.SizeMode = PictureBoxSizeMode.StretchImage;
            bannerImage.SizeMode = PictureBoxSizeMode.StretchImage;
            popOut.WindowState = FormWindowState.Maximized;
            popOut.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            popOut.Visible = false;
            tabControl1.TabPages.RemoveAt(1);
            findSteamDir();
        }

        private void findSteamDir()
        {
            key = Registry.CurrentUser.OpenSubKey("Software\\Valve\\Steam");
            steamPath = (string)key.GetValue("SteamPath");
            Console.WriteLine("Steam Path: " + steamPath);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (Environment.GetCommandLineArgs().Length > 1)
            {
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
                runApplication();
            }
            else
            {

            }
        }

        private void runApplication()
        {
            runSteamGame();
            Application.Exit();
        }

        private void runSteamGame()
        {
            string exeName = Environment.GetCommandLineArgs()[1];
            int steamAppID = 0;
            steamAppID = Convert.ToInt32(File.ReadAllText(@".\games\" + exeName + ".game").Split('|')[0]);
            exeName = File.ReadAllText(@".\games\" + exeName + ".game").Split('|')[1];
            bool waitForNewEXE = false;

            if (string.IsNullOrEmpty(exeName)) { waitForNewEXE = true; }
            Process.Start("steam://run/" + steamAppID);

            while (waitForNewEXE)
            {
                System.Threading.Thread.Sleep(100);
                Process[] allProcs = Process.GetProcesses();
                foreach (Process proc in allProcs)
                {
                    string procPath = Util.GetExecutablePath(proc).ToLower();
                    if (procPath.Contains("steamapps"))
                    {
                        string gameName = Environment.GetCommandLineArgs()[1];
                        File.WriteAllText(@".\games\" + gameName + ".game", steamAppID + "|" + proc.ProcessName);
                        exeName = proc.ProcessName;
                        waitForNewEXE = false;
                    }
                }
            }

            if (Environment.GetCommandLineArgs()[1] == steamAppID.ToString())
            {
                SteamGame game = figureGame(steamAppID.ToString());
                if (game != null)
                {
                    string gameName = Util.CleanFileName(game.game.data.name);

                    File.WriteAllText(@".\games\" + gameName + ".game", File.ReadAllText(@".\games\" + steamAppID + ".game"));
                    File.Delete(@".\games\" + steamAppID + ".game");
                }
            }

            this.Text = exeName;

            bool running = false;
            Process target = null;

            while (!running)
            {
                System.Threading.Thread.Sleep(100);
                Process[] allProcs = Process.GetProcesses();
                foreach (Process proc in allProcs)
                {
                    if (proc.ProcessName == exeName)
                    {
                        target = proc;
                        running = true;

                    }
                }
            }

            while (running)
            {
                if (target.HasExited)
                {
                    running = false;
                }
            }
        }

        private void findExe_Click(object sender, EventArgs e)
        {            
            if (Directory.Exists(steamPath + "/steamapps/common/"))
            {
                dialog.InitialDirectory = steamPath.Replace('/', '\\') + @"\steamapps\common\";
            }            
            
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                exePath.Text = dialog.FileName;
            }
        }

        private void readSteamGame_Click(object sender, EventArgs e)
        {
            exePath.Text = "";            

            if (!steamGameURL.Text.Contains("store"))
            {
                steamGameURL.Text = "http://store.steampowered.com/app/" + steamGameURL.Text;
            }

            grabGame(steamGameURL.Text);
        }

        public void grabGame(string url, bool isJson = false)
        {
            BackgroundWorker grabinfo = new BackgroundWorker();

            grabinfo.DoWork += (senderx, ex) =>
            {
                try
                {
                    using (WebClient client = new WebClient())
                    {
                        string apiurl = url;
                        if (!isJson)
                        {
                            if (url[url.Length - 1].ToString() == "/")
                            {
                                url = url.Substring(0, steamGameURL.Text.Length - 1);
                            }

                            id = url.Split('/')[url.Split('/').Length - 1];

                            apiurl = url.Replace("/app/", "/api/appdetails?appids=");
                            gameInfo = client.DownloadString(apiurl);
                        }
                        else
                        {
                            gameInfo = url;
                        }
                    }
                }
                catch (Exception ext) { MessageBox.Show("Fetch Error: " + ext.Message); }
            };

            grabinfo.RunWorkerCompleted += (senderx, ex) =>
            {
                try
                {
                    SteamGame game = new SteamGame();
                    gameInfo = gameInfo.Replace("{\"" + id + "\"", "{\"game\"").Replace("\"480\"", "\"LowDef\"");
                    game = JsonConvert.DeserializeObject<SteamGame>(gameInfo);
                    game.myJSON = gameInfo;

                    try { gameTitle.Text = game.game.data.name; }
                    catch { }

                    try
                    {
                        Console.WriteLine(steamPath + @"\steamapps\common\" + game.game.data.name);
                        string correctName = "";

                        if (Directory.Exists(steamPath + @"\steamapps\common\" + game.game.data.name + @"\"))
                        {
                            exePath.Text = ((steamPath + @"\steamapps\common\" + game.game.data.name + @"\"));
                            correctName = game.game.data.name;
                        }
                        string temp = game.game.data.name.Replace("- ", "");
                        if (Directory.Exists(steamPath + @"\steamapps\common\" + temp + @"\"))
                        {
                            exePath.Text = ((steamPath + @"\steamapps\common\" + temp + @"\"));
                            correctName = temp;
                        }

                        temp = temp.Replace(" ", "");
                        if (Directory.Exists(steamPath + @"\steamapps\common\" + temp + @"\"))
                        {
                            exePath.Text = ((steamPath + @"\steamapps\common\" + temp + @"\"));
                            correctName = temp;
                        }


                        if (correctName.Length > 0)
                        {
                            List<string> exeFiles = new List<string>();
                            foreach (string file in Directory.GetFiles(exePath.Text))
                            {
                                if (file.Contains(".exe"))
                                {
                                    Console.WriteLine(file);
                                    exeFiles.Add(file);
                                }
                            }

                            Console.WriteLine(exeFiles.Count);
                            if (exeFiles.Count == 1)
                            {
                                exePath.Text = exeFiles[0];
                            }
                            else
                            {
                                if (exeFiles.Count != 0)
                                {
                                    for (int i = 0; i < exeFiles.Count; i++)
                                    {
                                        exeFiles[i] = exeFiles[i].Replace(steamPath + @"\steamapps\common\" + correctName, "");
                                    }

                                    SteamGameLauncher.ExeSelect selectEXE = new SteamGameLauncher.ExeSelect();
                                    selectEXE.Show();
                                    selectEXE.comboBox1.Items.AddRange(exeFiles.ToArray());

                                    selectEXE.FormClosed += (sender, e) =>
                                    {
                                        exePath.Text = steamPath + @"\steamapps\common\" + correctName + selectEXE.comboBox1.Text;
                                    };
                                }
                                else
                                {
                                    exePath.Text = steamPath + @"\steamapps\common\";
                                }
                            }
                        }
                        else
                        {
                            DialogResult result = MessageBox.Show("Title not found, run Steam command to install game?", "Install Game?", MessageBoxButtons.YesNo);
                            if (result == DialogResult.Yes)
                            {
                                Process.Start("steam://install/" + game.game.data.steam_appid);
                            }
                        }
                    } 
                    catch { }


                    screenshotsList.ClearSelected();
                    videosList.ClearSelected();
                    screenshotsList.Items.Clear();
                    videosList.Items.Clear();

                    if (game.game.data.movies != null)
                    {
                        foreach (Movie m in game.game.data.movies)
                        {
                            if (hdMovieCheckbox.Checked)
                            {
                                if (m.webm.max != null)
                                {
                                    videosList.Items.Add(m.webm.max);
                                }
                            }
                            else
                            {
                                if (m.webm.max != null)
                                {
                                    videosList.Items.Add(m.webm.lowdef);
                                }
                            }
                        }
                    }

                    if (videosList.Items.Count > 0)
                    {
                        try { videosList.SetSelected(0, true); startPreview(false); }
                        catch (Exception _ex) { Console.WriteLine("Error adding movies: " + _ex.Message); }
                    }

                    if (game.game.data.screenshots != null)
                    {
                        foreach (Screenshot s in game.game.data.screenshots)
                        {
                            if (s.path_full != null)
                            {
                                screenshotsList.Items.Add(s.path_full);
                            }
                        }
                    }

                    if (screenshotsList.Items.Count > 0)
                    {
                        try { screenshotsList.SetSelected(0, true); startPreview(true); }
                        catch (Exception _ex) { Console.WriteLine("Error adding pictures: " + _ex.Message); }
                    }

                    if (game.game.data.header_image != null)
                    {
                        try
                        {
                            bannerImage.ImageLocation = game.game.data.header_image;
                        }
                        catch { }
                    }
                }
                catch (Exception ext) { MessageBox.Show("Parse Error: " + ext.Message); }
            };

            grabinfo.RunWorkerAsync();
        }

        private void pickScreenshotDirectory_Click(object sender, EventArgs e)
        {
            snapDirectory.Text = pickFolder();
        }

        private void pickVideoDirectory_Click(object sender, EventArgs e)
        {
            videoDirectory.Text = pickFolder();
        }

        private string pickFolder()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return dialog.SelectedPath;
            }

            return "";
        }

        private void selectScreenshot_Click(object sender, EventArgs e)
        {
            startPreview(true);
        }

        private void selectVideo_Click(object sender, EventArgs e)
        {
            startPreview(false);
        }

        private void startPreview(bool isImage)
        {
            if (isImage)
            {
                screenshotImage.ImageLocation = screenshotsList.SelectedItem.ToString();
            }
            else
            {
                foreach (Process proc in Process.GetProcesses())
                {
                    if (proc.ProcessName.ToLower().Contains("mplayer"))
                    {
                        Console.WriteLine("Found Mplayer");
                        proc.Kill();
                    }
                }


                if (videosList.SelectedItem != null)
                {
                    try
                    {
                        mplayer.StartInfo.FileName = @".\mplayer.exe";
                        mplayer.StartInfo.CreateNoWindow = true;
                        mplayer.StartInfo.UseShellExecute = false;
                        mplayer.StartInfo.RedirectStandardInput = true;
                        mplayer.StartInfo.RedirectStandardOutput = true;
                        mplayer.StartInfo.Arguments = "-quiet -volume 0 -slave -vo direct3d -fs -cache 8192 -wid " + videoPreview.Handle + " " + videosList.SelectedItem.ToString().Replace(@"\", @"");
                        mplayer.Start();
                    }
                    catch (Exception ex) { MessageBox.Show("Error playing video: " + ex.Message); }
                }
            }
        }

        private void popOutVideoPreview_Click(object sender, EventArgs e)
        {            
            if (popOut.Visible == false)
            {
                popOut.Controls.Add(videoPreview);
                popOut.Visible = true;
                popOut.Show();
                popOut.BringToFront();                               
            }
            else
            {                
                popOut.Hide();
                popOut.SendToBack();
                popOut.Visible = false;
                panel1.Controls.Add(videoPreview);                
            }            
        }

        private void popOutImagePreview_Click(object sender, EventArgs e)
        {
            if (popOut.Visible == false)
            {
                popOut.Controls.Add(screenshotImage);
                popOut.Visible = true;
                popOut.Show();
                popOut.BringToFront();
            }
            else
            {
                popOut.Hide();
                popOut.SendToBack();
                popOut.Visible = false;
                panel2.Controls.Add(screenshotImage);
            }
        }

        private void stopVideoPlayback_Click(object sender, EventArgs e)
        {
            try { mplayer.Kill(); }
            catch { }
        }

        private void createGameFile_Click(object sender, EventArgs e)
        {
            getMedia(gameTitle.Text, exePath.Text, id, titleDirectory.Text, snapDirectory.Text, videoDirectory.Text, true);
        }

        private void getMedia(string gameTitle, string exeName, string gameAppID, string titleDir, string snapsDir, string videoDir, bool showError)
        {
            string filename = @".\games\" + gameTitle;
            filename = filename.Replace(":", "");

            File.WriteAllText(filename + ".game", gameAppID + "|" + exeName);
            filename = filename.Replace(@".\games\", "");

            if (!String.IsNullOrEmpty(titleDir))
            {
                try { bannerImage.Image.Save(titleDir + @"\" + filename + ".jpg"); }
                catch (Exception ex) { if (showError) { MessageBox.Show("Title Error: " + ex.Message); Console.WriteLine("Title: " + titleDir + @"\" + filename + ".jpg"); } }
            }
            else
            {
                try { bannerImage.Image.Save(@".\games\" + filename + "-title.jpg"); }
                catch { }
            }

            if (!String.IsNullOrEmpty(snapsDir))
            {
                try { screenshotImage.Image.Save(snapsDir + @"\" + filename + ".jpg"); }
                catch (Exception ex) { if (showError) { MessageBox.Show("Snaps Error: " + ex.Message); Console.WriteLine("Snap: " + snapsDir + @"\" + filename + ".jpg"); } }
            }
            else
            {
                try { screenshotImage.Image.Save(@".\games\" + filename + "-snap.jpg"); }
                catch { }
            }

            if (String.IsNullOrEmpty(videoDir))
            {
                videoDir = @".\games\";
            }
                try
                {
                    this.Enabled = false;
                    mplayer.StartInfo.FileName = @".\mplayer.exe";
                    mplayer.StartInfo.CreateNoWindow = true;
                    mplayer.StartInfo.UseShellExecute = false;
                    mplayer.StartInfo.RedirectStandardInput = true;
                    mplayer.StartInfo.RedirectStandardOutput = true;
                    mplayer.StartInfo.Arguments = "-quiet -cache 8192 " + videosList.SelectedItem.ToString().Replace(@"\", @"") + " -dumpstream -dumpfile " + videoDir + @"\temp.mp4";
                    mplayer.Start();
                    mplayer.WaitForExit();

                    if (File.Exists(videoDir + @"\temp.mp4"))
                    {
                        File.Move(videoDir + @"\temp.mp4", videoDir + @"\" + filename + ".mp4");
                    }
                    while (!mplayer.HasExited) { System.Threading.Thread.Sleep(500); }
                    this.Enabled = true;
                }
                catch (Exception ex) { if (showError) { MessageBox.Show("Video Error: " + ex.Message); Console.WriteLine("Video: " + snapsDir + @"\" + filename + ".mp4"); this.Enabled = true; } }            
        }

        private void grabAllProfileGames_Click(object sender, EventArgs e)
        {
            int stop = 0;
            try
            {
                string id = "";
                textBox7.Text = textBox7.Text.ToLower();

                if (textBox7.Text.Contains("profiles"))
                {
                    id = textBox7.Text.Split('/')[textBox7.Text.Split('/').Length - 1];
                }
                else if (textBox7.Text.Contains("id"))
                {

                    using (WebClient client = new WebClient())
                    {
                        if (!textBox7.Text.Contains("http"))
                        {
                            textBox7.Text = "http://" + textBox7.Text;
                        }
                        string url = textBox7.Text + "?xml=1";

                        string[] xml = client.DownloadString(url).Split('\n');
                        foreach (string line in xml)
                        {
                            if (line.Contains("steamID64"))
                            {
                                id = line.Split('>')[1].Split('<')[0].Trim();
                            }
                        }
                    }
                }

                if (!String.IsNullOrEmpty(id))
                {
                    api = new SteamAPI();
                    api.Init(id);
                }

                textBox7.Text = id;
            }
            catch { }

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (_sender, _e) =>
            {
                Console.WriteLine("Starting...");
                id = textBox7.Text;

                if (!File.Exists(id + ".prf"))
                {
                    if (api == null || api.response == null || api.response.games == null) return;
                    allGameJSON.Clear();
                    foreach (Game game in api.response.games)
                    {
                        figureGame(game);
                        stop++;
                        if (stop == 5)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    api = JsonConvert.DeserializeObject<SteamAPI>(File.ReadAllText(id + ".prf"));
                    allGameJSON.Clear();
                    foreach (Game game in api.response.games)
                    {
                        figureGame(game);
                        stop++;
                        if (stop == 5)
                        {
                            break;
                        }
                    }
                }
            };

            worker.RunWorkerCompleted += (_sender, _e) =>
            {
                if (api == null || api.response == null || api.response.games == null) return;
                foreach (Game game in api.response.games)
                {
                    checkedListBox1.Items.Add(game.appid + "|" + game.game_name);
                }
                Console.WriteLine("Done..." + allGameJSON.Count);

                File.WriteAllText(@id + ".prf", JsonConvert.SerializeObject(api));
            };

            worker.RunWorkerAsync();
        }

        static List<SteamGame> allGameJSON = new List<SteamGame>();
        private static void figureGame(Game game)
        {
            try
            {
                string apiresult = "";

                using (WebClient client = new WebClient())
                {
                    apiresult = client.DownloadString("http://store.steampowered.com/api/appdetails?appids=" + game.appid);
                }

                apiresult = apiresult.Replace("{\"" + game.appid + "\"", "{\"game\"").Replace("\"480\"", "\"LowDef\"");

                SteamGame found = new SteamGame();
                found = Newtonsoft.Json.JsonConvert.DeserializeObject<SteamGame>(apiresult);
                found.myJSON = apiresult;
                allGameJSON.Add(found);

                game.game_name = found.game.data.name;
                Console.WriteLine("Found " + game.game_name + " (" + game.appid + ")");
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); System.Threading.Thread.Sleep(1500); Console.WriteLine("Retry " + game.appid); figureGame(game); }
            System.Threading.Thread.Sleep(1500);
        }

        private static SteamGame figureGame(string game)
        {
            try
            {
                string apiresult = "";

                using (WebClient client = new WebClient())
                {
                    apiresult = client.DownloadString("http://store.steampowered.com/api/appdetails?appids=" + game);
                }

                apiresult = apiresult.Replace("{\"" + game + "\"", "{\"game\"").Replace("\"480\"", "\"LowDef\"");

                SteamGame found = new SteamGame();
                found.myJSON = apiresult;
                found = Newtonsoft.Json.JsonConvert.DeserializeObject<SteamGame>(apiresult);

                Console.WriteLine("Found " + found.game.data.name + " (" + game + ")");
                return found;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); System.Threading.Thread.Sleep(1500); Console.WriteLine("Retry " + game); figureGame(game); }
            return null;
        }

        private void processProfile_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemCheckState(i) == CheckState.Checked)
                {
                    foreach (SteamGame game in allGameJSON)
                    {
                        if (game.game.data.steam_appid.ToString() == checkedListBox1.Items[i].ToString().Split('|')[0])
                        {
                            Console.WriteLine("Processing : " + game.game.data.name);
                            id = game.game.data.steam_appid.ToString();

                            grabGame(game.myJSON, true);
                            
                            string filename = @".\games\" + game.game.data.name.Replace(":", "");

                            if (videoDirectory.Text != "")
                            {
                                try
                                {
                                    mplayer.StartInfo.FileName = @".\mplayer.exe";
                                    mplayer.StartInfo.CreateNoWindow = true;
                                    mplayer.StartInfo.UseShellExecute = false;
                                    mplayer.StartInfo.RedirectStandardInput = true;
                                    mplayer.StartInfo.RedirectStandardOutput = true;
                                    mplayer.StartInfo.Arguments = "-quiet -cache 8192 " + game.game.data.movies[0].webm.max.Replace(@"\", @"") + " -dumpstream -dumpfile " + videoDirectory.Text + @"\temp.mp4";
                                    mplayer.Start();
                                    mplayer.WaitForExit();

                                    if (File.Exists(videoDirectory.Text + @"\temp.mp4"))
                                    {
                                        File.Move(videoDirectory.Text + @"\temp.mp4", videoDirectory.Text + @"\" + filename + ".mp4");
                                    }
                                }
                                catch { }
                            }                            

                            bannerImage.ImageLocation = game.game.data.header_image;
                            screenshotImage.ImageLocation = game.game.data.screenshots[0].path_full;
                            

                            bannerImage.Image.Save(titleDirectory.Text + @"\" + filename + ".jpg");
                            screenshotImage.Image.Save(snapDirectory.Text + @"\" + filename + ".jpg");
                        }
                    }
                }
            }
        }

        private void pickBannerDirectory_Click(object sender, EventArgs e)
        {
            titleDirectory.Text = pickFolder();
        }
    }
}
