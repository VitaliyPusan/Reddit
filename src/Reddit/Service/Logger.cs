using System;
using System.Diagnostics;
using Reddit.Service.Contracts;

namespace Reddit.Service
{
    internal class Logger : ILogger
    {
        public void Log(Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }

        public void Log(string message)
        {
            Debug.WriteLine(message);
        }
    }
}