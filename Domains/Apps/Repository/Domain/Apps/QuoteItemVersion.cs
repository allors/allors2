namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("AC703739-7176-4C4D-BFBD-C634CA47DA87")]
    #endregion
    public partial class QuoteItemVersion : IQuoteItem 
    {
        #region inherited properties
        public string InternalComment { get; set; }
        public Party Authorizer { get; set; }
        public Deliverable Deliverable { get; set; }
        public Product Product { get; set; }
        public DateTime EstimatedDeliveryDate { get; set; }
        public DateTime RequiredByDate { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }
        public ProductFeature ProductFeature { get; set; }
        public decimal UnitPrice { get; set; }
        public Skill Skill { get; set; }
        public WorkEffort WorkEffort { get; set; }
        public QuoteTerm[] QuoteTerms { get; set; }
        public int Quantity { get; set; }
        public RequestItem RequestItem { get; set; }
        public QuoteItemObjectState CurrentObjectState { get; set; }

        #endregion

        #region Allors
        [Id("C614F603-AC96-45C2-9E54-70AB70B1CDE4")]
        [AssociationId("CB50BC4C-EC00-46FF-A9BA-7B5E147BAE1E")]
        [RoleId("EB2B7F19-6566-42D6-AB65-9BF02A3B3B9D")]
        #endregion
        [Workspace]
        public DateTime TimeStamp { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }
        #endregion
    }
}