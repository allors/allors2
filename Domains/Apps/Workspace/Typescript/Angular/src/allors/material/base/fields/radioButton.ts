import { Component, Input } from '@angular/core';
import { ISessionObject } from '../../../../allors/domain';
import { MetaDomain, RoleType } from '../../../../allors/meta';

import { Field } from './Field';

@Component({
  selector: 'a-md-radio-button',
  template: `
  <md-input-container flex>
    <md-radio-group [(ngModel)]="model" [name]="name" [disabled]="!canWrite" [required]="required">
      <md-radio-button value="true">{{trueLabel}}</md-radio-button>
      <md-radio-button value="false">{{falseLabel}}</md-radio-button>
    </md-radio-group>
  </md-input-container>
  `,
})
export class RadioButtonComponent extends Field {
  @Input()
  trueLabel: string;

  @Input()
  falseLabel: string;
}
