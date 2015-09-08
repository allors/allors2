namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("13d519ec-468e-4fa7-9803-b95dbab4eb82")]
	#endregion
	[Plural("Services")]
	[Inherit(typeof(ProductInterface))]

  	public partial class ServiceInterface: Interface
	{

		public static ServiceInterface Instance {get; internal set;}

		internal ServiceInterface() : base(MetaPopulation.Instance)
        {
        }
	}
}