export * from './workeffort/workefforts-overview.module';
export * from './workeffortassignment/workeffortassignments-overview.module';

export * from './worktask/overview/worktask-overview.module';
export * from './worktask/list/worktasks-list.module';

export * from './worktask/edit/worktask-edit.module';

import { WorkEffortsOverviewModule } from './workeffort/workefforts-overview.module';
import { WorkEffortAssignmentsOverviewModule } from './workeffortassignment/workeffortassignments-overview.module';

import { WorkTaskOverviewModule } from './worktask/overview/worktask-overview.module';
import { WorkTasksOverviewModule } from './worktask/list/worktasks-list.module';

import { WorkTaskEditModule } from './worktask/edit/worktask-edit.module';

export const Modules = [
  WorkEffortsOverviewModule,
  WorkEffortAssignmentsOverviewModule,

  WorkTasksOverviewModule,
  WorkTaskOverviewModule,

  WorkTaskEditModule,
];
