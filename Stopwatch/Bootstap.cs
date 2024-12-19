using System;
using System.Windows.Forms;

using Stopwatch.Model;
using Stopwatch.Presenter;
using Stopwatch.View;

namespace Stopwatch
{


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

            Application.Run(buildApplication());
        }

        static private Form buildApplication()
        {
            IModel model = ModelFactory.BuildDefaultModelWithThreadTicker();
            IView view = new WInFormsView();

            new WinformsPresenter().ForView(view).ForModel(model);

            return (Form) view;
        }
    }
}
