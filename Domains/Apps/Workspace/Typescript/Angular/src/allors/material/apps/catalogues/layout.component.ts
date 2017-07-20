import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { TdMediaService } from '@covalent/core';

@Component({
  selector: 'a-td-layout-catalogue',
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
    { title: 'Catalogues', description: 'Manage catalogues', route: '/catalogues/catalogues', icon: 'dashboard' },
    { title: 'Categories', description: 'Manage categories', route: '/catalogues/categories', icon: 'dashboard' },
    { title: 'Products', description: 'Manage products', route: '/catalogues/goods', icon: 'dashboard' },
    { title: 'ProductTypes', description: 'Manage product types', route: '/catalogues/productTypes', icon: 'dashboard' },
    { title: 'Characteristics', description: 'Manage characteristics', route: '/catalogues/productCharacteristics', icon: 'dashboard' },
  ];

  @Input()
  title: string;

  constructor(public media: TdMediaService) {
  }
}
