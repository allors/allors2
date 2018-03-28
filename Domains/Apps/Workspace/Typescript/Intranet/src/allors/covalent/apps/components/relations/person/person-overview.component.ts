import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { MatSnackBar } from "@angular/material";
import { Title } from "@angular/platform-browser";
import { ActivatedRoute, UrlSegment } from "@angular/router";
import { TdDialogService, TdMediaService } from "@covalent/core";

import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";

import "rxjs/add/observable/combineLatest";

import { ErrorService, Invoked, Loaded, Saved, Scope, WorkspaceService } from "../../../../../angular";
import { CommunicationEvent, ContactMechanism, InternalOrganisation, Organisation, OrganisationContactRelationship, PartyContactMechanism, Person, PersonRole, WorkEffort, WorkEffortAssignment } from "../../../../../domain";
import { Fetch, Path, PullRequest, Query, TreeNode } from "../../../../../framework";
import { MetaDomain } from "../../../../../meta";
import { StateService } from "../../../services/StateService";
import { Fetcher } from "../../Fetcher";

@Component({
  templateUrl: "./person-overview.component.html",
})
export class PersonOverviewComponent implements OnInit, OnDestroy {

  public m: MetaDomain;

  public communicationEvents: CommunicationEvent[];
  public workEffortAssignments: WorkEffortAssignment[];

  public title: string = "Person overview";
  public person: Person;
  public organisation: Organisation;
  public internalOrganisation: InternalOrganisation;

  public contactMechanismsCollection: string = "Current";
  public currentContactMechanisms: PartyContactMechanism[] = [];
  public inactiveContactMechanisms: PartyContactMechanism[] = [];
  public allContactMechanisms: PartyContactMechanism[] = [];

