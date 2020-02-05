import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatOptionModule } from '@angular/material/core';
import { MatDividerModule } from '@angular/material/divider';
import { MatExpansionModule } from '@angular/material/expansion';
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
import { AllorsMaterialDatepickerModule } from '../../../../../core/components/role/datepicker';
import { AllorsMaterialFileModule } from '../../../../../core/components/role/file';
import { AllorsMaterialFilesModule } from '../../../../../core/components/role/files';
import { AllorsMaterialHeaderModule } from '../../../../../core/components/header';
import { AllorsMaterialInputModule } from '../../../../../core/components/role/input';
import { AllorsMaterialLocalisedTextModule } from '../../../../../core/components/role/localisedtext';
import { AllorsMaterialSelectModule } from '../../../../../core/components/role/select';
import { AllorsMaterialSideNavToggleModule } from '../../../../../core/components/sidenavtoggle';
import { AllorsMaterialSlideToggleModule } from '../../../../../core/components/role/slidetoggle';
import { AllorsMaterialStaticModule } from '../../../../../core/components/role/static';
import { AllorsMaterialTextAreaModule } from '../../../../../core/components/role/textarea';
import { AllorsMaterialQuillModule } from '../../../../../core/components/role/quill';
import { AllorsMaterialFooterModule } from '../../../../../core/components/footer';
import { AllorsMaterialMarkdownModule } from '../../../../../core/components/role/markdown';

import { SerialisedItemOverviewDetailComponent } from './serialiseditem-overview-detail.component';
export { SerialisedItemOverviewDetailComponent } from './serialiseditem-overview-detail.component';

@NgModule({
  declarations: [
    SerialisedItemOverviewDetailComponent,
  ],
  exports: [
    SerialisedItemOverviewDetailComponent,
  ],
  imports: [
    AllorsMaterialAutoCompleteModule,
    AllorsMaterialDatepickerModule,
    AllorsMaterialFileModule,
    AllorsMaterialFilesModule,
    AllorsMaterialHeaderModule,
    AllorsMaterialFooterModule,
    AllorsMaterialInputModule,
    AllorsMaterialLocalisedTextModule,
    AllorsMaterialSelectModule,
    AllorsMaterialSideNavToggleModule,
    AllorsMaterialSlideToggleModule,
    AllorsMaterialStaticModule,
    AllorsMaterialTextAreaModule,
    AllorsMaterialQuillModule,
    AllorsMaterialMarkdownModule,
    CommonModule,
    FormsModule,
    MatButtonModule,
    MatCardModule,
    MatExpansionModule,
    MatTabsModule,
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
export class SerialisedItemOverviewDetailModule { }
