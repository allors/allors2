namespace Allors.Meta
{
	#region Allors
	[Id("6674e32d-c139-4c99-97c5-92354d3ccc4c")]
	#endregion
	[Inherit(typeof(PeriodInterface))]
	[Inherit(typeof(AccessControlledObjectInterface))]
	public partial class PersonTrainingClass : Class
	{
		#region Allors
		[Id("023864ad-41e1-41cb-8ded-ad2bfa98afe3")]
		[AssociationId("04f1d7c4-1012-4b0e-b38e-02d6abc328be")]
		[RoleId("91bba22d-82b7-4425-ba55-2862f803088d")]
		#endregion
		[Indexed]
		[Type(typeof(TrainingClass))]
		[Plural("Trainings")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Training;

		public static PersonTrainingClass Instance {get; internal set;}

		internal PersonTrainingClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.Training.RoleType.IsRequired = true;

            this.ConcreteRoleTypeByRoleType[PeriodInterface.Instance.ThroughDate.RoleType].IsRequiredOverride = true;
        }
    }
}