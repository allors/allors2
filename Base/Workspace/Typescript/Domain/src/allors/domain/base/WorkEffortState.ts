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

const createdId = 'c082cd605c5f4948bdb106bd9c385751';
const inProgressId = '7a83df7b99184b108f9948896f9db105';
const cancelledId = 'f9fc3cd044c9434398fd494c4d6c9988';
const completedId = '4d942f823b8f42489ebc22b1e5f05d93';
const finishedId = '6a9716a181744b2686eb22a265b74e78';

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
