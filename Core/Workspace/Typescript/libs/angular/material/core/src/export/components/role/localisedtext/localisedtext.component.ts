import { Component, Optional } from '@angular/core';
import { NgForm } from '@angular/forms';

import { LocalisedRoleField } from '@allors/angular/core';

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
