namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("75AF1A8F-5AE4-4979-8F18-850A73D30DCC")]
    #endregion
    [Plural("IStatementsOfWork")]
    public partial interface IStatementOfWork : IQuote 
    {
    }
}