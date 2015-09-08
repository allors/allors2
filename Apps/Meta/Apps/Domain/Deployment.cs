namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("ee23df25-f7d7-4974-b62e-ee3cba56b709")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]
	[Inherit(typeof(PeriodInterface))]

	[Plural("Deployments")]
	public partial class DeploymentClass : Class
	{
		#region Allors
		[Id("212653db-1677-47bd-944c-b5468673ec63")]
		[AssociationId("7543cf10-97dd-4823-b386-f06379e398b2")]
		[RoleId("685a54f0-4e66-4ce3-93a2-f6f45dcf8c8b")]
		#endregion
		[Indexed]
		[Type(typeof(GoodClass))]
		[Plural("ProductOfferings")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType ProductOffering;

		#region Allors
		[Id("c322fbbd-3406-4e73-83ed-033282ab0cfb")]
		[AssociationId("d265b170-3854-4276-9a20-325984097991")]
		[RoleId("501b64c8-4181-45ca-a4f3-075232c8b270")]
		#endregion
		[Indexed]
		[Type(typeof(DeploymentUsageInterface))]
		[Plural("DeploymentUsages")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType DeploymentUsage;

		#region Allors
		[Id("d588ba7f-7b67-43fd-bb67-b9ff82fcffaf")]
		[AssociationId("bbee5696-6e53-4ea3-8f57-4e018e6bc61d")]
		[RoleId("33c8e0e5-be98-44bb-a9eb-cfbabd8451b2")]
		#endregion
		[Indexed]
		[Type(typeof(SerializedInventoryItemClass))]
		[Plural("SerializedInventoryItems")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType SerializedInventoryItem;



		public static DeploymentClass Instance {get; internal set;}

		internal DeploymentClass() : base(MetaPopulation.Instance)
        {
        }
	}
}