import { Workspace } from './base/workspace';
import { Population as MetaPopulation } from '../meta';
import { constructorByName } from './generated/domain.g';

const metaPopulation: MetaPopulation = new MetaPopulation();
metaPopulation.init();
export let workspace: Workspace = new Workspace(metaPopulation, constructorByName);

export * from './base/workspace';
export * from './base/database';
export * from './generated';

import './apps/AutomatedAgent';
import './apps/EmailAddress';
import './apps/WebAddress';
import './apps/Organisation';
import './apps/Person';
import './apps/PostalAddress';
import './apps/TelecommunicationsNumber';
import './apps/User';
