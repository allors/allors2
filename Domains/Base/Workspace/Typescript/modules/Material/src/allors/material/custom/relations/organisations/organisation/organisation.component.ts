import { AfterViewInit, Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, UrlSegment } from '@angular/router';

import { BehaviorSubject, Observable, Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { ErrorService, Field, SearchFactory, Loaded, Saved, WorkspaceService, ContextService, MetaService } from '../../../../../angular';
import { Organisation, Person } from '../../../../../domain';
import { PullRequest } from '../../../../../framework';
import { Meta } from '../../../../../meta';

@Component({
  templateUrl: './organisation.component.html',
  providers: [ContextService]
})
export class OrganisationComponent implements OnInit, AfterViewInit, OnDestroy {

  title: string;

  field: Field;

  m: Meta;
  people: Person[];

  organisation: Organisation;

  peopleFilter: SearchFactory;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;

  constructor(
    @Self() private allors: ContextService,
    private metaService: MetaService,
    private errorService: ErrorService,
    private titleService: Title,
    private route: ActivatedRoute,
  ) {

    this.title = 'Organisation';
    this.titleService.setTitle(this.title);

    this.m = this.metaService.m;

    this.peopleFilter = new SearchFactory({ objectType: this.m.Person, roleTypes: [this.m.Person.UserName] });

    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  public ngOnInit(): void {
    const route$: Observable<UrlSegment[]> = this.route.url;
    const combined$: Observable<[UrlSegment[], Date]> = combineLatest(route$, this.refresh$);

    const { pull } = this.metaService;

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

          return this.allors.context
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded: Loaded) => {

        this.allors.context.reset();

        this.organisation = loaded.objects.Organisation as Organisation || this.allors.context.create('Organisation') as Organisation;
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
    this.allors.context
      .invoke(this.organisation.ToggleCanWrite)
      .subscribe(() => {
        this.refresh();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public save(): void {

    this.allors.context
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
