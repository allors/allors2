import { Input, Component } from '@angular/core';
import { SideMenuItem } from './sidemenuitem';
import { Router } from '@angular/router';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-sidemenu',
  styleUrls: ['sidemenu.component.scss'],
  templateUrl: './sidemenu.component.html',
})
export class AllorsMaterialSideMenuComponent {

  @Input()
  public items: SideMenuItem[];

  constructor(public router: Router) { }

  public hasChildren(item: SideMenuItem): boolean {
    return item.children && item.children.length > 0;
  }
}
