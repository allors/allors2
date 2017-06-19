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
      icon: 'catalogues',
    }, {
      title: 'Categories',
      route: 'categories',
      icon: 'categories',
    }, {
      title: 'ProductTypes',
      route: 'productTypes',
      icon: 'productTypes',
    }, {
      title: 'Products',
      route: 'goods',
      icon: 'products',
    },
  ];

  constructor(public media: TdMediaService) {
  }

  ngAfterViewInit(): void {
    this.media.broadcast();
  }
}
