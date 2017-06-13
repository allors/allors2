import { Workspace } from './base/workspace';
import { Population as MetaPopulation } from '../meta';
import { constructorByName } from './generated/domain.g';

const metaPopulation = new MetaPopulation();
metaPopulation.init();

export let workspace = new Workspace(metaPopulation, constructorByName);
export * from './base/workspace';

// Base
export { ResponseType, PushRequest, SyncRequest, InvokeRequest, PullResponse, SyncResponse,
         PushResponse, InvokeResponse, PullRequest,
         Fetch, Path, Query, Predicate, And, Equals, Like, TreeNode, Sort, Page } from './base/database';

export * from './generated';

import './custom/Person';
