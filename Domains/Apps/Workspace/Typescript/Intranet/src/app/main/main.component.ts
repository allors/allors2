import { Component, AfterViewInit, ViewChild, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { BreakpointState, BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Title } from '@angular/platform-browser';
import { map } from 'rxjs/operators';
import { SideMenuItem, AllorsMaterialSideNavService } from '../../allors/material';
import { MenuService } from '../../allors/angular';
import { MatSidenav } from '@angular/material';

@Component({
  styleUrls: ["main.component.scss"],
  templateUrl: './main.component.html'
})
export class MainComponent implements OnDestroy {

  sideMenuItems: SideMenuItem[] = [];

  private toggleSubscription;
  @ViewChild('drawer') private sidenav: MatSidenav;

  private handsetSubscription: Subscription;

  constructor(private breakpointObserver: BreakpointObserver, private titleService: Title, private menuService: MenuService, private sideNavService: AllorsMaterialSideNavService) {
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

    this.toggleSubscription = sideNavService.toggle$.subscribe(() => {
      if (this.sidenav) {
        this.sidenav.toggle();
      }
    })

    this.handsetSubscription = this.breakpointObserver.observe(Breakpoints.Handset)
      .pipe(
        map(result => result.matches)
      ).subscribe((result) => {
        if(this.sidenav){
          if(result){
            this.sidenav.close();
          } else{
            this.sidenav.open();
          }
        }
      });
  }

  ngOnDestroy(): void {
    this.handsetSubscription.unsubscribe;
  }

  get title(): string {
    return this.titleService.getTitle();
  }
}
