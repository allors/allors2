import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatButtonModule,  MatIconModule, MatToolbarModule } from '@angular/material';
import { RouterModule } from '@angular/router';

import { AllorsMaterialFilterModule, AllorsMaterialTableModule} from '../../../../../material';

import { RequestsOverviewComponent } from './requests-overview.component';
export { RequestsOverviewComponent } from './requests-overview.component';

@NgModule({
  declarations: [
    RequestsOverviewComponent,
  ],
  exports: [
    RequestsOverviewComponent,
  ],
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatIconModule,
    MatToolbarModule,
    AllorsMaterialFilterModule,
    AllorsMaterialTableModule,
  ],
})
export class RequestsOverviewModule {}
