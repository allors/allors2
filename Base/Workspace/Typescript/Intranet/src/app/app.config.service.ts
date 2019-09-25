import { Injectable, Self } from '@angular/core';
import { Observable } from 'rxjs';

import { ContextService, MetaService, InternalOrganisationId, SingletonId} from '../allors/angular';
import { Organisation, Singleton } from '../allors/domain';
import { PullRequest, Equals } from '../allors/framework';
import { Loaded } from '../allors/angular';
import { tap } from 'rxjs/operators';

@Injectable()
export class ConfigService {

    constructor(
        @Self() public allors: ContextService,
        public metaService: MetaService,
        private internalOrganisationId: InternalOrganisationId,
        private singletonId: SingletonId,
    ) { }

    public setup(): Observable<any> {

        const { m, pull, x } = this.metaService;

        const pulls = [
            pull.Organisation({
                predicate: new Equals({ propertyType: m.Organisation.IsInternalOrganisation, value: true })
            }),
            pull.Singleton()
        ];

        return this.allors.context
            .load(new PullRequest({ pulls }))
            .pipe(
                tap((loaded: Loaded) => {
                    const internalOrganisations = loaded.collections.Organisations as Organisation[];

                    if (internalOrganisations && internalOrganisations.length > 0) {
                        const organisation = internalOrganisations.find(v => v.id === this.internalOrganisationId.value);
                        if (!organisation) {
                            this.internalOrganisationId.value = internalOrganisations[0].id;
                        }
                    }

                    const singletons = loaded.collections.Singletons as Singleton[];
                    this.singletonId.value = singletons[0].id;
                })
            );
    }
}
