using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp1.Codes
{
    public class MyLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            return new MyLogger();
        }

        private class MyLogger : ILogger
        {
            public Object Kilit = new () { };

            #region Write Log
            public void WriteLog(string S)
            {
                string strDay = DateTime.Today.ToString("yyyy.MM.dd");

                try
                {
                    if (!System.IO.Directory.Exists("logs"))
                    {
                        System.IO.Directory.CreateDirectory("logs");
                    }

                    lock (Kilit)
                    {
                        System.IO.StreamWriter dosya = System.IO.File.AppendText("logs/myLogger_" + strDay + ".txt");
                        dosya.WriteLine(S);
                        dosya.WriteLine("".PadLeft(30, '-'));
                        dosya.Close();
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        lock (Kilit)
                        {
                            System.IO.StreamWriter dosya = System.IO.File.AppendText("logs/myLogger_hata_" + strDay + ".txt");
                            dosya.WriteLine("hata(" + DateTime.Now.ToString() + "):" + ex.Message);
                            dosya.WriteLine("".PadLeft(30, '-'));
                            dosya.Close();
                        }
                    }
                    catch { }
                }
            }
            #endregion

            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                this.WriteLog(formatter(state, exception));
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                return null;
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
    
}

