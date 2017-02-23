namespace Allors.Repository
{
    using Attributes;

    #region Allors
    [Id("7847dd12-33ce-4d3f-b093-b0844c4ddb8f")]
    #endregion
    public partial interface AsyncDerivable :  Object 
    {
        [Id("444FFDA5-3C3C-418A-9459-F86D591D0AB7")]
        void AsyncDerive();
    }
}