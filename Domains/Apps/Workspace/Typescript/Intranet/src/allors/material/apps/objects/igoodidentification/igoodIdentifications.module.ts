import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatButtonModule, MatCardModule, MatDividerModule, MatFormFieldModule, MatIconModule, MatListModule, MatMenuModule, MatRadioModule, MatToolbarModule, MatTooltipModule, MatOptionModule, MatSelectModule, MatInputModule } from '@angular/material';

import { AllorsMaterialFileModule } from '../../../base/components/file';
import { AllorsMaterialInputModule } from '../../../base/components/input';
import { AllorsMaterialSelectModule } from '../../../base/components/select';
import { AllorsMaterialSideNavToggleModule } from '../../../base/components/sidenavtoggle';
import { AllorsMaterialSlideToggleModule } from '../../../base/components/slidetoggle';
import { AllorsMaterialStaticModule } from '../../../base/components/static';
import { AllorsMaterialTextAreaModule } from '../../../base/components/textarea';


import { IGoodIdentificationsComponent } from './igoodIdentifications.component';
export { IGoodIdentificationsComponent } from './igoodIdentifications.component';

@NgModule({
  declarations: [
    IGoodIdentificationsComponent,
  ],
  exports: [
    IGoodIdentificationsComponent,
  ],
  imports: [
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

    AllorsMaterialFileModule,
    AllorsMaterialInputModule,
    AllorsMaterialSelectModule,
    AllorsMaterialSideNavToggleModule,
    AllorsMaterialSlideToggleModule,
    AllorsMaterialStaticModule,
    AllorsMaterialTextAreaModule,
  ],
})
export class IGoodIdentificationsModule { }
