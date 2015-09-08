namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("e6f97f86-6aec-4dde-b828-4de04d42c248")]
	#endregion
	[Inherit(typeof(GeographicBoundaryInterface))]
	[Inherit(typeof(CityBoundInterface))]
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("Counties")]
	public partial class CountyClass : Class
	{
		#region Allors
		[Id("89a67d5c-8f78-41aa-9152-91f8496535bc")]
		[AssociationId("93664b6a-08d3-48b7-aada-214db9d19cb8")]
		[RoleId("20477c4e-3c7f-4239-a5ae-313465682966")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Names")]
		public RelationType Name;

		#region Allors
		[Id("926ce4e6-cc76-4005-964f-f4d5af5fe944")]
		[AssociationId("71bf2977-eb86-4c5d-84f3-7ee97412e460")]
		[RoleId("66743b3b-180e-4a8d-baec-b728fd4ed29c")]
		#endregion
		[Indexed]
		[Type(typeof(StateClass))]
		[Plural("States")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType State;



		public static CountyClass Instance {get; internal set;}

		internal CountyClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.Name.RoleType.IsRequired = true;
        }
    }
}