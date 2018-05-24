import { Component, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { BreakpointState, BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Title } from '@angular/platform-browser';
import { map } from 'rxjs/operators';
import { SideMenuItem } from '../../allors/material';
import { MenuService } from '../../allors/angular';

@Component({
  styleUrls: ["main.component.scss"],
  templateUrl: './main.component.html'
})
export class MainComponent {

  sideMenuItems: SideMenuItem[] = [];

  isHandset$ = this.breakpointObserver.observe(Breakpoints.Handset)
    .pipe(
      map(result => result.matches)
    );

  constructor(private breakpointObserver: BreakpointObserver, private titleService: Title, private menuService: MenuService) {
    menuService.pagesByModule.forEach((pages, module) => {
      const sideMenuItem = {
        icon: module.icon,
        title: module.title,
        children: pages.map((page) => {
          return {
            title: page.title,
            link: page.link,
          };
        }),
      }
      
      this.sideMenuItems.push(sideMenuItem);
    });
  }

  get title(): string {
    return this.titleService.getTitle();
  }
}
