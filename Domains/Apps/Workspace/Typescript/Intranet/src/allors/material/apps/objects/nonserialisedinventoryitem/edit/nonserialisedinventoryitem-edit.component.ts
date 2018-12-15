import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';

import { ErrorService, ContextService, NavigationService, NavigationActivatedRoute, MetaService } from '../../../../../angular';
import { InternalOrganisation, Part, Facility, Lot, NonSerialisedInventoryItem, NonSerialisedInventoryItemState } from '../../../../../domain';
import { PullRequest, Sort } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { StateService } from '../../../services/state';
import { Fetcher } from '../../Fetcher';

@Component({
  templateUrl: './nonserialisedinventoryitem-edit.component.html',
  providers: [ContextService]
})
export class NonSerialisedInventoryItemEditComponent implements OnInit, OnDestroy {

  readonly m: Meta;

  readonly title = 'InventoryItem';

  internalOrganisation: InternalOrganisation;
  nonSerialisedInventoryItem: NonSerialisedInventoryItem;
  part: Part;
  selectedFacility: Facility;
  addFacility = false;
  facilities: Facility[];
  lots: Lot[];

  add: boolean;
  edit: boolean;

  private subscription: Subscription;
  private readonly refresh$: BehaviorSubject<Date>;
  private readonly fetcher: Fetcher;
  nonSerialisedInventoryItemStates: NonSerialisedInventoryItemState[];

  constructor(
    @Self() public allors: ContextService,
    public metaService: MetaService,
    public navigationService: NavigationService,
    public location: Location,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private titleService: Title,
    private stateService: StateService) {

    this.m = metaService.m;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.fetcher = new Fetcher(this.stateService, metaService.pull);
    this.titleService.setTitle(this.title);
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.route.url, this.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([, refresh, internalOrganisationId]) => {

          const navRoute = new NavigationActivatedRoute(this.route);
          const id = navRoute.id();
          const partId = navRoute.queryParam(m.Part);

          let pulls = [
            this.fetcher.internalOrganisation,
            this.fetcher.facilities,
            pull.NonSerialisedInventoryItemState({ sort: new Sort(m.NonSerialisedInventoryItemState.Name) }),
            pull.SerialisedInventoryItemState({ sort: new Sort(m.SerialisedInventoryItemState.Name) }),
            pull.Lot({ sort: new Sort(m.Lot.LotNumber) })
          ];

          if (partId) {
            pulls = [
              ...pulls,
              pull.Part({
                object: partId,
              })
            ];
          }

          const add = !id;

          return this.allors.context
            .load('Pull', new PullRequest({ pulls }))
            .pipe(
              map((loaded) => ({ loaded, add }))
            );
        })
      )
      .subscribe(({ loaded, add }) => {
        this.allors.context.reset();

        this.facilities = loaded.collections.Facilities as Facility[];
        this.lots  = loaded.collections.Lots as Lot[];
        this.nonSerialisedInventoryItemStates  = loaded.collections.NonSerialisedInventoryItemStates as NonSerialisedInventoryItemState[];

        this.part = loaded.objects.Part as Part;

        if (add) {
          this.add = !(this.edit = false);

          this.nonSerialisedInventoryItem = this.allors.context.create('NonSerialisedInventoryItem') as NonSerialisedInventoryItem;

          if (this.part) {
            this.nonSerialisedInventoryItem.Part = this.part;
          }

        } else {
          this.edit = !(this.add = false);
        }

      }, this.errorService.handler);
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    this.nonSerialisedInventoryItem.Facility = this.selectedFacility;

    this.allors.context
      .save()
      .subscribe(() => {
        this.location.back();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public facilityAdded(facility: Facility): void {
    this.facilities.push(facility);
    this.selectedFacility = facility;

    facility.Owner = this.internalOrganisation;
  }
}
