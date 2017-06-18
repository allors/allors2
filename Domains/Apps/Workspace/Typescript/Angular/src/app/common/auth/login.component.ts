import { Subscription } from 'rxjs/Subscription';
import { Component, OnDestroy } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { AuthenticationService } from '../../../allors/angular';

@Component({
    styleUrls: ['./login.component.scss'],
    templateUrl: './login.component.html',
})
export class LoginComponent implements OnDestroy {

    private subscription: Subscription;

    public loginForm = this.formBuilder.group({
        userName: ['', Validators.required],
        password: ['', Validators.required]
    });

    constructor(private authService: AuthenticationService, private router: Router, public formBuilder: FormBuilder) { }

    login(event) {
        const userName = this.loginForm.controls.userName.value;
        const password = this.loginForm.controls.password.value;

        if (this.subscription) { this.subscription.unsubscribe(); }

        this.subscription = this.authService
            .login$(userName, password)
            .subscribe(
            result => {
                if (result.authenticated) {
                    this.router.navigate(['/']);
                } else {
                    alert(result.msg);
                }
            }
            );
    }

    ngOnDestroy() {
        if (this.subscription) {
            this.subscription.unsubscribe();
        }
    }
}
