import { domain } from '../domain';
import { WorkEffortState } from '../generated/WorkEffortState.g';

declare module '../generated/WorkEffortState.g' {
  interface WorkEffortState {
    created: boolean;
    inProgress: boolean;
    cancelled: boolean;
    completed: boolean;
    finished: boolean;
  }
}

const createdId = 'c082cd60-5c5f-4948-bdb1-06bd9c385751';
const inProgressId = '7a83df7b-9918-4b10-8f99-48896f9db105';
const cancelledId = 'f9fc3cd0-44c9-4343-98fd-494c4d6c9988';
const completedId = '4d942f82-3b8f-4248-9ebc-22b1e5f05d93';
const finishedId = '6a9716a1-8174-4b26-86eb-22a265b74e78';

domain.extend((workspace) => {

  const obj: WorkEffortState = workspace.prototypeByName.WorkEffortState;

  Object.defineProperty(obj, 'created', {
    configurable: true,
    get(this: WorkEffortState) {
      return this.UniqueId === createdId;
    },
  });

  Object.defineProperty(obj, 'inProgress', {
    configurable: true,
    get(this: WorkEffortState) {
      return this.UniqueId === inProgressId;
    },
  });

  Object.defineProperty(obj, 'cancelled', {
    configurable: true,
    get(this: WorkEffortState) {
      return this.UniqueId === cancelledId;
    },
  });

  Object.defineProperty(obj, 'completed', {
    configurable: true,
    get(this: WorkEffortState) {
      return this.UniqueId === completedId;
    },
  });

  Object.defineProperty(obj, 'finished', {
    configurable: true,
    get(this: WorkEffortState) {
      return this.UniqueId === finishedId;
    },
  });

});
