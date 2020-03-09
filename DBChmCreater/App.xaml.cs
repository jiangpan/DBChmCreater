using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DBChmCreater
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            string fileName =  "app_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            string path = System.IO.Path.Combine(AppContext.BaseDirectory, fileName);
            File.AppendAllText(path, $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} [{e.Exception.Message}] {"Application处理异常"} " + Environment.NewLine);
            e.Handled = true;

        }
    }
}
