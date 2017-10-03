import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { MdSnackBar, MdSnackBarConfig } from "@angular/material";
import { Title } from "@angular/platform-browser";
import { Router } from "@angular/router";
import { BehaviorSubject, Observable, Subject, Subscription } from "rxjs/Rx";

import { TdDialogService, TdLoadingService, TdMediaService } from "@covalent/core";

import { AllorsService, ErrorService, Invoked, Loaded, Saved, Scope } from "../../../../../angular";
import { And, ContainedIn, Contains, Equals, Like, Not, Or, Page, Predicate, PullRequest, Query, Sort, TreeNode } from "../../../../../domain";
import { CommunicationEvent, CommunicationEventPurpose, CommunicationEventState, Person, Priority, Singleton } from "../../../../../domain";
import { MetaDomain } from "../../../../../meta/index";

interface SearchData {
  subject: string;
  state: string;
  purpose: string;
  involved: string;
}

@Component({
  templateUrl: "./communicationEventsOverview.component.html",
})
export class CommunicationEventsOverviewComponent implements AfterViewInit, OnDestroy {

  public total: number;

  public title: string = "Communications";

  public searchForm: FormGroup;

  public data: CommunicationEvent[];

  public communicationEventStates: CommunicationEventState[];
  public selectedCommunicationEventState: CommunicationEventState;
  public communicationEventState: CommunicationEventState;

  public purposes: CommunicationEventPurpose[];
  public selectedPurpose: CommunicationEventPurpose;
  public purpose: CommunicationEventPurpose;

  public people: Person[];
  public selectedInvolved: Person;
  public involved: Person;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;

  private page$: BehaviorSubject<number>;

  constructor(
    private allors: AllorsService,
    private errorService: ErrorService,
    private formBuilder: FormBuilder,
    private titleService: Title,
    private snackBar: MdSnackBar,
    private router: Router,
    private dialogService: TdDialogService,
    private snackBarService: MdSnackBar,
    public media: TdMediaService,
    private changeDetectorRef: ChangeDetectorRef) {

    titleService.setTitle(this.title);
    this.scope = new Scope(allors.database, allors.workspace);
    this.refresh$ = new BehaviorSubject<Date>(undefined);

    this.searchForm = this.formBuilder.group({
      involved: [""],
      purpose: [""],
      state: [""],
      subject: [""],
    });

    this.page$ = new BehaviorSubject<number>(50);

    const search$: Observable<SearchData> = this.searchForm.valueChanges
      .debounceTime(400)
      .distinctUntilChanged()
      .startWith({});

    const combined$: Observable<any> = Observable.combineLatest(search$, this.page$)
      .scan(([previousData, previousTake]: [SearchData, number], [data, take]: [SearchData, number]): [SearchData, number] => {
        return [
          data,
          data !== previousData ? 50 : take,
        ];
      }, [] as [SearchData, number]);

    this.subscription = combined$
      .switchMap(([data, take]: [SearchData, number]) => {
        const m: MetaDomain = this.allors.meta;

        const objectStatesQuery: Query[] = [
          new Query(
            {
              name: "communicationEventStates",
              objectType: m.CommunicationEventState,
            }),
          new Query(
            {
              name: "purposes",
              objectType: m.CommunicationEventPurpose,
            }),
          new Query(
            {
              name: "persons",
              objectType: m.Person,
            }),
        ];

        return this.scope
          .load("Pull", new PullRequest({ query: objectStatesQuery }))
          .switchMap((loaded: Loaded) => {
            this.communicationEventStates = loaded.collections.communicationEventStates as CommunicationEventState[];
            this.communicationEventState = this.communicationEventStates.find((v: CommunicationEventState) => v.Name === data.state);

            this.purposes = loaded.collections.purposes as CommunicationEventPurpose[];
            this.purpose = this.purposes.find((v: CommunicationEventPurpose) => v.Name === data.purpose);

            this.people = loaded.collections.persons as Person[];
            this.involved = this.people.find((v: Person) => v.displayName === data.involved);

            const predicate: And = new And();
            const predicates: Predicate[] = predicate.predicates;

            if (data.subject) {
              const like: string = "%" + data.subject + "%";
              predicates.push(new Like({ roleType: m.CommunicationEvent.Subject, value: like }));
            }

            if (data.state) {
              predicates.push(new Equals({ roleType: m.CommunicationEvent.CommunicationEventState, value: this.communicationEventState }));
            }

            if (data.purpose) {
              predicates.push(new Contains({ roleType: m.CommunicationEvent.EventPurposes, object: this.purpose }));
            }

            if (data.involved) {
              predicates.push(new Contains({ roleType: m.CommunicationEvent.InvolvedParties, object: this.involved }));
            }

            const communicationsQuery: Query[] = [
              new Query(
                {
                  include: [
                    new TreeNode({ roleType: m.CommunicationEvent.CommunicationEventState }),
                    new TreeNode({ roleType: m.CommunicationEvent.FromParties }),
                    new TreeNode({ roleType: m.CommunicationEvent.ToParties }),
                    new TreeNode({ roleType: m.CommunicationEvent.InvolvedParties }),
                  ],
                  name: "communicationEvents",
                  objectType: m.CommunicationEvent,
                  predicate,
                  page: new Page({ skip: 0, take }),
                }),
            ];

            return this.scope
              .load("Pull", new PullRequest({ query: communicationsQuery }));
          });
      })
      .subscribe((loaded: Loaded) => {

        this.scope.session.reset();

        this.data = loaded.collections.communicationEvents as CommunicationEvent[];
        this.total = loaded.values.communicationEvents_total;
      },
      (error: any) => {
        this.errorService.message(error);
        this.goBack();
      });
  }

