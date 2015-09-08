namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("eab85f26-c3f4-4f47-97dc-8f9429856c00")]
	#endregion
	[Inherit(typeof(RequestInterface))]

	[Plural("RequestsForInformation")]
	public partial class RequestForInformationClass : Class
	{

		public static RequestForInformationClass Instance {get; internal set;}

		internal RequestForInformationClass() : base(MetaPopulation.Instance)
        {
        }
	}
}