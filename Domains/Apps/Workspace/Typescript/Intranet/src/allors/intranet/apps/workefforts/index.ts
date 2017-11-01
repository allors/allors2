export * from "./overview/overview.module";

export * from "./overview/workefforts/workEffortsOverview.module";
export * from "./overview/worktask/workTaskOverview.module";
export * from "./overview/worktasks/workTasksOverview.module";

export * from "./worktask/edit.module";

import { OverviewModule } from "./overview/overview.module";

import { WorkEffortsOverviewModule } from "./overview/workefforts/workEffortsOverview.module";
import { WorkTaskOverviewModule } from "./overview/worktask/workTaskOverview.module";
import { WorkTasksOverviewModule } from "./overview/worktasks/workTasksOverview.module";

import { WorkTaskEditModule } from "./worktask/edit.module";

export const Modules = [
  OverviewModule,
  WorkEffortsOverviewModule,
  WorkTasksOverviewModule,
  WorkTaskOverviewModule,

  WorkTaskEditModule,
];
