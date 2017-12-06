import { AfterViewInit, Component, EventEmitter, Input, Optional, Output, QueryList, ViewChildren } from "@angular/core";
import { NgForm, NgModel } from "@angular/forms";

import { ISessionObject } from "../../../../framework";

import { Field } from "../../../../angular";

@Component({
  selector: "a-mat-select",
  template: `
<mat-form-field style="width: 100%;" >
    <mat-select [(ngModel)]="model" [name]="name" [placeholder]="label" [multiple]="roleType.isMany" [required]="required" [disabled]="disabled">
    <mat-option *ngIf="!required">None</mat-option>
    <mat-option *ngFor="let option of options" [value]="option" (onSelectionChange)="selected(option)">
        {{option[display]}}
      </mat-option>
    </mat-select>
</mat-form-field>
`,
})
export class SelectComponent extends Field implements AfterViewInit {
  @Input()
  public display: string = "display";

  @Input()
  public options: ISessionObject[];

  @Output()
  public onSelect: EventEmitter<ISessionObject> = new EventEmitter();

  @ViewChildren(NgModel) private controls: QueryList<NgModel>;

  constructor( @Optional() private parentForm: NgForm) {
    super();
  }

  public ngAfterViewInit(): void {
    if (!!this.parentForm) {
      this.controls.forEach((control: NgModel) => {
        this.parentForm.addControl(control);
      });
    }
  }

  public selected(option: ISessionObject): void {
    this.onSelect.emit(option);
  }
}
