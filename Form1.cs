using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AudioSwitcher.AudioApi;
using AudioSwitcher.AudioApi.CoreAudio;

namespace VirtualMediaControls
{
    public partial class Form1 : Form
    {
        CoreAudioDevice device;
        Point DefaultLocation;
        public bool play = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            device = new CoreAudioController().DefaultPlaybackDevice;
            trackBar1.Value = Convert.ToInt32(device.Volume);
            DefaultLocation = Location;
        }

        #region drag
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        #endregion

        private void Button1_Click(object sender, EventArgs e)
        {
            Previous();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            play = !play;
            if (play)
            {
                button2.Image = Properties.Resources.baseline_play_arrow_black_48dp;
            }
            else
            {
                button2.Image = Properties.Resources.baseline_pause_black_48dp;
            }
            PlayPause();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            Next();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            Mute();
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            Stop();
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            UnMute();
        }

        #region keys
        [DllImport("user32.dll", SetLastError = true)]
        public static extern void keybd_event(byte virtualKey, byte scanCode, uint flags, IntPtr extraInfo);
        public const int VK_MEDIA_NEXT_TRACK = 0xB0;
        public const int VK_MEDIA_PLAY_PAUSE = 0xB3;
        public const int VK_MEDIA_STOP = 0xB2;
        public const int VK_MEDIA_PREV_TRACK = 0xB1;
        
        public const int KEYEVENTF_EXTENDEDKEY = 0x0001; //Key down flag
        public const int KEYEVENTF_KEYUP = 0x0002; //Key up flag

        private void PlayPause()
        {
            keybd_event(VK_MEDIA_PLAY_PAUSE, 0, KEYEVENTF_EXTENDEDKEY, IntPtr.Zero);
            keybd_event(VK_MEDIA_PLAY_PAUSE, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
        }

        private void Stop()
        {
            keybd_event(VK_MEDIA_STOP, 0, KEYEVENTF_EXTENDEDKEY, IntPtr.Zero);
            keybd_event(VK_MEDIA_STOP, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
        }
        private void Next()
        {
            keybd_event(VK_MEDIA_NEXT_TRACK, 0, KEYEVENTF_EXTENDEDKEY, IntPtr.Zero);
            keybd_event(VK_MEDIA_NEXT_TRACK, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
        }

        private void Previous()
        {
            keybd_event(VK_MEDIA_PREV_TRACK, 0, KEYEVENTF_EXTENDEDKEY, IntPtr.Zero);
            keybd_event(VK_MEDIA_PREV_TRACK, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
        }
        #endregion

        #region volume
        private void Mute()
        {
            device.Mute(true);
        }

        private void UnMute()
        {
            device.Mute(false);
        }

        private void Volume(int vol)
        {
            device.Volume = vol;
        }
        #endregion

        private void TrackBar1_Scroll(object sender, EventArgs e)
        {
            Volume(trackBar1.Value);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            //close logic here
            e.Cancel = true;
            base.OnClosing(e);
        }

        #region menu

        private void YesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            yesToolStripMenuItem.Checked = true;
            noToolStripMenuItem.Checked = false;
            Height = 170;
            trackBar1.Enabled = true;
            trackBar1.Visible = true;
        }

        private void NoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            yesToolStripMenuItem.Checked = false;
            noToolStripMenuItem.Checked = true;
            Height = 129;
            trackBar1.Enabled = false;
            trackBar1.Visible = false;
        }

        private void YesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            yesToolStripMenuItem1.Checked = true;
            noToolStripMenuItem1.Checked = false;
            TopMost = true;
        }

        private void NoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            yesToolStripMenuItem1.Checked = false;
            noToolStripMenuItem1.Checked = true;
            TopMost = false;
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void NotifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Location = DefaultLocation;
        }
        #endregion
    }
}
