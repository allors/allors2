import { MetaPopulation } from "./framework";
import { data, Meta } from "./meta";
import { Slug } from "./domain/custom/Organisation";

const metaPopulation = new MetaPopulation(data);
const m = metaPopulation as Meta;

// RoleTypes
m.Organisation.Name.isGatsby = true;
m.Organisation.Owner.isGatsby = true;
m.Person.FirstName.isGatsby = true;
m.Person.Photo.isGatsby = true;
m.Media.UniqueId.isGatsby = true;
m.Media.Revision.isGatsby = true;

// Properties
m.Organisation.gatsbyProperties = [
  { name: Slug, type: "String" }
];

metaPopulation.gatsbyDerive();

export default metaPopulation;
