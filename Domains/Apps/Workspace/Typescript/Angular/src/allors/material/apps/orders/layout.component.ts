import { Component, Input , ChangeDetectorRef } from '@angular/core';
import { Router } from '@angular/router';
import { TdMediaService } from '@covalent/core';

@Component({
  templateUrl: './layout.component.html',
})
export class LayoutComponent {

  navmenu: any[] = [
    { title: 'Requests', description: 'Manage requests', route: '/orders/requests', icon: 'dashboard', },
  ];

  constructor(public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef) {
  }
}
