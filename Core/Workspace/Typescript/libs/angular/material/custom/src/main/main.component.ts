import { Component, ViewChild, OnDestroy, OnInit, Self } from '@angular/core';
import { MatSidenav } from '@angular/material/sidenav';
import { Router, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';
import { Subscription } from 'rxjs';

import { ObjectType } from '@allors/meta/system';
import { SideMenuItem } from '@allors/angular/material/core';
import { Organisation } from '@allors/domain/generated';

import { menu } from './main.menu';
import { ContextService, MetaService } from '@allors/angular/services/core';
import { AllorsMaterialSideNavService } from '@allors/angular/material/services/core';

@Component({
  styleUrls: ['main.component.scss'],
  templateUrl: './main.component.html',
  providers: [ContextService]
})
export class MainComponent implements OnInit, OnDestroy {

  selectedInternalOrganisation: Organisation;
  internalOriganisations: Organisation[];

  sideMenuItems: SideMenuItem[] = [];

  private toggleSubscription: Subscription;
  private openSubscription: Subscription;
  private closeSubscription: Subscription;

  @ViewChild('drawer', { static: true }) private sidenav: MatSidenav;

  constructor(
    @Self() private allors: ContextService,
    public metaService: MetaService,
    private router: Router,
    private sideNavService: AllorsMaterialSideNavService
  ) { }

  public ngOnInit(): void {

    menu.forEach((menuItem) => {
      const objectType = menuItem.id ? this.metaService.m.metaObjectById.get(menuItem.id) as ObjectType : null;

      const sideMenuItem: SideMenuItem = {
        icon: menuItem.icon ?? objectType?.icon,
        title: menuItem.title ?? objectType?.displayName,
        link: menuItem.link ?? objectType?.list,
        children: menuItem.children && menuItem.children.map((childMenuItem) => {

          const childObjectType = childMenuItem.id ? this.metaService.m.metaObjectById.get(childMenuItem.id) as ObjectType : null;
          return {
            icon: childMenuItem.icon ?? childObjectType?.icon,
            title: childMenuItem.title ?? childObjectType?.displayName,
            link: childMenuItem.link ?? childObjectType?.list,
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
