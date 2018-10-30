import { TestBed, getTestBed } from '@angular/core/testing';
import { HttpClientModule, HTTP_INTERCEPTORS, HttpClient } from '@angular/common/http';
import { DatabaseConfig, AuthenticationConfig, AuthenticationInterceptor, AuthenticationService, DatabaseService, AllorsModule, Allors } from 'src/allors/angular';
import { environment } from 'src/environments/environment';

export class Fixture {

    allors: Allors;

    private testbed: TestBed;

    constructor() {
        TestBed.configureTestingModule({
            imports: [
                HttpClientModule,
                AllorsModule.forRoot(),
            ],
            providers: [
                { provide: DatabaseConfig, useValue: { url: environment.url } },
                { provide: AuthenticationConfig, useValue: { url: environment.url + environment.authenticationUrl } },
                { provide: HTTP_INTERCEPTORS, useClass: AuthenticationInterceptor, multi: true },
                AuthenticationService,
            ]
        });

        this.testbed = getTestBed();
    }

    async init() {
        const httpClient: HttpClient = this.testbed.get(HttpClient);
        await httpClient.get(environment.url + 'Test/Setup');

        const authenticationService: AuthenticationService = this.testbed.get(AuthenticationService);
        const authResult = await authenticationService.login$('administrator', '').toPromise();
        expect(authResult.authenticated).toBeTruthy();

        this.allors = this.testbed.get(Allors);
    }
}
