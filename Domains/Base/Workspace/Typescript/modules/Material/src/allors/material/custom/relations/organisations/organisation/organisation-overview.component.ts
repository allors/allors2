import { AfterViewInit, Component, OnDestroy, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, UrlSegment } from '@angular/router';
import { Observable, Subject, Subscription } from 'rxjs';

import { Locale, Organisation, Person } from '../../../../../domain';
import { Equals, Fetch, Like, Page, Path, PullRequest, Query, Sort, TreeNode } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';

import { ErrorService, Loaded, Scope, WorkspaceService } from '../../../../../angular';

@Component({
  templateUrl: './organisation-overview.component.html',
})
export class OrganisationOverviewComponent implements OnInit, AfterViewInit, OnDestroy {

  public title: string;

  public m: MetaDomain;

  public organisation: Organisation;
  public locales: Locale[];

  private subscription: Subscription;
  private scope: Scope;

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private titleService: Title,
    private route: ActivatedRoute) {

    this.title = 'Organisation Overview';
    this.titleService.setTitle(this.title);
    this.scope = this.workspaceService.createScope();
    this.m = this.workspaceService.metaPopulation.metaDomain;
  }

  public ngOnInit(): void {

    this.subscription = this.route.url
      .switchMap((url) => {

        const id: string = this.route.snapshot.paramMap.get('id');
        const m: MetaDomain = this.m;

        const fetches = [
          new Fetch({
            id,
            include: [
              new TreeNode({ roleType: m.Organisation.Owner }),
              new TreeNode({ roleType: m.Organisation.Employees }),
            ],
            name: 'organisation',
          }),
        ];

        this.scope.session.reset();

        return this.scope
          .load('Pull', new PullRequest({ fetches }));
      })
      .subscribe((loaded: Loaded) => {
        this.organisation = loaded.objects.organisation as Organisation;
      },
      (error: any) => {
        this.errorService.handle(error);
        this.goBack();
      },
    );
  }

  public ngAfterViewInit(): void {
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public goBack(): void {
    window.history.back();
  }

  public checkType(obj: any): string {
    return obj.objectType.name;
  }
}
