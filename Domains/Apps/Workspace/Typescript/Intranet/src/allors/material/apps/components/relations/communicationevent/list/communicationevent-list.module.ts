import { NgModule } from '@angular/core';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { CdkTableModule } from '@angular/cdk/table';
import { MatButtonModule, MatCardModule, MatDividerModule, MatFormFieldModule, MatIconModule, MatListModule, MatMenuModule, MatRadioModule, MatToolbarModule, MatTooltipModule, MatOptionModule, MatSelectModule, MatInputModule, MatTableModule, MatSortModule, MatPaginatorModule, MatCheckboxModule } from '@angular/material';
import { RouterModule } from '@angular/router';

import { AllorsMaterialAutoCompleteModule } from '../../../../../base/components/autocomplete';
import { AllorsMaterialChipsModule } from '../../../../../base/components/chips';
import { AllorsMaterialDatepickerModule } from '../../../../../base/components/datepicker';
import { AllorsMaterialFileModule } from '../../../../../base/components/file';
import { AllorsMaterialFilesModule } from '../../../../../base/components/files';
import { AllorsMaterialFilterModule } from '../../../../../base/components/filter';
import { AllorsMaterialInputModule } from '../../../../../base/components/input';
import { AllorsMaterialLocalisedTextModule } from '../../../../../base/components/localisedtext';
import { AllorsMaterialSelectModule } from '../../../../../base/components/select';
import { AllorsMaterialSideNavToggleModule } from '../../../../../base/components/sidenavtoggle';
import { AllorsMaterialSlideToggleModule } from '../../../../../base/components/slidetoggle';
import { AllorsMaterialStaticModule } from '../../../../../base/components/static';
import { AllorsMaterialTextAreaModule } from '../../../../../base/components/textarea';

import { CommunicationEventListComponent } from './communicationevent-list.component';
export { CommunicationEventListComponent } from './communicationevent-list.component';

@NgModule({
  declarations: [
    CommunicationEventListComponent,
  ],
  exports: [
    CommunicationEventListComponent,
  ],
  imports: [
    AllorsMaterialFileModule,
    AllorsMaterialFilterModule,
    AllorsMaterialInputModule,
    AllorsMaterialSelectModule,
    AllorsMaterialSideNavToggleModule,
    AllorsMaterialSlideToggleModule,
    AllorsMaterialStaticModule,
    AllorsMaterialTextAreaModule,
    CommonModule,
    CdkTableModule,
    FormsModule,
    MatButtonModule,
    MatCardModule,
    MatCheckboxModule,
    MatDividerModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatMenuModule,
    MatPaginatorModule,
    MatRadioModule,
    MatSelectModule,
    MatSortModule,
    MatTableModule,
    MatToolbarModule,
    MatTooltipModule,
    MatOptionModule,
    ReactiveFormsModule,
    RouterModule,
  ],
})
export class CommunicationEventsOverviewModule { }
