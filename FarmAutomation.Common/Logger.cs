using FarmAutomation.Common.Interfaces;
using StardewModdingAPI;

namespace FarmAutomation.Common
{
    class Logger : ILog
    {
        public void Debug(string message)
        {
            Log.Debug(message);
        }

        public void Info(string message)
        {
            Log.Info(message);
        }

        public void Error(string message)
        {
            Log.Error(message);
        }
    }
}
