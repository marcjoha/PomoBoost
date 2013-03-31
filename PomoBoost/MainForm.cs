using System;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Taskbar;
using PomoBoost.Properties;

namespace PomoBoost
{
    public partial class MainForm : Form
    {
        private readonly ThumbnailToolBarButton buttonPause;
        private readonly ThumbnailToolBarButton buttonPlay;
        private readonly PomodoroTimer pd;

        public MainForm()
        {
            InitializeComponent();

            // Set the icon
            Icon = Resources.Timer;

            pd = new PomodoroTimer(new TimeSpan(0, 25, 0));

            // Configure the progess bar
            progressBar1.Minimum = 0;
            progressBar1.Maximum = (int) pd.GetTimeSpan().TotalSeconds;
            progressBar1.Step = 1;

            // Configure the thumbnail toolbar
            buttonPlay = new ThumbnailToolBarButton(Resources.Play, "Run");
            buttonPlay.Click += buttonPlay_Click;
            buttonPause = new ThumbnailToolBarButton(Resources.Pause, "Pause");
            buttonPause.Click += buttonPause_Click;
            TaskbarManager.Instance.ThumbnailToolBars.AddButtons(Handle, buttonPlay, buttonPause);
        }

        // Event-handlers for thumbnail toolbar
        private void buttonPlay_Click(object sender, ThumbnailButtonClickedEventArgs e)
        {
            if (!timer1.Enabled)
            {
                timer1.Start();
                button1.Text = "Pause";
                buttonPlay.Enabled = false;
                buttonPause.Enabled = true;
            }
        }

        private void buttonPause_Click(object sender, ThumbnailButtonClickedEventArgs e)
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
            if (timer1.Enabled)
            {
                timer1.Stop();
                button1.Text = "Run";
                buttonPlay.Enabled = true;
                buttonPause.Enabled = false;
            }
            else
            {
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
            pd.Reset();

            TaskbarManager.Instance.SetProgressValue(0, (int) pd.GetTimeSpan().TotalSeconds);
            progressBar1.Value = 0;
            label1.Text = pd.ToString();
            buttonPlay.Enabled = true;
            buttonPause.Enabled = false;
            button1.Text = "Run";
            button1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pd.Tick(new TimeSpan(0, 0, 1));

            // Update the two progessbars
            TaskbarManager.Instance.SetProgressValue(
                (int) pd.GetTimeSpan().TotalSeconds - (int) pd.GetTimeRemaining().TotalSeconds,
                (int) pd.GetTimeSpan().TotalSeconds);
            progressBar1.PerformStep();

            if (pd.IsZero())
            {
                timer1.Stop();

                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Error);
                label1.Text = "Rest!";
                buttonPlay.Enabled = false;
                buttonPause.Enabled = false;
                button1.Enabled = true;

                // Raise window to notify user
                WindowState = FormWindowState.Normal;
            }
            else
            {
                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);
                label1.Text = pd.ToString();
            }
        }
    }
}