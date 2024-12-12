using System;

using System.Windows.Forms;

namespace Stopwatch.View
{
    public interface IView
    {
        string Message { get; set; }
        TimeSpan ElapsedTime { get; set; }

        public event EventHandler<Event> EventHandler;
        void RaiseEvent(Event stopwatchEvent);
    }

    public partial class WInFormsView : Form, IView
    {
        public WInFormsView()
        {
            InitializeComponent();
            disableResizing();
        }

        public string Message { 
            get { return this.MessageLabel.Text; }
            set { this.MessageLabel.Text = value; }
        }

        public TimeSpan ElapsedTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public event EventHandler<Event> EventHandler = delegate { };

        public void RaiseEvent(Event stopwatchEvent)
        {
            EventHandler(this, stopwatchEvent);
        }

        public void ResetButtonPressed(object sender, EventArgs e)
        {
            this.RaiseEvent(Event.Reset);
        }

        public void StartStopButtonPressed(object sender, EventArgs e)
        {
            string currentText = StartStopButton.Text;
            if (currentText.Contains("Start"))
            {
                this.RaiseEvent(Event.Start);
                StartStopButton.Text = "&Stop";
            }
            else
            {
                this.RaiseEvent(Event.Stop);
                StartStopButton.Text = "&Start";
            }
        }

        private void disableResizing()
        {
            FormBorderStyle = FormBorderStyle.FixedSingle;
        }
    }
}
