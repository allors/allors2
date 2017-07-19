import { Component, AfterViewInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { TdMediaService } from '@covalent/core';

@Component({
  templateUrl: './dashboard.component.html',
})
export class DashboardComponent implements AfterViewInit {

  title='Dashboard';

  constructor(public media: TdMediaService, private titleService: Title) { }

  ngAfterViewInit(): void {
    this.titleService.setTitle(this.title);
    this.media.broadcast();
  }
}
