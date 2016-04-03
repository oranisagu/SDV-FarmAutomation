using FarmAutomation.Common.Configuration;

namespace FarmAutomation.Common.Interfaces
{
    public interface IConfigurator
    {
        T LoadConfiguration<T>() where T : IConfigurationBase;
    }
}