  public more(): void {
    this.page$.next(this.data.length + 50);
  }

  public goBack(): void {
    window.history.back();
  }

  public ngAfterViewInit(): void {
    this.media.broadcast();
    this.changeDetectorRef.detectChanges();
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public cancel(communicationEvent: CommunicationEvent): void {
    this.dialogService
      .openConfirm({ message: "Are you sure you want to cancel this?" })
      .afterClosed().subscribe((confirm: boolean) => {
        if (confirm) {
          this.scope.invoke(communicationEvent.Cancel)
            .subscribe((invoked: Invoked) => {
              this.snackBar.open("Successfully cancelled.", "close", { duration: 5000 });
              this.refresh();
            },
            (error: Error) => {
              this.errorService.dialog(error);
            });
        }
      });
  }

  public close(communicationEvent: CommunicationEvent): void {
    this.dialogService
      .openConfirm({ message: "Are you sure you want to close this?" })
      .afterClosed().subscribe((confirm: boolean) => {
        if (confirm) {
          this.scope.invoke(communicationEvent.Close)
            .subscribe((invoked: Invoked) => {
              this.snackBar.open("Successfully closed.", "close", { duration: 5000 });
              this.refresh();
            },
            (error: Error) => {
              this.errorService.dialog(error);
            });
        }
      });
  }

  public reopen(communicationEvent: CommunicationEvent): void {
    this.dialogService
      .openConfirm({ message: "Are you sure you want to reopen this?" })
      .afterClosed().subscribe((confirm: boolean) => {
        if (confirm) {
          this.scope.invoke(communicationEvent.Reopen)
            .subscribe((invoked: Invoked) => {
              this.snackBar.open("Successfully reopened.", "close", { duration: 5000 });
              this.refresh();
            },
            (error: Error) => {
              this.errorService.dialog(error);
            });
        }
      });
  }

  public delete(communicationEvent: CommunicationEvent): void {
    this.dialogService
      .openConfirm({ message: "Are you sure you want to delete this communication event?" })
      .afterClosed()
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.scope.invoke(communicationEvent.Delete)
            .subscribe((invoked: Invoked) => {
              this.snackBar.open("Successfully deleted.", "close", { duration: 5000 });
              this.refresh();
            },
            (error: Error) => {
              this.errorService.dialog(error);
            });
        }
      });
  }

  public checkType(obj: any): string {
    return obj.objectType.name;
  }

  public onView(person: Person): void {
    this.router.navigate(["/relations/person/" + person.id]);
  }
}
