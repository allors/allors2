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

// Custom
export { Organisation } from './generated/Organisation.g';
export { UnitSample } from './generated/UnitSample.g';

import './custom/Person';
