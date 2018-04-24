import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { MatSnackBar } from "@angular/material";
import { ActivatedRoute, UrlSegment } from "@angular/router";
import { TdDialogService, TdMediaService } from "@covalent/core";

import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";

import "rxjs/add/observable/combineLatest";

import { ErrorService, Invoked, Loaded, Saved, Scope, WorkspaceService } from "../../../../../angular";
import { CommunicationEvent, ContactMechanism, InternalOrganisation, Organisation, OrganisationContactRelationship, OrganisationRole, PartyContactMechanism, Person, TelecommunicationsNumber } from "../../../../../domain";
import { And, Equals, Exists, Fetch, Not, Path, Predicate, PullRequest, Query, TreeNode } from "../../../../../framework";
import { MetaDomain } from "../../../../../meta";
import { StateService } from "../../../services/StateService";
import { Fetcher } from "../../Fetcher";

@Component({
  templateUrl: "./organisation-overview.component.html",
})
export class OrganisationOverviewComponent implements OnInit, OnDestroy {

  public m: MetaDomain;

  public title: string = "Organisation overview";
  public organisation: Organisation;
  public internalOrganisation: InternalOrganisation;
  public communicationEvents: CommunicationEvent[];

  public contactsCollection: string = "Current";
  public currentContactRelationships: OrganisationContactRelationship[] = [];
  public inactiveContactRelationships: OrganisationContactRelationship[] = [];
  public allContactRelationships: OrganisationContactRelationship[] = [];

  public contactMechanismsCollection: string = "Current";
  public currentContactMechanisms: PartyContactMechanism[] = [];
  public inactiveContactMechanisms: PartyContactMechanism[] = [];
  public allContactMechanisms: PartyContactMechanism[] = [];

