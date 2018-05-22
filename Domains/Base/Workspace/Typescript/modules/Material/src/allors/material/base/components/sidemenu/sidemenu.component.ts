import { Input, Component, OnInit } from '@angular/core';
import { SideMenuItem } from './sidemenuitem';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-sidemenu',
  styleUrls: ['sidemenu.component.scss'],
  templateUrl: './sidemenu.component.html',
})
export class SideMenuComponent implements OnInit {

  @Input()
  public items: SideMenuItem[];

  ngOnInit(): void {
  }
}
