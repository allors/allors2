namespace Allors.Repository
{
    using System;

    public partial class Currency : UnitOfMeasure 
    {
        #region inherited properties
        public Guid UniqueId { get; set; }
        public string Description { get; set; }
        public UnitOfMeasureConversion[] UnitOfMeasureConversions { get; set; }
        public string Abbreviation { get; set; }

        public bool IsActive { get; set; }
        #endregion

        #region inherited methods
        public void Delete()
        {
        }
        #endregion
    }
}