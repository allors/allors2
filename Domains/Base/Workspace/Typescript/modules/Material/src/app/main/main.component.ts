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
      title: 'Home', icon: 'home', link: '/dashboard',
    },
    {
      title: 'Relations', icon: 'share',
      children: [
        { title: 'Organisations', link: '/relations/organisations' },
        { title: 'People', link: '/relations/people' }
      ]
    },
    {
      title: 'Relations 2', icon: 'people',
      children: [
        { title: 'Organisations',  link: '/relations/organisations' },
        { title: 'People', link: '/relations/people' }
      ]
    },
    {
      title: 'Relations 3', icon: 'business',
      children: [
        { title: 'Organisations', link: '/relations/organisations' },
        { title: 'People', link: '/relations/people' }
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
