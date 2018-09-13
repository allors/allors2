import { AfterViewInit, Component, OnDestroy, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, UrlSegment } from '@angular/router';

import { BehaviorSubject, Observable, Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { ErrorService, Field, SearchFactory, Invoked, Loaded, Saved, Scope, WorkspaceService } from '../../../../../angular';
import { Organisation, Person } from '../../../../../domain';
import { PullRequest } from '../../../../../framework';
import { MetaDomain, PullFactory } from '../../../../../meta';

@Component({
  templateUrl: './organisation.component.html',
})
export class OrganisationComponent implements OnInit, AfterViewInit, OnDestroy {

  public title: string;

  public field: Field;

  public m: MetaDomain;
  public people: Person[];

  public organisation: Organisation;

  public peopleFilter: SearchFactory;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private titleService: Title,
    private route: ActivatedRoute,
  ) {

    this.title = 'Organisation';
    this.titleService.setTitle(this.title);
    this.scope = this.workspaceService.createScope();
    this.m = this.workspaceService.metaPopulation.metaDomain;

    this.peopleFilter = new SearchFactory({ objectType: this.m.Person, roleTypes: [this.m.Person.FirstName, this.m.Person.LastName] });

    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  public ngOnInit(): void {
    const route$: Observable<UrlSegment[]> = this.route.url;
    const combined$: Observable<[UrlSegment[], Date]> = combineLatest(route$, this.refresh$);

    const pull = new PullFactory(this.workspaceService.metaPopulation);

    this.subscription = combined$
      .pipe(
        switchMap(([]: [UrlSegment[], Date]) => {

          const id: string = this.route.snapshot.paramMap.get('id');

          const pulls = [
            pull.Organisation({
              object: id
            }),
            pull.Person()
          ];

          return this.scope
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded: Loaded) => {

        this.scope.session.reset();

        this.organisation = loaded.objects.Organisation as Organisation || this.scope.session.create('Organisation') as Organisation;
        this.people = loaded.collections.People as Person[];
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

  public toggleCanWrite() {
    this.scope
      .invoke(this.organisation.ToggleCanWrite)
      .subscribe(() => {
        this.refresh();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public save(): void {

    this.scope
      .save()
      .subscribe(() => {
        this.goBack();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public goBack(): void {
    window.history.back();
  }

  public ownerSelected(field: Field): void {
    this.field = field;
  }
}
