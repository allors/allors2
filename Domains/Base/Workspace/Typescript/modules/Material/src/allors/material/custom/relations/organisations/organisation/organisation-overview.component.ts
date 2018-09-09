import { AfterViewInit, Component, OnDestroy, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';

import { Locale, Organisation } from '../../../../../domain';
import { PullRequest } from '../../../../../framework';
import { MetaDomain, PullFactory } from '../../../../../meta';

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

    const x = {};
    const pull = new PullFactory(this.workspaceService.metaPopulation);

    this.subscription = this.route.url
      .switchMap((url) => {

        const id: string = this.route.snapshot.paramMap.get('id');
        const m: MetaDomain = this.m;

        const pulls = [
          pull.Organisation({
            object: id,
            include: {
              Owner: x,
              Employees: x,
            }
          })
        ];

        this.scope.session.reset();

        return this.scope
          .load('Pull', new PullRequest({ pulls }));
      })
      .subscribe((loaded: Loaded) => {
        this.organisation = loaded.objects.Organisation as Organisation;
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
