import { TestBed, getTestBed } from '@angular/core/testing';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { RouterTestingModule } from '@angular/router/testing';
import { enGB } from 'date-fns/locale';

import { environment } from '../environments/environment';
import { AllorsAngularModule } from '../allors/angular/module';
import { MetaService, ContextService } from '../allors/angular/framework';
import { AuthenticationService } from '../allors/angular/authentication';

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
