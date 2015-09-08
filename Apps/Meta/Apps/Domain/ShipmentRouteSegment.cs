namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("8e6eaa35-85da-4c80-848c-3f1ed6cd2f8a")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("ShipmentRouteSegments")]
	public partial class ShipmentRouteSegmentClass : Class
	{
		#region Allors
		[Id("02ef1727-e135-4af3-9d76-02bad7b122f3")]
		[AssociationId("c4fd1dd3-ddef-4f2c-bc22-388d3f979798")]
		[RoleId("1c1e24d0-5635-4d17-bd8b-5e0513b7f024")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("EndKilometers")]
		public RelationType EndKilometers;

		#region Allors
		[Id("2a697cc1-cdeb-4e40-a929-2a8df593877e")]
		[AssociationId("f2e40a37-c722-4608-9ed5-0b6f49819efc")]
		[RoleId("54e17ef2-abae-4c76-9d93-87a6545cfa87")]
		#endregion
		[Indexed]
		[Type(typeof(FacilityInterface))]
		[Plural("FromFacilities")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType FromFacility;

		#region Allors
		[Id("3f46506d-ea90-4103-b986-965194037cef")]
		[AssociationId("b0468fca-5eb7-4251-b935-2f18891e9a8f")]
		[RoleId("e8aea1c9-ca9b-4b77-b2c2-8f3e4f2d900b")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("StartKilometers")]
		public RelationType StartKilometers;

		#region Allors
		[Id("4a30a93c-d50b-44cf-b0a2-c29c970e6290")]
		[AssociationId("9754042f-3f58-42dd-b160-9c4339a6169d")]
		[RoleId("6be3b17d-03f1-4731-b17e-5956260d1d9a")]
		#endregion
		[Indexed]
		[Type(typeof(ShipmentMethodClass))]
		[Plural("ShipmentMethods")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType ShipmentMethod;

		#region Allors
		[Id("57f25750-a517-47a8-a6a0-feb160cd5f3e")]
		[AssociationId("0eb0c608-4d72-4aa2-b9c1-46d508a3ff32")]
		[RoleId("44572277-5c54-4d92-8916-1ad2afd13da2")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("EstimatedStartDateTimes")]
		public RelationType EstimatedStartDateTime;

		#region Allors
		[Id("591427f6-b61c-4c19-9f82-e97570d9bead")]
		[AssociationId("352996f3-ffa9-4453-a602-938c7543a7c1")]
		[RoleId("bd604c1a-3540-472a-90f5-69ed94a82f03")]
		#endregion
		[Indexed]
		[Type(typeof(FacilityInterface))]
		[Plural("ToFacilities")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType ToFacility;

		#region Allors
		[Id("6b3d4c25-823c-4197-8c05-80aeb887eb8b")]
		[AssociationId("f77e2ce0-97fb-4ccd-a6a3-dac8c09d5295")]
		[RoleId("85fe4467-bfde-41ac-9a40-e482a2f800a0")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("EstimatedArrivalDateTimes")]
		public RelationType EstimatedArrivalDateTime;

		#region Allors
		[Id("6bf54f85-7781-4fd3-a87f-6e7103042ecb")]
		[AssociationId("09234af6-ece2-403f-81ce-8c5a8e814135")]
		[RoleId("e9d8d7f8-5408-4bb8-85d2-6ce02a400796")]
		#endregion
		[Indexed]
		[Type(typeof(VehicleClass))]
		[Plural("Vehicles")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Vehicle;

		#region Allors
		[Id("928b9d1e-903b-4d56-aa72-b7aeaf3ba340")]
		[AssociationId("ace8a50d-e396-4e47-b13c-f02fa018f652")]
		[RoleId("2b85c556-8726-41d4-a236-197816b2824b")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("ActualArrivalDateTimes")]
		public RelationType ActualArrivalDateTime;

		#region Allors
		[Id("b080fe6b-382e-475d-be81-8632ddedb183")]
		[AssociationId("39dade5b-be0e-43ed-8368-00f24cfd3ce6")]
		[RoleId("e749db0f-c95e-4c81-87d0-f5932a31816c")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("ActualStartDateTimes")]
		public RelationType ActualStartDateTime;

		#region Allors
		[Id("c04769b1-f8dc-40c7-87d2-1e55a4702e71")]
		[AssociationId("7d5a3fa4-50bb-45b6-b355-2bad4485b9d1")]
		[RoleId("c8ad9159-10eb-4c38-8e5b-1339fa082406")]
		#endregion
		[Indexed]
		[Type(typeof(OrganisationClass))]
		[Plural("Carriers")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Carrier;



		public static ShipmentRouteSegmentClass Instance {get; internal set;}

		internal ShipmentRouteSegmentClass() : base(MetaPopulation.Instance)
        {
        }
	}
}