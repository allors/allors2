import { Observable, BehaviorSubject, Subscription } from 'rxjs/Rx';
import { Component, OnInit, AfterViewInit, OnDestroy , ChangeDetectorRef } from '@angular/core';
import { Validators } from '@angular/forms';
import { ActivatedRoute, UrlSegment } from '@angular/router';
import { MdSnackBar, MdSnackBarConfig } from '@angular/material';
import { TdMediaService, TdDialogService } from '@covalent/core';

import { MetaDomain } from '../../../../../../meta/index';
import { PullRequest, PushResponse, Fetch, Path, Query, Equals, Like, TreeNode, Sort, Page } from '../../../../../../domain';
import {
  Person, Party, PartyRelationship, CommunicationEvent, CommunicationEventPurpose, EmailAddress, EmailTemplate,
  PersonRole, Locale, Enumeration, EmailCommunication, Singleton, ContactMechanism, PartyContactMechanism, OrganisationContactRelationship, Organisation,
} from '../../../../../../domain';
import { AllorsService, ErrorService, Scope, Loaded, Saved, Invoked, Filter } from '../../../../../../angular';

@Component({
  templateUrl: './form.component.html',
})
export class PartycommunicationEventEditEmailCommunicationComponent implements OnInit, AfterViewInit, OnDestroy {

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;

  title: string = 'Email Communication';
  subTitle: string;

  addOriginator: boolean = false;
  addAddressee: boolean = false;

  m: MetaDomain;

  singleton: Singleton;
  communicationEvent: EmailCommunication;
  employees: Person[];
  party: Party;
  purposes: CommunicationEventPurpose[];
  partyRelationships: PartyRelationship[];
  emailAddresses: ContactMechanism[] = [];
  emailTemplate: EmailTemplate;

  constructor(
    private allors: AllorsService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private snackBar: MdSnackBar,
    private dialogService: TdDialogService,
    public media: TdMediaService,
    private changeDetectorRef: ChangeDetectorRef) {

    this.scope = new Scope(allors.database, allors.workspace);
    this.m = this.allors.meta;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  ngOnInit(): void {
    const route$: Observable<UrlSegment[]> = this.route.url;

    const combined$: Observable<[UrlSegment[], Date]> = Observable.combineLatest(route$, this.refresh$);

    this.subscription = combined$
      .switchMap(([urlSegments, date]: [UrlSegment[], Date]) => {

        const id: string = this.route.snapshot.paramMap.get('id');
        const roleId: string = this.route.snapshot.paramMap.get('roleId');

        const m: MetaDomain = this.allors.meta;

        const fetch: Fetch[] = [
          new Fetch({
            name: 'party',
            id: id,
          }),
          new Fetch({
            name: 'partyRelationships',
            id: id,
            path: new Path({ step: m.Party.CurrentPartyRelationships }),
            include: [
              new TreeNode({ roleType: m.PartyRelationship.CommunicationEvents }),
            ],
          }),
          new Fetch({
            name: 'communicationEvent',
            id: roleId,
            include: [
              new TreeNode({ roleType: m.EmailCommunication.Originator }),
              new TreeNode({ roleType: m.EmailCommunication.Addressees }),
              new TreeNode({ roleType: m.EmailCommunication.EmailTemplate }),
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
                      nodes: [
                        new TreeNode({
                          roleType: m.Party.CurrentPartyContactMechanisms,
                          nodes: [
                            new TreeNode({ roleType: m.PartyContactMechanism.ContactMechanism }),
                          ],
                        }),
                      ],
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
        this.communicationEvent = loaded.objects.communicationEvent as EmailCommunication;

        if (!this.communicationEvent) {
          this.communicationEvent = this.scope.session.create('EmailCommunication') as EmailCommunication;
          this.emailTemplate = this.scope.session.create('EmailTemplate') as EmailTemplate;
          this.communicationEvent.EmailTemplate = this.emailTemplate;
          this.partyRelationships.forEach((v: PartyRelationship) => v.AddCommunicationEvent(this.communicationEvent));
        }

        this.party = loaded.objects.party as Party;
        this.singleton = loaded.collections.singletons[0] as Singleton;
        this.employees = this.singleton.DefaultInternalOrganisation.Employees;
        this.purposes = loaded.collections.purposes as CommunicationEventPurpose[];

        for (let employee of this.employees) {
          let employeeContactMechanisms: ContactMechanism[] = employee.CurrentPartyContactMechanisms.map((v: PartyContactMechanism) => v.ContactMechanism);
          for (let contactMechanism of employeeContactMechanisms) {
            if (contactMechanism instanceof (EmailAddress)) {
              this.emailAddresses.push(contactMechanism);
            }
          }
        }

        let contactMechanisms: ContactMechanism[] = this.party.CurrentPartyContactMechanisms.map((v: PartyContactMechanism) => v.ContactMechanism);
        for (let contactMechanism of contactMechanisms) {
          if (contactMechanism instanceof (EmailAddress)) {
            this.emailAddresses.push(contactMechanism);
          }
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
    this.changeDetectorRef.detectChanges();
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  originatorCancelled(): void {
    this.addOriginator = false;
  }

  addresseeCancelled(): void {
    this.addAddressee = false;
  }

  originatorAdded(id: string): void {
    this.addOriginator = false;

    const emailAddress: EmailAddress = this.scope.session.get(id) as EmailAddress;
    const partyContactMechanism: PartyContactMechanism = this.scope.session.create('PartyContactMechanism') as PartyContactMechanism;
    partyContactMechanism.ContactMechanism = emailAddress;
    this.party.AddPartyContactMechanism(partyContactMechanism);

    this.emailAddresses.push(emailAddress);
    this.communicationEvent.Originator = emailAddress;
  }

  addresseeAdded(id: string): void {
    this.addAddressee = false;

    const emailAddress: EmailAddress = this.scope.session.get(id) as EmailAddress;
    const partyContactMechanism: PartyContactMechanism = this.scope.session.create('PartyContactMechanism') as PartyContactMechanism;
    partyContactMechanism.ContactMechanism = emailAddress;
    this.party.AddPartyContactMechanism(partyContactMechanism);

    this.emailAddresses.push(emailAddress);
    this.communicationEvent.AddAddressee(emailAddress);
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
