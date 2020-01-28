import { MetaPopulation } from "./framework";
import { data, Meta } from "./meta";
import { Slug } from "./domain/custom/Organisation";

const metaPopulation = new MetaPopulation(data);
const m = metaPopulation as Meta;

m.Organisation.gatsbyRoleTypes = [m.Organisation.Name, m.Organisation.Owner];
m.Organisation.gatsbyProperties = [
  { name: Slug, type: "String" }
];

m.Person.gatsbyRoleTypes = [m.Person.FirstName, m.Person.Photo];
m.Person.gatsbyProperties = [
  { name: Slug, type: "String" }
];

m.Media.gatsbyRoleTypes = [m.Media.UniqueId, m.Media.Revision, m.Media.FileName];


metaPopulation.gatsbyDerive();

export default metaPopulation;
