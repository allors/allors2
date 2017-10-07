import { Observable, BehaviorSubject, Subscription } from 'rxjs/Rx';
import { Component, OnInit, AfterViewInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { Validators } from '@angular/forms';
import { ActivatedRoute, UrlSegment } from '@angular/router';
import { MatSnackBar, MatSnackBarConfig } from '@angular/material';
import { TdMediaService } from '@covalent/core';

import { MetaDomain } from '../../../../meta/index';
import { PullRequest, PushResponse, Fetch, Path, Query, Equals, Like, TreeNode, Sort, Page } from '../../../../domain';
import { Locale, OrganisationRole, Organisation, CustomOrganisationClassification, IndustryClassification } from '../../../../domain';
import { AllorsService, ErrorService, Scope, Loaded, Saved } from '../../../../angular';

@Component({
  templateUrl: './form.component.html',
})
export class OrganisationEditComponent implements OnInit, AfterViewInit, OnDestroy {

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;

  private scope: Scope;

  title: string = 'Organisation';
  subTitle: string;

  m: MetaDomain;

  organisation: Organisation;

  locales: Locale[];
  roles: OrganisationRole[];
  classifications: CustomOrganisationClassification[];
  industries: IndustryClassification[];

  constructor(
    private allors: AllorsService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    public media: TdMediaService,
    private changeDetectorRef: ChangeDetectorRef) {

    this.scope = new Scope(allors.database, allors.workspace);
    this.m = this.allors.meta;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  ngOnInit(): void {
    const route$: Observable<UrlSegment[]> = this.route.url;

    const combined$: Observable<[UrlSegment[], Date]> = Observable.combineLatest(route$, this.refresh$);

    this.subscription = combined$
      .switchMap(([urlSegments, date]: [UrlSegment[], Date]) => {

        const id: string = this.route.snapshot.paramMap.get('id');

        const fetch: Fetch[] = [
          new Fetch({
            name: 'organisation',
            id: id,
          }),
        ];

        const query: Query[] = [
          new Query(
            {
              name: 'locales',
              objectType: this.m.Locale,
            }),
          new Query(
            {
              name: 'roles',
              objectType: this.m.OrganisationRole,
            }),
          new Query(
            {
              name: 'classifications',
              objectType: this.m.CustomOrganisationClassification,
            }),
          new Query(
            {
              name: 'industries',
              objectType: this.m.IndustryClassification,
            }),
        ];

        this.scope.session.reset();

        return this.scope
          .load('Pull', new PullRequest({ fetch: fetch, query: query }));
      })
      .subscribe((loaded: Loaded) => {

        this.subTitle = 'edit organisation';
        this.organisation = loaded.objects.organisation as Organisation;

        if (!this.organisation) {
          this.subTitle = 'add a new organisation';
          this.organisation = this.scope.session.create('Organisation') as Organisation;
        }

        this.locales = loaded.collections.locales as Locale[];
        this.roles = loaded.collections.roles as OrganisationRole[];
        this.classifications = loaded.collections.classifications as CustomOrganisationClassification[];
        this.industries = loaded.collections.industries as IndustryClassification[];
      },
      (error: any) => {
        this.errorService.message(error);
        this.goBack();
      },
    );
  }

  ngAfterViewInit(): void {
    this.media.broadcast();
    this.changeDetectorRef.detectChanges();
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  save(): void {

    this.scope
      .save()
      .subscribe((saved: Saved) => {
        this.goBack();
      },
      (error: Error) => {
        this.errorService.dialog(error);
      });
  }

  goBack(): void {
    window.history.back();
  }
}
