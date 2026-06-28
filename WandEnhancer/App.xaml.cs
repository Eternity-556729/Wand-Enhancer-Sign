using System;
using System.Threading.Tasks;
using System.Windows;
using WandEnhancer.Core;
using WandEnhancer.Core.Services;
using WandEnhancer.View.MainWindow;
using MessageBox = System.Windows.Forms.MessageBox;

namespace WandEnhancer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            DispatcherUnhandledException += OnDispatcherUnhandledException;
            ThemeManager.Initialize();
            LocalizationManager.Initialize();
            this.MainWindow.Show();
        }

        private void OnDispatcherUnhandledException(object sender,
            System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            // A UI-thread exception should not silently crash the patcher. Show the message and
            // keep the window alive instead of letting WPF tear the process down.
            MessageBox.Show(e.Exception.Message, "WandEnhancer",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            e.Handled = true;
        }

        public static void RequestShutdown()
        {
            Current.Dispatcher.Invoke(() => Current.Shutdown());
        }
    }
}