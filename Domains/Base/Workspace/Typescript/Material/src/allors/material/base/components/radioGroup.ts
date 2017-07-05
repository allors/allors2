import { Component, Input } from '@angular/core';
import { ISessionObject } from '../../../../allors/domain';
import { MetaDomain, RoleType } from '../../../../allors/meta';

import { Field } from '../../../angular';

export class RadioGroupOption {
  label?: string;
  value: any;
}

@Component({
  selector: 'a-md-radio-group',
  template: `
<md-input-container fxLayout="row">
  <md-radio-group fxFlex [(ngModel)]="model" [name]="name" [required]="required" [disabled]="disabled">
    <md-radio-button *ngFor="let option of options" [value]="optionValue(option)">{{optionLabel(option)}}</md-radio-button>
  </md-radio-group>
  <md-hint *ngIf="hint">{{hint}}</md-hint>
</md-input-container>
`,
})
export class RadioGroupComponent extends Field {
  @Input()
  options: RadioGroupOption[];

  get keys(): string[]{
    return Object.keys(this.options);
  }

  optionLabel(option: RadioGroupOption): string {
    return option.label ? option.label : this.humanize(option.value.toString());
  }

  optionValue(option: RadioGroupOption): any {
    return option.value;
  }
}
