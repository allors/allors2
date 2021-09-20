import { Component, Output, EventEmitter, OnInit, OnDestroy } from '@angular/core';

import { ContextService, MetaService } from '@allors/angular/services/core';
import { Facility, FacilityType, Organisation } from '@allors/domain/generated';
import { Meta } from '@allors/meta/generated';
import { Sort } from '@allors/data/system';
import { PullRequest } from '@allors/protocol/system';
import { FetcherService } from '@allors/angular/base';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'facility-inline',
  templateUrl: './facility-inline.component.html',
})
export class FacilityInlineComponent implements OnInit, OnDestroy {
  @Output() public saved: EventEmitter<Facility> = new EventEmitter<Facility>();

  @Output() public cancelled: EventEmitter<any> = new EventEmitter();

  public m: Meta;

  facilityTypes: FacilityType[];
  public facility: Facility;

  facilities: Facility[];
  internalOrganisation: Organisation;

  constructor(
    private allors: ContextService,
    public metaService: MetaService,
    private fetcher: FetcherService
  ) {

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { pull } = this.metaService;

    const pulls = [
      this.fetcher.internalOrganisation,
      pull.Facility(),
      pull.FacilityType({
        sort: new Sort(this.m.FacilityType.Name)
      })
    ];

    this.allors.context.load(new PullRequest({ pulls }))
      .subscribe((loaded) => {
        this.internalOrganisation = loaded.objects.InternalOrganisation as Organisation;
        this.facilities = loaded.collections.Facilities as Facility[];

        this.facilityTypes = loaded.collections.FacilityTypes as FacilityType[];
        const storageLocation = this.facilityTypes.find((v) => v.UniqueId === 'ff66c1ad-3048-48fd-a7d9-fbf97a090edd');

        this.facility = this.allors.context.create('Facility') as Facility;
        this.facility.Owner = this.internalOrganisation;
        this.facility.FacilityType = storageLocation;
      });
  }

  public ngOnDestroy(): void {
    if (!!this.facility) {
      this.allors.context.delete(this.facility);
    }
  }

  public cancel(): void {
    this.cancelled.emit();
  }

  public save(): void {
    this.saved.emit(this.facility);
    this.facility = undefined;
  }
}
