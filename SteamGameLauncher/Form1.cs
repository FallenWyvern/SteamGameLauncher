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
        }

        private void RunApplication()
        {
            string exeName = Environment.GetCommandLineArgs()[1];
            int steamAppID = 0;
            steamAppID = Convert.ToInt32(File.ReadAllText(@".\games\" + exeName + ".game").Split('|')[0]);
            exeName = File.ReadAllText(@".\games\" + exeName + ".game").Split('|')[1];
            Process.Start("steam://run/" + steamAppID);

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

            Application.Exit();
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
            BackgroundWorker grabinfo = new BackgroundWorker();

            grabinfo.DoWork += (senderx, ex) =>
            {
                try
                {
                    using (WebClient client = new WebClient())
                    {
                        string url = textBox1.Text;
                        if (url[textBox1.Text.Length - 1].ToString() == "/")
                        {
                            url = textBox1.Text.Substring(0, textBox1.Text.Length - 1);
                        }

                        id = url.Split('/')[url.Split('/').Length - 1];

                        string apiurl = url.Replace("/app/", "/api/appdetails?appids=");
                        gameInfo = client.DownloadString(apiurl);
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

                    try { textBox6.Text = game.game.data.name; }
                    catch { }

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
                catch { MessageBox.Show("Error playing video."); }
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
            mplayer.Kill();
        }

        private void button3_Click(object sender, EventArgs e)
        {            
            string filename = @".\games\" + textBox6.Text;     
            filename = filename.Replace(":", "");

            File.WriteAllText(filename + ".game", id + "|" + textBox2.Text);
            filename = filename.Replace(@".\games\", "");

            if (!String.IsNullOrEmpty(textBox5.Text))
            {                
                try { pictureBox3.Image.Save(textBox5.Text + @"\" + filename + ".jpg"); }
                catch (Exception ex) { MessageBox.Show("Snap Error: " + ex.Message); }
            }
            
            if (!String.IsNullOrEmpty(textBox3.Text))
            {
                try { pictureBox1.Image.Save(textBox3.Text + @"\" + filename + ".jpg"); }
                catch (Exception ex) { MessageBox.Show("Title Error: " + ex.Message); }
            }

            if (!String.IsNullOrEmpty(textBox4.Text))
            {
                try
                {
                    this.Enabled = false;
                    mplayer.StartInfo.FileName = @".\mplayer.exe";
                    mplayer.StartInfo.CreateNoWindow = true;
                    mplayer.StartInfo.UseShellExecute = false;
                    mplayer.StartInfo.RedirectStandardInput = true;
                    mplayer.StartInfo.RedirectStandardOutput = true;
                    mplayer.StartInfo.Arguments = "-quiet -cache 8192 " + listBox2.SelectedItem.ToString().Replace(@"\", @"") + " -dumpstream -dumpfile " + textBox4.Text + @"\temp.mp4";
                    mplayer.Start();
                    mplayer.WaitForExit();
                    
                    if (File.Exists(textBox4.Text + @"\temp.mp4"))
                    {                        
                        File.Move(textBox4.Text + @"\temp.mp4", textBox4.Text + @"\" + filename + ".mp4");
                    }

                    this.Enabled = true;                    
                }
                catch (Exception ex) { MessageBox.Show("Video Error: " + ex.Message); this.Enabled = true; }                
            }            
        }
    }
}
