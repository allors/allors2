import { AfterViewInit, Component, OnDestroy, OnInit } from '@angular/core';
import { Validators } from '@angular/forms';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';

import { BehaviorSubject, combineLatest, Observable, Subject, Subscription } from 'rxjs';

import { ErrorService, Field, FilterFactory, Invoked, Loaded, Saved, Scope, WorkspaceService } from '../../../../angular';
import { Enumeration, Locale, Organisation, Person } from '../../../../domain';
import { And, Equals, Fetch, Like, Or, Page, Path, PullRequest, PushResponse,
         Query, RoleType, Sort, TreeNode } from '../../../../framework';
import { MetaDomain } from '../../../../meta';

@Component({
  templateUrl: './form.component.html',
})
export class FormComponent implements OnInit, AfterViewInit, OnDestroy {

  public title: string;

  public m: MetaDomain;

  public peopleFilter: FilterFactory;

  public people: Person[];
  public organisation: Organisation;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private titleService: Title,
    private router: Router,
    private route: ActivatedRoute,
  ) {

    this.title = 'Form';
    this.titleService.setTitle(this.title);
    this.scope = this.workspaceService.createScope();
    this.m = this.workspaceService.metaPopulation.metaDomain;

    this.peopleFilter = new FilterFactory({objectType: this.m.Person, roleTypes: [this.m.Person.FirstName, this.m.Person.LastName]});

    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  public ngOnInit(): void {
    const route$: Observable<UrlSegment[]> = this.route.url;
    const combined$: Observable<[UrlSegment[], Date]> = combineLatest(route$, this.refresh$);

    this.subscription = combined$
        .switchMap(([urlSegments, date]: [UrlSegment[], Date]) => {

        const fetches = [
        ];

        const queries = [
          new Query(
            {
              name: 'people',
              objectType: this.m.Person,
            }),
        ];

        return this.scope
          .load('Pull', new PullRequest({ fetches, queries }));
      })
      .subscribe((loaded: Loaded) => {

        this.scope.session.reset();

        this.people = loaded.collections.people as Person[];

        this.organisation = this.scope.session.create("Organisation") as Organisation;
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

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public save(): void {

    this.scope
      .save()
      .subscribe((saved: Saved) => {
        this.goBack();
      },
      (error: Error) => {
        this.errorService.handle(error);
      });
  }

  public goBack(): void {
  }
}
