namespace Allors.Domain.TestPopulation
{
    using Allors.Domain;
 
    public static partial class SerialisedItemExtensions
    {
        public static string DisplayName(this SerialisedItem @this) => @this.ItemNumber + " " + @this.Name + " SN: " + @this.SerialNumber;
    }
}
