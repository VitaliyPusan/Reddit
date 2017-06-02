using System;

namespace Reddit.Service.Contracts
{
    public interface ILogger
    {
        void Log(Exception ex);
        void Log(string message);
    }
}