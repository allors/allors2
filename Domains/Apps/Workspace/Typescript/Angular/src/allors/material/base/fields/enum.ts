import { Component, Input } from '@angular/core';
import { ISessionObject } from '../../../../allors/domain';
import { MetaDomain, RoleType } from '../../../../allors/meta';

import { Field } from './Field';

class Enum {
  constructor(public value: number, public name: string) { }
}

@Component({
  selector: 'a-md-select-enum',
  template: `
  <md-input-container flex>
  <md-select [(ngModel)]="model" [name]="name" [placeholder]="roleType.name" [disabled]="!canWrite" [required]="required">
    <md-option *ngFor="let enum of enums" [value]="enum.number">
      {{enum.name}}
    </md-option>
  </md-select>
</md-input-container>
  `,
})
export class EnumComponent extends Field {

  @Input()
  type: any;

  get enums(): any[] {
    const enums: Enum[] = []
    for (const k in this.type) {
      if (this.type.hasOwnProperty(k)) {
        const value = this.type[k];
        if (typeof value === 'number') {
          const name = this.type[value];
          const enumeration = new Enum(value, name);
          enums.push(enumeration);
        }
      }
    }

    return enums;
  }
}
