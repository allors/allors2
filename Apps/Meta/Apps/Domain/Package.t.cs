namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("9371d5fc-748a-4ce4-95eb-6b21aa0ca841")]
	#endregion
	[Inherit(typeof(UniquelyIdentifiableInterface))]
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("Packages")]
	public partial class PackageClass : Class
	{
		#region Allors
		[Id("88b49c23-0c4c-4a2d-94aa-c6c8a12ac267")]
		[AssociationId("d1a984e7-2f57-43a0-8cca-e8682407498b")]
		[RoleId("cffa7e90-1c5b-459c-adbe-8fa008b36151")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Name")]
		public RelationType Name;



		public static PackageClass Instance {get; internal set;}

		internal PackageClass() : base(MetaPopulation.Instance)
        {
        }
	}
}