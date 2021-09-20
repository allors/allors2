import { Meta } from '@allors/meta/generated';
import { assert } from '@allors/meta/system';
import { WorkEffortState } from '@allors/domain/generated';
import { Workspace } from '@allors/domain/system';


const createdId = 'c082cd60-5c5f-4948-bdb1-06bd9c385751';
const inProgressId = '7a83df7b-9918-4b10-8f99-48896f9db105';
const cancelledId = 'f9fc3cd0-44c9-4343-98fd-494c4d6c9988';
const completedId = '4d942f82-3b8f-4248-9ebc-22b1e5f05d93';
const finishedId = '6a9716a1-8174-4b26-86eb-22a265b74e78';

export function extendWorkEffortState(workspace: Workspace) {
  const m = workspace.metaPopulation as Meta;
  const cls = workspace.constructorByObjectType.get(m.WorkEffortState);
  assert(cls);

  Object.defineProperty(cls.prototype, 'created', {
    configurable: true,
    get(this: WorkEffortState) {
      return this.UniqueId === createdId;
    },
  });

  Object.defineProperty(cls.prototype, 'inProgress', {
    configurable: true,
    get(this: WorkEffortState) {
      return this.UniqueId === inProgressId;
    },
  });

  Object.defineProperty(cls.prototype, 'cancelled', {
    configurable: true,
    get(this: WorkEffortState) {
      return this.UniqueId === cancelledId;
    },
  });

  Object.defineProperty(cls.prototype, 'completed', {
    configurable: true,
    get(this: WorkEffortState) {
      return this.UniqueId === completedId;
    },
  });

  Object.defineProperty(cls.prototype, 'finished', {
    configurable: true,
    get(this: WorkEffortState) {
      return this.UniqueId === finishedId;
    },
  });
}
