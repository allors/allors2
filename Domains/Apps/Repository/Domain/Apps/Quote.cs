namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("066bf242-2710-4a68-8ff6-ce4d7d88a04a")]
    #endregion
	public partial interface Quote : Transitional, Printable, Auditable, AccessControlledObject
    {
        #region Allors
        [Id("D566DD5B-BF58-45A6-A68F-FD7D2652FB4D")]
        [AssociationId("8F0A31C1-1B2A-4531-84BA-7636F4D0B9DF")]
        [RoleId("00EF6AB3-E075-45F5-A839-96576E3546AB")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        DateTime RequiredResponseDate { get; set; }
        
        #region Allors
        [Id("033df6dd-fdf7-44e4-84ca-5c7e100cb3f5")]
        [AssociationId("4b19f443-0d27-447d-8186-e5361a094460")]
        [RoleId("fa17ef86-c074-414e-b223-b62522d68280")]
        #endregion
        [Workspace]
        DateTime ValidFromDate { get; set; }

        #region Allors
        [Id("05e3454a-0a7a-488d-b4b1-f0fd41392ddf")]
        [AssociationId("ca3f0d26-9ead-4691-8f7f-f79272065251")]
        [RoleId("92e46228-ad44-4b9b-b727-23159a59bca3")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        QuoteTerm[] QuoteTerms { get; set; }

        #region Allors
        [Id("20880670-0496-4d24-8c97-69b83867c09e")]
        [AssociationId("c2bfd7fd-7956-4c28-960e-539f8159e46a")]
        [RoleId("3242cf4f-589c-457b-9ecd-59110041ab34")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        Party Issuer { get; set; }

        #region Allors
        [Id("2140e106-2ef3-427a-be94-458c2b8e154d")]
        [AssociationId("9d81ada4-a4f3-44bb-9098-bc1a3e61de19")]
        [RoleId("60581583-2536-4b09-acae-f0f877169dae")]
        #endregion
        [Workspace]
        DateTime ValidThroughDate { get; set; }

        #region Allors
        [Id("3da51ccc-24b9-4b03-9218-7da06492224d")]
        [AssociationId("602c70c9-ddc4-4cf5-a79f-0abcc0beba15")]
        [RoleId("d4d93ad0-c59d-40e7-a82c-4fb1e54a85f2")]
        #endregion
        [Required]
        [Size(-1)]
        [Workspace]
        string Description { get; set; }

        #region Allors
        [Id("9119c598-cd98-43da-bfdf-1e6573112c9e")]
        [AssociationId("d48cd46d-889b-4e2d-a6d6-ee26f30fb3e5")]
        [RoleId("56f5d5ee-1ab5-48f2-a413-7b80dd2c283e")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        Party Receiver { get; set; }

        #region Allors
        [Id("b5bcf357-ef14-424d-ad8d-01a8e3ff478c")]
        [AssociationId("b9338369-9081-4fa7-91c2-140a46ea7d27")]
        [RoleId("984b073d-0213-4539-8d3c-a35a81a71bd5")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal Amount { get; set; }

        #region Allors
        [Id("d7dc81e8-76e7-4c68-9843-a2aaf8293510")]
        [AssociationId("6fbc80d1-e72b-4484-a9b1-e606f15d2435")]
        [RoleId("219cb27f-20b5-48b3-9d89-4b119798b092")]
        #endregion
        [Workspace]
        DateTime IssueDate { get; set; }

        #region Allors
        [Id("e250154a-77c5-4a0b-ae3d-28668a9037d1")]
        [AssociationId("b5ba8cfd-2b16-4a50-89cd-46927d59b97a")]
        [RoleId("f5b6881b-c4d5-42e3-a024-0ae4564cb970")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        QuoteItem[] QuoteItems { get; set; }

        #region Allors
        [Id("e76cbd73-78b7-4ef8-a24c-9ac0db152f7f")]
        [AssociationId("057ad29f-c245-44b2-8a95-71bd6607830b")]
        [RoleId("218e3a6e-b530-41f7-a60e-7587f8072c8c")]
        #endregion
        [Size(256)]
        [Workspace]
        string QuoteNumber { get; set; }

        #region Allors
        [Id("AEF6FD44-E668-4F60-A1D8-C9A677A65205")]
        [AssociationId("1E01D0B0-268E-414E-B75D-35553899441F")]
        [RoleId("FB6CDE20-3D63-4B95-8886-A047C6127607")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Derived]
        [Indexed]
        [Workspace]
        QuoteStatus[] QuoteStatuses { get; set; }

        #region Allors
        [Id("399E26DD-81C6-4238-A8AE-0C70F36E57B4")]
        [AssociationId("2DD366A5-34B9-47D0-9035-0B18A8AD69C1")]
        [RoleId("F77E8B5A-9E12-4C81-BB79-E3570241B513")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Required]
        [Workspace]
        QuoteObjectState CurrentObjectState { get; set; }

        #region Allors
        [Id("1A1E04D6-AFD9-41CA-AF0A-CFDCB645D28A")]
        [AssociationId("7A775728-0DF9-4713-9505-DABC9A587B93")]
        [RoleId("148C16F6-0279-4840-AEED-2D325E67FBC8")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Derived]
        [Workspace]
        QuoteStatus CurrentQuoteStatus { get; set; }

        #region Allors
        [Id("94DE208B-5FF9-45F5-BD35-5BB7D7B33FB7")]
        [AssociationId("10F24038-D3E1-40DC-9D4F-27E738BE7F3D")]
        [RoleId("3D269537-523B-489A-A7DA-008DC8585F60")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        Request Request { get; set; }

        #region Allors
        [Id("519F70DC-0C4C-43E7-8929-378D8871CD84")]
        #endregion
        [Workspace]
        void Approve();

        #region Allors
        [Id("39694549-7173-4904-8AE0-DA7390F595A5")]
        #endregion
        [Workspace]
        void Reject();
    }
}