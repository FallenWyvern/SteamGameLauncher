using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Net;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Newtonsoft.Json;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        Form popOut = new Form();
        Process mplayer = new Process();
        SteamAPI api;
        string gameInfo = "";
        string id = "0";

        public Form1()
        {
            InitializeComponent();
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            popOut.WindowState = FormWindowState.Maximized;
            popOut.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            popOut.Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (Environment.GetCommandLineArgs().Length > 1)
            {
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
                RunApplication();
            }
            else
            {

            }
        }

        private void RunApplication()
        {
            RunSteamGame();
            Application.Exit();
        }

        private void RunSteamGame()
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
                SteamGame game = FigureGame(steamAppID.ToString());
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

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox2.Text = dialog.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GrabGame(textBox1.Text);
        }

        public void GrabGame(string url, bool isJson = false)
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
                                url = url.Substring(0, textBox1.Text.Length - 1);
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

                    try { textBox6.Text = game.game.data.name; }
                    catch { }

                    listBox1.ClearSelected();
                    listBox2.ClearSelected();
                    listBox1.Items.Clear();
                    listBox2.Items.Clear();

                    if (game.game.data.movies != null)
                    {
                        foreach (Movie m in game.game.data.movies)
                        {
                            if (checkBox1.Checked)
                            {
                                if (m.webm.max != null)
                                {
                                    listBox2.Items.Add(m.webm.max);
                                }
                            }
                            else
                            {
                                if (m.webm.max != null)
                                {
                                    listBox2.Items.Add(m.webm.lowdef);
                                }
                            }
                        }
                    }

                    if (listBox2.Items.Count > 0)
                    {
                        try { listBox2.SetSelected(0, true); StartPreview(false); }
                        catch (Exception _ex) { Console.WriteLine("Error adding movies: " + _ex.Message); }
                    }

                    if (game.game.data.screenshots != null)
                    {
                        foreach (Screenshot s in game.game.data.screenshots)
                        {
                            if (s.path_full != null)
                            {
                                listBox1.Items.Add(s.path_full);
                            }
                        }
                    }

                    if (listBox1.Items.Count > 0)
                    {
                        try { listBox1.SetSelected(0, true); StartPreview(true); }
                        catch (Exception _ex) { Console.WriteLine("Error adding pictures: " + _ex.Message); }
                    }

                    if (game.game.data.header_image != null)
                    {
                        try
                        {
                            pictureBox3.ImageLocation = game.game.data.header_image;
                        }
                        catch { }
                    }
                }
                catch (Exception ext) { MessageBox.Show("Parse Error: " + ext.Message); }
            };

            grabinfo.RunWorkerAsync();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox3.Text = PickFolder();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox4.Text = PickFolder();
        }

        private string PickFolder()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return dialog.SelectedPath;
            }

            return "";
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            StartPreview(true);
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            StartPreview(false);
        }

        private void StartPreview(bool isImage)
        {
            if (isImage)
            {
                pictureBox1.ImageLocation = listBox1.SelectedItem.ToString();
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


                if (listBox2.SelectedItem != null)
                {
                    try
                    {
                        mplayer.StartInfo.FileName = @".\mplayer.exe";
                        mplayer.StartInfo.CreateNoWindow = true;
                        mplayer.StartInfo.UseShellExecute = false;
                        mplayer.StartInfo.RedirectStandardInput = true;
                        mplayer.StartInfo.RedirectStandardOutput = true;
                        mplayer.StartInfo.Arguments = "-quiet -volume 0 -slave -fs -cache 8192 -wid " + pictureBox2.Handle + " " + listBox2.SelectedItem.ToString().Replace(@"\", @"");
                        mplayer.Start();
                    }
                    catch (Exception ex) { MessageBox.Show("Error playing video: " + ex.Message); }
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            PictureBox proxy = (PictureBox)sender;
            if (popOut.Visible == false)
            {
                popOut.Visible = true;
                popOut.Show();
                popOut.BringToFront();
                popOut.Controls.Add(proxy);
                proxy.Dock = DockStyle.Fill;
            }
            else
            {
                popOut.Visible = false;
                popOut.Hide();
                popOut.SendToBack();
                proxy.Dock = DockStyle.None;
                this.Controls.Add(proxy);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try { mplayer.Kill(); }
            catch { }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            GetMedia(textBox6.Text, textBox2.Text, id, textBox5.Text, textBox3.Text, textBox4.Text, true);
        }

        private void GetMedia(string gameTitle, string exeName, string gameAppID, string titleDir, string snapsDir, string videoDir, bool showError)
        {
            string filename = @".\games\" + gameTitle;
            filename = filename.Replace(":", "");

            File.WriteAllText(filename + ".game", gameAppID + "|" + exeName);
            filename = filename.Replace(@".\games\", "");

            if (!String.IsNullOrEmpty(titleDir))
            {
                try { pictureBox3.Image.Save(titleDir + @"\" + filename + ".jpg"); }
                catch (Exception ex) { if (showError) { MessageBox.Show("Title Error: " + ex.Message); Console.WriteLine("Title: " + titleDir + @"\" + filename + ".jpg"); } }
            }

            if (!String.IsNullOrEmpty(snapsDir))
            {
                try { pictureBox1.Image.Save(snapsDir + @"\" + filename + ".jpg"); }
                catch (Exception ex) { if (showError) { MessageBox.Show("Snaps Error: " + ex.Message); Console.WriteLine("Snap: " + snapsDir + @"\" + filename + ".jpg"); } }
            }

            if (!String.IsNullOrEmpty(videoDir))
            {
                try
                {
                    this.Enabled = false;
                    mplayer.StartInfo.FileName = @".\mplayer.exe";
                    mplayer.StartInfo.CreateNoWindow = true;
                    mplayer.StartInfo.UseShellExecute = false;
                    mplayer.StartInfo.RedirectStandardInput = true;
                    mplayer.StartInfo.RedirectStandardOutput = true;
                    mplayer.StartInfo.Arguments = "-quiet -cache 8192 " + listBox2.SelectedItem.ToString().Replace(@"\", @"") + " -dumpstream -dumpfile " + videoDir + @"\temp.mp4";
                    mplayer.Start();
                    mplayer.WaitForExit();

                    if (File.Exists(videoDir + @"\temp.mp4"))
                    {
                        File.Move(videoDir + @"\temp.mp4", videoDir + @"\" + filename + ".mp4");
                    }

                    this.Enabled = true;
                }
                catch (Exception ex) { if (showError) { MessageBox.Show("Video Error: " + ex.Message); Console.WriteLine("Video: " + snapsDir + @"\" + filename + ".mp4"); this.Enabled = true; } }
            }
        }

        private void button8_Click(object sender, EventArgs e)
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
                        FigureGame(game);
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
                        FigureGame(game);
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
        private static void FigureGame(Game game)
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
            catch (Exception ex) { Console.WriteLine(ex.Message); System.Threading.Thread.Sleep(1500); Console.WriteLine("Retry " + game.appid); FigureGame(game); }
            System.Threading.Thread.Sleep(1500);
        }

        private static SteamGame FigureGame(string game)
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
            catch (Exception ex) { Console.WriteLine(ex.Message); System.Threading.Thread.Sleep(1500); Console.WriteLine("Retry " + game); FigureGame(game); }
            return null;
        }

        private void button9_Click(object sender, EventArgs e)
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

                            GrabGame(game.myJSON, true);
                            
                            string filename = @".\games\" + game.game.data.name.Replace(":", "");

                            if (textBox4.Text != "")
                            {
                                try
                                {
                                    mplayer.StartInfo.FileName = @".\mplayer.exe";
                                    mplayer.StartInfo.CreateNoWindow = true;
                                    mplayer.StartInfo.UseShellExecute = false;
                                    mplayer.StartInfo.RedirectStandardInput = true;
                                    mplayer.StartInfo.RedirectStandardOutput = true;
                                    mplayer.StartInfo.Arguments = "-quiet -cache 8192 " + game.game.data.movies[0].webm.max.Replace(@"\", @"") + " -dumpstream -dumpfile " + textBox4.Text + @"\temp.mp4";
                                    mplayer.Start();
                                    mplayer.WaitForExit();

                                    if (File.Exists(textBox4.Text + @"\temp.mp4"))
                                    {
                                        File.Move(textBox4.Text + @"\temp.mp4", textBox4.Text + @"\" + filename + ".mp4");
                                    }
                                }
                                catch { }
                            }                            

                            pictureBox3.ImageLocation = game.game.data.header_image;
                            pictureBox1.ImageLocation = game.game.data.screenshots[0].path_full;
                            

                            pictureBox3.Image.Save(textBox5.Text + @"\" + filename + ".jpg");
                            pictureBox1.Image.Save(textBox3.Text + @"\" + filename + ".jpg");
                        }
                    }
                }
            }
        }
    }
}
