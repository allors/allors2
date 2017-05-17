import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { AuthService } from './auth.service';

import { MdSnackBar, MdSnackBarConfig } from '@angular/material';
import { TdLoadingService } from '@covalent/core';

@Component({
  selector: 'qs-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent {

    public loginForm = this.fb.group({
        userName: ['', Validators.required],
        password: ['', Validators.required]
    });

    constructor(private authService: AuthService,
                private _loadingService: TdLoadingService,
                private router: Router,
                public fb: FormBuilder,
                public snackBar: MdSnackBar) { }

    login(event) {
        const userName = this.loginForm.controls.userName.value;
        const password = this.loginForm.controls.password.value;

        this.authService
            .login(userName, password)
            .then((loggedIn) => {
                if (loggedIn) {
                    if (this.authService.redirectUrl) {
                        this.router.navigateByUrl(this.authService.redirectUrl);
                    } else {
                        this.router.navigate(['/']);
                    }
                } else {
                    let config = new MdSnackBarConfig();
                    config.duration = 5000;
                    this.snackBar.open('Login failed', 'close', config );
                }
            })
            .catch((e) => {
                this.snackBar.open(e.toString(), 'close');
            });
    }
}
