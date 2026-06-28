using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using WandEnhancer.View.MainWindow;

namespace WandEnhancer
{
    public static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            // Give every Regex a default match timeout so a pathological pattern on an
            // unsupported, multi-megabyte minified bundle fails fast instead of pinning a CPU core.
            AppDomain.CurrentDomain.SetData("REGEX_DEFAULT_MATCH_TIMEOUT", TimeSpan.FromSeconds(10));

            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;

            List<LogEntry> logEntries = new List<LogEntry>();
            if (args.Length > 0)
            {
                // TODO: Command line arguments handling
            }

            var application = new App();
            application.InitializeComponent();
            application.MainWindow = new MainWindow();
            foreach (var logEntry in logEntries)
            {
                MainWindow.Instance.ViewModel.LogList.Add(logEntry);
            }
            application.Run();
        }
        
        
        private static void OnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            // Background tasks (update checks, fire-and-forget work) must never take the whole app
            // down. Mark the exception observed so the runtime does not escalate it.
            e.SetObserved();
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.ExceptionObject.ToString());
            Environment.Exit(1);
        }
    }
}