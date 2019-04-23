import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';

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
export class DashboardComponent implements OnInit {

  apps: Item[];

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
