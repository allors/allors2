import { Component } from '@angular/core';
import { TdMediaService } from '@covalent/core';

@Component({
  selector: 'relations-navigation',
  template: `
<md-nav-list>
  <ng-template let-item let-last="last" ngFor [ngForOf]="navmenu">
    <a md-list-item [routerLink]="[item.route]">
      <md-icon md-list-avatar>{{item.icon}}</md-icon>
      <h3 md-line> {{item.title}} </h3>
      <p md-line> {{item.description}} </p>
    </a>
    <md-divider md-inset *ngIf="!last"></md-divider>
  </ng-template>
</md-nav-list>
`,
})
export class RelationsNavigationComponent {

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
}
