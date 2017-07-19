import { Component, Input, AfterViewInit } from '@angular/core';
import { TdMediaService } from '@covalent/core';

@Component({
  selector: 'relations',
  templateUrl: './relations.component.html',
})
export class RelationsComponent {

 routes = [
   { title: 'Dashboard', route: '/', icon: 'dashboard', },
   { title: 'Relations', route: '/relations', icon: 'people', }];

  usermenu = [
    { icon: 'tune', route: '.', title: 'Account settings', },
    { icon: 'exit_to_app', route: '.', title: 'Sign out', },
  ];

  navmenu = [
    { title: 'Organisations', description: 'Manage organisations', route: '/relations/organisations', icon: 'business center' },
    { title: 'People', description: 'Manage people', route: '/relations/people', icon: 'people' },
  ];

  @Input()
  title: string;

  constructor(public media: TdMediaService) { }
}
