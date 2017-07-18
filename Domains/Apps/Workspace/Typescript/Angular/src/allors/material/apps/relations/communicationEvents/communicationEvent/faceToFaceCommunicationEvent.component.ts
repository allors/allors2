import { Observable, BehaviorSubject, Subject, Subscription } from 'rxjs/Rx';
import { Component, OnInit, AfterViewInit, OnDestroy } from '@angular/core';
import { Validators } from '@angular/forms';
import { ActivatedRoute, UrlSegment } from '@angular/router';
import { MdSnackBar, MdSnackBarConfig } from '@angular/material';
import { TdMediaService, TdDialogService } from '@covalent/core';

import { MetaDomain } from '../../../../../meta/index';
import { PullRequest, PushResponse, Fetch, Path, Query, Equals, Like, TreeNode, Sort, Page } from '../../../../../domain';
import {
  CommunicationEvent, CommunicationEventPurpose, OrganisationContactRelationship, Party, PartyRelationship, Person,
  FaceToFaceCommunication, Organisation, PartyContactMechanism, Singleton,
} from '../../../../../domain';
import { AllorsService, ErrorService, Scope, Loaded, Saved, Filter, Invoked } from '../../../../../angular';

@Component({
  templateUrl: './faceToFaceCommunicationEvent.component.html',
})
export class FaceToFaceCommunicationEventFormComponent implements OnInit, AfterViewInit, OnDestroy {

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;

  addParticipant: boolean = false;

  flex: string = '100%';
  flex2: string = `calc(50%-25px)`;

  m: MetaDomain;

  communicationEvent: FaceToFaceCommunication;
  parties: Party[];
  party: Party;
  purposes: CommunicationEventPurpose[];
  partyRelationships: PartyRelationship[];
  singleton: Singleton;
  employees: Person[];
  contacts: Party[] = [];

  constructor(
    private allors: AllorsService,
    private errorService: ErrorService,
    private snackBar: MdSnackBar,
    private route: ActivatedRoute,
    private dialogService: TdDialogService,
    public media: TdMediaService) {

    this.scope = new Scope(allors.database, allors.workspace);
    this.m = this.allors.meta;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
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
        this.communicationEvent = loaded.objects.communicationEvent as FaceToFaceCommunication;

        if (!this.communicationEvent) {
          this.communicationEvent = this.scope.session.create('PhoneCommunication') as FaceToFaceCommunication;
          this.partyRelationships.forEach((v: PartyRelationship) => v.AddCommunicationEvent(this.communicationEvent));
        }

        this.party = loaded.objects.party as Party;

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

  participantCancelled(): void {
    this.addParticipant = false;
  }

  participantAdded(id: string): void {
    this.addParticipant = false;

    const participant: Person = this.scope.session.get(id) as Person;
    const relationShip: OrganisationContactRelationship = this.scope.session.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    relationShip.Contact = participant;
    relationShip.Organisation = this.party as Organisation;

    this.communicationEvent.AddParticipant(participant);
  }

  ngAfterViewInit(): void {
    this.media.broadcast();
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
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
