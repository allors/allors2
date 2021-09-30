import { Component, ViewChild, OnDestroy, OnInit, Self } from '@angular/core';
import { MatSidenav } from '@angular/material/sidenav';
import { Subscription } from 'rxjs';
import { switchMap, filter } from 'rxjs/operators';

import { ContextService, AllorsBarcodeService, Loaded, MetaService } from '@allors/angular/services/core';
import { Organisation } from '@allors/domain/generated';
import { SideMenuItem } from '@allors/angular/material/core';
import { AllorsMaterialSideNavService } from '@allors/angular/material/services/core';
import { Router, NavigationEnd } from '@angular/router';
import { ObjectType } from 'libs/meta/system/src/ObjectType';
import { Equals } from '@allors/data/system';
import { PullRequest } from '@allors/protocol/system';
import { InternalOrganisationId } from '@allors/angular/base';
import { TestScope } from '@allors/angular/core';

import { menu } from './main.menu';


@Component({
  styleUrls: ['main.component.scss'],
  templateUrl: './main.component.html',
  providers: [ContextService]
})
export class MainComponent extends TestScope implements OnInit, OnDestroy {
  selectedInternalOrganisation: Organisation;
  internalOriganisations: Organisation[];

  sideMenuItems: SideMenuItem[] = [];

  private subscription: Subscription;
  private toggleSubscription;
  private openSubscription;
  private closeSubscription;

  @ViewChild('drawer', { static: true }) private sidenav: MatSidenav;

  constructor(
    @Self() public allors: ContextService,
    public metaService: MetaService,
    private router: Router,
    private sideNavService: AllorsMaterialSideNavService,
    private internalOrganisationId: InternalOrganisationId,
  ) {
    super();
  }

  public ngOnInit(): void {
    menu.forEach(menuItem => {
      const objectType = this.metaService.m.metaObjectById.get(menuItem.id) as ObjectType;

      const sideMenuItem: SideMenuItem = {
        icon: menuItem.icon || (objectType && objectType.icon),
        title: menuItem.title || (objectType && objectType.displayName),
        link: menuItem.link || (objectType && objectType.list),
        id: objectType && objectType.id,
        children:
          menuItem.children &&
          menuItem.children.map(childMenuItem => {
            const childObjectType = this.metaService.m.metaObjectById.get(childMenuItem.id) as ObjectType;
            return {
              icon:
                childMenuItem.icon || (childObjectType && childObjectType.icon),
              title:
                childMenuItem.title ||
                (childObjectType && childObjectType.displayName),
              link:
                childMenuItem.link || (childObjectType && childObjectType.list),
              id: childObjectType && childObjectType.id
            };
          })
      };

      this.sideMenuItems.push(sideMenuItem);
    });

    this.router.onSameUrlNavigation = 'reload';
    this.router.events
      .pipe(filter(v => v instanceof NavigationEnd))
      .subscribe(() => {
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

    const { m, pull } = this.metaService;

    this.subscription = this.internalOrganisationId.observable$
      .pipe(
        switchMap(internalOrganisationId => {
          const pulls = [
            pull.InternalOrganisation({
              object: internalOrganisationId
            }),
            pull.Organisation({
              predicate: new Equals({
                propertyType: m.Organisation.IsInternalOrganisation,
                value: true
              })
            })
          ];

          return this.allors.context.load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded: Loaded) => {
        this.allors.context.reset();
        this.internalOriganisations = loaded.collections
          .InternalOrganisations as Organisation[];
        this.selectedInternalOrganisation = loaded.objects
          .InternalOrganisation as Organisation;
      });
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
    this.toggleSubscription.unsubscribe();
    this.openSubscription.unsubscribe();
    this.closeSubscription.unsubscribe();
  }

  public toggle() {
    this.sideNavService.toggle();
  }
}
