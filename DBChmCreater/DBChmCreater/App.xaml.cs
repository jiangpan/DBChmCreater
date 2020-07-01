using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Serilog;

namespace DBChmCreater
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public App()
        {
            this.Dispatcher.UnhandledException += OnDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            Log.Logger = new LoggerConfiguration()
.MinimumLevel.Debug()
.Enrich.WithProperty("Application", "synyi.hdr.suite")
.WriteTo.Conditional(p => p.Level == Serilog.Events.LogEventLevel.Error, s => s.File(@"Logs\Error.log", shared: true, fileSizeLimitBytes: 10485760, rollOnFileSizeLimit: true, rollingInterval: RollingInterval.Day))
//.WriteTo.Logger(l=> l.Filter.ByIncludingOnly(e=>e.Level == Serilog.Events.LogEventLevel.Error).WriteTo.File(@"Logs\Error-{Date}.log",rollingInterval: RollingInterval.Day) )
.WriteTo.File(@"Logs\Log.log", shared: true, fileSizeLimitBytes: 10485760, rollOnFileSizeLimit: true, rollingInterval: RollingInterval.Day)
.CreateLogger();
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            string fileName = "app_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            string path = System.IO.Path.Combine(AppContext.BaseDirectory, fileName);
            File.AppendAllText(path, $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} [{e.Exception.Message}] {"Application处理异常"} " + Environment.NewLine);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            string fileName = "app_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            string path = System.IO.Path.Combine(AppContext.BaseDirectory, fileName);
            File.AppendAllText(path, $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} [{e.ExceptionObject.ToString()}] {"Application处理异常"} " + Environment.NewLine);
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            string fileName = "app_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            string path = System.IO.Path.Combine(AppContext.BaseDirectory, fileName);
            File.AppendAllText(path, $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} [{e.Exception.Message}] {"Application处理异常"} " + Environment.NewLine);
            e.Handled = true;
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            string fileName =  "app_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            string path = System.IO.Path.Combine(AppContext.BaseDirectory, fileName);
            File.AppendAllText(path, $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} [{e.Exception.Message}] {"Application处理异常"} " + Environment.NewLine);
            e.Handled = true;
        }
    }
}
