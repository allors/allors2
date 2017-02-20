import { Workspace } from "./base/Workspace";
import { Population as MetaPopulation } from "../meta";
import { constructorByName } from "./generated/domain.g";

export { Counter } from "./generated/Counter.g";
export { Enumeration } from "./generated/Enumeration.g";
export { Media } from "./generated/Media.g";
export { ObjectState } from "./generated/ObjectState.g";
export { Person } from "./generated/Person.g";
export { Role } from "./generated/Role.g";
export { UniquelyIdentifiable } from "./generated/UniquelyIdentifiable.g";
export { UserGroup } from "./generated/UserGroup.g";

let metaPopulation = new MetaPopulation();
metaPopulation.init();

export let workspace = new Workspace(metaPopulation, constructorByName);
