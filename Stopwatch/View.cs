using System;
using System.IO;
using System.Media;
using System.Reflection;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Stopwatch.View
{
    public enum ViewEvent
    {
        Start,
        Stop,
        Reset,
        Closing
    }
    public interface IView
    {
        string Message { get; set; }
        TimeSpan ElapsedTime { get; set; }

        public event EventHandler<ViewEvent> EventHandler;
        void RaiseEvent(ViewEvent stopwatchEvent);
    }

    public partial class WInFormsView : Form, IView
    {
        private readonly SoundPlayer clickSound = new();
        private byte[] clickSoundBytes = Array.Empty<byte>();

        private bool millisecondsVisible = true;

        public WInFormsView()
        {

            cacheClickSound();
            InitializeComponent();
            DisableResizing();

            StartStopButton.Text = "&Start";
            RunningRadioButton.Checked = false;
            StoppedRadioButton.Checked = true;
        }

        private void cacheClickSound()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            using Stream? stream = assembly.GetManifestResourceStream("Stopwatch.Resources.stopwatch.wav");
            if (stream is null)
            {
                return;
            }

            using BinaryReader br = new(stream);
            clickSoundBytes = br.ReadBytes((int)stream.Length);
        }

        private void PlayClickSound()
        {
            using (MemoryStream ms = new MemoryStream(clickSoundBytes))
            {
                clickSound.Stream = ms;
                clickSound.Play();
            }
        }

        public string Message { 
            get { return MessageLabel.Text; }
            set { MessageLabel.Text = value; }
        }

        private TimeSpan elapsedTime;
        public TimeSpan ElapsedTime { 
            get { return elapsedTime;  }
            set { 
                elapsedTime = value;
                UpdateElapsedTimeText();
            }
        }

        private void UpdateElapsedTimeText()
        {
            try
            {
                if (ElapsedTimeLabel.InvokeRequired)
                {
                    ElapsedTimeLabel.Invoke(new Action(UpdateElapsedTimeText));
                }
                else
                {
                    ElapsedTimeLabel.Rtf = formatElapsedTime();
                    ElapsedTimeLabel.SelectionAlignment = HorizontalAlignment.Center;
                    ElapsedTimeLabel.Refresh();
                }
            }
            catch (Exception)
            {
                // Don't care to refresh if form is dieing.
            }
        }

        private string formatElapsedTime()
        {
            string timeAsText = elapsedTime.ToString(@"hh\:mm\:ss");
            if (millisecondsVisible)
            {
                string millisecondsAsText = elapsedTime.ToString(@"\.fff");
                timeAsText = @"{\rtf1\ansi{" + timeAsText + @"\super" + millisecondsAsText + @"\nosupersub}}";
            } else
            {
                timeAsText = @"{\rtf1\ansi{" + timeAsText + "}";
            }

            return timeAsText;
        }

        public event EventHandler<ViewEvent> EventHandler = delegate { };

        public void RaiseEvent(ViewEvent viewEvent)
        {
            EventHandler(this, viewEvent);
        }

        public void ResetButtonPressed(object sender, EventArgs e)
        {
            PlayClickSound();
            RaiseEvent(ViewEvent.Reset);
            RenderStoppedState();
        }

        public void StartStopButtonPressed(object sender, EventArgs e)
        {
            PlayClickSound();
            string currentText = StartStopButton.Text;
            if (currentText.Contains("Start"))
            {
                RaiseEvent(ViewEvent.Start);
                RenderRunningState();
            }
            else
            {
                RaiseEvent(ViewEvent.Stop);
                RenderStoppedState();
            }
        }

        private void RenderRunningState()
        {
            StartStopButton.Text = "&Stop";
            RunningRadioButton.Checked = true;
            StoppedRadioButton.Checked = false;
        }

        private void RenderStoppedState()
        {
            StartStopButton.Text = "&Start";
            RunningRadioButton.Checked = false;
            StoppedRadioButton.Checked = true;
        }

        private void DisableResizing()
        {
            FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        public void ToggleMilliseconds(object sender, EventArgs e)
        {
            millisecondsVisible = !millisecondsVisible;
            UpdateElapsedTimeText();
        }

        public void WinFormsView_FormClosing(object sender, FormClosingEventArgs e)
        {
            RaiseEvent(ViewEvent.Closing);
        }
    }

    public class NullView : IView
    {
        public string Message { get; set; } = string.Empty;
        public TimeSpan ElapsedTime { get; set; } = TimeSpan.Zero;

        public event EventHandler<ViewEvent>? EventHandler;

        public void RaiseEvent(ViewEvent stopwatchEvent)
        {
            // deliberately does nothing.
        }
    }

}
