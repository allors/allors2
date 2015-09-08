namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("452ae775-def1-4e75-b325-2e9184eb8c1f")]
	#endregion
	[Plural("CommunicationAttachments")]
	[Inherit(typeof(AccessControlledObjectInterface))]

  	public partial class CommunicationAttachmentInterface: Interface
	{

		public static CommunicationAttachmentInterface Instance {get; internal set;}

		internal CommunicationAttachmentInterface() : base(MetaPopulation.Instance)
        {
        }
	}
}