  public roles: OrganisationRole[];
  public activeRoles: OrganisationRole[] = [];
  public rolesText: string;
  private customerRole: OrganisationRole;
  private supplierRole: OrganisationRole;
  private manufacturerRole: OrganisationRole;
  private isActiveCustomer: boolean;
  private isActiveSupplier: boolean;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private fetcher: Fetcher;
  private scope: Scope;

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private dialogService: TdDialogService,
    private snackBar: MatSnackBar,
    public media: TdMediaService,
    private changeDetectorRef: ChangeDetectorRef,
    private stateService: StateService) {

    this.scope = this.workspaceService.createScope();
    this.m = this.workspaceService.metaPopulation.metaDomain;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.fetcher = new Fetcher(this.stateService, this.m);
  }

  get contactRelationships(): any {

    switch (this.contactsCollection) {
      case "Current":
        return this.currentContactRelationships;
      case "Inactive":
        return this.inactiveContactRelationships;
      case "All":
      default:
        return this.allContactRelationships;
    }
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

        const organisationContactRelationshipTreeNodes: TreeNode[] = [
          new TreeNode({ roleType: m.OrganisationContactRelationship.ContactKinds }),
          new TreeNode({
            nodes: [
              new TreeNode({ roleType: m.Person.Salutation }),
              new TreeNode({
                nodes: [
                  new TreeNode({
                    nodes: [
                      new TreeNode({ roleType: m.ContactMechanism.ContactMechanismType }),
                    ],
                    roleType: m.PartyContactMechanism.ContactMechanism,
                  }),
                ],
                roleType: m.Person.PartyContactMechanisms,
              }),
            ],
            roleType: m.OrganisationContactRelationship.Contact,
          }),
        ];

        const partyContactMechanismTreeNodes: TreeNode[] = [
          new TreeNode({ roleType: m.PartyContactMechanism.ContactPurposes }),
          new TreeNode({
            nodes: [
              new TreeNode({ roleType: m.ContactMechanism.ContactMechanismType }),
              new TreeNode({
                nodes: [
                  new TreeNode({ roleType: m.PostalBoundary.Country }),
                ],
                roleType: m.PostalAddress.PostalBoundary,
              }),
            ],
            roleType: m.PartyContactMechanism.ContactMechanism,
          }),
        ];

        const fetches: Fetch[] = [
          this.fetcher.internalOrganisation,
          new Fetch({
            id,
            include: [
              new TreeNode({ roleType: m.Party.Locale }),
              new TreeNode({ roleType: m.Organisation.LogoImage }),
              new TreeNode({ roleType: m.Organisation.IndustryClassifications }),
              new TreeNode({ roleType: m.Organisation.OrganisationClassifications }),
              new TreeNode({ roleType: m.Organisation.LastModifiedBy }),
              new TreeNode({
                nodes: [
                  new TreeNode({
                    nodes: partyContactMechanismTreeNodes,
                    roleType: m.Person.PartyContactMechanisms,
                  }),
                ],
                roleType: m.Party.CurrentContacts,
              }),
              new TreeNode({
                nodes: organisationContactRelationshipTreeNodes,
                roleType: m.Party.CurrentOrganisationContactRelationships,
              }),
              new TreeNode({
                nodes: organisationContactRelationshipTreeNodes,
                roleType: m.Party.InactiveOrganisationContactRelationships,
              }),
              new TreeNode({
                nodes: partyContactMechanismTreeNodes,
                roleType: m.Party.PartyContactMechanisms,
              }),
              new TreeNode({
                nodes: partyContactMechanismTreeNodes,
                roleType: m.Party.CurrentPartyContactMechanisms,
              }),
              new TreeNode({
                nodes: partyContactMechanismTreeNodes,
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
                roleType: m.Organisation.GeneralCorrespondence,
              }),
            ],
            name: "organisation",
          }),
          new Fetch({
            id,
            include: [
              new TreeNode({ roleType: m.CommunicationEvent.CommunicationEventState }),
              new TreeNode({ roleType: m.CommunicationEvent.FromParties }),
              new TreeNode({ roleType: m.CommunicationEvent.ToParties }),
            ],
            name: "communicationEvents",
            path: new Path({ step: m.Party.CommunicationEventsWhereInvolvedParty }),
          }),
        ];

        const queries: Query[] = [
          new Query(this.m.OrganisationRole),
          ];

        return this.scope
          .load("Pull", new PullRequest({ fetches, queries }));
      })
      .subscribe((loaded) => {
        this.scope.session.reset();
        this.internalOrganisation = loaded.objects.internalOrganisation as InternalOrganisation;

        this.organisation = loaded.objects.organisation as Organisation;
        this.communicationEvents = loaded.collections.communicationEvents as CommunicationEvent[];

        this.currentContactRelationships = this.organisation.CurrentOrganisationContactRelationships as OrganisationContactRelationship[];
        this.inactiveContactRelationships = this.organisation.InactiveOrganisationContactRelationships as OrganisationContactRelationship[];
        this.allContactRelationships = this.currentContactRelationships.concat(this.inactiveContactRelationships);

        this.currentContactMechanisms = this.organisation.CurrentPartyContactMechanisms as PartyContactMechanism[];
        this.inactiveContactMechanisms = this.organisation.InactivePartyContactMechanisms as PartyContactMechanism[];
        this.allContactMechanisms = this.currentContactMechanisms.concat(this.inactiveContactMechanisms);

        this.roles = loaded.collections.OrganisationRoleQuery as OrganisationRole[];
        this.customerRole = this.roles.find((v: OrganisationRole) => v.UniqueId.toUpperCase() === "8B5E0CEE-4C98-42F1-8F18-3638FBA943A0");
        this.supplierRole = this.roles.find((v: OrganisationRole) => v.UniqueId.toUpperCase() === "8C6D629B-1E27-4520-AA8C-E8ADF93A5095");
        this.manufacturerRole = this.roles.find((v: OrganisationRole) => v.UniqueId.toUpperCase() === "32E74BEF-2D79-4427-8902-B093AFA81661");

        this.activeRoles = [];
        this.rolesText = "";
        if (this.internalOrganisation.ActiveCustomers.includes(this.organisation)) {
          this.isActiveCustomer = true;
          this.activeRoles.push(this.customerRole);
        }

        if (this.internalOrganisation.ActiveSuppliers.includes(this.organisation)) {
          this.isActiveSupplier = true;
          this.activeRoles.push(this.supplierRole);
        }

        if (this.organisation.IsManufacturer) {
          this.activeRoles.push(this.manufacturerRole);
        }

        if (this.activeRoles.length > 0) {
        this.rolesText = this.activeRoles
          .map((v: OrganisationRole) => v.Name)
          .reduce((acc: string, cur: string) => acc + ", " + cur);
        }
      },
      (error: any) => {
        this.errorService.message(error);
        this.goBack();
      },
    );
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public removeContact(organisationContactRelationship: OrganisationContactRelationship): void {
    organisationContactRelationship.ThroughDate = new Date();
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

  public activateContact(organisationContactRelationship: OrganisationContactRelationship): void {
    organisationContactRelationship.ThroughDate = undefined;
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

  public deleteContact(person: Person): void {
    this.dialogService
      .openConfirm({ message: "Are you sure you want to delete this?" })
      .afterClosed().subscribe((confirm: boolean) => {
        if (confirm) {
          this.scope.invoke(person.Delete)
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

  public goBack(): void {
    window.history.back();
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public isPhone(contactMechanism: ContactMechanism): boolean {
    if (contactMechanism.objectType.name === "TelecommunicationsNumber") {
      const phoneCommunication: TelecommunicationsNumber = contactMechanism as TelecommunicationsNumber;
      return phoneCommunication.ContactMechanismType && phoneCommunication.ContactMechanismType.Name === "Phone";
    }
  }

  public checkType(obj: any): string {
    return obj.objectType.name;
  }
}
