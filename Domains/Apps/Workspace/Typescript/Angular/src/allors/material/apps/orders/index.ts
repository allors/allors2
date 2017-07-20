// Routing
import { LayoutComponent } from './layout.component';

import { OrderDashboardComponent } from './dashboard/order-dashboard.component';
import { RequestOverviewComponent } from './requests/request/request-overview.component';
import { RequestsComponent } from './requests/requests.component';
import { RequestFormComponent } from './requests/request/request.component';

export const ORDERS: any[] = [
];

export const ORDERS_ROUTING: any[] = [
  LayoutComponent,
  OrderDashboardComponent, RequestOverviewComponent, RequestFormComponent, RequestsComponent,
];

export {
  LayoutComponent,
  OrderDashboardComponent, RequestFormComponent, RequestOverviewComponent, RequestsComponent,
};
