namespace Allors.Repository
{
    using System;
    using Attributes;


    #region Allors
    [Id("b05371ff-0c9e-4ee3-b31d-e2edeed8649e")]
    #endregion
    public partial interface CommunicationEvent : Deletable, Commentable, UniquelyIdentifiable, Auditable, ICommunicationEvent
    {
        #region Allors
        [Id("F1D66D21-15CC-45C3-980C-E4179F66FD57")]
        #endregion
        [Workspace]
        void Cancel();

        #region Allors
        [Id("97011DA3-10B1-4B27-A4A0-E06D5D6CE04A")]
        #endregion
        [Workspace]
        void Close();

        #region Allors
        [Id("731D1CF2-01CE-44FE-8065-762E4DB1C5E0")]
        #endregion
        [Workspace]
        void Reopen();
    }
}