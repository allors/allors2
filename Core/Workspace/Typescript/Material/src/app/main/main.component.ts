import { Component, ViewChild, OnDestroy, OnInit, Self } from '@angular/core';
import { MatSidenav } from '@angular/material/sidenav';

import { filter } from 'rxjs/operators';

import { ObjectType } from '../../allors/framework';
import { SideMenuItem, AllorsMaterialSideNavService } from '../../allors/material';
import { ContextService, MetaService } from '../../allors/angular';
import { Organisation } from '../../allors/domain';
import { Router, NavigationEnd } from '@angular/router';
import { menu } from './main.menu';

@Component({
  styleUrls: ['main.component.scss'],
  templateUrl: './main.component.html',
  providers: [ContextService]
})
export class MainComponent implements OnInit, OnDestroy {

  selectedInternalOrganisation: Organisation;
  internalOriganisations: Organisation[];

  sideMenuItems: SideMenuItem[] = [];

  private toggleSubscription;
  private openSubscription;
  private closeSubscription;

  @ViewChild('drawer', { static: true }) private sidenav: MatSidenav;

  constructor(
    @Self() private allors: ContextService,
    public metaService: MetaService,
    private router: Router,
    private sideNavService: AllorsMaterialSideNavService
  ) { }

  public ngOnInit(): void {

    menu.forEach((menuItem) => {
      const objectType = this.metaService.m.metaObjectById.get(menuItem.id) as ObjectType;

      const sideMenuItem: SideMenuItem = {
        icon: menuItem.icon || objectType && objectType.icon,
        title: menuItem.title || objectType && objectType.displayName,
        link: menuItem.link || objectType && objectType.list,
        children: menuItem.children && menuItem.children.map((childMenuItem) => {

          const childObjectType = this.metaService.m.metaObjectById.get(childMenuItem.id) as ObjectType;
          return {
            icon: childMenuItem.icon || childObjectType && childObjectType.icon,
            title: childMenuItem.title || childObjectType && childObjectType.displayName,
            link: childMenuItem.link || childObjectType && childObjectType.list,
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
