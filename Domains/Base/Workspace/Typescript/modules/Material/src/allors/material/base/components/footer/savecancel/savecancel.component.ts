import { Component, Optional } from '@angular/core';
import { Location } from '@angular/common';
import { NgForm } from '@angular/forms';

import { Allors } from '../../../../../angular';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-footer-save-cancel',
  templateUrl: './savecancel.component.html',
})
export class AllorsMaterialFooterSaveCancelComponent {

  constructor(@Optional() public form: NgForm, public allors: Allors, public location: Location) {
  }

}
