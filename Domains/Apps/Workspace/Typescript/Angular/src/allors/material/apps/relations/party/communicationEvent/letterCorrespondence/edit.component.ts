import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy , OnInit } from "@angular/core";
import { Validators } from "@angular/forms";
import { MdSnackBar, MdSnackBarConfig } from "@angular/material";
import { ActivatedRoute, UrlSegment } from "@angular/router";
import { TdDialogService, TdMediaService } from "@covalent/core";
import { BehaviorSubject, Observable, Subscription } from "rxjs/Rx";

import { AllorsService, ErrorService, Filter, Invoked, Loaded, Saved, Scope } from "../../../../../../angular";
import { Equals, Fetch, Like, Page, Path, PullRequest, PushResponse, Query, Sort, TreeNode } from "../../../../../../domain";
import {
  CommunicationEvent, CommunicationEventPurpose, ContactMechanism, Enumeration, LetterCorrespondence, Locale, Organisation, OrganisationContactRelationship,
  Party, PartyContactMechanism, PartyRelationship, Person, PersonRole, PostalAddress, Singleton,
} from "../../../../../../domain";
import { MetaDomain } from "../../../../../../meta/index";

@Component({
  templateUrl: "./form.component.html",
})
export class PartyCommunicationEventEditLetterCorrespondenceComponent implements OnInit, AfterViewInit, OnDestroy {

  public title: string = "Letter Correspondence";
  public subTitle: string;

  public addSender: boolean = false;
  public addReceiver: boolean = false;
  public addAddress: boolean = false;

  public m: MetaDomain;

  public singleton: Singleton;
  public communicationEvent: LetterCorrespondence;
  public employees: Person[];
  public contacts: Party[] = [];
  public party: Party;
  public purposes: CommunicationEventPurpose[];
  public postalAddresses: ContactMechanism[] = [];

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;

