import { CommonModule} from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatToolbarModule } from '@angular/material/toolbar';

import { AllorsMaterialStaticModule, AllorsMaterialSideNavToggleModule } from '../../../../../material';

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

    AllorsMaterialSideNavToggleModule,
    AllorsMaterialStaticModule,
  ],
})
export class OrganisationOverviewModule {}
