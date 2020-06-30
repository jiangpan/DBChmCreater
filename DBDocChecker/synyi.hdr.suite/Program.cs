using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Serilog;

namespace synyi.hdr.suite
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {


            Log.Logger = new LoggerConfiguration()
.MinimumLevel.Debug()
.Enrich.WithProperty("Application", "synyi.hdr.suite")
.WriteTo.Conditional(p => p.Level == Serilog.Events.LogEventLevel.Error, s => s.File(@"Logs\Error.log", shared: true, fileSizeLimitBytes: 10485760, rollOnFileSizeLimit: true, rollingInterval: RollingInterval.Day))
//.WriteTo.Logger(l=> l.Filter.ByIncludingOnly(e=>e.Level == Serilog.Events.LogEventLevel.Error).WriteTo.File(@"Logs\Error-{Date}.log",rollingInterval: RollingInterval.Day) )
.WriteTo.File(@"Logs\Log.log", shared: true, fileSizeLimitBytes: 10485760, rollOnFileSizeLimit: true, rollingInterval: RollingInterval.Day)
.CreateLogger();


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }
    }
}
