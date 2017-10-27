// Overview
import { WorkEffortsOverviewComponent } from "./overview/workefforts/workEffortsOverview.component";
import { WorkTaskOverviewComponent } from "./overview/worktask/workTaskOverview.component";
import { WorkTasksOverviewComponent } from "./overview/worktasks/workTasksOverview.component";

// WorkTask
import { WorkTaskEditComponent } from "./worktask/edit.component";
import { WorkTaskInlineComponent } from "./worktask/inline.component";

export const WORKEFFORTS: any[] = [
];

export const WORKEFFORTS_ROUTING: any[] = [
  // Overview
  WorkEffortsOverviewComponent,
  WorkTasksOverviewComponent,
  WorkTaskOverviewComponent,

  // WorkTask
  WorkTaskEditComponent,
  WorkTaskInlineComponent,
];

export {
  // Overview
  WorkEffortsOverviewComponent,
  WorkTasksOverviewComponent,
  WorkTaskOverviewComponent,

  // WorkTask
  WorkTaskEditComponent,
  WorkTaskInlineComponent,
};
