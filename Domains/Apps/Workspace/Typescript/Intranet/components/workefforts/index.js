"use strict";
function __export(m) {
    for (var p in m) if (!exports.hasOwnProperty(p)) exports[p] = m[p];
}
Object.defineProperty(exports, "__esModule", { value: true });
__export(require("./overview.module"));
__export(require("./workeffort/workefforts-overview.module"));
__export(require("./worktask/worktask-overview.module"));
__export(require("./worktask/worktasks-overview.module"));
__export(require("./worktask/worktask.module"));
const overview_module_1 = require("./overview.module");
const workefforts_overview_module_1 = require("./workeffort/workefforts-overview.module");
const worktask_overview_module_1 = require("./worktask/worktask-overview.module");
const worktasks_overview_module_1 = require("./worktask/worktasks-overview.module");
const worktask_module_1 = require("./worktask/worktask.module");
exports.Modules = [
    overview_module_1.OverviewModule,
    workefforts_overview_module_1.WorkEffortsOverviewModule,
    worktasks_overview_module_1.WorkTasksOverviewModule,
    worktask_overview_module_1.WorkTaskOverviewModule,
    worktask_module_1.WorkTaskEditModule,
];
