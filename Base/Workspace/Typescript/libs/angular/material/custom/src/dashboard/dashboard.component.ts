import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { TestScope } from '@allors/angular/core';

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

  constructor(
    private titleService: Title
  ) {
    super();

    this.titleService.setTitle('Dashboard');
  }

  ngOnInit(): void {
  }
}
