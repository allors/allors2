import { Injectable, Self } from '@angular/core';
import { Observable } from 'rxjs';

import { SessionService } from '../allors/angular';
import { Organisation, Singleton } from '../allors/domain';
import { PullRequest, Equals } from '../allors/framework';
import { Loaded } from '../allors/angular';
import { StateService } from '../allors/material';
import { tap } from 'rxjs/operators';

@Injectable()
export class ConfigService {

    constructor(
        @Self() private allors: SessionService,
        private stateService: StateService
    ) { }

    public setup(): Observable<any> {

        const { m, pull, x } = this.allors;

        const pulls = [
            pull.Organisation({
                predicate: new Equals({ propertyType: m.Organisation.IsInternalOrganisation, value: true })
            }),
            pull.Singleton()
        ];

        return this.allors
            .load('Pull', new PullRequest({ pulls }))
            .pipe(
                tap((loaded: Loaded) => {
                    const internalOrganisations = loaded.collections.Organisations as Organisation[];

                    if (internalOrganisations && internalOrganisations.length > 0) {
                        const organisation = internalOrganisations.find(v => v.id === this.stateService.internalOrganisationId);
                        if (!organisation) {
                            this.stateService.internalOrganisationId = internalOrganisations[0].id;
                        }
                    }

                    const singletons = loaded.collections.Singletons as Singleton[];
                    this.stateService.singletonId = singletons[0].id;
                })
            );
    }
}
