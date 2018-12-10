import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute } from '@angular/router';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';

import { ErrorService, ContextService, NavigationService, NavigationActivatedRoute, MetaService, PanelService, RefreshService } from '../../../../../../angular';
import { CommunicationEventPurpose, ContactMechanism, EmailAddress, EmailCommunication, EmailTemplate, InternalOrganisation, Party, PartyContactMechanism, Person, Organisation } from '../../../../../../domain';
import { PullRequest, Sort, Equals } from '../../../../../../framework';
import { MetaDomain } from '../../../../../../meta';
import { StateService } from '../../../../services/state';
import { AllorsMaterialDialogService } from '../../../../../base/services/dialog';
import { switchMap, map, filter } from 'rxjs/operators';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'emailcommunication-overview-detail',
  templateUrl: './emailcommunication-overview-detail.component.html',
  providers: [ContextService, PanelService]
})
export class EmailCommunicationOverviewDetailComponent implements OnInit, OnDestroy {

  public title = 'Email Communication';

  public addOriginator = false;
  public addAddressee = false;

  public m: MetaDomain;

  public party: Party;
  public person: Person;
  public organisation: Organisation;
  public purposes: CommunicationEventPurpose[];
  public contacts: Party[] = [];

  public ownEmailAddresses: EmailAddress[] = [];
  public allEmailAddresses: EmailAddress[];
  public emailTemplate: EmailTemplate;

  public emailCommunication: EmailCommunication;

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
    panel.icon = 'email';
    panel.expandable = true;

    // Minimized
    const pullName = `${this.panel.name}_${this.m.EmailCommunication.objectType.name}`;

    panel.onPull = (pulls) => {
      if (this.panel.isCollapsed) {
        const { pull, x } = this.metaService;
        const id = this.panel.manager.id;

        pulls.push(
          pull.EmailCommunication({
            name: pullName,
            object: id,
            include: {
              Originator: x,
              Addressees: x,
              EmailTemplate: x,
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
      if (this.panel.isCollapsed) {
        this.emailCommunication = loaded.objects[pullName] as EmailCommunication;
      }
    };
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.route.url, this.route.queryParams, this.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        filter((v) => {
          return this.panel.isExpanded;
        }),
        switchMap(([, , , internalOrganisationId]) => {

          const navRoute = new NavigationActivatedRoute(this.route);

          const id = this.panel.manager.id;
          const personId = navRoute.queryParam(m.Person);
          const organisationId = navRoute.queryParam(m.Organisation);

          let pulls = [
            pull.EmailCommunication({
              object: id,
              include: {
                Originator: x,
                Addressees: x,
                EmailTemplate: x,
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
                    ContactMechanism: x,
                  }
                }
              }
            }),
            pull.EmailAddress({
              sort: new Sort(m.EmailAddress.ElectronicAddressString)
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
        this.allEmailAddresses = loaded.collections.EmailAddresses as EmailAddress[];
        this.ownEmailAddresses = internalOrganisation.ActiveEmployees
          .map((v) => v.CurrentPartyContactMechanisms
            .filter((w) => w && w.ContactMechanism.objectType === m.EmailAddress.objectType)
            .map((w) => w.ContactMechanism as EmailAddress))
          .reduce((acc, v) => acc.concat(v), []);

        this.emailCommunication = loaded.objects.EmailCommunication as EmailCommunication;
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

  public cancel(): void {

    const cancelFn: () => void = () => {
      this.allors.context.invoke(this.emailCommunication.Cancel)
        .subscribe(() => {
          this.refresh();
          this.snackBar.open('Successfully cancelled.', 'close', { duration: 5000 });
        },
          (error: Error) => {
            this.errorService.handle(error);
          });
    };

    if (this.allors.context.hasChanges) {
      this.dialogService
        .confirm({ message: 'Save changes?' })
        .subscribe((confirm: boolean) => {
          if (confirm) {
            this.allors.context.save()
              .subscribe(() => {
                this.allors.context.reset();
                cancelFn();
              },
                (error: Error) => {
                  this.errorService.handle(error);
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
      this.allors.context.invoke(this.emailCommunication.Close)
        .subscribe(() => {
          this.refresh();
          this.snackBar.open('Successfully closed.', 'close', { duration: 5000 });
        },
          (error: Error) => {
            this.errorService.handle(error);
          });
    };

    if (this.allors.context.hasChanges) {
      this.dialogService
        .confirm({ message: 'Save changes?' })
        .subscribe((confirm: boolean) => {
          if (confirm) {
            this.allors.context.save()
              .subscribe(() => {
                this.allors.context.reset();
                cancelFn();
              },
                (error: Error) => {
                  this.errorService.handle(error);
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
      this.allors.context.invoke(this.emailCommunication.Reopen)
        .subscribe(() => {
          this.refresh();
          this.snackBar.open('Successfully reopened.', 'close', { duration: 5000 });
        },
          (error: Error) => {
            this.errorService.handle(error);
          });
    };

    if (this.allors.context.hasChanges) {
      this.dialogService
        .confirm({ message: 'Save changes?' })
        .subscribe((confirm: boolean) => {
          if (confirm) {
            this.allors.context.save()
              .subscribe(() => {
                this.allors.context.reset();
                cancelFn();
              },
                (error: Error) => {
                  this.errorService.handle(error);
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
}
