import { Observable, BehaviorSubject, Subject, Subscription } from 'rxjs/Rx';
import { Component, OnInit, AfterViewInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, UrlSegment } from '@angular/router';
import { MdSnackBar, MdSnackBarConfig } from '@angular/material';
import { TdMediaService, TdDialogService } from '@covalent/core';

import { MetaDomain } from '../../../../../meta';
import { PullRequest, Fetch, Path, Query, Equals, Like, TreeNode, Sort, Page } from '../../../../../domain';
import { CommunicationEvent, Person, Locale, Organisation, OrganisationContactRelationship } from '../../../../../domain';
import { AllorsService, ErrorService, Scope, Loaded, Saved, Invoked } from '../../../../../angular';

@Component({
  templateUrl: './person-overview.component.html',
})
export class PersonOverviewComponent implements OnInit, AfterViewInit, OnDestroy {

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;
  m: MetaDomain;

  communicationEvents: CommunicationEvent[];

  person: Person;
  organisation: Organisation;

  constructor(
    private allors: AllorsService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private dialogService: TdDialogService,
    private snackBar: MdSnackBar,

    public media: TdMediaService) {

    this.scope = new Scope(allors.database, allors.workspace);
    this.m = this.allors.meta;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  ngOnInit(): void {

    const route$: Observable<UrlSegment[]> = this.route.url;

    const combined$: Observable<[UrlSegment[], Date]> = Observable.combineLatest(route$, this.refresh$);

    this.subscription = combined$
      .switchMap((url: any) => {

        const id: string = this.route.snapshot.paramMap.get('id');
        const m: MetaDomain = this.m;

        const fetch: Fetch[] = [
          new Fetch({
            name: 'person',
            id: id,
            include: [
              new TreeNode({ roleType: m.Party.Locale }),
              new TreeNode({ roleType: m.Person.PersonRoles }),
              new TreeNode({ roleType: m.Person.LastModifiedBy }),
              new TreeNode({
                roleType: m.Party.PartyContactMechanisms,
                nodes: [
                  new TreeNode({ roleType: m.PartyContactMechanism.ContactPurposes }),
                  new TreeNode({ roleType: m.PartyContactMechanism.ContactMechanism }),
                ],
              }),
              new TreeNode({
                roleType: m.Party.CurrentPartyContactMechanisms,
                nodes: [
                  new TreeNode({ roleType: m.PartyContactMechanism.ContactPurposes }),
                  new TreeNode({ roleType: m.PartyContactMechanism.ContactMechanism }),
                ],
              }),
              new TreeNode({
                roleType: m.Party.InactivePartyContactMechanisms,
                nodes: [
                  new TreeNode({ roleType: m.PartyContactMechanism.ContactPurposes }),
                  new TreeNode({ roleType: m.PartyContactMechanism.ContactMechanism }),
                ],
              }),
              new TreeNode({
                roleType: m.Person.GeneralCorrespondence,
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
              new TreeNode({ roleType: m.CommunicationEvent.InvolvedParties }),
            ],
          }),
          new Fetch({
            name: 'organisationContactRelationships',
            id: id,
            path: new Path({ step: m.Person.OrganisationContactRelationshipsWhereContact }),
            include: [
              new TreeNode({ roleType: m.OrganisationContactRelationship.Organisation }),
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

        this.person = loaded.objects.person as Person;
        const organisationContactRelationships: OrganisationContactRelationship[] = loaded.collections.organisationContactRelationships as OrganisationContactRelationship[];
        this.organisation = organisationContactRelationships.length > 0 ? organisationContactRelationships[0].Organisation as Organisation : undefined;
        this.communicationEvents = loaded.collections.communicationEvents as CommunicationEvent[];
      },
      (error: any) => {
        this.errorService.message(error);
        this.goBack();
      },
    );
  }

  delete(communicationEvent: CommunicationEvent): void {
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

  ngAfterViewInit(): void {
    this.media.broadcast();
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  refresh(): void {
    this.refresh$.next(new Date());
  }

  goBack(): void {
    window.history.back();
  }

  checkType(obj: any): string {
    return obj.objectType.name;
  }
}