  public roles: PersonRole[];
  public activeRoles: PersonRole[] = [];
  public rolesText: string;
  private customerRole: PersonRole;
  private employeeRole: PersonRole;
  private contactRole: PersonRole;
  private isActiveCustomer: boolean;
  private isActiveEmployee: boolean;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;
  private fetcher: Fetcher;

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private dialogService: TdDialogService,
    private snackBar: MatSnackBar,
    public media: TdMediaService,
    private changeDetectorRef: ChangeDetectorRef,
    private titleService: Title,
    private stateService: StateService) {

    titleService.setTitle(this.title);

    this.scope = this.workspaceService.createScope();
    this.m = this.workspaceService.metaPopulation.metaDomain;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.fetcher = new Fetcher(this.stateService, this.m);
  }

  get contactMechanisms(): any {

    switch (this.contactMechanismsCollection) {
      case "Current":
        return this.currentContactMechanisms;
      case "Inactive":
        return this.inactiveContactMechanisms;
      case "All":
      default:
        return this.allContactMechanisms;
    }
  }

  public ngOnInit(): void {

    this.subscription = Observable.combineLatest(this.route.url, this.refresh$, this.stateService.internalOrganisationId$)
      .switchMap(([urlSegments, date, internalOrganisationId]) => {

        const id: string = this.route.snapshot.paramMap.get("id");
        const m: MetaDomain = this.m;

        const fetch: Fetch[] = [
          this.fetcher.internalOrganisation,
          new Fetch({
            id,
            include: [
              new TreeNode({ roleType: m.Party.Locale }),
              // new TreeNode({ roleType: m.Person.PersonRoles }),
              new TreeNode({ roleType: m.Person.LastModifiedBy }),
              new TreeNode({ roleType: m.Person.Salutation }),
              new TreeNode({
                nodes: [
                  new TreeNode({ roleType: m.PartyContactMechanism.ContactPurposes }),
                  new TreeNode({ roleType: m.PartyContactMechanism.ContactMechanism }),
                ],
                roleType: m.Party.PartyContactMechanisms,
              }),
              new TreeNode({
                nodes: [
                  new TreeNode({ roleType: m.PartyContactMechanism.ContactPurposes }),
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
                roleType: m.Party.PartyContactMechanisms,
              }),
              new TreeNode({
                nodes: [
                  new TreeNode({ roleType: m.PartyContactMechanism.ContactPurposes }),
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
              new TreeNode({
                nodes: [
                  new TreeNode({ roleType: m.PartyContactMechanism.ContactPurposes }),
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
                roleType: m.Party.InactivePartyContactMechanisms,
              }),
              new TreeNode({
                nodes: [
                  new TreeNode({
                    nodes: [
                      new TreeNode({ roleType: m.PostalBoundary.Country }),
                    ],
                    roleType: m.PostalAddress.PostalBoundary,
                  }),
                ],
                roleType: m.Person.GeneralCorrespondence,
              }),
            ],
            name: "person",
          }),
          new Fetch({
            id,
            include: [
              new TreeNode({ roleType: m.CommunicationEvent.CommunicationEventState }),
              new TreeNode({ roleType: m.CommunicationEvent.FromParties }),
              new TreeNode({ roleType: m.CommunicationEvent.ToParties }),
              new TreeNode({ roleType: m.CommunicationEvent.InvolvedParties }),
            ],
            name: "communicationEvents",
            path: new Path({ step: m.Party.CommunicationEventsWhereInvolvedParty }),
          }),
          new Fetch({
            id,
            include: [
              new TreeNode({
                nodes: [
                  new TreeNode({ roleType: m.WorkEffort.WorkEffortState }),
                  new TreeNode({ roleType: m.WorkEffort.Priority }),
                ],
                roleType: m.WorkEffortAssignment.Assignment,
              }),
            ],
            name: "workEffortAssignments",
            path: new Path({ step: m.Person.WorkEffortAssignmentsWhereProfessional }),
          }),
          new Fetch({
            id,
            include: [
              new TreeNode({ roleType: m.OrganisationContactRelationship.Organisation }),
            ],
            name: "organisationContactRelationships",
            path: new Path({ step: m.Person.OrganisationContactRelationshipsWhereContact }),
          }),
        ];

        const query: Query[] = [
          new Query(this.m.PersonRole),
        ];

        return this.scope
          .load("Pull", new PullRequest({ fetch, query }));
      })
      .subscribe((loaded) => {
        this.scope.session.reset();
        this.internalOrganisation = loaded.objects.internalOrganisation as InternalOrganisation;

        this.person = loaded.objects.person as Person;
        const organisationContactRelationships: OrganisationContactRelationship[] = loaded.collections.organisationContactRelationships as OrganisationContactRelationship[];
        this.organisation = organisationContactRelationships.length > 0 ? organisationContactRelationships[0].Organisation as Organisation : undefined;
        this.communicationEvents = loaded.collections.communicationEvents as CommunicationEvent[];
        this.workEffortAssignments = loaded.collections.workEffortAssignments as WorkEffortAssignment[];

        this.currentContactMechanisms = this.person.CurrentPartyContactMechanisms as PartyContactMechanism[];
        this.inactiveContactMechanisms = this.person.InactivePartyContactMechanisms as PartyContactMechanism[];
        this.allContactMechanisms = this.currentContactMechanisms.concat(this.inactiveContactMechanisms);

        this.roles = loaded.collections.PersonRoleQuery as PersonRole[];
        this.customerRole = this.roles.find((v: PersonRole) => v.UniqueId.toUpperCase() === "B29444EF-0950-4D6F-AB3E-9C6DC44C050F");
        this.employeeRole = this.roles.find((v: PersonRole) => v.UniqueId.toUpperCase() === "DB06A3E1-6146-4C18-A60D-DD10E19F7243");
        this.contactRole = this.roles.find((v: PersonRole) => v.UniqueId.toUpperCase() === "FA2DF11E-7795-4DF7-8B3F-4FD87D0C4D8E");

        this.activeRoles = [];
        this.rolesText = "";
        if (this.internalOrganisation.ActiveCustomers.includes(this.person)) {
          this.isActiveCustomer = true;
          this.activeRoles.push(this.customerRole);
        }

        if (this.internalOrganisation.ActiveEmployees.includes(this.person)) {
          this.isActiveEmployee = true;
          this.activeRoles.push(this.employeeRole);
        }

        if (this.organisation !== null) {
          this.activeRoles.push(this.contactRole);
        }

        if (this.activeRoles.length > 0) {
          this.rolesText = this.activeRoles
          .map((v: PersonRole) => v.Name)
          .reduce((acc: string, cur: string) => acc + ", " + cur);
        }
      },
      (error: any) => {
        this.errorService.message(error);
        this.goBack();
      },
    );
  }

  public removeContactMechanism(partyContactMechanism: PartyContactMechanism): void {
    partyContactMechanism.ThroughDate = new Date();
    this.scope
      .save()
      .subscribe((saved: Saved) => {
        this.scope.session.reset();
        this.refresh();
      },
      (error: Error) => {
        this.errorService.dialog(error);
      });
  }

  public activateContactMechanism(partyContactMechanism: PartyContactMechanism): void {
    partyContactMechanism.ThroughDate = undefined;
    this.scope
      .save()
      .subscribe((saved: Saved) => {
        this.scope.session.reset();
        this.refresh();
      },
      (error: Error) => {
        this.errorService.dialog(error);
      });
  }

  public deleteContactMechanism(contactMechanism: ContactMechanism): void {
    this.dialogService
      .openConfirm({ message: "Are you sure you want to delete this?" })
      .afterClosed().subscribe((confirm: boolean) => {
        if (confirm) {
          this.scope.invoke(contactMechanism.Delete)
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

  public cancelCommunication(communicationEvent: CommunicationEvent): void {
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

  public closeCommunication(communicationEvent: CommunicationEvent): void {
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

  public reopenCommunication(communicationEvent: CommunicationEvent): void {
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

  public deleteCommunication(communicationEvent: CommunicationEvent): void {
    this.dialogService
      .openConfirm({ message: "Are you sure you want to delete this?" })
      .afterClosed().subscribe((confirm: boolean) => {
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

  public delete(workEffort: WorkEffort): void {
    this.dialogService
      .openConfirm({ message: "Are you sure you want to delete this work effort?" })
      .afterClosed()
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.scope.invoke(workEffort.Delete)
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

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public goBack(): void {
    window.history.back();
  }

  public checkType(obj: any): string {
    return obj.objectType.name;
  }
}
