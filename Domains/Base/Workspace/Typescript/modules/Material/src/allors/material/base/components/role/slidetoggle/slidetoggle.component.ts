import { Component, Optional } from '@angular/core';
import { NgForm } from '@angular/forms';

import { RoleField } from '../../../../../angular';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-slidetoggle',
  templateUrl: './slidetoggle.component.html',
})
export class AllorsMaterialSlideToggleComponent extends RoleField {

  constructor(@Optional() parentForm: NgForm) {
    super(parentForm);
  }
}
