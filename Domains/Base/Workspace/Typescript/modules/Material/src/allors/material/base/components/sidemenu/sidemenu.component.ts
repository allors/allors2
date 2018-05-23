import { Input, Component } from '@angular/core';
import { SideMenuItem } from './sidemenuitem';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-sidemenu',
  styleUrls: ['sidemenu.component.scss'],
  templateUrl: './sidemenu.component.html',
})
export class SideMenuComponent {

  @Input()
  public items: SideMenuItem[];

  public hasChildren(item: SideMenuItem): boolean {
    return item.children && item.children.length > 0;
  }
}
