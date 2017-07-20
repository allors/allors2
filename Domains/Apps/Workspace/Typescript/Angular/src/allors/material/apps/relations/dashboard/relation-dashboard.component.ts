import { Component, AfterViewInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { TdMediaService } from '@covalent/core';

@Component({
  templateUrl: './relation-dashboard.component.html',
})
export class RelationDashboardComponent implements AfterViewInit {
  title: string = 'Relations Dashboard';

  constructor(public media: TdMediaService, private titleService: Title) {
      this.titleService.setTitle(this.title);
  }

  ngAfterViewInit(): void {
      this.media.broadcast();
  }
}
