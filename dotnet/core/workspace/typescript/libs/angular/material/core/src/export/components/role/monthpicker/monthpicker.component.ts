import { Component, Optional, Output, EventEmitter, Self, Inject, Input, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MatDatepicker } from '@angular/material/datepicker';

import { RoleField } from '@allors/angular/core';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-monthpicker',
  styleUrls: ['./monthpicker.component.scss'],
  templateUrl: './monthpicker.component.html',
})
export class AllorsMaterialMonthpickerComponent extends RoleField {
  @Output()
  public selected: EventEmitter<Date> = new EventEmitter();

  @ViewChild(MatDatepicker) picker;

  constructor(@Optional() parentForm: NgForm) {
    super(parentForm);
  }
  public monthSelected(params) {
    this.model = params;
    this.picker.close();
  }
  public onModelChange(selected: Date): void {
    this.selected.emit(selected);
  }
}
