export * from "./auth/login.module";
export * from "./dashboard/dashboard.module";
export * from "./main/main.module";

import { LoginModule } from "./auth/login.module";
import { DashboardModule } from "./dashboard/dashboard.module";
import { MainModule } from "./main/main.module";

export const Modules = [ LoginModule, DashboardModule, MainModule ];
