import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { AuthService } from './auth.service';

@Component({
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent {

    public loginForm = this.fb.group({
        userName: ['', Validators.required],
        password: ['', Validators.required]
    });

    constructor(private authService: AuthService, private router: Router, public fb: FormBuilder) { }

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
                    alert('login failed');
                }
            })
            .catch((e) => {
                alert(e.toString());
            });
    }
}
