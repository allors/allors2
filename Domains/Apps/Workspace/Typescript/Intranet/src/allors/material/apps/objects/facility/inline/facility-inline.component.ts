import { Component, EventEmitter, OnDestroy, OnInit, Output } from '@angular/core';

import { ContextService, ErrorService, MetaService } from '../../../../../angular';
import { Facility, FacilityType } from '../../../../../domain';
import { MetaDomain } from '../../../../../meta';
import { PullRequest, Sort } from 'src/allors/framework';
import { Fetcher } from '../../Fetcher';
import { StateService } from '../../../services/state';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'facility-inline',
  templateUrl: './facility-inline.component.html',
})
export class FacilityInlineComponent implements OnInit, OnDestroy {
  @Output() public saved: EventEmitter<Facility> = new EventEmitter<Facility>();

  @Output() public cancelled: EventEmitter<any> = new EventEmitter();

  public m: MetaDomain;

  facilityTypes: FacilityType[];
  public facility: Facility;

  private readonly fetcher: Fetcher;
  facilities: Facility[];

  constructor(private allors: ContextService,
    private errorService: ErrorService,
    public metaService: MetaService,
    private stateService: StateService) {

    this.m = this.metaService.m;
    this.fetcher = new Fetcher(this.stateService, this.metaService.pull);
  }

  public ngOnInit(): void {

    const { pull, x } = this.metaService;

    const pulls = [
      this.fetcher.facilities,
      pull.FacilityType({
        sort: new Sort(this.m.FacilityType.Name)
      })
    ];

    this.allors.context.load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded) => {
        this.facilityTypes = loaded.collections.FacilityTypes as FacilityType[];
        this.facilities = loaded.collections.Facilities as Facility[];

        this.facility = this.allors.context.create('Facility') as Facility;
      }, this.errorService.handler);
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
