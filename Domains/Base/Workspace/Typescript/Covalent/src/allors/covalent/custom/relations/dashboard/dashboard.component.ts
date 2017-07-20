import { Component, AfterViewInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { TdMediaService } from '@covalent/core';

@Component({
  templateUrl: './dashboard.component.html',
})
export class DashboardComponent {

  title: string;

  constructor(public media: TdMediaService, private titleService: Title) {

      this.title = 'Dashboard';
      this.titleService.setTitle(this.title);
  }
}
