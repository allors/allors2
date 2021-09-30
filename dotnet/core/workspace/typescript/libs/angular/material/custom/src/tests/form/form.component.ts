import { AfterViewInit, Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, UrlSegment } from '@angular/router';

import { BehaviorSubject, combineLatest, Observable, Subscription } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { DateAdapter } from '@angular/material/core';
import { ContextService, MetaService, WorkspaceService, Loaded } from '@allors/angular/services/core';
import { Organisation, Person, Data, Locale } from '@allors/domain/generated';
import { RadioGroupOption } from '@allors/angular/material/core';
import { PullRequest } from '@allors/protocol/system';
import { Meta } from '@allors/meta/generated';
import { TestScope, SearchFactory } from '@allors/angular/core';
import { SaveService } from '@allors/angular/material/services/core';

@Component({
  templateUrl: './form.component.html',
  providers: [ContextService],
})
export class FormComponent extends TestScope implements OnInit, AfterViewInit, OnDestroy {
  title: string;
  m: Meta;

  organisations: Organisation[];
  people: Person[];
  locale: Locale;

  jane: Person | undefined;

  organisationFilter: SearchFactory;
  peopleFilter: SearchFactory;

  data: Data | null;

  radioGroupOptions: RadioGroupOption[] = [
    { label: 'One', value: 'one' },
    { label: 'Two', value: 'two' },
    { label: 'Three', value: 'three' },
  ];

  get organisationsWithManagers(): Organisation[] {
    return this.organisations?.filter((v) => v.Manager);
  }

  get organisationsWithEmployees(): Organisation[] {
    return this.organisations?.filter((v) => v.Employees?.length > 0);
  }

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    public metaService: MetaService,
    private workspaceService: WorkspaceService,
    private titleService: Title,
    private route: ActivatedRoute,
    private saveService: SaveService,
    private dateAdapter: DateAdapter<string>
  ) {
    super();

    this.title = 'Form';
    this.titleService.setTitle(this.title);

    this.m = this.metaService.m;

    this.organisationFilter = new SearchFactory({
      objectType: this.m.Organisation,
      roleTypes: [this.m.Organisation.Name],
    });
    this.peopleFilter = new SearchFactory({
      objectType: this.m.Person,
      roleTypes: [this.m.Person.FirstName, this.m.Person.LastName, this.m.Person.UserName],
    });

    this.refresh$ = new BehaviorSubject<Date>(new Date());
  }

  ngOnInit(): void {
    const route$: Observable<UrlSegment[]> = this.route.url;
    const combined$: Observable<[UrlSegment[], Date]> = combineLatest(route$, this.refresh$);

    const { m, pull, x } = this.metaService;

    this.subscription = combined$
      .pipe(
        switchMap(([]: [UrlSegment[], Date]) => {
          const pulls = [
            pull.Data({
              include: {
                AutocompleteFilter: x,
                AutocompleteOptions: x,
                Chips: x,
                File: x,
                MultipleFiles: x,
              },
            }),
            pull.Organisation({
              include: {
                OneData: x,
                ManyDatas: x,
                Owner: x,
                Employees: x,
              },
            }),
            pull.Person(),
            pull.Locale({
              include: {
                Language: x,
                Country: x,
              },
            }),
          ];

          return this.allors.context.load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded: Loaded) => {
        this.allors.context.reset();

        this.organisations = loaded.collections.Organisations as Organisation[];
        this.people = loaded.collections.People as Person[];
        const datas = loaded.collections.Datas as Data[];

        this.locale = (loaded.collections.Locales as Locale[]).find((v) => v.Name === 'nl-BE');
        this.jane = this.people.find((v) => v.FirstName === 'Jane');

        if (datas && datas.length > 0) {
          this.data = datas[0];
        } else {
          this.data = this.allors.context.create(this.m.Data) as Data;
        }
      });
  }

  ngAfterViewInit(): void {}

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  reset() {
    this.allors.context.reset();
    this.data = this.allors.context.create(this.m.Data) as Data;
  }

  newDate() {
    if (this.data) {
      var today = this.dateAdapter.today();
      this.data.Date = today;
    }
  }

  newDateTime() {
    if (this.data) {
      var today = this.dateAdapter.today();
      this.data.DateTime = today;
    }
  }

  newDateTime2() {
    if (this.data) {
      var today = this.dateAdapter.today();
      this.data.DateTime2 = today;
    }
  }

  refresh(): void {
    this.refresh$.next(new Date());
  }

  save(): void {
    console.log('save');

    this.allors.context.save().subscribe(() => {
      this.data = null;
      this.refresh();
    }, this.saveService.errorHandler);
  }

  public goBack(): void {}
}
