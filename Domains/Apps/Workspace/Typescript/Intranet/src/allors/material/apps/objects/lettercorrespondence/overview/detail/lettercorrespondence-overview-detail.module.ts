import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatButtonModule, MatCardModule, MatDividerModule, MatFormFieldModule, MatIconModule, MatListModule, MatMenuModule, MatRadioModule, MatToolbarModule, MatTooltipModule, MatOptionModule, MatSelectModule, MatInputModule } from '@angular/material';

import { AllorsMaterialDatepickerModule } from '../../../../../base/components/datepicker/datepicker.module';
import { AllorsMaterialDatetimepickerModule } from '../../../../../base/components/datetimepicker/datetimepicker.module';
import { AllorsMaterialChipsModule } from '../../../../../base/components/chips/chips.module';
import { AllorsMaterialFileModule } from '../../../../../base/components/file';
import { AllorsMaterialHeaderModule } from '../../../../../base/components/header';
import { AllorsMaterialInputModule } from '../../../../../base/components/input';
import { AllorsMaterialSelectModule } from '../../../../../base/components/select';
import { AllorsMaterialSideNavToggleModule } from '../../../../../base/components/sidenavtoggle';
import { AllorsMaterialSlideToggleModule } from '../../../../../base/components/slidetoggle';
import { AllorsMaterialStaticModule } from '../../../../../base/components/static';
import { AllorsMaterialTextAreaModule } from '../../../../../base/components/textarea';
import { AllorsMaterialFooterModule } from '../../../../../base/components/footer';

import { PersonInlineModule } from '../../../person/inline/person-inline.module';
import { PostalAddressInlineModule } from '../../../postaladdress/inline/postaladdress-inline.module';

import { LetterCorrespondenceOverviewDetailComponent } from './lettercorrespondence-overview-detail.component';
export { LetterCorrespondenceOverviewDetailComponent } from './lettercorrespondence-overview-detail.component';

@NgModule({
  declarations: [
    LetterCorrespondenceOverviewDetailComponent,
  ],
  exports: [
    LetterCorrespondenceOverviewDetailComponent,
  ],
  imports: [
    PersonInlineModule,
    PostalAddressInlineModule,

    AllorsMaterialChipsModule,
    AllorsMaterialDatepickerModule,
    AllorsMaterialDatetimepickerModule,
    AllorsMaterialFileModule,
    AllorsMaterialHeaderModule,
    AllorsMaterialFooterModule,
    AllorsMaterialInputModule,
    AllorsMaterialSelectModule,
    AllorsMaterialSideNavToggleModule,
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
export class LetterCorrespondenceOverviewDetailModule { }
