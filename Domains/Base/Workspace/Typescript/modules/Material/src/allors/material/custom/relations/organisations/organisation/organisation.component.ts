import { AfterViewInit, Component, OnDestroy, OnInit } from '@angular/core';
import { Validators } from '@angular/forms';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';

import { BehaviorSubject, combineLatest, Observable, Subject, Subscription } from 'rxjs';

import { ErrorService, Field, FilterFactory, Invoked, Loaded, Saved, Scope, WorkspaceService } from '../../../../../angular';
import { Enumeration, Locale, Organisation, Person } from '../../../../../domain';
import { And, Equals, Fetch, Like, Or, Page, Path, PullRequest, PushResponse,
         Query, RoleType, Sort, TreeNode } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';

@Component({
  templateUrl: './organisation.component.html',
})
export class OrganisationComponent implements OnInit, AfterViewInit, OnDestroy {

  public title: string;

  public field: Field;

  public m: MetaDomain;
  public people: Person[];

  public organisation: Organisation;

  public peopleFilter: FilterFactory;

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

    this.title = 'Organisation';
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

        const id: string = this.route.snapshot.paramMap.get('id');

        const fetches = [
          new Fetch({
            id,
            name: 'organisation',
          }),
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

        this.organisation = loaded.objects.organisation as Organisation;
        if (!this.organisation) {
          this.organisation = this.scope.session.create('Organisation') as Organisation;
        }

        this.people = loaded.collections.people as Person[];
      },
      (error: any) => {
        this.errorService.message(error);
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

  public toggleCanWrite() {
    this.scope
    .invoke(this.organisation.ToggleCanWrite)
    .subscribe((invoded: Invoked) => {
      this.refresh();
    },
    (error: Error) => {
      this.errorService.dialog(error);
    });
  }

  public save(): void {

    this.scope
      .save()
      .subscribe((saved: Saved) => {
        this.goBack();
      },
      (error: Error) => {
        this.errorService.dialog(error);
      });
  }

  public goBack(): void {
    window.history.back();
  }

  public ownerSelected(field: Field): void {
    this.field = field;
  }
}
