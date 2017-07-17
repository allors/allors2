import { Observable, BehaviorSubject, Subscription } from 'rxjs/Rx';
import { Component, OnInit, AfterViewInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, UrlSegment } from '@angular/router';
import { MdSnackBar, MdSnackBarConfig } from '@angular/material';
import { TdDialogService, TdMediaService } from '@covalent/core';

import { MetaDomain } from '../../../../../meta';
import { PullRequest, Fetch, Path, Query, Equals, Like, TreeNode, Sort, Page } from '../../../../../domain';
import { CommunicationEvent, Organisation, Locale } from '../../../../../domain';
import { AllorsService, ErrorService, Scope, Loaded, Saved, Invoked } from '../../../../../angular';

@Component({
  templateUrl: './organisation-overview.component.html',
})
export class OrganisationOverviewComponent implements OnInit, AfterViewInit, OnDestroy {

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;

  private scope: Scope;
  m: MetaDomain;

  communicationEvents: CommunicationEvent[];

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
      .switchMap(([urlSegments, date]: [UrlSegment[], Date]) => {

        const id: string = this.route.snapshot.paramMap.get('id');
        const m: MetaDomain = this.m;

        const fetch: Fetch[] = [
          new Fetch({
            name: 'organisation',
            id: id,
            include: [
              new TreeNode({ roleType: m.Party.Locale }),
              new TreeNode({ roleType: m.Organisation.OrganisationRoles }),
              new TreeNode({ roleType: m.Organisation.LastModifiedBy }),
              new TreeNode({
                roleType: m.Party.CurrentContacts,
                nodes: [
                  new TreeNode({
                    roleType: m.Person.PartyContactMechanisms,
                    nodes: [
                      new TreeNode({ roleType: m.PartyContactMechanism.ContactPurposes }),
                      new TreeNode({ roleType: m.PartyContactMechanism.ContactMechanism }),
                    ],
                  }),
                ],
              }),
              new TreeNode({
                roleType: m.Party.CurrentOrganisationContactRelationships,
                nodes: [
                  new TreeNode({ roleType: m.OrganisationContactRelationship.ContactKinds }),
                  new TreeNode({ roleType: m.OrganisationContactRelationship.Contact }),
                ],
              }),
              new TreeNode({
                roleType: m.Party.InactiveOrganisationContactRelationships,
                nodes: [
                  new TreeNode({ roleType: m.OrganisationContactRelationship.ContactKinds }),
                  new TreeNode({ roleType: m.OrganisationContactRelationship.Contact }),
                ],
              }),
              new TreeNode({
                roleType: m.Party.PartyContactMechanisms,
                nodes: [
                  new TreeNode({ roleType: m.PartyContactMechanism.ContactPurposes }),
                  new TreeNode({
                    roleType: m.PartyContactMechanism.ContactMechanism,
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
              new TreeNode({
                roleType: m.Party.CurrentPartyContactMechanisms,
                nodes: [
                  new TreeNode({ roleType: m.PartyContactMechanism.ContactPurposes }),
                  new TreeNode({
                    roleType: m.PartyContactMechanism.ContactMechanism,
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
              new TreeNode({
                roleType: m.Party.InactivePartyContactMechanisms,
                nodes: [
                  new TreeNode({ roleType: m.PartyContactMechanism.ContactPurposes }),
                  new TreeNode({
                    roleType: m.PartyContactMechanism.ContactMechanism,
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

        this.scope.session.reset();

        return this.scope
          .load('Pull', new PullRequest({ fetch: fetch, query: query }));
      })
      .subscribe((loaded: Loaded) => {
        this.organisation = loaded.objects.organisation as Organisation;
        this.communicationEvents = loaded.collections.communicationEvents as CommunicationEvent[];
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
