import { Component, Optional } from '@angular/core';
import { NgForm } from '@angular/forms';

import { RoleField } from '@allors/angular/core';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-checkbox',
  templateUrl: './checkbox.component.html',
})
export class AllorsMaterialCheckboxComponent extends RoleField {

  constructor(@Optional() parentForm: NgForm) {
    super(parentForm);
  }
}
