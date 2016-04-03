namespace FarmAutomation.ItemCollector.Interfaces
{
    internal interface IAnimalHouseProcessorConfiguration
    {
        int AdditionalFriendshipFromCollecting { get; set; }
        bool PetAnimals { get; set; }
        bool MuteAnimalsWhenCollecting { get; set; }
    }
}