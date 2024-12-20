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
        private readonly SoundPlayer clickSound = new SoundPlayer();

        public WInFormsView()
        {

            Assembly assembly = Assembly.GetExecutingAssembly();

            Stream stream = assembly.GetManifestResourceStream("Stopwatch.Resources.stopwatch.wav");
            clickSound.Stream = stream;


            InitializeComponent();
            DisableResizing();
            StartStopButton.Text = "&Start";
            RunningRadioButton.Checked = false;
            StoppedRadioButton.Checked = true;
        }

        public string Message { 
            get { return this.MessageLabel.Text; }
            set { this.MessageLabel.Text = value; }
        }

        private TimeSpan elapsedTime;
        public TimeSpan ElapsedTime { 
            get { return elapsedTime;  }
            set { 
                this.elapsedTime = value;
                UpdateElapsedTimeText();
            }
        }

        private void UpdateElapsedTimeText()
        {
            this.ElapsedTimeLabel.Rtf = formatElapsedTime();
            this.ElapsedTimeLabel.Refresh();
        }

        private string formatElapsedTime()
        {
            string timeAsText = elapsedTime.ToString(@"hh\:mm\:ss");
            string millisecondsAsText = elapsedTime.ToString(@"\.fff");
            string formattedTimeAsText = @"{\rtf1\ansi{" + timeAsText + @"\super" + millisecondsAsText + @"\nosupersub}}";
            return formattedTimeAsText;
        }

        public event EventHandler<ViewEvent> EventHandler = delegate { };

        public void RaiseEvent(ViewEvent viewEvent)
        {
            EventHandler(this, viewEvent);
        }

        public void ResetButtonPressed(object sender, EventArgs e)
        {
            clickSound.Play();
            this.RaiseEvent(ViewEvent.Reset);
            RenderStoppedState();
        }

        public void StartStopButtonPressed(object sender, EventArgs e)
        {
            clickSound.Play();
            string currentText = StartStopButton.Text;
            if (currentText.Contains("Start"))
            {
                this.RaiseEvent(ViewEvent.Start);
                RenderRunningState();
            }
            else
            {
                this.RaiseEvent(ViewEvent.Stop);
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

        public void WinFormsView_FormClosing(object sender, FormClosingEventArgs e)
        {
            RaiseEvent(ViewEvent.Closing);
        }
    }

    public class NullView : IView
    {
        public string Message { get; set; }
        public TimeSpan ElapsedTime { get => TimeSpan.Zero; set; }

        public event EventHandler<ViewEvent> EventHandler;

        public void RaiseEvent(ViewEvent stopwatchEvent)
        {
            // deliberately does nothing.
        }
    }

}