  constructor(
    private allors: AllorsService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private snackBar: MdSnackBar,
    private dialogService: TdDialogService,
    public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef) {

    this.scope = new Scope(allors.database, allors.workspace);
    this.m = this.allors.meta;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  get PartyIsOrganisation(): boolean {
    return this.party instanceof (Organisation);
  }

  public ngOnInit(): void {
    const route$: Observable<UrlSegment[]> = this.route.url;

    const combined$: Observable<[UrlSegment[], Date]> = Observable.combineLatest(route$, this.refresh$);

    this.subscription = combined$
      .switchMap(([urlSegments, date]: [UrlSegment[], Date]) => {

        const id: string = this.route.snapshot.paramMap.get("id");
        const roleId: string = this.route.snapshot.paramMap.get("roleId");

        const m: MetaDomain = this.allors.meta;

        const fetch: Fetch[] = [
          new Fetch({
            id,
            include: [
              new TreeNode({ roleType: m.Party.CurrentContacts }),
              new TreeNode({
                nodes: [
                  new TreeNode({
                    nodes: [
                      new TreeNode({
                        nodes: [
                          new TreeNode({ roleType: m.PostalBoundary.Country }),
                        ],
                        roleType: m.PostalAddress.PostalBoundary,
                      }),
                    ],
                    roleType: m.PartyContactMechanism.ContactMechanism,
                  }),
                ],
                roleType: m.Party.CurrentPartyContactMechanisms,
              }),
            ],
            name: "party",
          }),
          new Fetch({
            id: roleId,
            include: [
              new TreeNode({ roleType: m.LetterCorrespondence.Originators }),
              new TreeNode({ roleType: m.LetterCorrespondence.Receivers }),
              new TreeNode({ roleType: m.CommunicationEvent.EventPurposes }),
              new TreeNode({ roleType: m.CommunicationEvent.CommunicationEventState }),
              new TreeNode({
                nodes: [
                  new TreeNode({
                    nodes: [
                      new TreeNode({ roleType: m.PostalBoundary.Country }),
                    ],
                    roleType: m.PostalAddress.PostalBoundary,
                  }),
                ],
                roleType: m.LetterCorrespondence.PostalAddresses,
              }),
            ],
            name: "communicationEvent",
          }),
        ];

        const query: Query[] = [
          new Query(
            {
              include: [
                new TreeNode({
                  nodes: [
                    new TreeNode({
                      nodes: [
                        new TreeNode({
                          nodes: [
                            new TreeNode({
                              nodes: [
                                new TreeNode({
                                  nodes: [
                                    new TreeNode({ roleType: m.PostalBoundary.Country }),
                                  ],
                                  roleType: m.PostalAddress.PostalBoundary,
                                }),
                              ],
                              roleType: m.PartyContactMechanism.ContactMechanism,
                            }),
                          ],
                          roleType: m.Party.CurrentPartyContactMechanisms,
                        }),
                      ],
                      roleType: m.InternalOrganisation.ActiveEmployees,
                    }),
                  ],
                  roleType: m.Singleton.InternalOrganisation,
                }),
              ],
              name: "singletons",
              objectType: this.m.Singleton,
            }),
          new Query(
            {
              name: "purposes",
              objectType: this.m.CommunicationEventPurpose,
            }),
        ];

        return this.scope
          .load("Pull", new PullRequest({ fetch, query }));
      })
      .subscribe((loaded: Loaded) => {

        this.scope.session.reset();

        this.communicationEvent = loaded.objects.communicationEvent as LetterCorrespondence;

        if (!this.communicationEvent) {
          this.communicationEvent = this.scope.session.create("LetterCorrespondence") as LetterCorrespondence;
          this.communicationEvent.AddOriginator(this.party);
        }

        this.party = loaded.objects.party as Party;
        this.singleton = loaded.collections.singletons[0] as Singleton;
        this.employees = this.singleton.InternalOrganisation.ActiveEmployees;
        this.purposes = loaded.collections.purposes as CommunicationEventPurpose[];

        for (const employee of this.employees) {
          const employeeContactMechanisms: ContactMechanism[] = employee.CurrentPartyContactMechanisms.map((v: PartyContactMechanism) => v.ContactMechanism);
          for (const contactMechanism of employeeContactMechanisms) {
            if (contactMechanism instanceof (PostalAddress)) {
              this.postalAddresses.push(contactMechanism);
            }
          }
        }

        const contactMechanisms: ContactMechanism[] = this.party.CurrentPartyContactMechanisms.map((v: PartyContactMechanism) => v.ContactMechanism);
        for (const contactMechanism of contactMechanisms) {
          if (contactMechanism instanceof (PostalAddress)) {
            this.postalAddresses.push(contactMechanism);
          }
        }

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

  public ngAfterViewInit(): void {
    this.media.broadcast();
    this.changeDetectorRef.detectChanges();
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public senderCancelled(): void {
    this.addSender = false;
  }

  public receiverCancelled(): void {
    this.addReceiver = false;
  }

  public addressCancelled(): void {
    this.addAddress = false;
  }

  public senderAdded(id: string): void {
    this.addSender = false;

    const sender: Person = this.scope.session.get(id) as Person;
    const relationShip: OrganisationContactRelationship = this.scope.session.create("OrganisationContactRelationship") as OrganisationContactRelationship;
    relationShip.Contact = sender;
    relationShip.Organisation = this.party as Organisation;

    this.communicationEvent.AddOriginator(sender);
  }

  public receiverAdded(id: string): void {
    this.addReceiver = false;

    const receiver: Person = this.scope.session.get(id) as Person;
    const relationShip: OrganisationContactRelationship = this.scope.session.create("OrganisationContactRelationship") as OrganisationContactRelationship;
    relationShip.Contact = receiver;
    relationShip.Organisation = this.party as Organisation;

    this.communicationEvent.AddReceiver(receiver);
  }

  public addressAdded(id: string): void {
    this.addAddress = false;

    const postalAddress: PostalAddress = this.scope.session.get(id) as PostalAddress;
    const partyContactMechanism: PartyContactMechanism = this.scope.session.create("PartyContactMechanism") as PartyContactMechanism;
    partyContactMechanism.ContactMechanism = postalAddress;
    this.party.AddPartyContactMechanism(partyContactMechanism);

    this.postalAddresses.push(postalAddress);
    this.communicationEvent.AddPostalAddress(postalAddress);
  }

  public cancel(): void {
    const cancelFn: () => void = () => {
      this.scope.invoke(this.communicationEvent.Cancel)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open("Successfully cancelled.", "close", { duration: 5000 });
        },
        (error: Error) => {
          this.errorService.dialog(error);
        });
    };

    if (this.scope.session.hasChanges) {
      this.dialogService
        .openConfirm({ message: "Save changes?" })
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

  public close(): void {
    const cancelFn: () => void = () => {
      this.scope.invoke(this.communicationEvent.Close)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open("Successfully closed.", "close", { duration: 5000 });
        },
        (error: Error) => {
          this.errorService.dialog(error);
        });
    };

    if (this.scope.session.hasChanges) {
      this.dialogService
        .openConfirm({ message: "Save changes?" })
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

  public reopen(): void {
    const cancelFn: () => void = () => {
      this.scope.invoke(this.communicationEvent.Reopen)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open("Successfully reopened.", "close", { duration: 5000 });
        },
        (error: Error) => {
          this.errorService.dialog(error);
        });
    };

    if (this.scope.session.hasChanges) {
      this.dialogService
        .openConfirm({ message: "Save changes?" })
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

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public goBack(): void {
    window.history.back();
  }
}
