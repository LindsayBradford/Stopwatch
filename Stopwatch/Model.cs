using System;
using System.Threading;
using Stowpatch.Model;

namespace Stowpatch.Model
{
    public interface TickRecipient
    {
        bool ReceivingTicks { get; set; }
        void Tick();
    }

    public interface Timer
    {
        void Start();
        void Stop();
        void Reset();
        TimeSpan ElapsedTime { get; }
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
        public TimeSpan Elapsed { get { return End - Start; } }

    }

    public class DefaultTimer : Timer, TickRecipient
    {
        private ElapsedTime _elapsed = new ElapsedTime();

        public bool ReceivingTicks { get; set; }

        public TimeSpan ElapsedTime { get { return _elapsed.Elapsed; } }

        public void Timer() {
            this.Reset();
        }
        public void Reset()
        {
            _elapsed = new ElapsedTime();
            this.ReceivingTicks = false;
        }

        public void Start()
        {
            _elapsed.Start = DateTime.Now;
            _elapsed.End = _elapsed.Start;

            this.ReceivingTicks = true;
        }

        public void Stop()
        {
            this.ReceivingTicks = false;
            _elapsed.End = DateTime.Now;
        }
        public void Tick()
        {
            if (this.ReceivingTicks) {
                _elapsed.End = DateTime.Now;
            }
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
        public const UInt16 DEFAULT_TICK_FREQUENCY = 10;

        private UInt16 tickFrequency;  // milliseconds
        private Thread tickingThread;

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
            tickingThread.Start();
        }

        protected override void tickIfNeeded()
        {
            while(true){
                Thread.Sleep(tickFrequency);

                if (recipient.ReceivingTicks)
                    recipient.Tick();
            }
        }
    }
}

