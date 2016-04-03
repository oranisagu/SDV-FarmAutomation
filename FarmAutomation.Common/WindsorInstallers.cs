using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using FarmAutomation.Common.Configuration;
using FarmAutomation.Common.Interfaces;

namespace FarmAutomation.Common
{
    public class WindsorInstallers : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly());
            container.Register(Component.For<IFarmerFactory>().ImplementedBy<GhostFarmer.GhostFarmerFactory>());
            container.Register(Component.For<IItemFinder>().ImplementedBy<ItemFinder>());
            container.Register(Component.For<IConfigurator>().ImplementedBy<Configurator>());
            container.Register(Component.For<ILog>().ImplementedBy<Logger>());
            container.Register(Component.For<ILocationHelper>().ImplementedBy<LocationHelper>());
            container.Register(Component.For<ISoundHelper>().ImplementedBy<SoundHelper>());
        }
    }
}
