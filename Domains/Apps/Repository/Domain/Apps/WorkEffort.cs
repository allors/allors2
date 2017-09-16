namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("553a5280-a768-4ba1-8b5d-304d7c4bb7f1")]
    #endregion
    public partial interface WorkEffort : IWorkEffort, UniquelyIdentifiable, Deletable, Auditable 
    {
        #region Allors
        [Id("B95571A0-84DF-4648-80FD-C4FE9067991F")]
        #endregion
        [Workspace]
        void Confirm();

        #region Allors
        [Id("46D78F1B-D77A-4240-87AB-14934BA12761")]
        #endregion
        [Workspace]
        void Finish();

        #region Allors
        [Id("D9234724-215F-4F6C-B3E8-9743CB22A245")]
        #endregion
        [Workspace]
        void Cancel();

        #region Allors
        [Id("F581B87C-EE9D-4D43-9719-8BC5CCFAC2C3")]
        #endregion
        [Workspace]
        void Reopen();
    }
}