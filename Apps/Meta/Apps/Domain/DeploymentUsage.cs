namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("ca0f0654-3974-4e5e-a57e-593216c05e16")]
	#endregion
	[Plural("DeploymentUsages")]
	[Inherit(typeof(AccessControlledObjectInterface))]
	[Inherit(typeof(CommentableInterface))]
	[Inherit(typeof(PeriodInterface))]

  	public partial class DeploymentUsageInterface: Interface
	{
		#region Allors
		[Id("50c6bc05-83ff-4d40-b476-51418355eb0c")]
		[AssociationId("e8aa74ab-d70a-43f4-9cac-de0160e3f257")]
		[RoleId("cc27af60-5ddd-4cce-bcc1-d68b3d5c6ab4")]
		#endregion
		[Indexed]
		[Type(typeof(TimeFrequencyClass))]
		[Plural("TimeFrequencies")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType TimeFrequency;



		public static DeploymentUsageInterface Instance {get; internal set;}

		internal DeploymentUsageInterface() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.TimeFrequency.RoleType.IsRequired = true;
        }
    }
}