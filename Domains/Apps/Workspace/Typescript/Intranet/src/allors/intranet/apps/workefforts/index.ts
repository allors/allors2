export * from "./overview.module";

export * from "./workeffort/workefforts-overview.module";

export * from "./worktask/worktask-overview.module";
export * from "./worktask/worktasks-overview.module";

export * from "./worktask/worktask.module";

import { OverviewModule } from "./overview.module";

import { WorkEffortsOverviewModule } from "./workeffort/workefforts-overview.module";

import { WorkTaskOverviewModule } from "./worktask/worktask-overview.module";
import { WorkTasksOverviewModule } from "./worktask/worktasks-overview.module";

import { WorkTaskEditModule } from "./worktask/worktask.module";

export const Modules = [
  OverviewModule,
  WorkEffortsOverviewModule,
  WorkTasksOverviewModule,
  WorkTaskOverviewModule,

  WorkTaskEditModule,
];
