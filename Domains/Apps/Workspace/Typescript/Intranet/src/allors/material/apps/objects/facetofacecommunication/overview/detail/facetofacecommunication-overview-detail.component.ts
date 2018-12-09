import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute } from '@angular/router';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';

import { ErrorService, ContextService, NavigationService, NavigationActivatedRoute, MetaService, PanelService, RefreshService } from '../../../../../../angular';
import { CommunicationEventPurpose, InternalOrganisation, Party, Person, Organisation, FaceToFaceCommunication, OrganisationContactRelationship } from '../../../../../../domain';
import { PullRequest, Sort, Equals } from '../../../../../../framework';
import { MetaDomain } from '../../../../../../meta';
import { StateService } from '../../../../services/state';
import { AllorsMaterialDialogService } from '../../../../../base/services/dialog';
import { switchMap, filter } from 'rxjs/operators';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'facetofacecommunication-overview-detail',
  templateUrl: './facetofacecommunication-overview-detail.component.html',
  providers: [ContextService, PanelService]
})
export class FaceToFaceCommunicationOverviewDetailComponent implements OnInit, OnDestroy {

  public title = 'Email Communication';

  public addOriginator = false;
  public addAddressee = false;

  public m: MetaDomain;

  addParticipant = false;

  party: Party;
  person: Person;
  organisation: Organisation;
  purposes: CommunicationEventPurpose[];
  contacts: Party[] = [];

  faceToFaceCommunication: FaceToFaceCommunication;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;

  constructor(
    @Self() private allors: ContextService,
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    public navigation: NavigationService,
    private errorService: ErrorService,
    private dialogService: AllorsMaterialDialogService,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private stateService: StateService) {

    this.m = this.metaService.m;
    this.refresh$ = new BehaviorSubject<Date>(undefined);

    panel.name = 'detail';
    panel.title = 'Details';
    panel.icon = 'meeting_room';
    panel.maximizable = true;

    // Minimized
    const pullName = `${this.panel.name}_${this.m.FaceToFaceCommunication.objectType.name}`;

    panel.onPull = (pulls) => {
      if (this.panel.isNormal) {
        const { pull, x } = this.metaService;
        const id = this.panel.manager.id;

        pulls.push(
          pull.FaceToFaceCommunication({
            name: pullName,
            object: id,
            include: {
              FromParties: x,
              ToParties: x,
              EventPurposes: x,
              CommunicationEventState: x,
              ContactMechanisms: x,
              WorkEfforts: {
                WorkEffortState: x,
                Priority: x,
              }
            }
          })
        );
      }
    };

    panel.onPulled = (loaded) => {
      if (this.panel.isNormal) {
        this.faceToFaceCommunication = loaded.objects[pullName] as FaceToFaceCommunication;
      }
    };
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.route.url, this.route.queryParams, this.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        filter((v) => {
          return this.panel.isMaximized;
        }),
        switchMap(([, , , internalOrganisationId]) => {

          const navRoute = new NavigationActivatedRoute(this.route);
          const id = this.panel.manager.id;
          const personId = navRoute.queryParam(m.Person);
          const organisationId = navRoute.queryParam(m.Organisation);

          let pulls = [
            pull.FaceToFaceCommunication({
              object: id,
              include: {
                Participants: x,
                EventPurposes: x,
                CommunicationEventState: x,
              }
            }),
            pull.Organisation({
              object: internalOrganisationId,
              name: 'InternalOrganisation',
              include: {
                ActiveEmployees: {
                  CurrentPartyContactMechanisms: {
                    ContactMechanism: x
                  }
                }
              }
            }),
            pull.CommunicationEventPurpose({
              predicate: new Equals({ propertyType: m.CommunicationEventPurpose.IsActive, value: true }),
              sort: new Sort(m.CommunicationEventPurpose.Name)
            }),
          ];

          if (!!organisationId) {
            pulls = [
              ...pulls,
              pull.Organisation({
                object: organisationId,
                include: {
                  CurrentContacts: x,
                  CurrentPartyContactMechanisms: {
                    ContactMechanism: x,
                  }
                }
              }
              )
            ];
          }

          if (!!personId) {
            pulls = [
              ...pulls,
              pull.Person({
                object: personId,
              }),
              pull.Person({
                object: personId,
                fetch: {
                  OrganisationContactRelationshipsWhereContact: {
                    Organisation: {
                      include: {
                        CurrentContacts: x,
                        CurrentPartyContactMechanisms: {
                          ContactMechanism: x,
                        }
                      }
                    }
                  }
                }
              })
            ];
          }

          return this.allors.context.load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        this.allors.context.reset();

        this.purposes = loaded.collections.CommunicationEventPurposes as CommunicationEventPurpose[];
        const internalOrganisation = loaded.objects.InternalOrganisation as InternalOrganisation;

        this.person = loaded.objects.Person as Person;
        this.organisation = loaded.objects.Organisation as Organisation;
        if (!this.organisation && loaded.collections.Organisations && loaded.collections.Organisations.length > 0) {
          // TODO: check active
          this.organisation = loaded.collections.Organisations[0] as Organisation;
        }

        this.party = this.organisation || this.person;

        this.contacts = this.contacts.concat(internalOrganisation.ActiveEmployees);
        this.contacts = this.contacts.concat(this.organisation && this.organisation.CurrentContacts);
        if (!!this.person) {
          this.contacts.push(this.person);
        }

        this.faceToFaceCommunication = loaded.objects.FaceToFaceCommunication as FaceToFaceCommunication;
      },
        (error: any) => {
          this.errorService.handle(error);
          this.goBack();
        },
      );
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    this.allors.context.save()
      .subscribe(() => {
        this.goBack();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public goBack(): void {
    window.history.back();
  }

  public participantCancelled(): void {
    this.addParticipant = false;
  }

  public participantAdded(id: string): void {

    this.addParticipant = false;

    const participant: Person = this.allors.context.get(id) as Person;
    this.faceToFaceCommunication.AddParticipant(participant);

    if (!!this.organisation) {
      const relationShip: OrganisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
      relationShip.Contact = participant;
      relationShip.Organisation = this.organisation;
    }

  }
}
