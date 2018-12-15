import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatButtonModule, MatCardModule, MatDividerModule, MatFormFieldModule, MatIconModule, MatListModule, MatMenuModule, MatRadioModule, MatToolbarModule, MatTooltipModule, MatOptionModule, MatSelectModule, MatInputModule, MatTabsModule, MatExpansionModule, MatDatepickerModule } from '@angular/material';

import { AllorsMaterialFileModule } from '../../../../../base/components/file';
import { AllorsMaterialHeaderModule } from '../../../../../base/components/header';
import { AllorsMaterialInputModule } from '../../../../../base/components/input';
import { AllorsMaterialSelectModule } from '../../../../../base/components/select';
import { AllorsMaterialSideNavToggleModule } from '../../../../../base/components/sidenavtoggle';
import { AllorsMaterialSlideToggleModule } from '../../../../../base/components/slidetoggle';
import { AllorsMaterialStaticModule } from '../../../../../base/components/static';
import { AllorsMaterialTextAreaModule } from '../../../../../base/components/textarea';
import { AllorsMaterialFooterModule } from '../../../../../base/components/footer';
import { AllorsMaterialLocalisedTextModule } from '../../../../../base/components/localisedtext';
import { AllorsMaterialFilesModule } from '../../../../../base/components/files';
import { AllorsMaterialDatepickerModule } from '../../../../../base/components/datepicker';

import { BrandInlineModule } from '../../../brand/inline/brand-inline.module';
import { ModelInlineModule } from '../../../model/inline/model-inline.module';

import { PartOverviewDetailComponent } from './part-overview-detail.component';
export { PartOverviewDetailComponent as GoodOverviewDetailComponent } from './part-overview-detail.component';

@NgModule({
  declarations: [
    PartOverviewDetailComponent,
  ],
  exports: [
    PartOverviewDetailComponent,
  ],
  imports: [
    BrandInlineModule,
    ModelInlineModule,

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

    CommonModule,
    FormsModule,
    MatButtonModule,
    MatCardModule,
    MatDatepickerModule,
    MatDividerModule,
    MatExpansionModule,
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
export class PartOverviewDetailModule { }
