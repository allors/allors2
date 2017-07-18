import { Component, Input } from '@angular/core';
import { TdMediaService } from '@covalent/core';

@Component({
  selector: 'relations-header',
  template: `
<button md-icon-button tdLayoutToggle>
  <md-icon>menu</md-icon>
</button>
<md-icon [routerLink]="['/']" class="md-icon-logo cursor-pointer" svgIcon="assets:teradata"></md-icon>
<span [routerLink]="['/']" class="cursor-pointer">{{title}}</span>
`,
})
export class RelationsHeaderComponent {
  @Input()
  title: string;

  constructor(public media: TdMediaService) { }
}
