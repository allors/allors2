// <copyright file="PurchaseInvoice.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("7d7e4b6d-eebd-460c-b771-a93cd8d64bce")]
    #endregion
    public partial class PurchaseInvoice : Invoice, Versioned, WorkItem
    {
        #region inherited properties

        public Media[] ElectronicDocuments { get; set; }

        public ObjectState[] PreviousObjectStates { get; set; }

        public ObjectState[] LastObjectStates { get; set; }

        public ObjectState[] ObjectStates { get; set; }

        public string InternalComment { get; set; }

        public Currency AssignedCurrency { get; set; }

        public Currency DerivedCurrency { get; set; }

        public string Description { get; set; }

        public OrderAdjustment[] OrderAdjustments { get; set; }

        public string CustomerReference { get; set; }

        public decimal AmountPaid { get; set; }

        public decimal TotalDiscount { get; set; }

        public BillingAccount BillingAccount { get; set; }

        public decimal TotalIncVat { get; set; }

        public decimal GrandTotal { get; set; }

        public decimal TotalSurcharge { get; set; }

        public decimal TotalBasePrice { get; set; }

        public DateTime InvoiceDate { get; set; }

        public DateTime EntryDate { get; set; }

        public decimal TotalShippingAndHandling { get; set; }

        public decimal TotalExVat { get; set; }

        public SalesTerm[] SalesTerms { get; set; }

        public string InvoiceNumber { get; set; }

        public string Message { get; set; }

        public VatRegime AssignedVatRegime { get; set; }

        public VatRegime DerivedVatRegime { get; set; }

        public VatRate DerivedVatRate { get; set; }

        public decimal TotalVat { get; set; }

        public decimal TotalFee { get; set; }

        public decimal TotalExtraCharge { get; set; }

        public InvoiceItem[] ValidInvoiceItems { get; set; }

        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public string Comment { get; set; }

        public LocalisedText[] LocalisedComments { get; set; }

        public PrintDocument PrintDocument { get; set; }

        public string WorkItemDescription { get; set; }

        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public IrpfRegime AssignedIrpfRegime { get; set; }

        public IrpfRegime DerivedIrpfRegime { get; set; }

        public IrpfRate DerivedIrpfRate { get; set; }

        public decimal TotalIrpf { get; set; }

        public int SortableInvoiceNumber { get; set; }

        public Guid DerivationTrigger { get; set; }

        public decimal TotalIrpfInPreferredCurrency { get; set; }

        public decimal TotalExVatInPreferredCurrency { get; set; }

        public decimal TotalVatInPreferredCurrency { get; set; }

        public decimal TotalIncVatInPreferredCurrency { get; set; }

        public decimal GrandTotalInPreferredCurrency { get; set; }

        public decimal TotalSurchargeInPreferredCurrency { get; set; }

        public decimal TotalDiscountInPreferredCurrency { get; set; }

        public decimal TotalShippingAndHandlingInPreferredCurrency { get; set; }

        public decimal TotalFeeInPreferredCurrency { get; set; }

        public decimal TotalExtraChargeInPreferredCurrency { get; set; }

        public decimal TotalBasePriceInPreferredCurrency { get; set; }

        public decimal TotalListPriceInPreferredCurrency { get; set; }

        #endregion

        #region ObjectStates
        #region PurchaseInvoiceState
        #region Allors
        [Id("EDC12BA8-41F6-4E3A-8430-9592201A821E")]
        [AssociationId("F633172D-B01C-4E06-8FAB-02D811E46A43")]
        [RoleId("A6421D9C-7F24-4009-A76B-EACABCE8138C")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public PurchaseInvoiceState PreviousPurchaseInvoiceState { get; set; }

        #region Allors
        [Id("96B88C50-E18C-4776-86CF-D3126A4E5C1B")]
        [AssociationId("3B7BCFD7-D56D-4F02-84BC-6EB4E55293FB")]
        [RoleId("63FD899E-5617-4CED-998A-7FAC30AF007D")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public PurchaseInvoiceState LastPurchaseInvoiceState { get; set; }

        #region Allors
        [Id("AAB01767-7EA3-48E4-85ED-153DED6CB873")]
        [AssociationId("481BBBE1-1429-4CBB-9D1B-1478B3A76AEB")]
        [RoleId("E19CF950-9F3C-4696-91CE-EEA33B4BC054")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public PurchaseInvoiceState PurchaseInvoiceState { get; set; }
        #endregion
        #endregion

        #region Versioning
        #region Allors
        [Id("E1F38604-4DB9-4D34-A34E-9B64649ABDE9")]
        [AssociationId("18F624C0-1535-4259-970A-336B8D3265DE")]
        [RoleId("E11301DE-46C3-4FC4-84F2-B2930CDB3872")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public PurchaseInvoiceVersion CurrentVersion { get; set; }

        #region Allors
        [Id("AC26A490-1260-4E2D-B621-E827C12FAA39")]
        [AssociationId("FD0DD85A-3792-45FE-B3B4-4AFB7D920C35")]
        [RoleId("30411BBB-CB85-4043-8ADB-4641C2DB21FD")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public PurchaseInvoiceVersion[] AllVersions { get; set; }
        #endregion

        #region Allors
        [Id("d4bbc5ed-08a4-4d89-ad53-7705ae71d029")]
        [AssociationId("8ce81b66-22e5-4195-a270-5e9f761ff51e")]
        [RoleId("58245287-7a75-45c4-a000-d3944ec9319a")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public Party BilledFrom { get; set; }

        #region Allors
        [Id("0CE57597-A6E0-4F9D-B619-A8688E02A045")]
        [AssociationId("E6C6BF05-0DE8-494D-912D-65D8A1ADC1E3")]
        [RoleId("255DC694-15C3-4750-B09A-C7483C161D90")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public ContactMechanism AssignedBilledFromContactMechanism { get; set; }

        #region Allors
        [Id("868472ef-a005-44a0-98ab-fd33fc5b1d3a")]
        [AssociationId("ae236200-6337-4aff-8775-60b64d9059a1")]
        [RoleId("64f1cefa-56be-47bb-b02b-38df13ba77d1")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Workspace]
        public ContactMechanism DerivedBilledFromContactMechanism { get; set; }

        #region Allors
        [Id("9254C511-081D-4E05-98EB-5F4E52A700E3")]
        [AssociationId("4A9F0B0C-99AB-433A-BC4F-C6CF65188CB7")]
        [RoleId("5E7F2132-0434-4C27-9A89-B327AAF14D8C")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Person BilledFromContactPerson { get; set; }

        #region Allors
        [Id("045918FA-CC14-4616-A2B0-519E2ACEBA31")]
        [AssociationId("5DB9DFF5-4964-4DE6-AEA6-FB6111D35502")]
        [RoleId("2B69921B-A93D-49F7-A950-D65F9EDB2A94")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public InternalOrganisation BilledTo { get; set; }

        #region Allors
        [Id("B75B10EE-6AA5-4C48-BDC0-61FA814B3E19")]
        [AssociationId("93EA9833-3715-461B-8AB3-821211659185")]
        [RoleId("2EBDB046-8D94-4970-84BE-0FBD7B8070DA")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Person BilledToContactPerson { get; set; }

        #region Allors
        [Id("F37F0194-EDF4-49DA-BDB2-AA9C2A601809")]
        [AssociationId("87C2A5CD-1C7C-499E-8757-43191CBC9FE9")]
        [RoleId("3A4150A9-B63E-4422-960C-D8AEF707E679")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public Party ShipToCustomer { get; set; }

        #region Allors
        [Id("3E8E3FAF-2FFE-483A-8F7C-3ABF4BC29BD6")]
        [AssociationId("99126CF2-C82D-4501-8F96-3A354AACD105")]
        [RoleId("35580F2F-9148-4505-BA61-954FE8FE2855")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public PostalAddress AssignedShipToCustomerAddress { get; set; }

        #region Allors
        [Id("996d7b2e-5085-41cf-8c7b-ce3d790fdf65")]
        [AssociationId("0456f639-5215-4bd4-90de-f65cbc963b64")]
        [RoleId("6990ef27-45a8-4d2f-95bd-ba1edbb90ff8")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Workspace]
        public PostalAddress DerivedShipToCustomerAddress { get; set; }

        #region Allors
        [Id("5EF823D6-7EED-40C6-9E4A-9106A655B9E1")]
        [AssociationId("E6CF7038-D008-4CF4-9D7D-1D2E48241991")]
        [RoleId("69DA77F9-9AF7-4E7A-8477-9E115299A6AE")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Person ShipToCustomerContactPerson { get; set; }

        #region Allors
        [Id("ABB96E7A-F5D1-4173-AEDB-62B217A22495")]
        [AssociationId("1AC98B51-187C-46E4-9909-7DA70F75334B")]
        [RoleId("7C6964CA-365C-402E-9046-E913EACEB020")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public Party BillToEndCustomer { get; set; }

        #region Allors
        [Id("B9ED27E2-9429-40AF-8B1E-4C4210023F5F")]
        [AssociationId("5C01982A-11AC-4983-8B27-229BF86A3A45")]
        [RoleId("39C7038E-2687-4F9B-9282-9D10E3929603")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public ContactMechanism AssignedBillToEndCustomerContactMechanism { get; set; }

        #region Allors
        [Id("5004df78-2aac-4e58-9e6b-b83ac59253ca")]
        [AssociationId("08abf9b2-15ba-4393-9896-6425c96ad655")]
        [RoleId("67525709-262a-4a03-9195-8214bdf04840")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Workspace]
        public ContactMechanism DerivedBillToEndCustomerContactMechanism { get; set; }

        #region Allors
        [Id("91CF5142-0240-47A6-9423-3EA8F2EA43F4")]
        [AssociationId("9EDB19E1-8577-411E-BF4F-5DA7BF630030")]
        [RoleId("B3D5E1F7-5849-40FE-AE4D-6753EED19F9A")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Person BillToEndCustomerContactPerson { get; set; }

        #region Allors
        [Id("0E1BB984-6D42-4473-9BA1-A3EBDEF84A54")]
        [AssociationId("DCBFDD56-D47A-4229-9762-07C35D2CB826")]
        [RoleId("A2BA0164-3E36-494D-96D3-CBEA0C737279")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public Party ShipToEndCustomer { get; set; }

        #region Allors
        [Id("B4EC8C7B-E4BA-4428-9898-6FB9B6C048A4")]
        [AssociationId("9A43B583-CE97-4379-B5FB-5BDE616F0F10")]
        [RoleId("E1698EAF-2904-4C69-83EC-22FD04CE8BD3")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public PostalAddress AssignedShipToEndCustomerAddress { get; set; }

        #region Allors
        [Id("3af66b6f-0f5d-4b3c-b0a3-7276b687403b")]
        [AssociationId("3c77a8fa-1321-4756-a9a0-21d2bdd15a46")]
        [RoleId("a1f11970-f2c1-4542-b5cb-19fb8bb20600")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Derived]
        [Workspace]
        public PostalAddress DerivedShipToEndCustomerAddress { get; set; }

        #region Allors
        [Id("02C6EEC2-0DBD-4F63-9FFC-6C107E90E303")]
        [AssociationId("31D6F0ED-3848-4422-814E-1785877B0597")]
        [RoleId("9863AA72-FCBD-446A-A162-ACB075025CFB")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Person ShipToEndCustomerContactPerson { get; set; }

        #region Allors
        [Id("F3CF7DEC-452C-4A68-9653-E7BB8987F8A1")]
        [AssociationId("DDBB0961-232F-427D-9957-CAD860377ACE")]
        [RoleId("31AB05CD-6611-4E0A-B655-F98F47DEEF16")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public PaymentMethod AssignedBillToCustomerPaymentMethod { get; set; }

        #region Allors
        [Id("5a2ab60a-00a0-4302-81a6-49d8211eaea6")]
        [AssociationId("4f43f615-7962-407d-a6de-f56672177174")]
        [RoleId("2361681d-ed2c-4e90-9d4a-a7cbf3a53f38")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Workspace]
        public PaymentMethod DerivedBillToCustomerPaymentMethod { get; set; }

        #region Allors
        [Id("4cf09eb7-820f-4677-bfc0-92a48d0a938b")]
        [AssociationId("5a71ca58-db28-4edc-9065-32396380bd80")]
        [RoleId("fa280c8d-ac7b-4d99-80dd-fba155d4aef9")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        public PurchaseInvoiceItem[] PurchaseInvoiceItems { get; set; }

        #region Allors
        [Id("e444b5e7-0128-49fc-86cb-a6fe39c280ae")]
        [AssociationId("d6240de5-9b99-4525-b7d0-ef28a3381821")]
        [RoleId("6c911870-2737-4997-87a6-65ca55c17c55")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public PurchaseInvoiceType PurchaseInvoiceType { get; set; }

        #region Allors
        [Id("147372B8-ADC3-442E-BF60-968A0B13FBDD")]
        [AssociationId("8C96181E-4527-4A79-BA6A-9B36E132A8F2")]
        [RoleId("CBCAB42E-08A8-49C7-94F1-D89E7B20B9DD")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Workspace]
        public PurchaseOrder[] PurchaseOrders { get; set; }

        #region Allors
        [Id("B1C82298-2ABF-4FF7-BCC1-EF6B77AB6B50")]
        [AssociationId("CC579E01-EDD4-4E01-9652-29A2E454EBBF")]
        [RoleId("B4EC06D6-BF85-49DD-8407-07191E7AE41E")]
        #endregion
        [Workspace]
        public DateTime DueDate { get; set; }

        #region Allors
        [Id("b1a0d63f-a0bc-424e-9294-e3b7b37e9c6e")]
        [AssociationId("aa45e232-6893-4932-abf5-2b7a82af2464")]
        [RoleId("cecf02cf-8102-4889-880f-b0861451c266")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal ActualInvoiceAmount { get; set; }

        #region Allors
        [Id("797A9C2C-A2CF-4AE3-8395-B2F25D0F40C1")]
        #endregion
        [Workspace]
        public void Confirm() { }

        #region Allors
        [Id("B188B7B5-BA61-4FF5-9D9A-812E22F8A289")]
        #endregion
        [Workspace]
        public void Approve() { }

        #region Allors
        [Id("55DA112F-F7BF-4400-B1FD-7A87A7B3C67B")]
        #endregion
        [Workspace]
        public void Reject() { }

        #region Allors
        [Id("07A2BE5F-5686-4B0A-8B05-8875FA277622")]
        #endregion
        [Workspace]
        public void Cancel() { }

        #region Allors
        [Id("2D4FDE1F-FE36-4880-9B95-ACFE1B20C085")]
        #endregion
        [Workspace]
        public void Reopen() { }

        #region Allors
        [Id("422DD593-DECC-40FD-9216-D5A25458B59F")]
        #endregion
        [Workspace]
        public void CreateSalesInvoice() { }

        #region Allors
        [Id("4BF977FA-75AF-4D6D-8CD7-7250D527EF61")]
        #endregion
        [Workspace]
        public void SetPaid() { }

        #region Allors
        [Id("3bd0368b-78dc-4872-8437-62645b16ee2b")]
        #endregion
        [Workspace]
        public void FinishRevising() { }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        public void Delete() { }

        public void Print() { }

        public void Create() { }

        public void Revise() { }

        #endregion
    }
}
