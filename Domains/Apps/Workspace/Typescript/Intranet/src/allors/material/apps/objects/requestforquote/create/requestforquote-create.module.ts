import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatButtonModule, MatCardModule, MatDividerModule, MatFormFieldModule, MatIconModule, MatListModule, MatMenuModule, MatRadioModule, MatToolbarModule, MatTooltipModule, MatOptionModule, MatSelectModule, MatInputModule } from '@angular/material';

import { AllorsMaterialAutoCompleteModule } from '../../../../base/components/autocomplete';
import { AllorsMaterialHeaderModule } from '../../../../base/components/header';
import { AllorsMaterialFileModule } from '../../../../base/components/file';
import { AllorsMaterialInputModule } from '../../../../base/components/input';
import { AllorsMaterialSelectModule } from '../../../../base/components/select';
import { AllorsMaterialSideNavToggleModule } from '../../../../base/components/sidenavtoggle';
import { AllorsMaterialSlideToggleModule } from '../../../../base/components/slidetoggle';
import { AllorsMaterialStaticModule } from '../../../../base/components/static';
import { AllorsMaterialTextAreaModule } from '../../../../base/components/textarea';
import { AllorsMaterialFooterModule } from '../../../../base/components/footer';
import { AllorsMaterialDatepickerModule } from '../../../../base/components/datepicker';

import { PersonInlineModule } from '../../person/inline/person-inline.module';
import { ContactMechanismInlineModule } from '../../contactmechanism/inline/contactmechanism-inline.module';


import { RequestForQuoteCreateComponent } from './requestforquote-create.component';
export { RequestForQuoteCreateComponent } from './requestforquote-create.component';

@NgModule({
  declarations: [
    RequestForQuoteCreateComponent,
  ],
  exports: [
    RequestForQuoteCreateComponent,
  ],
  imports: [
    PersonInlineModule,
    ContactMechanismInlineModule,

    AllorsMaterialAutoCompleteModule,
    AllorsMaterialDatepickerModule,
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
export class RequestForQuoteCreateModule { }
