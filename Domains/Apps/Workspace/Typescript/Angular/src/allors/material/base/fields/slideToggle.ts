import { Component, Input } from '@angular/core';
import { ISessionObject } from '../../../../allors/domain';
import { MetaDomain, RoleType } from '../../../../allors/meta';

import { Field } from './Field';

@Component({
  selector: 'a-md-slide-toggle',
  template: `
<md-slide-toggle [(ngModel)]="model" [name]="name" [disabled]="!canWrite" [required]="required">
{{label}}
</md-slide-toggle>
`,
})
export class SlideToggleComponent extends Field {
}
