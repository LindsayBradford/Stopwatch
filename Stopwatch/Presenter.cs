using System;
using Stopwatch.Model;
using Stopwatch.View;

namespace Stopwatch.Presenter
{

    public interface IPresenter
    {
        public IPresenter ForModel(Model.IModel model);
        public IPresenter ForView(IView view);

        void HandleViewEvent(Object source, ViewEvent stopwatchEvent);
        void HandleModelEvent(Object source, ModelEvent stopwatchEvent);
    }

    public class WinformsPresenter : IPresenter
    {

        public IPresenter ForModel(Model.IModel model)
        {
            model.EventHandler += this.HandleModelEvent;
            this.model = model;
            return this;
        }

        public IPresenter ForView(IView view)
        {
            view.EventHandler += this.HandleViewEvent;
            this.view = view;
            return this;
        }

        private Model.IModel model = new NullModel(); 
        private IView view = new NullView();
       public void HandleViewEvent(Object source,  ViewEvent viewEvent)
        {
            switch (viewEvent)
            {
                case ViewEvent.Start:
                    model.Start();
                    view.Message = "Stopwatch running...";
                    break;
                case ViewEvent.Stop:
                    model.Stop();
                    view.Message = "Stopwatch paused.";
                    break;
                case ViewEvent.Reset:
                    model.Reset();
                    view.Message = "Stopwatch elapsed time reset.";
                    break;
                case ViewEvent.Closing:
                    model.Die();
                    break;
                default:
                    // Deliberately does nothing
                    break;
            }
        }

        public void HandleModelEvent(object source, ModelEvent modelEvent)
        {
            switch (modelEvent)
            {
                case ModelEvent.ElapsedTimeChanged:
                    view.ElapsedTime = model.ElapsedTime;
                    break;
                default:
                    // Deliberately does nothing
                    break;
            }
        }
    }
}