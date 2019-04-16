import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Subscription } from 'rxjs';

import { ContextService, MetaService, InternalOrganisationId } from '../../../../../angular';
import { Equals, PullRequest, Sort } from '../../../../../framework';
import { Organisation } from '../../../../../domain';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'internalorganisation-select',
  templateUrl: './internalorganisation-select.component.html',
  providers: [ContextService]
})
export class SelectInternalOrganisationComponent implements OnInit, OnDestroy {

  public get internalOrganisation() {
    const internalOrganisation = this.internalOrganisations.find(v => v.id === this.internalOrganisationId.value);
    return internalOrganisation;
  }

  public set internalOrganisation(value: Organisation) {
    this.internalOrganisationId.value = value.id;
  }

  public internalOrganisations: Organisation[];

  private subscription: Subscription;

  constructor(
    @Self() private allors: ContextService,
    public metaService: MetaService,
    private internalOrganisationId: InternalOrganisationId,
  ) { }

  ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    const pulls = [
      pull.Organisation(
        {
          predicate: new Equals({ propertyType: m.Organisation.IsInternalOrganisation, value: true }),
          sort: new Sort(m.Organisation.PartyName)
        }
      )
    ];

    this.subscription = this.allors.context.load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded) => {
        this.internalOrganisations = loaded.collections.Organisations as Organisation[];
      });
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

}
