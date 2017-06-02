import { Component, OnDestroy } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { AuthService } from './auth.service';
import { Subscription } from 'rxjs/Subscription';

@Component({
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnDestroy{

    public loginForm = this.fb.group({
        userName: ['', Validators.required],
        password: ['', Validators.required]
    });

    private postStream$: Subscription;

    constructor(private authService: AuthService, private router: Router, public fb: FormBuilder) { }

    login(event) {
        const userName = this.loginForm.controls.userName.value;
        const password = this.loginForm.controls.password.value;

        if (this.postStream$) { this.postStream$.unsubscribe(); }

        this.postStream$ = this.authService.login$(userName, password)
        .subscribe(
            result => {
                if (result.state === 1) {
                    this.router.navigate(['/']);
                } else {
                    alert(result.msg);
                }
            }
        )
    }

     ngOnDestroy() {
         if(this.postStream$){
             this.postStream$.unsubscribe();
            }
     }
}
