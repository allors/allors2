import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { TdMediaService } from '@covalent/core';

@Component({
  selector: 'a-td-layout-relation',
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
    { title: 'Organisations', description: 'Manage organisations', route: '/relations/organisations', icon: 'business center' },
    { title: 'People', description: 'Manage people', route: '/relations/people', icon: 'people' },
  ];

  @Input()
  title: string;

  constructor(public media: TdMediaService) {
  }
}
