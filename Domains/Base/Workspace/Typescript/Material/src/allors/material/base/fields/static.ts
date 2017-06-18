import { Component, Input } from '@angular/core';
import { ISessionObject } from '../../../../allors/domain';
import { MetaDomain, RoleType } from '../../../../allors/meta';

import { Field } from '../../../angular';

@Component({
  selector: 'a-md-static',
  template: `
<md-input-container>
  <input mdInput type="type" [ngModel]="static" [name]="name" [placeholder]="label" readonly>
</md-input-container>
`,
})
export class StaticComponent extends Field {
  @Input()
  display: string;

  get static(): string {
    if (this.ExistObject) {
      if (this.roleType.objectType.isUnit) {
        return this.model;
      } else {
        if (this.roleType.isOne) {
          return this.model[this.display];
        } else {
          const roles: any[] = this.model;
          if (roles) {
            return roles
              .map((v: any) => v[this.display])
              .reduce((acc: string, cur: string) => acc + ', ' + cur);
          }
        }
      }
    }
  }
}
