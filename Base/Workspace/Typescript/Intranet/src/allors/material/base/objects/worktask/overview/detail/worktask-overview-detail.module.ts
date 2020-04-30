import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatOptionModule } from '@angular/material/core';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import { MatRadioModule } from '@angular/material/radio';
import { MatSelectModule } from '@angular/material/select';
import { MatTabsModule } from '@angular/material/tabs';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatTooltipModule } from '@angular/material/tooltip';

import { AllorsMaterialAutoCompleteModule } from '../../../../../core/components/role/autocomplete';
import { AllorsMaterialDatetimepickerModule } from '../../../../../core/components/role/datetimepicker';
import { AllorsMaterialFileModule } from '../../../../../core/components/role/file';
import { AllorsMaterialFilesModule } from '../../../../../core/components/role/files';
import { AllorsMaterialHeaderModule } from '../../../../../core/components/header';
import { AllorsMaterialInputModule } from '../../../../../core/components/role/input';
import { AllorsMaterialSelectModule } from '../../../../../core/components/role/select';
import { AllorsMaterialSideNavToggleModule } from '../../../../../core/components/sidenavtoggle';
import { AllorsMaterialSlideToggleModule } from '../../../../../core/components/role/slidetoggle';
import { AllorsMaterialStaticModule } from '../../../../../core/components/role/static';
import { AllorsMaterialTextAreaModule } from '../../../../../core/components/role/textarea';
import { AllorsMaterialFooterModule } from '../../../../../core/components/footer';

import { PersonInlineModule } from '../../../person/inline/person-inline.module';
import { ContactMechanismInlineModule } from '../../../contactmechanism/inline/contactmechanism-inline.module';

import { WorkTaskOverviewDetailComponent } from './worktask-overview-detail.component';
export { WorkTaskOverviewDetailComponent } from './worktask-overview-detail.component';

@NgModule({
  declarations: [
    WorkTaskOverviewDetailComponent,
  ],
  exports: [
    WorkTaskOverviewDetailComponent,
  ],
  imports: [
    AllorsMaterialAutoCompleteModule,
    AllorsMaterialDatetimepickerModule,
    AllorsMaterialFileModule,
    AllorsMaterialFilesModule,
    AllorsMaterialHeaderModule,
    AllorsMaterialFooterModule,
    AllorsMaterialInputModule,
    AllorsMaterialSelectModule,
    AllorsMaterialSideNavToggleModule,
    AllorsMaterialSlideToggleModule,
    AllorsMaterialStaticModule,
    AllorsMaterialTextAreaModule,

    PersonInlineModule,
    ContactMechanismInlineModule,

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
    MatTabsModule,
    MatToolbarModule,
    MatTooltipModule,
    MatOptionModule,
    ReactiveFormsModule,
    RouterModule,
  ],
})
export class WorkTaskOverviewDetailModule { }
