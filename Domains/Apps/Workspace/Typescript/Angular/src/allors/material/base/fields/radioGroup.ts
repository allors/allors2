import { Component, Input } from '@angular/core';
import { ISessionObject } from '../../../../allors/domain';
import { MetaDomain, RoleType } from '../../../../allors/meta';

import { Field } from './Field';

export class RadioGroupOption {
  value: any;
  label?: string;
}

@Component({
  selector: 'a-md-radio-group',
  template: `
  <md-input-container flex>
    <md-radio-group [(ngModel)]="model" [name]="name" [disabled]="!canWrite" [required]="required">
      <md-radio-button *ngFor="let option of options" [value]="optionValue(option)">{{optionLabel(option)}}</md-radio-button>
    </md-radio-group>
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
    return option.label ? option.label : option.value.toString();
  }

  optionValue(option: RadioGroupOption): any {
    return option.value;
  }
}
