namespace Allors.Repository
{
    public partial class Currency : IUnitOfMeasure
    {
        #region inherited properties
        public string Abbreviation { get; set; }

        public LocalisedText[] LocalisedAbbreviations { get; set; }

        public string Description { get; set; }
        public UnitOfMeasureConversion[] UnitOfMeasureConversions { get; set; }

        public string Symbol { get; set; }
        #endregion
    }
}