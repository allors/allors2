import { Component, EventEmitter, OnDestroy, OnInit, Output } from '@angular/core';

import { SessionService, ErrorService } from '../../../../../angular';
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

  constructor(private allors: SessionService,
    private errorService: ErrorService,
    private stateService: StateService) {

    this.m = this.allors.m;
    this.fetcher = new Fetcher(this.stateService, allors.pull);
  }

  public ngOnInit(): void {

    const { pull, x } = this.allors;

    const pulls = [
      this.fetcher.facilities,
      pull.FacilityType({
        sort: new Sort(this.m.FacilityType.Name)
      })
    ];

    this.allors
      .load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded) => {
        this.facilityTypes = loaded.collections.FacilityTypes as FacilityType[];
        this.facilities = loaded.collections.Facilities as Facility[];

        this.facility = this.allors.session.create('Facility') as Facility;
      }, this.errorService.handler);
  }

  public ngOnDestroy(): void {
    if (!!this.facility) {
      this.allors.session.delete(this.facility);
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
