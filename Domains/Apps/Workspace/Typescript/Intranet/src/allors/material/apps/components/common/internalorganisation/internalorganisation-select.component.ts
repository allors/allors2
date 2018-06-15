import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';

import { StateService } from '../../../services/StateService';
import { WorkspaceService, Loaded, ErrorService } from '../../../../../angular';
import { MetaDomain } from '../../../../../meta';
import { Query, Equals, PullRequest } from '../../../../../framework';
import { Organisation } from '../../../../../domain';

@Component({
  selector: 'internalorganisation-select',
  templateUrl: './internalorganisation-select.component.html',
})
export class SelectInternalOrganisationComponent implements OnInit, OnDestroy {

  public get internalOrganisation() {
    var internalOrganisation = this.internalOrganisations.find(v => v.id === this.stateService.internalOrganisationId);
    return internalOrganisation;
  }

  public set internalOrganisation(value: Organisation) {
    this.stateService.internalOrganisationId = value.id;
  }

  public internalOrganisations: Organisation[];

  private subscription: Subscription;

  constructor(
    private workspaceService: WorkspaceService,
    private stateService: StateService,
    private errorService: ErrorService) { }

  ngOnInit(): void {

    const scope = this.workspaceService.createScope();

    const m: MetaDomain = this.workspaceService.metaPopulation.metaDomain;
    const queries = [
      new Query({
        name: "internalOrganisations",
        objectType: m.Organisation,
        predicate: new Equals({ roleType: m.Organisation.IsInternalOrganisation, value: true })
      })
    ];

    this.subscription = scope
      .load("Pull", new PullRequest({ queries }))
      .subscribe((loaded) => {
        this.internalOrganisations = loaded.collections.internalOrganisations as Organisation[];
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
