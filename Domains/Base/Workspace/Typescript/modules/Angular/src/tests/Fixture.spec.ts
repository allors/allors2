import { TestBed, getTestBed } from '@angular/core/testing';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { ContextService, AuthenticationService, AllorsModule, AuthenticationModule, MetaService } from 'src/allors/angular';
import { RouterTestingModule } from '@angular/router/testing';
import { environment } from '../environments/environment';

export class Fixture {

    meta: MetaService;
    allors: ContextService;

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

        this.meta = this.testbed.get(MetaService);
        this.allors = this.testbed.get(ContextService);
    }
}
