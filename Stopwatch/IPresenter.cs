using System;
using Stopwatch;
using Stopwatch.View;

namespace Stopwatch.Presenter
{

    public interface IPresenter
    {
        Model.Model Model { get; set; }
        View.IView View { get; set; }

        void HandleEvent(Object source, Event stopwatchEvent);
    }

    public class WInformsPresenter : IPresenter
    {

        private Model.Model model; 
        public Model.Model Model
        {
            get { return model; }
            set
            {
                model = value;
                // TODO: Tie in event handling
            }
        }

        private IView view;
        public View.IView View { 
            get { return view; }
            set {
                view = value;
                view.EventHandler += this.HandleEvent;
            }
        }
       public void HandleEvent(Object source, Event stopwatchEvent)
        {
            switch (stopwatchEvent)
            {
                case Event.Start:
                    View.Message = "Stopwatch Started...";
                    Model.Start();
                    break;
                case Event.Tick:
                    View.ElapsedTime = Model.ElapsedTime;
                    Model.Stop();
                    break;
                case Event.Stop:
                    View.Message = "Stopwatch Stopped...";
                    Model.Stop();
                    break;
                case Event.Reset:
                    Model.Reset();
                    View.Message = "Stopwatch Reset";
                    break;

                default:
                    // Deliberately does nothing
                    break;
            }
        }


    }
}