// Meta extensions
import '@allors/meta/core';
import '@allors/angular/core';

import 'jest-extended';

import { TestBed, getTestBed } from '@angular/core/testing';
import { APP_INITIALIZER } from '@angular/core';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { RouterTestingModule } from '@angular/router/testing';
import { enGB } from 'date-fns/locale';

import { MetaService, ContextService, AuthenticationService, WorkspaceService } from '@allors/angular/core';
import { MetaPopulation } from '@allors/meta/system';
import { Workspace } from '@allors/domain/system';
import { AllorsAngularModule } from '@allors/angular/module';

import { environment } from '../../../../apps/angular/app/src/environments/environment';

import { extend as extendDomain } from '@allors/domain/custom';
import { extend as extendAngular } from '@allors/angular/core';
import { data } from '@allors/meta/generated';

function appInitFactory(workspaceService: WorkspaceService) {
  return () => {
    const metaPopulation = new MetaPopulation(data);
    const workspace = new Workspace(metaPopulation);

    // Domain extensions
    extendDomain(workspace);
    extendAngular(workspace);

    workspaceService.workspace = workspace;
  };
}

export class Fixture {
  meta: MetaService;
  allors: ContextService;

  private testbed: TestBed;

  constructor() {
    TestBed.configureTestingModule({
      imports: [
        HttpClientModule,
        RouterTestingModule,

        AllorsAngularModule.forRoot({
          databaseConfig: { url: environment.url },
          authenticationConfig: {
            url: environment.url + environment.authenticationUrl,
          },
          dateConfig: {
            locale: enGB,
          },
          mediaConfig: { url: environment.url },
        }),
      ],
      providers: [
        {
          provide: APP_INITIALIZER,
          useFactory: appInitFactory,
          deps: [WorkspaceService],
          multi: true,
        },
      ],
    });

    this.testbed = getTestBed();
  }

  async init() {
    const httpClient: HttpClient = this.testbed.get(HttpClient);
    await httpClient.get(environment.url + 'Test/Setup', { responseType: 'text' }).toPromise();

    const authenticationService: AuthenticationService = this.testbed.get(AuthenticationService);
    const authResult = await authenticationService.login$('administrator', '').toPromise();
    expect(authResult.authenticated).toBeTruthy();

    this.meta = this.testbed.get(MetaService);
    this.allors = this.testbed.get(ContextService);
  }
}
