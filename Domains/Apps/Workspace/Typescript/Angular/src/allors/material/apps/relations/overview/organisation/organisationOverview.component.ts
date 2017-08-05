import { Observable, BehaviorSubject, Subscription } from 'rxjs/Rx';
import { Component, OnInit, AfterViewInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute, UrlSegment } from '@angular/router';
import { MdSnackBar, MdSnackBarConfig } from '@angular/material';
import { TdDialogService, TdMediaService } from '@covalent/core';

import { MetaDomain } from '../../../../../meta';
import { PullRequest, Fetch, Path, Query, Equals, Like, TreeNode, Sort, Page } from '../../../../../domain';
import { CommunicationEvent, ContactMechanism, Locale, Organisation, OrganisationContactRelationship, PartyContactMechanism, Person, } from '../../../../../domain';
import { AllorsService, ErrorService, Scope, Loaded, Saved, Invoked } from '../../../../../angular';

@Component({
  templateUrl: './organisationOverview.component.html',
})
export class OrganisationOverviewComponent implements OnInit, AfterViewInit, OnDestroy {

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;

  private scope: Scope;
  m: MetaDomain;

  title: string = 'Organisation overview';
  organisation: Organisation;
  communicationEvents: CommunicationEvent[];

  contactsCollection: string = 'Current';
  currentContactRelationships: OrganisationContactRelationship[] = [];
  inactiveContactRelationships: OrganisationContactRelationship[] = [];
  allContactRelationships: OrganisationContactRelationship[] = [];

  contactMechanismsCollection: string = 'Current';
  currentContactMechanisms: PartyContactMechanism[] = [];
  inactiveContactMechanisms: PartyContactMechanism[] = [];
  allContactMechanisms: PartyContactMechanism[] = [];

