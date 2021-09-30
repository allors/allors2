// Meta extensions
import '@allors/meta/core';
import '@allors/angular/core';

import 'jest-extended';

import { TestBed, getTestBed } from '@angular/core/testing';
import { APP_INITIALIZER } from '@angular/core';
import { HttpClientModule, HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterTestingModule } from '@angular/router/testing';

import { MetaPopulation } from '@allors/meta/system';
import { Workspace } from '@allors/domain/system';

// Allors Angular Services Core
import {
  WorkspaceService,
  DatabaseService,
  DatabaseConfig,
  ContextService,
  AuthenticationService,
  MetaService,
} from '@allors/angular/services/core';

// Allors Angular Core
import { AuthenticationConfig, AuthenticationInterceptor, AuthenticationServiceCore } from '@allors/angular/core';

import { environment } from '../../../../apps/angular/app/src/environments/environment';

import { extend as extendDomain } from '@allors/domain/custom';
import { extend as extendAngular } from '@allors/angular/core';
import { data } from '@allors/meta/generated';

import { configure } from '../../../../apps/angular/app/src/app/configure';

environment.url = "http://localhost:5000/allors/"

function appInitFactory(workspaceService: WorkspaceService) {
  return () => {
    const metaPopulation = new MetaPopulation(data);
    const workspace = new Workspace(metaPopulation);

    // Domain extensions
    extendDomain(workspace);
    extendAngular(workspace);

    // Configuration
    configure(metaPopulation);

    workspaceService.workspace = workspace;
  };
}

export class Fixture {
  meta: MetaService;
  allors: ContextService;

  private testbed: TestBed;

  constructor() {
    TestBed.configureTestingModule({
      imports: [HttpClientModule, RouterTestingModule],
      providers: [
        {
          provide: APP_INITIALIZER,
          useFactory: appInitFactory,
          deps: [WorkspaceService],
          multi: true,
        },
        DatabaseService,
        { provide: DatabaseConfig, useValue: { url: environment.url } },
        WorkspaceService,
        ContextService,
        { provide: AuthenticationService, useClass: AuthenticationServiceCore },
        {
          provide: AuthenticationConfig,
          useValue: {
            url: environment.url + environment.authenticationUrl,
          },
        },
        {
          provide: HTTP_INTERCEPTORS,
          useClass: AuthenticationInterceptor,
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
