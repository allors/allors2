// Overview
import { OverviewComponent } from "./overview.component";

import { OrdersOverviewComponent } from "./overview/orders/ordersOverview.component";
import { RequestOverviewComponent } from "./overview/request/requestOverview.component";
import { RequestsOverviewComponent } from "./overview/requests/requestsOverview.component";

// Request
import { RequestEditComponent } from "./request/edit.component";

export const ORDERS: any[] = [
];

export const ORDERS_ROUTING: any[] = [
  // Overview
  OverviewComponent,

  OrdersOverviewComponent,
  RequestsOverviewComponent,
  RequestOverviewComponent,

  // Request
  RequestEditComponent,
];

export {
  // Overview
  OverviewComponent,
  OrdersOverviewComponent,
  RequestsOverviewComponent,
  RequestOverviewComponent,

  // Party
  RequestEditComponent,
};
