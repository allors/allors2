import { Component, Input, AfterViewInit } from '@angular/core';
import { TdMediaService } from '@covalent/core';

@Component({
  templateUrl: './layout.component.html',
})
export class LayoutComponent {

   navmenu = [
    { title: 'Organisations', description: 'Manage organisations', route: '/relations/organisations', icon: 'business center' },
    { title: 'People', description: 'Manage people', route: '/relations/people', icon: 'people' },
  ];

  constructor(public media: TdMediaService) { }
}
