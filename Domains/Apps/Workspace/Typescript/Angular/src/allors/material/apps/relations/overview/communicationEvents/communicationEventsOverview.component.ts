import { Observable, BehaviorSubject, Subject, Subscription } from 'rxjs/Rx';
import { Component, OnInit, AfterViewInit, OnDestroy, ViewChild, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { MdSnackBar, MdSnackBarConfig } from '@angular/material';

import { TdLoadingService, TdDialogService, TdMediaService } from '@covalent/core';

import { PullRequest, Query, Predicate, And, Or, Not, Equals, Like, Contains, ContainedIn, TreeNode, Sort, Page } from '../../../../../domain';
import { MetaDomain } from '../../../../../meta/index';
import { CommunicationEvent, CommunicationEventObjectState, CommunicationEventPurpose, Person, Priority, Singleton } from '../../../../../domain';
import { AllorsService, ErrorService, Scope, Loaded, Saved, Invoked } from '../../../../../angular';

interface SearchData {
  subject: string;
  state: string;
  purpose: string;
  involved: string;
}

@Component({
  templateUrl: './communicationEventsOverview.component.html',
})
export class CommunicationEventsOverviewComponent implements AfterViewInit, OnDestroy {

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;

  private page$: BehaviorSubject<number>;
  total: number;

  title: string = 'Communications';

  searchForm: FormGroup;

  data: CommunicationEvent[];

  communicationEventObjectStates: CommunicationEventObjectState[];
  selectedCommunicationEventObjectState: CommunicationEventObjectState;
  communicationEventObjectState: CommunicationEventObjectState;

  purposes: CommunicationEventPurpose[];
  selectedPurpose: CommunicationEventPurpose;
  purpose: CommunicationEventPurpose;

  people: Person[];
  selectedInvolved: Person;
  involved: Person;

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
      subject: [''],
      state: [''],
      purpose: [''],
      involved: [''],
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
              name: 'communicationEventObjectStates',
              objectType: m.CommunicationEventObjectState,
            }),
          new Query(
            {
              name: 'purposes',
              objectType: m.CommunicationEventPurpose,
            }),
          new Query(
            {
              name: 'persons',
              objectType: m.Person,
            }),
        ];

        return this.scope
          .load('Pull', new PullRequest({ query: objectStatesQuery }))
          .switchMap((loaded: Loaded) => {
            this.communicationEventObjectStates = loaded.collections.communicationEventObjectStates as CommunicationEventObjectState[];
            this.communicationEventObjectState = this.communicationEventObjectStates.find((v: CommunicationEventObjectState) => v.Name === data.state);

            this.purposes = loaded.collections.purposes as CommunicationEventPurpose[];
            this.purpose = this.purposes.find((v: CommunicationEventPurpose) => v.Name === data.purpose);

            this.people = loaded.collections.persons as Person[];
            this.involved = this.people.find((v: Person) => v.displayName === data.involved);

            const predicate: And = new And();
            const predicates: Predicate[] = predicate.predicates;

            if (data.subject) {
              const like: string = '%' + data.subject + '%';
              predicates.push(new Like({ roleType: m.CommunicationEvent.Subject, value: like }));
            }

            if (data.state) {
              predicates.push(new Equals({ roleType: m.CommunicationEvent.CurrentObjectState, value: this.communicationEventObjectState }));
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
                  name: 'communicationEvents',
                  objectType: m.CommunicationEvent,
                  predicate: predicate,
                  page: new Page({ skip: 0, take: take }),
                  include: [
                    new TreeNode({ roleType: m.CommunicationEvent.CurrentObjectState }),
                    new TreeNode({ roleType: m.CommunicationEvent.FromParties }),
                    new TreeNode({ roleType: m.CommunicationEvent.ToParties }),
                    new TreeNode({ roleType: m.CommunicationEvent.InvolvedParties }),
                  ],
                }),
            ];

            return this.scope
              .load('Pull', new PullRequest({ query: communicationsQuery }));
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

  more(): void {
    this.page$.next(this.data.length + 50);
  }

  goBack(): void {
    this.router.navigate(['/']);
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

  refresh(): void {
    this.refresh$.next(new Date());
  }

  cancel(communicationEvent: CommunicationEvent): void {
    this.dialogService
      .openConfirm({ message: 'Are you sure you want to cancel this?' })
      .afterClosed().subscribe((confirm: boolean) => {
        if (confirm) {
          this.scope.invoke(communicationEvent.Cancel)
            .subscribe((invoked: Invoked) => {
              this.snackBar.open('Successfully cancelled.', 'close', { duration: 5000 });
              this.refresh();
            },
            (error: Error) => {
              this.errorService.dialog(error);
            });
        }
      });
  }

  close(communicationEvent: CommunicationEvent): void {
    this.dialogService
      .openConfirm({ message: 'Are you sure you want to close this?' })
      .afterClosed().subscribe((confirm: boolean) => {
        if (confirm) {
          this.scope.invoke(communicationEvent.Close)
            .subscribe((invoked: Invoked) => {
              this.snackBar.open('Successfully closed.', 'close', { duration: 5000 });
              this.refresh();
            },
            (error: Error) => {
              this.errorService.dialog(error);
            });
        }
      });
  }

  reopen(communicationEvent: CommunicationEvent): void {
    this.dialogService
      .openConfirm({ message: 'Are you sure you want to reopen this?' })
      .afterClosed().subscribe((confirm: boolean) => {
        if (confirm) {
          this.scope.invoke(communicationEvent.Reopen)
            .subscribe((invoked: Invoked) => {
              this.snackBar.open('Successfully reopened.', 'close', { duration: 5000 });
              this.refresh();
            },
            (error: Error) => {
              this.errorService.dialog(error);
            });
        }
      });
  }

  delete(communicationEvent: CommunicationEvent): void {
    this.dialogService
      .openConfirm({ message: 'Are you sure you want to delete this communication event?' })
      .afterClosed()
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.scope.invoke(communicationEvent.Delete)
            .subscribe((invoked: Invoked) => {
              this.snackBar.open('Successfully deleted.', 'close', { duration: 5000 });
              this.refresh();
            },
            (error: Error) => {
              this.errorService.dialog(error);
            });
        }
      });
  }

  checkType(obj: any): string {
    return obj.objectType.name;
  }

  onView(person: Person): void {
    this.router.navigate(["/relations/person/" + person.id]);
  }
}
