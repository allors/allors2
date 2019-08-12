import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { TestScope } from '../../allors/angular';

interface Item {
  title: string;
  subtitle: string;
  icon: string;
  routerLink: string[];
}

@Component({
  styleUrls: ['./dashboard.component.scss'],
  templateUrl: './dashboard.component.html'
})
export class DashboardComponent extends TestScope implements OnInit {

  apps: Item[];

  constructor(
    private titleService: Title
  ) {
    super();

    this.titleService.setTitle('Dashboard');
  }

  ngOnInit(): void {
    this.apps = [{
      title: 'WorkOrder',
      subtitle: 'Manage workorder',
      icon: 'assignment',
      routerLink: ['/app/workorder'],
    },
    {
      title: 'Timesheet',
      subtitle: 'Manage time entries',
      icon: 'alarm',
      routerLink: ['/app/timesheet'],
    },
    ];
  }
}
