import { LayoutComponent } from './layout.component';
// Routing
import { OrderDashboardComponent } from './dashboard/order-dashboard.component';
import { RequestOverviewComponent } from './requests/request/request-overview.component';
import { RequestsComponent } from './requests/requests.component';
import { RequestFormComponent } from './requests/request/request.component';

export const ORDERS: any[] = [
  LayoutComponent,
];

export const ORDERS_ROUTING: any[] = [
  OrderDashboardComponent, RequestOverviewComponent, RequestFormComponent, RequestsComponent,
];

export {
  OrderDashboardComponent, RequestFormComponent, RequestOverviewComponent, RequestsComponent,
};
