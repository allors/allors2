import { Component, Input } from '@angular/core';
import { ISessionObject } from '../../../../allors/domain';
import { MetaDomain, RoleType } from '../../../../allors/meta';

import { Field } from './Field';

@Component({
  selector: 'a-md-static',
  template: `
  <md-input-container flex>
    <input mdInput type="text" [ngModel]="model" [name]="name" [placeholder]="roleType.name" disabled="true">
  </md-input-container>
  `,
})
export class StaticComponent extends Field {
}
