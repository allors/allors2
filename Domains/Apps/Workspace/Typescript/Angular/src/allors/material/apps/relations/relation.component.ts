import { Component, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';
import { TdMediaService } from '@covalent/core';

@Component({
  templateUrl: './relation.component.html',
})
export class RelationComponent implements AfterViewInit {

  routes: Object[] = [{
      title: 'Organisations',
      route: 'organisations',
      icon: 'business',
    }, {
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