  constructor(
    private allors: AllorsService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private dialogService: TdDialogService,
    private snackBar: MdSnackBar,
    public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef) {

    this.scope = new Scope(allors.database, allors.workspace);
    this.m = this.allors.meta;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  get contactRelationships(): any {

    switch (this.contactsCollection) {
      case 'Current':
        return this.currentContactRelationships;
      case 'Inactive':
        return this.inactiveContactRelationships;
      case 'All':
      default:
        return this.allContactRelationships;
    }
  }

  get contactMechanisms(): any {

    switch (this.contactMechanismsCollection) {
      case 'Current':
        return this.currentContactMechanisms;
      case 'Inactive':
        return this.inactiveContactMechanisms;
      case 'All':
      default:
        return this.allContactMechanisms;
    }
  }

  ngOnInit(): void {

    const route$: Observable<UrlSegment[]> = this.route.url;

    const combined$: Observable<[UrlSegment[], Date]> = Observable.combineLatest(route$, this.refresh$);

    this.subscription = combined$
      .switchMap(([urlSegments, date]: [UrlSegment[], Date]) => {

        const id: string = this.route.snapshot.paramMap.get('id');
        const m: MetaDomain = this.m;

        const organisationContactRelationshipTreeNodes: TreeNode[] = [
          new TreeNode({ roleType: m.OrganisationContactRelationship.ContactKinds }),
          new TreeNode({
            roleType: m.OrganisationContactRelationship.Contact,
            nodes: [
              new TreeNode({
                roleType: m.Person.PartyContactMechanisms,
                nodes: [
                  new TreeNode({
                    roleType: m.PartyContactMechanism.ContactMechanism,
                    nodes: [
                      new TreeNode({ roleType: m.ContactMechanism.ContactMechanismType }),
                    ],
                  }),
                ],
              }),
            ],
          }),
        ];

        const partyContactMechanismTreeNodes: TreeNode[] = [
          new TreeNode({ roleType: m.PartyContactMechanism.ContactPurposes }),
          new TreeNode({
            roleType: m.PartyContactMechanism.ContactMechanism,
            nodes: [
              new TreeNode({ roleType: m.ContactMechanism.ContactMechanismType }),
              new TreeNode({
                roleType: m.PostalAddress.PostalBoundary,
                nodes: [
                  new TreeNode({ roleType: m.PostalBoundary.Country }),
                ],
              }),
            ],
          }),
        ];

        const fetch: Fetch[] = [
          new Fetch({
            name: 'organisation',
            id: id,
            include: [
              new TreeNode({ roleType: m.Party.Locale }),
              new TreeNode({ roleType: m.Organisation.IndustryClassifications }),
              new TreeNode({ roleType: m.Organisation.OrganisationRoles }),
              new TreeNode({ roleType: m.Organisation.OrganisationClassifications }),
              new TreeNode({ roleType: m.Organisation.LastModifiedBy }),
              new TreeNode({
                roleType: m.Party.CurrentContacts,
                nodes: [
                  new TreeNode({
                    roleType: m.Person.PartyContactMechanisms,
                    nodes: partyContactMechanismTreeNodes,
                  }),
                ],
              }),
              new TreeNode({
                roleType: m.Party.CurrentOrganisationContactRelationships,
                nodes: organisationContactRelationshipTreeNodes,
              }),
              new TreeNode({
                roleType: m.Party.InactiveOrganisationContactRelationships,
                nodes: organisationContactRelationshipTreeNodes,
              }),
              new TreeNode({
                roleType: m.Party.PartyContactMechanisms,
                nodes: partyContactMechanismTreeNodes,
              }),
              new TreeNode({
                roleType: m.Party.CurrentPartyContactMechanisms,
                nodes: partyContactMechanismTreeNodes,
              }),
              new TreeNode({
                roleType: m.Party.InactivePartyContactMechanisms,
                nodes: partyContactMechanismTreeNodes,
              }),
              new TreeNode({
                roleType: m.Organisation.GeneralCorrespondence,
                nodes: [
                  new TreeNode({
                    roleType: m.PostalAddress.PostalBoundary,
                    nodes: [
                      new TreeNode({ roleType: m.PostalBoundary.Country }),
                    ],
                  }),
                ],
              }),
            ],
          }),
          new Fetch({
            name: 'communicationEvents',
            id: id,
            path: new Path({ step: m.Party.CommunicationEventsWhereInvolvedParty }),
            include: [
              new TreeNode({ roleType: m.CommunicationEvent.CurrentObjectState }),
              new TreeNode({ roleType: m.CommunicationEvent.FromParties }),
              new TreeNode({ roleType: m.CommunicationEvent.ToParties }),
            ],
          }),
        ];

        const query: Query[] = [
          new Query(
            {
              name: 'countries',
              objectType: m.Country,
            }),
          new Query(
            {
              name: 'genders',
              objectType: m.GenderType,
            }),
          new Query(
            {
              name: 'salutations',
              objectType: m.Salutation,
            }),
          new Query(
            {
              name: 'organisationContactKinds',
              objectType: m.OrganisationContactKind,
            }),
          new Query(
            {
              name: 'contactMechanismPurposes',
              objectType: m.ContactMechanismPurpose,
            }),
          new Query(
            {
              name: 'internalOrganisation',
              objectType: m.InternalOrganisation,
            }),
        ];

        return this.scope
          .load('Pull', new PullRequest({ fetch: fetch, query: query }));
      })
      .subscribe((loaded: Loaded) => {
        this.scope.session.reset();

        this.organisation = loaded.objects.organisation as Organisation;
        this.communicationEvents = loaded.collections.communicationEvents as CommunicationEvent[];

        this.currentContactRelationships = this.organisation.CurrentOrganisationContactRelationships as OrganisationContactRelationship[];
        this.inactiveContactRelationships = this.organisation.InactiveOrganisationContactRelationships as OrganisationContactRelationship[];
        this.allContactRelationships = this.currentContactRelationships.concat(this.inactiveContactRelationships);

        this.currentContactMechanisms = this.organisation.CurrentPartyContactMechanisms as PartyContactMechanism[];
        this.inactiveContactMechanisms = this.organisation.InactivePartyContactMechanisms as PartyContactMechanism[];
        this.allContactMechanisms = this.currentContactMechanisms.concat(this.inactiveContactMechanisms);
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

  removeContact(organisationContactRelationship: OrganisationContactRelationship): void {
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

  activateContact(organisationContactRelationship: OrganisationContactRelationship): void {
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

  deleteContact(person: Person): void {
    this.dialogService
      .openConfirm({ message: 'Are you sure you want to delete this?' })
      .afterClosed().subscribe((confirm: boolean) => {
        if (confirm) {
          this.scope.invoke(person.Delete)
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

  removeContactMechanism(partyContactMechanism: PartyContactMechanism): void {
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

  activateContactMechanism(partyContactMechanism: PartyContactMechanism): void {
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

  deleteContactMechanism(contactMechanism: ContactMechanism): void {
    this.dialogService
      .openConfirm({ message: 'Are you sure you want to delete this?' })
      .afterClosed().subscribe((confirm: boolean) => {
        if (confirm) {
          this.scope.invoke(contactMechanism.Delete)
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

  cancelCommunication(communicationEvent: CommunicationEvent): void {
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

  closeCommunication(communicationEvent: CommunicationEvent): void {
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

  reopenCommunication(communicationEvent: CommunicationEvent): void {
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

  deleteCommunication(communicationEvent: CommunicationEvent): void {
    this.dialogService
      .openConfirm({ message: 'Are you sure you want to delete this?' })
      .afterClosed().subscribe((confirm: boolean) => {
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

  goBack(): void {
    window.history.back();
  }

  refresh(): void {
    this.refresh$.next(new Date());
  }

  checkType(obj: any): string {
    return obj.objectType.name;
  }
}
