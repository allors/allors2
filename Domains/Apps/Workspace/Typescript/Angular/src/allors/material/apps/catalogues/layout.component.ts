import { Component, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';
import { TdMediaService } from '@covalent/core';

@Component({
  selector: 'a-td-layout-catalogue',
  templateUrl: './layout.component.html',
})
export class LayoutComponent implements AfterViewInit {

  routes: Object[] = [
    { title: 'Catalogues', route: '/catalogues/catalogues', icon: 'dashboard' },
    { title: 'Categories', route: '/catalogues/categories', icon: 'dashboard' },
    { title: 'Products', route: '/catalogues/goods', icon: 'dashboard' },
    { title: 'ProductTypes', route: '/catalogues/productTypes', icon: 'dashboard' },
    { title: 'Characteristics', route: '/catalogues/productCharacteristics', icon: 'dashboard' },
  ];

  constructor(public media: TdMediaService) {
  }

  ngAfterViewInit(): void {
    this.media.broadcast();
  }
}
