import { AfterViewInit, Component, EventEmitter, Input, Optional, Output, QueryList, ViewChildren } from "@angular/core";
import { NgForm, NgModel } from "@angular/forms";

import { ISessionObject } from "../../../../framework";

import { Field } from "../../../../angular";

@Component({
  selector: "a-mat-select",
  templateUrl: "./select.component.html",
})
export class SelectComponent extends Field {
  @Input()
  public display: string = "display";

  @Input()
  public options: ISessionObject[];

  @Output()
  public onSelect: EventEmitter<ISessionObject> = new EventEmitter();

  constructor(@Optional() parentForm: NgForm) {
    super(parentForm);
  }

  public onModelChange(option: ISessionObject): void {
    this.onSelect.emit(option);
  }
}
