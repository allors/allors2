import { Component, Input , ChangeDetectorRef } from '@angular/core';
import { Router } from '@angular/router';
import { TdMediaService } from '@covalent/core';

@Component({
  templateUrl: './layout.component.html',
})
export class LayoutComponent {

  navmenu: any[] = [
    { title: 'Catalogues', description: 'Manage catalogues', route: '/catalogues/catalogues', icon: 'dashboard' },
    { title: 'Categories', description: 'Manage categories', route: '/catalogues/categories', icon: 'dashboard' },
    { title: 'Products', description: 'Manage products', route: '/catalogues/goods', icon: 'dashboard' },
    { title: 'ProductTypes', description: 'Manage product types', route: '/catalogues/productTypes', icon: 'dashboard' },
    { title: 'Characteristics', description: 'Manage characteristics', route: '/catalogues/productCharacteristics', icon: 'dashboard' },
  ];

  constructor(public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef) {
  }
}
