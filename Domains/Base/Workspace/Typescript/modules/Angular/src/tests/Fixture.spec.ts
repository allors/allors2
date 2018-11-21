import { TestBed, getTestBed } from '@angular/core/testing';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { SessionService, AuthenticationService, AllorsModule, AuthenticationModule } from 'src/allors/angular';
import { RouterTestingModule } from '@angular/router/testing';
import { environment } from '../environments/environment';

export class Fixture {

    allors: SessionService;

    private testbed: TestBed;

    constructor() {
        TestBed.configureTestingModule({
            imports: [
                HttpClientModule,
                RouterTestingModule,
                AllorsModule.forRoot({ url: environment.url }),
                AuthenticationModule.forRoot({ url: environment.url + environment.authenticationUrl }),
            ],
        });

        this.testbed = getTestBed();
    }

    async init() {
        const httpClient: HttpClient = this.testbed.get(HttpClient);
        await (httpClient.get(environment.url + 'Test/Setup', { responseType: 'text' }).toPromise());

        const authenticationService: AuthenticationService = this.testbed.get(AuthenticationService);
        const authResult = await authenticationService.login$('administrator', '').toPromise();
        expect(authResult.authenticated).toBeTruthy();

        this.allors = this.testbed.get(SessionService);
    }
}
