namespace FarmAutomation.Common.Interfaces
{
    public interface ILog
    {
        void Debug(string message);
        void Info(string message);
        void Error(string message);
    }
}
