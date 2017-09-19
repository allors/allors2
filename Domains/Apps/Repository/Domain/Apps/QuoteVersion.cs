namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("")]
    #endregion
    public partial interface QuoteVersion : Version
    {
        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Workspace]
        [Size(-1)]
        string InternalComment { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        DateTime RequiredResponseDate { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Workspace]
        DateTime ValidFromDate { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        QuoteTerm[] QuoteTerms { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        Party Issuer { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Workspace]
        DateTime ValidThroughDate { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Required]
        [Size(-1)]
        [Workspace]
        string Description { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        Party Receiver { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        ContactMechanism FullfillContactMechanism { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal Amount { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        Currency Currency { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Workspace]
        DateTime IssueDate { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        QuoteItem[] QuoteItems { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Size(256)]
        [Workspace]
        string QuoteNumber { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        QuoteObjectState CurrentObjectState { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        Request Request { get; set; }
    }
}