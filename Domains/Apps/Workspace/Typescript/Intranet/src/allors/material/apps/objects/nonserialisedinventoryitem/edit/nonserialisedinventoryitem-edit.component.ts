import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';

import { ErrorService, SessionService, NavigationService, NavigationActivatedRoute } from '../../../../../angular';
import { InternalOrganisation, Part, Facility, Lot, NonSerialisedInventoryItem } from '../../../../../domain';
import { PullRequest, Sort } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { StateService } from '../../../services/state';
import { Fetcher } from '../../Fetcher';

@Component({
  templateUrl: './nonserialisedinventoryitem-edit.component.html',
  providers: [SessionService]
})
export class NonSerialisedInventoryItemEditComponent implements OnInit, OnDestroy {

  readonly m: MetaDomain;

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

  constructor(
    @Self() public allors: SessionService,
    public navigationService: NavigationService,
    public location: Location,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private titleService: Title,
    private stateService: StateService) {

    this.m = allors.m;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.fetcher = new Fetcher(this.stateService, allors.pull);
    this.titleService.setTitle(this.title);
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.allors;

    this.subscription = combineLatest(this.route.url, this.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([, refresh, internalOrganisationId]) => {

          const navRoute = new NavigationActivatedRoute(this.route);
          const id = navRoute.id();
          const partId = navRoute.queryParam(m.Part);

          let pulls = [
            this.fetcher.internalOrganisation,
            this.fetcher.facilities,
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

          return this.allors
            .load('Pull', new PullRequest({ pulls }))
            .pipe(
              map((loaded) => ({ loaded, add }))
            );
        })
      )
      .subscribe(({ loaded, add }) => {
        this.allors.session.reset();

        this.facilities = loaded.collections.Facilities as Facility[];
        this.lots  = loaded.collections.Lots as Lot[];

        this.part = loaded.objects.Part as Part;

        if (add) {
          this.add = !(this.edit = false);

          this.nonSerialisedInventoryItem = this.allors.session.create('NonSerialisedInventoryItem') as NonSerialisedInventoryItem;

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

    this.allors
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
