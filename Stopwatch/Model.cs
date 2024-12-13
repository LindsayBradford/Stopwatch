using System;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Windows.Forms;
using Stopwatch.View;

namespace Stopwatch.Model
{

    public class ModelFactory
    {
        public static Model BuildDefaultModelWithThreadTicker()
        {
            Model model = new DefaultModel();
            ThreadTicker ticker = new(model);
            model.EventHandler += ticker.HandleModelEvent;

            return model;
        }
    }

    public enum ModelEvent
    {
        ElapsedTimeChanged,
        Dieing
    }

    public interface TickRecipient
    {
        bool ReceivingTicks { get; set; }
        void Tick();
    }

    public interface Model: TickRecipient
    {
        void Start();
        void Stop();
        void Reset();
        void Die();
        TimeSpan ElapsedTime { get; }

        public event EventHandler<ModelEvent> EventHandler;
    }

    public class ElapsedTime
    {

        public ElapsedTime()
        {
            this.Start = DateTime.Now;
            this.End = this.Start;
        }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public TimeSpan Elapsed { 
            get { return End - Start; } }

    }

    public class DefaultModel : Model, TickRecipient
    {
        public event EventHandler<ModelEvent> EventHandler = delegate { };

        private ElapsedTime _elapsed = new();

        public bool ReceivingTicks { get; set; }

        public TimeSpan ElapsedTime { get; set; }
        public void Timer() {
            this.Reset();
        }
        public void Reset()
        {
            this.ReceivingTicks = false;
            _elapsed = new ElapsedTime();
            this.ElapsedTime = TimeSpan.Zero;
            this.ReceivingTicks = false;
            RaiseEvent(ModelEvent.ElapsedTimeChanged);
        }

        public void Start()
        {
            _elapsed.Start = DateTime.Now;
            this.ReceivingTicks = true;
        }

        public void Stop()
        {
            this.ReceivingTicks = false;
            _elapsed.End = DateTime.Now;
            this.ElapsedTime += _elapsed.Elapsed;
            _elapsed.Start = _elapsed.End;

            RaiseEvent(ModelEvent.ElapsedTimeChanged);
        }
        public void Tick()
        {
            if (this.ReceivingTicks) {
                _elapsed.End = DateTime.Now;
                this.ElapsedTime += _elapsed.Elapsed;
                _elapsed.Start = _elapsed.End;

                RaiseEvent(ModelEvent.ElapsedTimeChanged);
            }
        }

        public void Die()
        {
            Stop();
            RaiseEvent(ModelEvent.Dieing);
        }
        private void RaiseEvent(ModelEvent modelEvent)
        {
            EventHandler(this, modelEvent);
        }
    }

    public abstract class AbstractTicker
    {
        protected readonly TickRecipient recipient;

        public AbstractTicker(TickRecipient recipient) {
            this.recipient = recipient;
        }

        protected virtual void tickIfNeeded()
        {
            if (recipient.ReceivingTicks)
                recipient.Tick();
        }
    }

    public class ThreadTicker : AbstractTicker
    {
        public const UInt16 DEFAULT_TICK_FREQUENCY = 16;  // Approximately 60hz refresh rate.

        private UInt16 tickFrequency;  // milliseconds
        private Thread tickingThread;
        private bool _running = false;

        public ThreadTicker(TickRecipient recipient) :base(recipient)
        {
            this.tickFrequency = DEFAULT_TICK_FREQUENCY;
            BootstrapTickingThread();
        }

        public ThreadTicker(TickRecipient recipient, UInt16 tickFrequency = DEFAULT_TICK_FREQUENCY) : base(recipient)
        {
            if (tickFrequency != DEFAULT_TICK_FREQUENCY) 
                this.tickFrequency = tickFrequency;
            BootstrapTickingThread();
        }

        private void BootstrapTickingThread()
        {
            tickingThread = new Thread(tickIfNeeded);
            _running = true;
            tickingThread.Start();
        }

        protected override void tickIfNeeded()
        {
            while (_running)
            {
                Thread.Sleep(tickFrequency);

                if (recipient.ReceivingTicks)
                    recipient.Tick();
            }
        }

        private void Die()
        {
            _running = false;
        }

        public void HandleModelEvent(object source, ModelEvent modelEvent)
        {
            switch (modelEvent)
            {
                case ModelEvent.Dieing:
                    Die();
                    break;
                default:
                    // Deliberately does nothing
                    break;
            }
        }
    }
}

