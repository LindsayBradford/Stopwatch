using System;
using System.Windows.Forms;

using Stopwatch.Model;
using Stopwatch.Presenter;
using Stopwatch.View;

namespace Stopwatch
{

    public enum Event
    {
        Start,
        Stop,
        Tick,
        Reset
    }
    internal static class Bootstap
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Presenter.IPresenter presenter = buildApplication();

            Application.Run((Form) presenter.View);
        }

        static private Presenter.IPresenter buildApplication()
        {
            Presenter.IPresenter presenter = new WInformsPresenter();

            presenter.Model = new DefaultModel();
            presenter.View = new WInFormsView();

            return presenter;
        }
    }
}
