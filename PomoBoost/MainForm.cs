using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Taskbar;

namespace PomoBoost
{
    public partial class MainForm : Form
    {
        private PomodoroTimer pd;
        private ThumbnailToolbarButton buttonPlay, buttonPause;

        public MainForm()
        {
            InitializeComponent();

            // Set the icon
            this.Icon = Properties.Resources.Timer;

            this.pd = new PomodoroTimer(new TimeSpan(0, 25, 0));

            // Configure the progess bar
            progressBar1.Minimum = 0;
            progressBar1.Maximum = (int)this.pd.GetTimeSpan().TotalSeconds;
            progressBar1.Step = 1;

            // Configure the thumbnail toolbar
            buttonPlay = new ThumbnailToolbarButton(Properties.Resources.Play, "Run");
            buttonPlay.Click += new EventHandler<ThumbnailButtonClickedEventArgs>(buttonPlay_Click);
            buttonPause = new ThumbnailToolbarButton(Properties.Resources.Pause, "Pause");
            buttonPause.Click += new EventHandler<ThumbnailButtonClickedEventArgs>(buttonPause_Click);
            TaskbarManager.Instance.ThumbnailToolbars.AddButtons(this.Handle, buttonPlay, buttonPause);
        }

        // Event-handlers for thumbnail toolbar
        void buttonPlay_Click(object sender, ThumbnailButtonClickedEventArgs e)
        {
            if (!timer1.Enabled)
            {
                timer1.Start();
                button1.Text = "Pause";
                buttonPlay.Enabled = false;
                buttonPause.Enabled = true;
            }
        }
        void buttonPause_Click(object sender, ThumbnailButtonClickedEventArgs e)
        {
            if (timer1.Enabled)
            {
                timer1.Stop();
                button1.Text = "Run";
                buttonPlay.Enabled = true;
                buttonPause.Enabled = false;
            }
        }

        // Run/Pause button
        private void button1_Click(object sender, EventArgs e)
        {
            if(timer1.Enabled) {
                timer1.Stop();
                button1.Text = "Run";
                buttonPlay.Enabled = true;
                buttonPause.Enabled = false;
            } else {
                timer1.Start();
                button1.Text = "Pause";
                buttonPlay.Enabled = false;
                buttonPause.Enabled = true;
            }

            // Enable pause button
            button1.Enabled = true;
        }

        // Reset button
        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            this.pd.Reset();

            TaskbarManager.Instance.SetProgressValue(0, (int)this.pd.GetTimeSpan().TotalSeconds);
            progressBar1.Value = 0;
            label1.Text = this.pd.ToString();
            buttonPlay.Enabled = true;
            buttonPause.Enabled = false;
            button1.Text = "Run";
            button1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.pd.Tick(new TimeSpan(0, 0, 1));

            // Update the two progessbars
            TaskbarManager.Instance.SetProgressValue((int)this.pd.GetTimeSpan().TotalSeconds - (int)this.pd.GetTimeRemaining().TotalSeconds, (int)this.pd.GetTimeSpan().TotalSeconds);
            progressBar1.PerformStep();

            if (this.pd.IsZero())
            {
                timer1.Stop();

                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Error);
                label1.Text = "Rest!";
                buttonPlay.Enabled = false;
                buttonPause.Enabled = false;
                button1.Enabled = true;

                // Raise window to notify user
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);
                label1.Text = this.pd.ToString();               
            }
        }

    }
}
