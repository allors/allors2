import { Component, AfterViewInit, ViewChild, OnDestroy, OnInit } from '@angular/core';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Title } from '@angular/platform-browser';
import { MatSidenav } from '@angular/material';

import { Subscription } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';

import { SideMenuItem, AllorsMaterialSideNavService } from '../../allors/material';
import { MenuService, WorkspaceService, Loaded, Scope, DataService } from '../../allors/angular';
import { MetaDomain } from '../../allors/meta';
import { Fetch, Equals, PullRequest } from '../../allors/framework';
import { StateService } from '../../allors/material/apps/services/StateService';
import { Organisation } from '../../allors/domain';

@Component({
  styleUrls: ['main.component.scss'],
  templateUrl: './main.component.html'
})
export class MainComponent implements OnInit, OnDestroy {


  constructor(
    private workspaceService: WorkspaceService,
    private dataService: DataService,
    private stateService: StateService,
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

    this.toggleSubscription = sideNavService.toggle$.subscribe(() => {
      if (this.sidenav) {
        this.sidenav.toggle();
      }
    });

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

  get title(): string {
    return this.titleService.getTitle();
  }

  selectedInternalOrganisation: Organisation;
  internalOriganisations: Organisation[];

  sideMenuItems: SideMenuItem[] = [];

  m: MetaDomain;

  private subscription: Subscription;
  private toggleSubscription;

  private scope: Scope;

  @ViewChild('drawer') private sidenav: MatSidenav;

  private handsetSubscription: Subscription;

  isHandset$ = this.breakpointObserver
    .observe(Breakpoints.Handset)
    .pipe(
      map(result => result.matches)
    );

  public ngOnInit(): void {

    const { m, pull } = this.dataService;

    this.subscription = this.stateService.internalOrganisationId$
      .pipe(
        switchMap((internalOrganisationId) => {

          const pulls = [
            pull.InternalOrganisation({
              object: internalOrganisationId,
            }),
            pull.Organisation({
              predicate: new Equals({ propertyType: this.m.Organisation.IsInternalOrganisation, value: true }),
            })
          ];

          return this.scope
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded: Loaded) => {
        this.scope.session.reset();
        this.internalOriganisations = loaded.collections.internalOrganisations as Organisation[];
        this.selectedInternalOrganisation = loaded.objects.internalOrganisation as Organisation;
      });
  }

  ngOnDestroy(): void {
    this.handsetSubscription.unsubscribe();
  }
}
