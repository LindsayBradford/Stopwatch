using Stopwatch.View;
using Stopwatch.Model;
using NSubstitute;
using NSubstitute.ReceivedExtensions;


namespace Stopwatch.Presenter.Test
{

    public class TestWinformsPresenter
    {

        private WinformsPresenter _presenterUnderTest;

        private IModel _modelSpy = Substitute.For<IModel>();
        private IView _viewSpy = Substitute.For<IView>();

        [SetUp]
        public void Setup()
        {
            _presenterUnderTest = new WinformsPresenter();
        }

        [Test]
        public void Init_HandleViewEvent_DoesNothing()
        {
            // when
            try
            {
                _presenterUnderTest.HandleViewEvent(this, ViewEvent.Closing);
            }
            catch (Exception ex)
            {
                Assert.Fail("Expected no execptions, but caught [" + ex.Message + "]");
            }
        }

        [Test]
        public void Init_HandleModelEvent_DoesNothing()
        {
            // when
            try
            {
                _presenterUnderTest.HandleModelEvent(this, ModelEvent.Dieing);
            }
            catch (Exception ex)
            {
                Assert.Fail("Expected no execptions, but caught [" + ex.Message + "]");
            }
        }

        [Test]
        public void HandleViewEvent_Closing_ModelDieTriggered()
        {
            // given
            _presenterUnderTest.ForModel(_modelSpy);

            // when
            _presenterUnderTest.HandleViewEvent(this, ViewEvent.Closing);

            // then

            _modelSpy.Received(1).Die();
        }

        [Test]
        public void HandleViewEvent_Start_ModelStartTriggered()
        {
            // given
            _presenterUnderTest.ForModel(_modelSpy);

            // when
            _presenterUnderTest.HandleViewEvent(this, ViewEvent.Start);

            // then
            _modelSpy.Received(1).Start();
        }

        [Test]
        public void HandleViewEvent_Stop_ModelStopTriggered()
        {
            // given
            _presenterUnderTest.ForModel(_modelSpy);

            // when
            _presenterUnderTest.HandleViewEvent(this, ViewEvent.Stop);

            // then
            _modelSpy.Received(1).Stop();
        }

        [Test]
        public void HandleViewEvent_Reset_ModelResetTriggered()
        {
            // given
            _presenterUnderTest.ForModel(_modelSpy);

            // when
            _presenterUnderTest.HandleViewEvent(this, ViewEvent.Reset);

            // then
            _modelSpy.Received(1).Reset();
        }

        [Test]
        public void HandleModelEvent_ElapsedTimeChanged_ViewElapsedTimeUpdated()
        {
            // given
            _presenterUnderTest.ForView(_viewSpy);

            // when
            _presenterUnderTest.HandleModelEvent(this, ModelEvent.ElapsedTimeChanged);

            // then
            _viewSpy.Received(1).ElapsedTime = TimeSpan.Zero;  // what default NullModel emits. 
        }
    }
}