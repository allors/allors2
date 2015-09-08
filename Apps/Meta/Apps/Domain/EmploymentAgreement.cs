namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("d402d086-0d7a-4e98-bcb1-8f8e1cfabb99")]
	#endregion
	[Inherit(typeof(AgreementInterface))]

	[Plural("EmploymentAgreements")]
	public partial class EmploymentAgreementClass : Class
	{

		public static EmploymentAgreementClass Instance {get; internal set;}

		internal EmploymentAgreementClass() : base(MetaPopulation.Instance)
        {
        }
	}
}