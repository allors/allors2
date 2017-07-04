import { Component, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';
import { TdMediaService } from '@covalent/core';

@Component({
  selector: 'a-td-layout-order',
  templateUrl: './layout.component.html',
})
export class LayoutComponent implements AfterViewInit {

  routes: Object[] = [
    { title: 'Requests', route: '/orders/requests', icon: 'dashboard', },
  ];

  constructor(public media: TdMediaService) {
  }

  ngAfterViewInit(): void {
    this.media.broadcast();
  }
}
