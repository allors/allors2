import { ChangeDetectorRef, Component , Input } from "@angular/core";
import { ISessionObject } from "@baseDomain";
import { RoleType } from "@baseMeta";

import { Field } from "@baseAngular";

export interface RadioGroupOption {
  label?: string;
  value: any;
}

@Component({
  selector: "a-mat-radio-group",
  template: `
<mat-input-container fxLayout="row">
  <mat-radio-group [(ngModel)]="model" [name]="name" [required]="required" [disabled]="disabled">
    <mat-radio-button *ngFor="let option of options" [value]="optionValue(option)">{{optionLabel(option)}}</mat-radio-button>
  </mat-radio-group>
  <mat-hint *ngIf="hint">{{hint}}</mat-hint>
</mat-input-container>
`,
})
export class RadioGroupComponent extends Field {
  @Input()
  public options: RadioGroupOption[];

  get keys(): string[]{
    return Object.keys(this.options);
  }

  public optionLabel(option: RadioGroupOption): string {
    return option.label ? option.label : this.humanize(option.value.toString());
  }

  public optionValue(option: RadioGroupOption): any {
    return option.value;
  }
}
