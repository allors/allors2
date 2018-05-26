import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AllorsMaterialFileModule } from '../../../../base/components/file';
import { MatButtonModule, MatCardModule, MatDividerModule, MatFormFieldModule, MatIconModule, MatListModule, MatMenuModule, MatRadioModule, MatToolbarModule, MatTooltipModule, MatOptionModule, MatSelectModule, MatInputModule } from '@angular/material';
import { RouterModule } from '@angular/router';
import { AllorsMaterialStaticModule } from '../../../../base/components/static';
import { AllorsMaterialSelectModule, AllorsMaterialInputModule, AllorsMaterialTextAreaModule, AllorsMaterialSlideToggleModule } from '../../../..';

import { OrganisationComponent } from './organisation.component';
export { OrganisationComponent } from './organisation.component';

@NgModule({
  declarations: [
    OrganisationComponent,
  ],
  exports: [
    OrganisationComponent,
  ],
  imports: [
    AllorsMaterialFileModule,
    AllorsMaterialInputModule,
    AllorsMaterialSelectModule,
    AllorsMaterialSlideToggleModule,
    AllorsMaterialStaticModule,
    AllorsMaterialTextAreaModule,
    CommonModule,
    FormsModule,
    MatButtonModule,
    MatCardModule,
    MatDividerModule,
    MatFormFieldModule,  
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatMenuModule,
    MatRadioModule,
    MatSelectModule,
    MatToolbarModule,
    MatTooltipModule,
    MatOptionModule,
    ReactiveFormsModule,
    RouterModule,
  ],
})
export class OrganisationModule {}
