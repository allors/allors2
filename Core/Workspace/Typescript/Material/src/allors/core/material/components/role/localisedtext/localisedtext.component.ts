import { Component, Optional } from '@angular/core';
import { NgForm } from '@angular/forms';

import { LocalisedRoleField } from '../../../../../angular/core/forms';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-localised-text',
  templateUrl: './localisedtext.component.html',
})
export class AllorsMaterialLocalisedTextComponent extends LocalisedRoleField {

  constructor(@Optional() parentForm: NgForm) {
    super(parentForm);
  }
}
