import { Component, AfterViewInit, ViewChild, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BreakpointState, BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Title } from '@angular/platform-browser';
import { MatSidenav } from '@angular/material';

import { Observable, Subscription } from 'rxjs';
import { map } from 'rxjs/operators';

import { SideMenuItem, AllorsMaterialSideNavService } from '../../allors/material';
import { MenuService, WorkspaceService, Loaded, Scope } from '../../allors/angular';
import { MetaDomain } from '../../allors/meta';
import { Fetch, Query, Equals, PullRequest } from '../../allors/framework';
import { Organisation } from '../../allors/domain';

@Component({
  styleUrls: ["main.component.scss"],
  templateUrl: './main.component.html'
})
export class MainComponent implements OnInit, OnDestroy {

  sideMenuItems: SideMenuItem[] = [];

  m: MetaDomain;

  private subscription: Subscription;
  private toggleSubscription;

  private scope: Scope;
  
  @ViewChild('drawer') private sidenav: MatSidenav;

  private handsetSubscription: Subscription;

  constructor(
    private workspaceService: WorkspaceService,
    private breakpointObserver: BreakpointObserver,
    private titleService: Title,
    private menuService: MenuService,
    private sideNavService: AllorsMaterialSideNavService) {

    this.m = workspaceService.metaPopulation.metaDomain;
    this.scope = this.workspaceService.createScope(); 

    menuService.pagesByModule.forEach((pages, module) => {
      const sideMenuItem = {
        icon: module.icon,
        title: module.title,
        link: !module.children || module.children.length == 0 ? module.link : undefined,
        children: pages.map((page) => {
          return {
            title: page.title,
            link: page.link,
          };
        }),
      }

      this.sideMenuItems.push(sideMenuItem);
    });

    this.toggleSubscription = sideNavService.toggle$.subscribe(() => {
      if (this.sidenav) {
        this.sidenav.toggle();
      }
    })

    this.handsetSubscription = this.breakpointObserver.observe(Breakpoints.Handset)
      .pipe(
        map(result => result.matches)
      ).subscribe((result) => {
        if (this.sidenav) {
          if (result) {
            this.sidenav.close();
          } else {
            this.sidenav.open();
          }
        }
      });
  }

  public ngOnInit(): void {
  }

  ngOnDestroy(): void {
    this.handsetSubscription.unsubscribe;
  }

  isHandset$ = this.breakpointObserver.observe(Breakpoints.Handset)
    .pipe(
      map(result => result.matches)
    );
    
  get title(): string {
    return this.titleService.getTitle();
  }
}