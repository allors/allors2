import { Component, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { BreakpointState, BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Title } from '@angular/platform-browser';

@Component({
  templateUrl: './main.component.html'
})
export class MainComponent {

  routes = [
    { title: 'Relations', icon: 'share',
          children: [
            {title: 'Organisations', icon: 'business_center', link: '/relations/organisations'},
            {title: 'People', icon: 'people', link: '/relations/people'}
          ]}
  ];

  isHandset: Observable<BreakpointState> = this.breakpointObserver.observe(Breakpoints.Handset);
  constructor(private breakpointObserver: BreakpointObserver, private titleService: Title) {}

  get title(): string{
    return this.titleService.getTitle();
  }
}
