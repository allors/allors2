import { Component, Input } from '@angular/core';
import { ISessionObject } from '../../../../allors/domain';
import { MetaDomain, RoleType } from '../../../../allors/meta';

import { Field } from './Field';

@Component({
  selector: 'a-md-text',
  template: `
<md-input-container flex>
  <input mdInput [type]="type" [(ngModel)]="model" [name]="name" [placeholder]="roleType.name" [disabled]="!canWrite" [required]="required">
</md-input-container>
`,
})
export class InputComponent extends Field {

  get type(): string {
    if (this.roleType) {
      if (this.roleType.objectType.name === 'Integer' ||
        this.roleType.objectType.name === 'Decimal' ||
        this.roleType.objectType.name === 'Float') {
        return 'number';
      }
    }

    return 'text';
  }
}
