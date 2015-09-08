namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("f6dceab0-f4a7-435e-abce-ac9f7bd28ae4")]
	#endregion
	[Inherit(typeof(GeographicBoundaryInterface))]
	[Inherit(typeof(AccessControlledObjectInterface))]
	[Inherit(typeof(CountryBoundInterface))]

	[Plural("Cities")]
	public partial class CityClass : Class
	{
		#region Allors
		[Id("05ea705c-9800-4442-a684-b8b4251b51ed")]
		[AssociationId("a584625d-889d-4943-a130-fab2697def9f")]
		[RoleId("889ccbe9-96a3-4d8e-9b8c-a1877ab89255")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Names")]
		public RelationType Name;

		#region Allors
		[Id("559dd596-e784-4067-a993-b651ac17329d")]
		[AssociationId("06cc0af4-6bb9-4a86-a3e9-496f36002c92")]
		[RoleId("89811da3-093a-42fe-8142-60692f1c3f05")]
		#endregion
		[Indexed]
		[Type(typeof(StateClass))]
		[Plural("States")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType State;



		public static CityClass Instance {get; internal set;}

		internal CityClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.Name.RoleType.IsRequired = true;
        }
    }
}