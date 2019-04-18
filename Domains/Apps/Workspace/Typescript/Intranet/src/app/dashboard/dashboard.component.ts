import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';

@Component({
  styleUrls: ['./dashboard.component.scss'],
  templateUrl: './dashboard.component.html'
})
export class DashboardComponent implements OnInit {

  apps: any[];

  constructor(private titleService: Title) {
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
