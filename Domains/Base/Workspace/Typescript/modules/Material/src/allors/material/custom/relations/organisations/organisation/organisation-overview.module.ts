import { CommonModule} from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { MatButtonModule, MatCardModule, MatIconModule, MatListModule, MatToolbarModule } from '@angular/material';

import { AllorsMaterialStaticModule } from '../../../../../material';

import { OrganisationOverviewComponent } from './organisation-overview.component';
export { OrganisationOverviewComponent } from './organisation-overview.component';

@NgModule({
  declarations: [
    OrganisationOverviewComponent,
  ],
  exports: [
    OrganisationOverviewComponent,
  ],
  imports: [
    CommonModule,
    RouterModule,
    MatButtonModule,
    MatCardModule,
    MatIconModule,
    MatListModule,
    MatToolbarModule,
    AllorsMaterialStaticModule,
  ],
})
export class OrganisationOverviewModule {}
