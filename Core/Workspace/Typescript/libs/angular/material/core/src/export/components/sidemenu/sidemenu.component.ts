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
    if (item.children) {
      return item.children.length > 0 ?? false;
    }

    return false;
  }
}
