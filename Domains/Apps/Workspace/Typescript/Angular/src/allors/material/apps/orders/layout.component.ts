import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { TdMediaService } from '@covalent/core';

@Component({
  selector: 'a-td-layout-order',
  templateUrl: './layout.component.html',
})
export class LayoutComponent {

  routes: any[] = [
    { title: 'Dashboard', route: '/', icon: 'dashboard', },
    { title: 'Relations', route: '/relations', icon: 'people', },
    { title: 'Catalogues', route: '/catalogues', icon: 'work', },
    { title: 'Orders', route: '/orders', icon: 'dashboard', },
  ];

  usermenu: any[] = [
    { icon: 'tune', route: '.', title: 'Account settings', },
    { icon: 'exit_to_app', route: '.', title: 'Sign out', },
  ];

  navmenu: any[] = [
    { title: 'Requests', description: 'Manage requests', route: '/orders/requests', icon: 'dashboard', },
  ];

  @Input()
  title: string;

  constructor(public media: TdMediaService) {
  }
}
