import './meta/MetaPopulation';
import './meta/ObjectType';

import { Workspace } from '@allors/domain/system';

export { createName } from './utils/createName';
export { createSlug } from './utils/createSlug';

export { ContentDigests } from './ContentDigests';
export { MediaNodeBuilder } from './MediaNodeBuilder';
export { NodeMapper } from './NodeMapper';
export { SchemaBuilder } from './SchemaBuilder';

export * as roles from './domain/roles';

import { extendMedia } from './domain/Media';

export function extend(workspace: Workspace) {
  extendMedia(workspace);
}
