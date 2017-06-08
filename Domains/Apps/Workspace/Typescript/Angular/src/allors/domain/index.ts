import { Workspace } from './base/Workspace';
import { Population as MetaPopulation } from '../meta';
import { constructorByName } from './generated/domain.g';

const metaPopulation = new MetaPopulation();
metaPopulation.init();

export let workspace = new Workspace(metaPopulation, constructorByName);
export { Session } from './base/Session';

// Base
export { PullRequest } from './base/database/pull/PullRequest';
export { Fetch } from './base/database/pull/Fetch';
export { Path } from './base/database/pull/Path';
export { Query } from './base/database/pull/Query';
export { Predicate } from './base/database/pull/Predicate';
export { And } from './base/database/pull/And';
export { Equals } from './base/database/pull/Equals';
export { Like } from './base/database/pull/Like';
export { TreeNode } from './base/database/pull/TreeNode';
export { Sort } from './base/database/pull/Sort';
export { Page } from './base/database/pull/Page';

export { Counter } from './generated/Counter.g';
export { Enumeration } from './generated/Enumeration.g';
export { Media } from './generated/Media.g';
export { ObjectState } from './generated/ObjectState.g';
export { Person } from './generated/Person.g';
export { Role } from './generated/Role.g';
export { UniquelyIdentifiable } from './generated/UniquelyIdentifiable.g';
export { UserGroup } from './generated/UserGroup.g';
export { Locale } from './generated/Locale.g';

// Apps
export { CommunicationEvent } from './generated/CommunicationEvent.g';
export { CustomerRelationship } from './generated/CustomerRelationship.g';
export { Country } from './generated/Country.g';
export { InternalOrganisation } from './generated/InternalOrganisation.g';
export { Organisation } from './generated/Organisation.g';
export { OrganisationContactKind } from './generated/OrganisationContactKind.g';
export { OrganisationContactRelationship } from './generated/OrganisationContactRelationship.g';
export { PartyContactMechanism } from './generated/PartyContactMechanism.g';
export { EmailAddress } from './generated/EmailAddress.g';
export { PostalAddress } from './generated/PostalAddress.g';
export { PostalBoundary } from './generated/PostalBoundary.g';
export { TelecommunicationsNumber } from './generated/TelecommunicationsNumber.g';
export { WebAddress } from './generated/WebAddress.g';

// Custom
import './custom/Person';
