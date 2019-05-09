import { Component, EventEmitter, OnDestroy, OnInit, Output } from '@angular/core';

import { ContextService, MetaService, FetcherService } from '../../../../../angular';
import { Facility, FacilityType, Organisation } from '../../../../../domain';
import { Meta } from '../../../../../meta';
import { PullRequest, Sort } from '../../../../../../allors/framework';

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

    const { pull, x } = this.metaService;

    const pulls = [
      this.fetcher.internalOrganisation,
      pull.Facility(),
      pull.FacilityType({
        sort: new Sort(this.m.FacilityType.Name)
      })
    ];

    this.allors.context.load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded) => {
        this.internalOrganisation = loaded.objects.InternalOrganisation as Organisation;
        this.facilities = loaded.collections.Facilities as Facility[];

        this.facilityTypes = loaded.collections.FacilityTypes as FacilityType[];
        const warehouse = this.facilityTypes.find((v) => v.UniqueId === '56ad0a651fc040eabda8daddfa6cbe63');

        this.facility = this.allors.context.create('Facility') as Facility;
        this.facility.Owner = this.internalOrganisation;
        this.facility.FacilityType = warehouse;
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
