namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("5f16236a-0fa4-4866-9b3d-3951edbd4c81")]
	#endregion
	[Inherit(typeof(FacilityInterface))]
	[Inherit(typeof(ContainerInterface))]

	[Plural("Rooms")]
	public partial class RoomClass : Class
	{

		public static RoomClass Instance {get; internal set;}

		internal RoomClass() : base(MetaPopulation.Instance)
        {
        }
	}
}