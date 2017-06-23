import { Component, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';
import { TdMediaService } from '@covalent/core';

@Component({
  templateUrl: './catalogue.component.html',
})
export class CatalogueComponent implements AfterViewInit {

  routes: Object[] = [{
      title: 'Dashboard',
      route: 'dashboard',
      icon: 'dashboard',
    }, {
      title: 'Catalogues',
      route: 'catalogues',
      icon: 'dashboard',
    }, {
      title: 'Categories',
      route: 'categories',
      icon: 'dashboard',
    }, {
      title: 'ProductTypes',
      route: 'productTypes',
      icon: 'dashboard',
    }, {
      title: 'Products',
      route: 'goods',
      icon: 'dashboard',
    },
  ];

  constructor(public media: TdMediaService) {
  }

  ngAfterViewInit(): void {
    this.media.broadcast();
  }
}
