import { Component, AfterViewInit } from '@angular/core';
import { TdMediaService } from '@covalent/core';

@Component({
  template: `
<td-layout>
  <td-navigation-drawer flex sidenavTitle="Covalent" logo="assets:teradata" name="Firstname Lastname" email="firstname.lastname@company.com">
    <md-nav-list>
      <a *ngFor="let item of routes" md-list-item>
        <md-icon>{{item.icon}}</md-icon>{{item.title}}</a>
    </md-nav-list>
    <div td-navigation-drawer-menu>
      <md-nav-list>
        <a *ngFor="let item of usermenu" md-list-item>
          <md-icon>{{item.icon}}</md-icon>{{item.title}}</a>
      </md-nav-list>
    </div>
  </td-navigation-drawer>

  <router-outlet></router-outlet>

</td-layout>
`,
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
