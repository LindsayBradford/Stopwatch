using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Stowpatch.Model;

namespace Stopwatch.Model.Test
{

    public class TestElapsedTime
    {

        private ElapsedTime _elapsedTimeUnderTest;

        [SetUp]
        public void Setup()
        {
          _elapsedTimeUnderTest = new ElapsedTime();
        }

        [Test]
        public void Init_NoElapsedTime()
        {
            Assert.That(_elapsedTimeUnderTest.Elapsed, Is.EqualTo(TimeSpan.Zero));
        }

        [Test]
        public void Pause_TimeElapsed()
        {
            // Given

            UInt16 pauseMilliseconds = 100;
            TimeSpan expectedElapsed = TimeSpan.FromMilliseconds(pauseMilliseconds);
            TimeSpan acceptableTollerance = TimeSpan.FromMilliseconds(15);

            // When

            _elapsedTimeUnderTest.Start = DateTime.Now;
            Thread.Sleep(pauseMilliseconds);
            _elapsedTimeUnderTest.End = DateTime.Now;

            TimeSpan actualElapsed = _elapsedTimeUnderTest.Elapsed;

            // Then

            Assert.That(actualElapsed, Is.EqualTo(expectedElapsed).Within(acceptableTollerance));
        }
    }

    public class TestThreadTicker
    {
        private class TickerSpy : TickRecipient
        {
            public UInt64 TickCount { get; set; }

            public bool ReceivingTicks { get; set;}

            public void Tick()
            {
                this.TickCount +=1;
            }
        }

        private TickerSpy _tickerSpy;
        private ThreadTicker _tickerUnderTest;

        [SetUp]
        public void Setup()
        {
            _tickerSpy = new();
            _tickerUnderTest = new(_tickerSpy, 10);
        }

        [Test]
        public void Init_NoTickCallback()
        {
            // Given
            DateTime targetDelay = DateTime.Now.AddMilliseconds(60);

            // When

            while (DateTime.Now < targetDelay) { /* deliberately doing nothing */ }

            // Then

            Assert.That(_tickerSpy.TickCount, Is.Zero);
        }

        [Test]
        public void Tick_TriggersCallback()
        {
            // Given
            DateTime targetDelay = DateTime.Now.AddMilliseconds(60);

            // When

            _tickerSpy.ReceivingTicks = true;
            while (DateTime.Now < targetDelay) { /* deliberately doing nothing */ }
            _tickerSpy.ReceivingTicks = false;

            // Then

            Assert.That(_tickerSpy.TickCount, Is.GreaterThan(2));
        }

    }

    class MockTicker : AbstractTicker
    {
        public MockTicker(TickRecipient recipient) : base(recipient) { }

        public void ForceTick()
        {
            this.tickIfNeeded();
        }
    }

    public class TestDefaultTimer
    {
        private DefaultTimer _timerUnderTest;
        private MockTicker _mockTicker;

        [SetUp]
        public void Setup()
        {
            _timerUnderTest = new DefaultTimer();
            _mockTicker = new MockTicker(_timerUnderTest);
        }

        [Test]
        public void Init_NoTimeElapsing()
        {
            TimeSpan actualElapsedTime = _timerUnderTest.ElapsedTime;
            Assert.That(actualElapsedTime, Is.EqualTo(TimeSpan.Zero));
        }

        [Test]
        public void Init_TickerFiring_NoTimeElapsing()
        {
            _mockTicker.ForceTick();

            TimeSpan actualElapsedTime = _timerUnderTest.ElapsedTime;
            Assert.That(actualElapsedTime, Is.EqualTo(TimeSpan.Zero));
        }

        [Test]
        public void Reset_NoTimeElapsing()
        {
            _timerUnderTest.Reset();

            TimeSpan actualElapsedTime = _timerUnderTest.ElapsedTime;
            Assert.That(actualElapsedTime, Is.EqualTo(TimeSpan.Zero));
        }

        [Test]
        public void Start_NoTick_NoTimeElapses()
        {
            _timerUnderTest.Start();

            TimeSpan actualElapsedTime = _timerUnderTest.ElapsedTime;
            Assert.That(actualElapsedTime, Is.EqualTo(TimeSpan.Zero));
        }

        [Test]
        public void Start_Tick_TimeElapses()
        {
            _timerUnderTest.Start();
            _mockTicker.ForceTick();

            TimeSpan actualElapsedTime = _timerUnderTest.ElapsedTime;
            Assert.That(actualElapsedTime, Is.GreaterThan(TimeSpan.Zero));
        }

        [Test]
        public void ResetAfterTimeElapses_NoTimeElapsedReported()
        {
            _timerUnderTest.Start();
            _mockTicker.ForceTick();
            _timerUnderTest.Reset();

            TimeSpan actualElapsedTime = _timerUnderTest.ElapsedTime;
            Assert.That(actualElapsedTime, Is.EqualTo(TimeSpan.Zero));
        }

        [Test]
        public void ResetThenTick_NoTimeElapsedReported()
        {
            _timerUnderTest.Start();
            _mockTicker.ForceTick();

            _timerUnderTest.Reset();
            _mockTicker.ForceTick();

            TimeSpan actualElapsedTime = _timerUnderTest.ElapsedTime;
            Assert.That(actualElapsedTime, Is.EqualTo(TimeSpan.Zero));
        }

        [Test]
        public void Stop_CapturesElapsedTimeFromStart()
        {
            _timerUnderTest.Start();
            TimeSpan firstElapsedTime = _timerUnderTest.ElapsedTime;

            _timerUnderTest.Stop();
            TimeSpan secondElapsedTime = _timerUnderTest.ElapsedTime;
            Assert.That(secondElapsedTime, Is.GreaterThan(firstElapsedTime));
        }

        public void Stop_CapturesElapsedTimeSinceLastTick()
        {
            _timerUnderTest.Start();
            TimeSpan startElapsedTime = _timerUnderTest.ElapsedTime;

            _mockTicker.ForceTick();
            TimeSpan tickElapsedTime = _timerUnderTest.ElapsedTime;
            Assert.That(tickElapsedTime, Is.GreaterThan(startElapsedTime));

            _timerUnderTest.Stop();
            TimeSpan stopElapsedTime = _timerUnderTest.ElapsedTime;
            Assert.That(stopElapsedTime, Is.GreaterThan(tickElapsedTime));
        }

        [Test]
        public void Stop_NoMoreTimeElapsesAfterStop()
        {
            // given
            _timerUnderTest.Start();
            _timerUnderTest.Stop();
            TimeSpan stopElapsedTime = _timerUnderTest.ElapsedTime;

            // when
            _mockTicker.ForceTick();
            TimeSpan subsequentElapsedTime = _timerUnderTest.ElapsedTime;

            // then
            Assert.That(subsequentElapsedTime, Is.EqualTo(stopElapsedTime));
        }
    }
}