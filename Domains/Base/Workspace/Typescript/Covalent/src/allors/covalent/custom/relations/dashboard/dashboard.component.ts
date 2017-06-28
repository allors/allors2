import { Component, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';
import { TdMediaService } from '@covalent/core';

@Component({
  templateUrl: './dashboard.component.html',
})
export class DashboardComponent implements AfterViewInit {

  routes: Object[] = [{
    title: 'People',
    route: 'people',
    icon: 'people',
  },
  ];

  constructor(public media: TdMediaService) {
  }

  ngAfterViewInit(): void {
    this.media.broadcast();
  }
}
