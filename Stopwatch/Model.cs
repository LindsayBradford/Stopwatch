using System;
using System.Threading;

namespace Stopwatch.Model
{

    public class ModelFactory
    {
        public static IModel BuildDefaultModelWithThreadTicker()
        {
            IModel model = new DefaultModel();
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

    public interface ITickRecipient
    {
        bool ReceivingTicks { get; set; }
        void Tick();
    }

    public interface IModel: ITickRecipient
    {
        void Start();
        void Stop();
        void Reset();
        void Die();
        TimeSpan ElapsedTime { get; }

        public event EventHandler<ModelEvent> EventHandler;
    }

    public class NullModel : IModel
    {
        public TimeSpan ElapsedTime => TimeSpan.Zero;

        public bool ReceivingTicks { get; set; } = false;

        public event EventHandler<ModelEvent>? EventHandler;

        public void Die() {}

        public void Reset() {}

        public void Start() {}

        public void Stop() {}

        public void Tick() {}
    }

    public class ElapsedTime
    {

        public ElapsedTime()
        {
            Start = DateTime.Now;
            End = Start;
        }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public TimeSpan Elapsed { 
            get { return End - Start; } }

    }

    public class DefaultModel : IModel, ITickRecipient
    {
        public event EventHandler<ModelEvent> EventHandler = delegate { };

        private ElapsedTime _elapsed = new();

        public bool ReceivingTicks { get; set; }

        public TimeSpan ElapsedTime { get; set; }
        public void Timer() {
            Reset();
        }
        public void Reset()
        {
            ReceivingTicks = false;
            _elapsed = new ElapsedTime();
            ElapsedTime = TimeSpan.Zero;
            RaiseEvent(ModelEvent.ElapsedTimeChanged);
        }

        public void Start()
        {
            _elapsed.Start = DateTime.Now;
            ReceivingTicks = true;
        }

        public void Stop()
        {
            ReceivingTicks = false;
            _elapsed.End = DateTime.Now;
            ElapsedTime += _elapsed.Elapsed;
            _elapsed.Start = _elapsed.End;

            RaiseEvent(ModelEvent.ElapsedTimeChanged);
        }
        public void Tick()
        {
            if (ReceivingTicks) {
                _elapsed.End = DateTime.Now;
                ElapsedTime += _elapsed.Elapsed;
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
        protected readonly ITickRecipient recipient;

        public AbstractTicker(ITickRecipient recipient) {
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

        private readonly UInt16 tickFrequency;  // milliseconds
        private Thread? tickingThread;
        private bool _running = false;

        public ThreadTicker(ITickRecipient recipient) :base(recipient)
        {
            tickFrequency = DEFAULT_TICK_FREQUENCY;
            BootstrapTickingThread();
        }

        public ThreadTicker(ITickRecipient recipient, UInt16 tickFrequency = DEFAULT_TICK_FREQUENCY) : base(recipient)
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

        public void HandleModelEvent(object? source, ModelEvent modelEvent)
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
        private void Die()
        {
            _running = false;
        }

    }
}

