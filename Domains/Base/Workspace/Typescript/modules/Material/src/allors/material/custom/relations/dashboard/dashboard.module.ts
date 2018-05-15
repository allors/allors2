import { NgModule } from '@angular/core';

import { MatButtonModule, MatCardModule, MatIconModule, MatListModule, MatToolbarModule } from '@angular/material';

import { DashboardComponent } from './dashboard.component';
export { DashboardComponent } from './dashboard.component';

@NgModule({
  declarations: [
    DashboardComponent,
  ],
  exports: [
    DashboardComponent,
  ],
  imports: [
    MatButtonModule,
    MatCardModule,
    MatIconModule,
    MatListModule,
    MatToolbarModule,
  ],
})
export class DashboardModule { }
