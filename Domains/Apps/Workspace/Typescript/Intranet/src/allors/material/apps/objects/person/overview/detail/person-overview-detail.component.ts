import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Subscription } from 'rxjs';
import { switchMap, filter } from 'rxjs/operators';

import { ContextService, NavigationService, PanelService, RefreshService, MetaService, FetcherService, TestScope } from '../../../../../../angular';
import { Enumeration, InternalOrganisation, Locale, Organisation, Person } from '../../../../../../domain';
import { Equals, PullRequest, Sort } from '../../../../../../framework';
import { Meta } from '../../../../../../meta';
import { SaveService } from '../../../../../../../allors/material';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'person-overview-detail',
  templateUrl: './person-overview-detail.component.html',
  providers: [PanelService, ContextService]
})
export class PersonOverviewDetailComponent extends TestScope implements OnInit, OnDestroy {

  readonly m: Meta;

  person: Person;

  internalOrganisation: InternalOrganisation;
  locales: Locale[];
  genders: Enumeration[];
  salutations: Enumeration[];

  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    @Self() public panel: PanelService,
    private metaService: MetaService,
    public refreshService: RefreshService,
    public navigationService: NavigationService,
    private saveService: SaveService,
    private fetcher: FetcherService,
  ) {
    super();

    this.m = this.metaService.m;

    panel.name = 'detail';
    panel.title = 'Personal Data';
    panel.icon = 'person';
    panel.expandable = true;

    // Minimized
    const pullName = `${this.panel.name}_${this.m.Person.name}`;

    panel.onPull = (pulls) => {

      this.person = undefined;

      if (this.panel.isCollapsed) {
        const { pull, x } = this.metaService;
        const id = this.panel.manager.id;

        pulls.push(
          pull.Person({
            name: pullName,
            object: id,
            include: {
              GeneralEmail: x,
              PersonalEmailAddress: x,
            }
          })
        );
      }
    };

    panel.onPulled = (loaded) => {
      if (this.panel.isCollapsed) {
        this.person = loaded.objects[pullName] as Person;
      }
    };
  }

  public ngOnInit(): void {

    // Maximized
    this.subscription = this.panel.manager.on$
      .pipe(
        filter(() => {
          return this.panel.isExpanded;
        }),
        switchMap(() => {

          this.person = undefined;

          const { m, pull, x } = this.metaService;
          const id = this.panel.manager.id;

          const pulls = [
            this.fetcher.internalOrganisation,
            this.fetcher.locales,
            pull.GenderType({
              predicate: new Equals({ propertyType: m.GenderType.IsActive, value: true }),
              sort: new Sort(m.GenderType.Name),
            }),
            pull.Salutation({
              predicate: new Equals({ propertyType: m.Salutation.IsActive, value: true }),
              sort: new Sort(m.Salutation.Name),
            }),
            pull.Person({
              object: id,
              fetch: {
                OrganisationContactRelationshipsWhereContact: x
              }
            }),
            pull.Person({
              object: id,
              include: {
                Gender: x,
                Salutation: x,
                Locale: x,
                Picture: x,
              }
            }),
          ];

          return this.allors.context.load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        this.person = loaded.objects.Person as Person;
        this.internalOrganisation = loaded.objects.InternalOrganisation as Organisation;
        this.locales = loaded.collections.AdditionalLocales as Locale[];
        this.genders = loaded.collections.GenderTypes as Enumeration[];
        this.salutations = loaded.collections.Salutations as Enumeration[];
      });

  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    this.allors.context.save()
      .subscribe(() => {
        this.panel.toggle();
      },
        this.saveService.errorHandler
      );
  }
}
