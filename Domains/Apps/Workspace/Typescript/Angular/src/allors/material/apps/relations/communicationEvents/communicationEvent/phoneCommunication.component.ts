import { Observable, BehaviorSubject, Subscription } from 'rxjs/Rx';
import { Component, OnInit, AfterViewInit, OnDestroy } from '@angular/core';
import { Validators } from '@angular/forms';
import { ActivatedRoute, UrlSegment } from '@angular/router';
import { MdSnackBar, MdSnackBarConfig } from '@angular/material';
import { TdMediaService, TdDialogService } from '@covalent/core';

import { MetaDomain } from '../../../../../meta/index';
import { PullRequest, PushResponse, Fetch, Path, Query, Equals, Like, TreeNode, Sort, Page } from '../../../../../domain';
import {
  Person, Party, PartyRelationship, CommunicationEvent, CommunicationEventPurpose, TelecommunicationsNumber,
  PersonRole, Locale, Enumeration, PhoneCommunication, Singleton, ContactMechanism, PartyContactMechanism, OrganisationContactRelationship, Organisation,
} from '../../../../../domain';
import { AllorsService, ErrorService, Scope, Loaded, Saved, Invoked, Filter } from '../../../../../angular';

@Component({
  templateUrl: './phoneCommunication.component.html',
})
export class PhoneCommunicationFormComponent implements OnInit, AfterViewInit, OnDestroy {

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;

  addCaller: boolean = false;
  addReceiver: boolean = false;
  addPhoneNumber: boolean = false;

  flex: string = '100%';
  flex2: string = `calc(50%-25px)`;

  m: MetaDomain;

  singleton: Singleton;
  communicationEvent: PhoneCommunication;
  employees: Person[];
  contacts: Party[] = [];
  party: Party;
  purposes: CommunicationEventPurpose[];
  partyRelationships: PartyRelationship[];
  phonenumbers: ContactMechanism[] = [];

