export * from './workeffort/workefforts-overview.module';
export * from './workeffortassignment/workeffortassignments-overview.module';

export * from './worktask/worktask-overview.module';
export * from './worktask/worktasks-overview.module';

export * from './worktask/worktask.module';

import { WorkEffortsOverviewModule } from './workeffort/workefforts-overview.module';
import { WorkEffortAssignmentsOverviewModule } from './workeffortassignment/workeffortassignments-overview.module';

import { WorkTaskOverviewModule } from './worktask/worktask-overview.module';
import { WorkTasksOverviewModule } from './worktask/worktasks-overview.module';

import { WorkTaskEditModule } from './worktask/worktask.module';

export const Modules = [
  WorkEffortsOverviewModule,
  WorkEffortAssignmentsOverviewModule,

  WorkTasksOverviewModule,
  WorkTaskOverviewModule,

  WorkTaskEditModule,
];
