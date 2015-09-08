namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("3587d2e1-c3f6-4c55-a96c-016e0501d99c")]
	#endregion
	[Inherit(typeof(UserInterface))]
	[Inherit(typeof(PartyInterface))]

	[Plural("AutomatedAgents")]
	public partial class AutomatedAgentClass : Class
	{
		#region Allors
		[Id("4e158d75-d0b5-4cb7-ad41-e8ed3002d175")]
		[AssociationId("6f2a83eb-17e9-408e-b18b-9bb2b9a3e812")]
		[RoleId("4fac2dd3-8711-4115-96b9-a38f62e2d093")]
		#endregion
		[Indexed]
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Names")]
		public RelationType Name;

		#region Allors
		[Id("58870c93-b066-47b7-95f7-5411a46dbc7e")]
		[AssociationId("31925ed6-e66c-4718-963f-c8a71d566fe8")]
		[RoleId("eee42775-b172-4fde-9042-a0f9b2224ec3")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Descriptions")]
		public RelationType Description;



		public static AutomatedAgentClass Instance {get; internal set;}

		internal AutomatedAgentClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.Description.RoleType.IsRequired = true;
        }
    }
}