  constructor(
    private allors: AllorsService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private snackBar: MdSnackBar,
    private dialogService: TdDialogService,
    public media: TdMediaService) {

    this.scope = new Scope(allors.database, allors.workspace);
    this.m = this.allors.meta;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  get PartyIsOrganisation(): boolean {
    return this.party instanceof (Organisation);
  }

  ngOnInit(): void {
    const route$: Observable<UrlSegment[]> = this.route.url;

    const combined$: Observable<[UrlSegment[], Date]> = Observable.combineLatest(route$, this.refresh$);

    this.subscription = combined$
      .switchMap(([urlSegments, date]: [UrlSegment[], Date]) => {

        const partyId: string = this.route.snapshot.paramMap.get('partyId');
        const id: string = this.route.snapshot.paramMap.get('id');

        const m: MetaDomain = this.allors.meta;

        const fetch: Fetch[] = [
          new Fetch({
            name: 'party',
            id: partyId,
            include: [
              new TreeNode({ roleType: m.Party.CurrentContacts }),
              new TreeNode({
                roleType: m.Party.CurrentPartyContactMechanisms,
                nodes: [
                  new TreeNode({ roleType: m.PartyContactMechanism.ContactMechanism }),
                ],
              }),
            ],
          }),
          new Fetch({
            name: 'partyRelationships',
            id: partyId,
            path: new Path({ step: m.Party.CurrentPartyRelationships }),
            include: [
              new TreeNode({ roleType: m.PartyRelationship.CommunicationEvents }),
            ],
          }),
          new Fetch({
            name: 'communicationEvent',
            id: id,
            include: [
              new TreeNode({ roleType: m.CommunicationEvent.FromParties }),
              new TreeNode({ roleType: m.CommunicationEvent.ToParties }),
              new TreeNode({ roleType: m.CommunicationEvent.EventPurposes }),
              new TreeNode({ roleType: m.CommunicationEvent.CurrentObjectState }),
            ],
          }),
        ];

        const query: Query[] = [
          new Query(
            {
              name: 'singletons',
              objectType: this.m.Singleton,
              include: [
                new TreeNode({
                  roleType: m.Singleton.DefaultInternalOrganisation,
                  nodes: [
                    new TreeNode({
                      roleType: m.InternalOrganisation.Employees,
                    }),
                  ],
                }),
              ],
            }),
          new Query(
            {
              name: 'purposes',
              objectType: this.m.CommunicationEventPurpose,
            }),
        ];

        return this.scope
          .load('Pull', new PullRequest({ fetch: fetch, query: query }));
      })
      .subscribe((loaded: Loaded) => {

        this.scope.session.reset();

        this.partyRelationships = loaded.collections.partyRelationships as PartyRelationship[];
        this.communicationEvent = loaded.objects.communicationEvent as PhoneCommunication;

        if (!this.communicationEvent) {
          this.communicationEvent = this.scope.session.create('PhoneCommunication') as PhoneCommunication;
          this.partyRelationships.forEach((v: PartyRelationship) => v.AddCommunicationEvent(this.communicationEvent));
        }

        this.party = loaded.objects.party as Party;

        let contactMechanisms: ContactMechanism[] = this.party.CurrentPartyContactMechanisms.map((v: PartyContactMechanism) => v.ContactMechanism);
        for (let contactMechanism of contactMechanisms) {
          if (contactMechanism instanceof (TelecommunicationsNumber)) {
            this.phonenumbers.push(contactMechanism);
          }
        }

        this.singleton = loaded.collections.singletons[0] as Singleton;
        this.employees = this.singleton.DefaultInternalOrganisation.Employees;
        this.purposes = loaded.collections.purposes as CommunicationEventPurpose[];

        this.contacts.push(this.party);
        if (this.employees.length > 0) {
          this.contacts = this.contacts.concat(this.employees);
        }

        if (this.party.CurrentContacts.length > 0) {
          this.contacts = this.contacts.concat(this.party.CurrentContacts);
        }
      },
      (error: any) => {
        this.errorService.message(error);
        this.goBack();
      },
    );
  }

  ngAfterViewInit(): void {
    this.media.broadcast();
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  phoneNumberCancelled(): void {
    this.addPhoneNumber = false;
  }

  callerCancelled(): void {
    this.addCaller = false;
  }

  receiverCancelled(): void {
    this.addReceiver = false;
  }

  phoneNumberAdded(id: string): void {
    this.addPhoneNumber = false;

    const telecommunicationsNumber: TelecommunicationsNumber = this.scope.session.get(id) as TelecommunicationsNumber;
    const partyContactMechanism: PartyContactMechanism = this.scope.session.create('PartyContactMechanism') as PartyContactMechanism;
    partyContactMechanism.ContactMechanism = telecommunicationsNumber;
    this.party.AddPartyContactMechanism(partyContactMechanism);

    this.phonenumbers.push(telecommunicationsNumber);
    this.communicationEvent.AddContactMechanism(telecommunicationsNumber);
  }

  callerAdded(id: string): void {
    this.addCaller = false;

    const caller: Person = this.scope.session.get(id) as Person;
    const relationShip: OrganisationContactRelationship = this.scope.session.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    relationShip.Contact = caller;
    relationShip.Organisation = this.party as Organisation;

    this.communicationEvent.AddCaller(caller);
  }

  receiverAdded(id: string): void {
    this.addReceiver = false;

    const receiver: Person = this.scope.session.get(id) as Person;
    const relationShip: OrganisationContactRelationship = this.scope.session.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    relationShip.Contact = receiver;
    relationShip.Organisation = this.party as Organisation;

    this.communicationEvent.AddReceiver(receiver);
  }

  cancel(): void {
    const cancelFn: () => void = () => {
      this.scope.invoke(this.communicationEvent.Cancel)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open('Successfully cancelled.', 'close', { duration: 5000 });
        },
        (error: Error) => {
          this.errorService.dialog(error);
        });
    };

    if (this.scope.session.hasChanges) {
      this.dialogService
        .openConfirm({ message: 'Save changes?' })
        .afterClosed().subscribe((confirm: boolean) => {
          if (confirm) {
            this.scope
              .save()
              .subscribe((saved: Saved) => {
                this.scope.session.reset();
                cancelFn();
              },
              (error: Error) => {
                this.errorService.dialog(error);
              });
          } else {
            cancelFn();
          }
        });
    } else {
      cancelFn();
    }
  }

  close(): void {
    const cancelFn: () => void = () => {
      this.scope.invoke(this.communicationEvent.Close)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open('Successfully closed.', 'close', { duration: 5000 });
        },
        (error: Error) => {
          this.errorService.dialog(error);
        });
    };

    if (this.scope.session.hasChanges) {
      this.dialogService
        .openConfirm({ message: 'Save changes?' })
        .afterClosed().subscribe((confirm: boolean) => {
          if (confirm) {
            this.scope
              .save()
              .subscribe((saved: Saved) => {
                this.scope.session.reset();
                cancelFn();
              },
              (error: Error) => {
                this.errorService.dialog(error);
              });
          } else {
            cancelFn();
          }
        });
    } else {
      cancelFn();
    }
  }

  reopen(): void {
    const cancelFn: () => void = () => {
      this.scope.invoke(this.communicationEvent.Reopen)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open('Successfully reopened.', 'close', { duration: 5000 });
        },
        (error: Error) => {
          this.errorService.dialog(error);
        });
    };

    if (this.scope.session.hasChanges) {
      this.dialogService
        .openConfirm({ message: 'Save changes?' })
        .afterClosed().subscribe((confirm: boolean) => {
          if (confirm) {
            this.scope
              .save()
              .subscribe((saved: Saved) => {
                this.scope.session.reset();
                cancelFn();
              },
              (error: Error) => {
                this.errorService.dialog(error);
              });
          } else {
            cancelFn();
          }
        });
    } else {
      cancelFn();
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

  refresh(): void {
    this.refresh$.next(new Date());
  }

  goBack(): void {
    window.history.back();
  }
}
