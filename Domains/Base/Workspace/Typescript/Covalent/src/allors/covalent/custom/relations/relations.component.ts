import { Component, AfterViewInit } from '@angular/core';
import { TdMediaService } from '@covalent/core';

@Component({
  templateUrl: './relations.component.html',
})
export class RelationsComponent implements AfterViewInit {

 routes: Object[] = [{
    title: 'Dashboard',
    route: '/',
    icon: 'dashboard',
  }, {
    title: 'Relations',
    route: '/relations',
    icon: 'people',
  }];

  usermenu: Object[] = [{
    icon: 'tune',
    route: '.',
    title: 'Account settings',
  }, {
    icon: 'exit_to_app',
    route: '.',
    title: 'Sign out',
  },
  ];

  navmenu: Object[] = [
    { title: 'Organisations', description: 'Manage organisations', route: '/relations/organisations', icon: 'business center' },
    { title: 'People', description: 'Manage people', route: '/relations/people', icon: 'people' },
  ];

  constructor(public media: TdMediaService) { }

  ngAfterViewInit(): void {
    // broadcast to all listener observables when loading the page
    this.media.broadcast();
  }
}
