import { AfterViewInit, Component, OnDestroy, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';

import { BehaviorSubject, combineLatest, Observable, Subject, Subscription } from 'rxjs';

import { ErrorService, Field, FilterFactory, Invoked, Loaded, Saved, Scope, WorkspaceService } from '../../../../angular';
import { Enumeration, Locale, Organisation, Person, Data } from '../../../../domain';
import { And, Equals, Fetch, Like, Or, Page, Path, PullRequest, PushResponse,
         Query, RoleType, Sort, TreeNode } from '../../../../framework';
import { MetaDomain } from '../../../../meta';
import { QueryFactory } from '../../../../meta/generated/query.g';

@Component({
  templateUrl: './form.component.html',
})
export class FormComponent implements OnInit, AfterViewInit, OnDestroy {

  public title: string;

  public m: MetaDomain;

  public data: Data;

  public people: Person[];

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

    const metaPopulation = this.workspaceService.metaPopulation;
    const query = new QueryFactory(metaPopulation);

    this.subscription = combined$
        .switchMap(([urlSegments, date]: [UrlSegment[], Date]) => {

        const fetches = [
        ];

        const queries = [
          query.Datas(),
          query.People(),
        ];

        return this.scope
          .load('Pull', new PullRequest({ fetches, queries }));
      })
      .subscribe((loaded: Loaded) => {

        this.scope.session.reset();

        this.people = loaded.collections.People as Person[];
        var datas = loaded.collections.Datas as Data[];
        
        if(datas && datas.length > 0){
          this.data = datas[0];
        } else{
          this.data = this.scope.session.create("Data") as Data;
        }
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
        this.data = undefined;
        this.refresh();
      },
      (error: Error) => {
        this.errorService.handle(error);
      });
  }

  public goBack(): void {
  }
}
