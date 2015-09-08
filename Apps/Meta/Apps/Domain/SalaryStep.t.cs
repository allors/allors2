namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("6ebf4c66-dd19-494f-8081-67d7a10a16fc")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("SalarySteps")]
	public partial class SalaryStepClass : Class
	{
		#region Allors
		[Id("162b31b7-78fd-4ec5-95f7-3913be0662e2")]
		[AssociationId("c00111ef-5eb8-4155-a621-fd09d0aa1a6c")]
		[RoleId("2872381c-833b-4dce-83f4-a56bbbd416b3")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("ModifiedDates")]
		public RelationType ModifiedDate;

		#region Allors
		[Id("7cb593b7-48ac-4049-b78c-1e84bdd2fa3a")]
		[AssociationId("39c58f18-a640-4c5e-9878-2f82ea90bd0a")]
		[RoleId("553fe45b-2c69-432d-9686-c2f049610eaa")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("Amounts")]
		public RelationType Amount;



		public static SalaryStepClass Instance {get; internal set;}

		internal SalaryStepClass() : base(MetaPopulation.Instance)
        {
        }
	}
}