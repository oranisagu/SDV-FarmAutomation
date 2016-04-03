using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using FarmAutomation.ItemCollector.Interfaces;
using FarmAutomation.ItemCollector.Processors;

namespace FarmAutomation.ItemCollector
{
    public class ItemCollecterInstallers :IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IAnimalHouseProcessor>().ImplementedBy<AnimalHouseProcessor>());
            container.Register(Component.For<IMachineHelper>().ImplementedBy<MachineHelper>());
            container.Register(Component.For<IMachinesProcessor>().ImplementedBy<MachinesProcessor>());
            container.Register(Component.For<IMaterialHelper>().ImplementedBy<MaterialHelper>());
        }
    }
}