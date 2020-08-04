import { Workspace } from '@allors/domain/system';
import { extendMedia } from './domain/Media';

export * from './module';

export * from './actions';
export * from './filter';
export * from './forms';
export * from './framework';
export * from './search';
export * from './test';


// Meta extensions
import './meta/ObjectType';

// Domain extensions
export function extend(workspace: Workspace) {
  extendMedia(workspace);
}
