namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("354524c8-355e-4994-b07e-91fc6bcb06cf")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]
	[Inherit(typeof(DeletableInterface))]

	[Plural("SalesChannelRevenues")]
	public partial class SalesChannelRevenueClass : Class
	{
		#region Allors
		[Id("5b5d13cd-fc96-4a9d-826c-a47a21188717")]
		[AssociationId("400cc1d3-5e23-4067-a49e-ad0158801a8e")]
		[RoleId("9db9bfe7-e5df-44b7-b651-f0c0fba31806")]
		#endregion
		[Type(typeof(AllorsIntegerUnit))]
		[Plural("Years")]
		public RelationType Year;

		#region Allors
		[Id("9c1afcc0-d16b-4d39-bc6a-7d59e5c5487f")]
		[AssociationId("d2caaef3-8d0e-4423-b36c-074652f648aa")]
		[RoleId("55574345-6243-4647-b1cd-eec78926d5ad")]
		#endregion
		[Type(typeof(AllorsIntegerUnit))]
		[Plural("Months")]
		public RelationType Month;

		#region Allors
		[Id("a2ace411-df41-4191-b867-5f73ae5a1be7")]
		[AssociationId("1786e0dc-0483-4b80-942c-0c7d7ee8e913")]
		[RoleId("44f58270-a4c7-49ca-838f-8a70a3ebe6d1")]
		#endregion
		[Indexed]
		[Type(typeof(CurrencyClass))]
		[Plural("Currencies")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Currency;

		#region Allors
		[Id("b79bed4e-c20f-448d-8748-090bfbfd803c")]
		[AssociationId("468dce88-9789-47fb-874c-bc501c9cbcea")]
		[RoleId("f8154bd8-0d12-4e70-86d1-f16d627bdb4d")]
		#endregion
		[Indexed]
		[Type(typeof(SalesChannelClass))]
		[Plural("SalesChannels")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType SalesChannel;

		#region Allors
		[Id("c86138a3-6cdd-4e97-96ca-08f015f13e78")]
		[AssociationId("43473687-dc7e-4a4b-8745-5e98747c3731")]
		[RoleId("7d2869b9-d5e9-48ff-a12c-f32c7667d764")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("SalesChannelNames")]
		public RelationType SalesChannelName;

		#region Allors
		[Id("e650c7f3-3be0-4625-93fb-d0c1e72be9d0")]
		[AssociationId("ed420ba0-89a1-42b2-a353-bfb99f05fe63")]
		[RoleId("efce65ee-43a1-4a3e-b9ec-be7b0e273f7f")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("Revenues")]
		public RelationType Revenue;

		#region Allors
		[Id("f0fb8f34-e639-4c48-a1b7-66197520572d")]
		[AssociationId("9c2f6135-d7c7-400b-9450-b65abc402a8c")]
		[RoleId("936d33cb-b303-4663-90cb-0bd64c864d21")]
		#endregion
		[Indexed]
		[Type(typeof(InternalOrganisationClass))]
		[Plural("InternalOrganisations")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType InternalOrganisation;



		public static SalesChannelRevenueClass Instance {get; internal set;}

		internal SalesChannelRevenueClass() : base(MetaPopulation.Instance)
        {
        }
	}
}