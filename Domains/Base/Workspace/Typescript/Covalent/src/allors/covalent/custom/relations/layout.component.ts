import { Component, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';
import { TdMediaService } from '@covalent/core';

@Component({
  selector: 'a-td-layout-relation',
  templateUrl: './layout.component.html',
})
export class LayoutComponent implements AfterViewInit {

  routes: Object[] = [
    { title: 'Organisations', route: '/relations/organisations', icon: 'business center' },
    { title: 'People', route: '/relations/people', icon: 'people' },
  ];

  constructor(public media: TdMediaService) {
  }

  ngAfterViewInit(): void {
    this.media.broadcast();
  }
}
