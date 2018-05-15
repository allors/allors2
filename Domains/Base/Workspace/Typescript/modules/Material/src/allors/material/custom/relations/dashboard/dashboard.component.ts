import { AfterViewInit, Component } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';

@Component({
  templateUrl: './dashboard.component.html',
})
export class DashboardComponent {

  public title: string;

  constructor(private titleService: Title) {
      this.title = 'Dashboard';
      this.titleService.setTitle(this.title);
  }
}
