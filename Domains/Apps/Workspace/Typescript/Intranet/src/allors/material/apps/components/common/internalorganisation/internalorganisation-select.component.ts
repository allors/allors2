import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Subscription } from 'rxjs';

import { StateService } from '../../../services/StateService';
import { ErrorService, Allors } from '../../../../../angular';
import { Equals, PullRequest, Sort } from '../../../../../framework';
import { Organisation } from '../../../../../domain';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'internalorganisation-select',
  templateUrl: './internalorganisation-select.component.html',
  providers: [Allors]
})
export class SelectInternalOrganisationComponent implements OnInit, OnDestroy {

  public get internalOrganisation() {
    const internalOrganisation = this.internalOrganisations.find(v => v.id === this.stateService.internalOrganisationId);
    return internalOrganisation;
  }

  public set internalOrganisation(value: Organisation) {
    this.stateService.internalOrganisationId = value.id;
  }

  public internalOrganisations: Organisation[];

  private subscription: Subscription;

  constructor(
    @Self() private allors: Allors,
    private stateService: StateService,
    private errorService: ErrorService) { }

  ngOnInit(): void {

    const { m, pull, scope } = this.allors;

    const pulls = [
      pull.Organisation(
        {
          predicate: new Equals({ propertyType: m.Organisation.IsInternalOrganisation, value: true }),
          sort: new Sort(m.Organisation.PartyName)
        }
      )
    ];

    this.subscription = scope
      .load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded) => {
        this.internalOrganisations = loaded.collections.Organisations as Organisation[];
      },
        (error: any) => {
          this.errorService.handle(error);
        },
      );
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

}
