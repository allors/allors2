import { Component, Optional } from '@angular/core';
import { NgForm } from '@angular/forms';

import { Test, RoleField } from '../../../../../angular';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-textarea',
  templateUrl: './textarea.component.html',
})
@Test
export class AllorsMaterialTextareaComponent extends RoleField {

  constructor(@Optional() parentForm: NgForm) {
    super(parentForm);
  }
}
