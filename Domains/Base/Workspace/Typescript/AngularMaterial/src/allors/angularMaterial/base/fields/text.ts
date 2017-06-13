import { Component, Input } from '@angular/core';
import { ISessionObject } from '../../../../allors/domain';
import { MetaDomain, RoleType } from '../../../../allors/meta';

@Component({
  selector: 'a-md-text',
  template: `
<div layout="row" *ngIf="object">
  <md-input-container flex>
    <label>{{roleType.name}}</label>
    <input mdInput value="{{ object[roleType.name] }}" type="text">
  </md-input-container>
</div>
  `,
})
export class MaterialTextComponent {
    @Input()
    object: ISessionObject;

    @Input()
    roleType: RoleType;
}
