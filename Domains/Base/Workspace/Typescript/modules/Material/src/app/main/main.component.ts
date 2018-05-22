import { Component, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { BreakpointState, BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Title } from '@angular/platform-browser';
import { map } from 'rxjs/operators';
import { SideMenuItem } from '../../allors/material';

@Component({
  styleUrls: ['./main.component.scss'],
  templateUrl: './main.component.html'
})
export class MainComponent {

  sideMenuItems: SideMenuItem[] = [
    {
      title: 'Relations', icon: 'share',
      children: [
        { title: 'Organisations', icon: 'business_center', link: '/relations/organisations' },
        { title: 'People', icon: 'people', link: '/relations/people' }
      ]
    }
  ];

  isHandset$ = this.breakpointObserver.observe(Breakpoints.Handset)
    .pipe(
      map(result => result.matches)
    );

  constructor(private breakpointObserver: BreakpointObserver, private titleService: Title) { }

  get title(): string {
    return this.titleService.getTitle();
  }
}
