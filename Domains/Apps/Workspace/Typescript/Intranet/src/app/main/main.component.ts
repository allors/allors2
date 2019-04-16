import { Component, ViewChild, OnDestroy, OnInit, Self } from '@angular/core';
import { MatSidenav, MatExpansionPanelDescription } from '@angular/material';

import { Subscription } from 'rxjs';
import { switchMap, filter } from 'rxjs/operators';

import { SideMenuItem, AllorsMaterialSideNavService } from '../../allors/material';
import { Loaded, ContextService, MetaService, AllorsBarcodeService, InternalOrganisationId } from '../../allors/angular';
import { Equals, PullRequest, ObjectType } from '../../allors/framework';
import { Organisation } from '../../allors/domain';
import { Router, NavigationEnd } from '@angular/router';

import { menu } from './main.menu';
import { environment } from '../../environments/environment';

@Component({
  styleUrls: ['main.component.scss'],
  templateUrl: './main.component.html',
  providers: [ContextService]
})
export class MainComponent implements OnInit, OnDestroy {
  selectedInternalOrganisation: Organisation;
  internalOriganisations: Organisation[];

  sideMenuItems: SideMenuItem[] = [];

  isProduction = environment.production;

  private subscription: Subscription;
  private toggleSubscription;
  private openSubscription;
  private closeSubscription;

  @ViewChild('drawer') private sidenav: MatSidenav;

  constructor(
    @Self() private allors: ContextService,
    public metaService: MetaService,
    private barcodeService: AllorsBarcodeService,
    private router: Router,
    private sideNavService: AllorsMaterialSideNavService,
    private internalOrganisationId: InternalOrganisationId,
  ) { }

  public ngOnInit(): void {
    menu.forEach(menuItem => {
      const objectType = this.metaService.m.metaObjectById[
        menuItem.id
      ] as ObjectType;

      const sideMenuItem: SideMenuItem = {
        icon: menuItem.icon || (objectType && objectType.icon),
        title: menuItem.title || (objectType && objectType.displayName),
        link: menuItem.link || (objectType && objectType.list),
        id: objectType && objectType.id,
        children:
          menuItem.children &&
          menuItem.children.map(childMenuItem => {
            const childObjectType = this.metaService.m.metaObjectById[
              childMenuItem.id
            ] as ObjectType;
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

    const { m, pull, x } = this.metaService;

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

          return this.allors.context.load('Pull', new PullRequest({ pulls }));
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
