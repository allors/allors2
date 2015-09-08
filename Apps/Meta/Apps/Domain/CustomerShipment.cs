namespace Allors.Meta
{
	#region Allors
	[Id("9301efcb-2f08-4825-aa60-752c031e4697")]
	#endregion
	[Inherit(typeof(DeletableInterface))]
	[Inherit(typeof(ShipmentInterface))]
	public partial class CustomerShipmentClass : Class
	{
        #region Allors
        [Id("9E89A8AD-2EFE-4A21-815B-9598D7D7C1F7")]
        #endregion
        public MethodType Hold;

        #region Allors
        [Id("1A64504B-0115-4D4D-BBE0-35792A8BCA1A")]
        #endregion
        public MethodType PutOnHold;

        #region Allors
        [Id("9DD73148-A1C0-4631-91AF-E13116FC0102")]
        #endregion
        public MethodType Cancel;

        #region Allors
        [Id("6E09CAC6-327F-49DD-B4AB-07D075C7579E")]
        #endregion
        public MethodType Continue;

        #region Allors
        [Id("1B56BF7E-08BE-49B1-92A1-4CE89B329D77")]
        #endregion
        public MethodType Ship;

        #region Allors
        [Id("9AFF4390-9B51-4C33-A0CF-125FED33E34F")]
        #endregion
        public MethodType ProcessOnContinue;

        #region Allors
        [Id("BD7F0406-29E2-4A10-AE55-C2849D257B01")]
        #endregion
        public MethodType SetPicked;

        #region Allors
        [Id("F484244D-BB1D-4158-9A4D-40267D4B7D5B")]
        #endregion
        public MethodType SetPacked;

        #region Allors
        [Id("15e8f37c-3963-490c-8f22-7fb1e40209df")]
		[AssociationId("30b4e232-dd11-4ee6-b1dd-ef1e05b54d92")]
		[RoleId("a282ae7a-2280-4ea8-a8c8-cf170f0714ac")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(CustomerShipmentStatusClass))]
		[Plural("CurrentShipmentStatus")]
		[Multiplicity(Multiplicity.OneToOne)]
		public RelationType CurrentShipmentStatus;

		#region Allors
		[Id("4f7c79be-9f0d-4646-9488-dc86761866cd")]
		[AssociationId("06ff523b-b43d-424e-b54a-c184c5d3ac5f")]
		[RoleId("526cb9db-f5d7-42bc-a37d-c1ae680d1f92")]
		#endregion
		[Type(typeof(AllorsBooleanUnit))]
		[Plural("ReleasedsManually")]
		public RelationType ReleasedManually;

		#region Allors
		[Id("7b1b6b60-9678-4a52-aee8-33bad04eeb40")]
		[AssociationId("8cf76b47-a09f-4112-8bec-733a30abc323")]
		[RoleId("6c812e1e-204b-4e85-8cfb-5dae89fb2bf2")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(CustomerShipmentObjectStateClass))]
		[Plural("CurrentObjectStates")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType CurrentObjectState;

		#region Allors
		[Id("7b6a8a4f-574f-494f-b43b-7c5b7428d685")]
		[AssociationId("83787439-402b-4d57-8e70-aa157aa8d1fa")]
		[RoleId("0022a581-9823-4b8d-a3f5-ce068ab60fe8")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(CustomerShipmentStatusClass))]
		[Plural("ShipmentStatuses")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType ShipmentStatus;

		#region Allors
		[Id("897bcb4f-fa89-4d9b-8666-49bb061a69ae")]
		[AssociationId("d2945852-755a-45ef-b6dc-914767d3d2e5")]
		[RoleId("a3ab7835-d97e-4221-831d-0ba1ffe3c9d0")]
		#endregion
		[Indexed]
		[Type(typeof(PaymentMethodInterface))]
		[Plural("PaymentMethods")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType PaymentMethod;

		#region Allors
		[Id("a754a290-571f-4c25-bd1c-d96a9765eec6")]
		[AssociationId("6d117db4-ef4d-483a-a68d-75c69e325bba")]
		[RoleId("66a18574-7b90-4e36-9d5d-a4f31bc6eba1")]
		#endregion
		[Type(typeof(AllorsBooleanUnit))]
		[Plural("WithoutChargess")]
		public RelationType WithoutCharges;

		#region Allors
		[Id("b94fa6e5-cfdf-4545-8eb3-43d03aceffc5")]
		[AssociationId("2d9a286e-95d5-4adb-ab29-7a9d95f83146")]
		[RoleId("33382f4f-5ebc-4589-b906-a8a2a3be28d2")]
		#endregion
		[Type(typeof(AllorsBooleanUnit))]
		[Plural("HeldsManually")]
		public RelationType HeldManually;

		#region Allors
		[Id("f0fe5bc1-74d1-4fee-8039-b6952edecc92")]
		[AssociationId("c11d0979-373c-4c27-94d2-4d7350afc1c4")]
		[RoleId("2348278f-bf03-4133-b34c-2da5955a0a41")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("ShipmentValues")]
		public RelationType ShipmentValue;

		public static CustomerShipmentClass Instance {get; internal set;}

		internal CustomerShipmentClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.ShipmentValue.RoleType.IsRequired = true;

            this.CurrentObjectState.RoleType.IsRequired = true;
            this.ReleasedManually.RoleType.IsRequired = true;
            this.HeldManually.RoleType.IsRequired = true;
            this.WithoutCharges.RoleType.IsRequired = true;

            this.ConcreteRoleTypeByRoleType[ShipmentInterface.Instance.ShipToAddress.RoleType].IsRequiredOverride = true;
            this.ConcreteRoleTypeByRoleType[ShipmentInterface.Instance.ShipFromAddress.RoleType].IsRequiredOverride = true;
            this.ConcreteRoleTypeByRoleType[ShipmentInterface.Instance.ShipmentMethod.RoleType].IsRequiredOverride = true;
            this.ConcreteRoleTypeByRoleType[ShipmentInterface.Instance.Carrier.RoleType].IsRequiredOverride = true;
            this.ConcreteRoleTypeByRoleType[ShipmentInterface.Instance.EstimatedShipDate.RoleType].IsRequiredOverride = true;
            this.ConcreteRoleTypeByRoleType[ShipmentInterface.Instance.ShipFromParty.RoleType].IsRequiredOverride = true;
        }
    }
}