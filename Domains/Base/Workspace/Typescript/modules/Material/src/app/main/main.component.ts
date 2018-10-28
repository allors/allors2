import { Component, ViewChild, OnDestroy, OnInit, Self } from '@angular/core';
import { MatSidenav } from '@angular/material';

import { filter } from 'rxjs/operators';

import { SideMenuItem, AllorsMaterialSideNavService } from '../../allors/material';
import { MenuService, Allors } from '../../allors/angular';
import { Organisation } from '../../allors/domain';
import { Router, NavigationEnd } from '@angular/router';

@Component({
  styleUrls: ['main.component.scss'],
  templateUrl: './main.component.html',
  providers: [Allors]
})
export class MainComponent implements OnInit, OnDestroy {

  selectedInternalOrganisation: Organisation;
  internalOriganisations: Organisation[];

  sideMenuItems: SideMenuItem[] = [];

  private toggleSubscription;
  private openSubscription;
  private closeSubscription;

  @ViewChild('drawer') private sidenav: MatSidenav;

  constructor(
    @Self() private allors: Allors,
    private router: Router,
    private sideNavService: AllorsMaterialSideNavService,
    private menuService: MenuService,
  ) { }

  public ngOnInit(): void {

    this.menuService.pagesByModule.forEach((pages, module) => {
      const sideMenuItem = {
        icon: module.icon,
        title: module.title,
        link: !module.children || module.children.length === 0 ? module.link : undefined,
        children: pages.map((page) => {
          return {
            title: page.title,
            link: page.link,
          };
        }),
      };

      this.sideMenuItems.push(sideMenuItem);
    });

    this.router.onSameUrlNavigation = 'reload';
    this.router.events
      .pipe(
        filter((v) => v instanceof NavigationEnd)
      ).subscribe(() => {
        if (this.sidenav) {
          this.sidenav.close();
        }
      });

    this.toggleSubscription = this.sideNavService.toggle$.subscribe(() => {
      this.sidenav.toggle();
    });

    this.openSubscription = this.sideNavService.open$.subscribe(() => {
      this.sidenav.open();
    });

    this.closeSubscription = this.sideNavService.close$.subscribe(() => {
      this.sidenav.close();
    });
  }

  ngOnDestroy(): void {
    this.toggleSubscription.unsubscribe();
    this.openSubscription.unsubscribe();
    this.closeSubscription.unsubscribe();
  }

  public toggle() {
    this.sideNavService.toggle();
  }
}
