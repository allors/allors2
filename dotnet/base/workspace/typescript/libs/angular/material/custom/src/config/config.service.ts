import { Injectable, Self } from '@angular/core';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

import { ContextService, SingletonId, UserId, Loaded, MetaService } from '@allors/angular/services/core';
import { Equals } from '@allors/data/system';
import { PullRequest } from '@allors/protocol/system';
import { Organisation, Singleton } from '@allors/domain/generated';
import { InternalOrganisationId } from '@allors/angular/base';

@Injectable({
    providedIn: 'root',
  })
export class ConfigService {

    constructor(
        @Self() public allors: ContextService,
        public metaService: MetaService,
        private internalOrganisationId: InternalOrganisationId,
        private singletonId: SingletonId,
        private userId: UserId,
    ) { }

    public setup(): Observable<any> {

        const { m, pull, x } = this.metaService;

        const pulls = [
            pull.Organisation({
                predicate: new Equals({ propertyType: m.Organisation.IsInternalOrganisation, value: true })
            }),
            pull.Person({
                object: this.userId.value,
                fetch: {
                    UserProfile: {
                        DefaultInternalOrganization: x
                    }
                }
            }),
            pull.Singleton()
        ];

        return this.allors.context
            .load(new PullRequest({ pulls }))
            .pipe(
                tap((loaded: Loaded) => {
                    const internalOrganisations = loaded.collections.Organisations as Organisation[];
                    const defaultInternalOrganization = loaded.objects.DefaultInternalOrganization as Organisation;

                    if (internalOrganisations && internalOrganisations.length > 0) {
                        const organisation = internalOrganisations.find(v => v.id === this.internalOrganisationId.value);

                        if (!organisation && defaultInternalOrganization) {
                            this.internalOrganisationId.value = defaultInternalOrganization.id;
                        } else if (!organisation) {
                            this.internalOrganisationId.value = internalOrganisations[0].id;
                        }
                    }

                    const singletons = loaded.collections.Singletons as Singleton[];
                    this.singletonId.value = singletons[0].id;
                })
            );
    }
}
