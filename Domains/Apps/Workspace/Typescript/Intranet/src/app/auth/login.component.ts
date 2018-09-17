import { Component, OnDestroy } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';

import { AuthenticationService } from '../../allors/angular';
import { ConfigService } from '../app.config.service';

@Component({
  templateUrl: './login.component.html',
})
export class LoginComponent implements OnDestroy {
  public loginForm = this.formBuilder.group({
    password: ['', Validators.required],
    userName: ['', Validators.required],
  });

  private subscription: Subscription;

  constructor(
    private configService: ConfigService,
    private authService: AuthenticationService,
    private router: Router,
    public formBuilder: FormBuilder,
  ) {}

  public login(event) {
    const userName = this.loginForm.controls.userName.value;
    const password = this.loginForm.controls.password.value;

    if (this.subscription) {
      this.subscription.unsubscribe();
    }

    this.subscription = this.authService
    .login$(userName, password)
    .subscribe(
      (result) => {
        if (result.authenticated) {
          this.configService.setup()
            .subscribe(() => {
              this.router.navigate(['/']);
            },
            (error) => {
              alert('Error during setup. Please restart.');
            });
        } else {
          alert('Could not log in');
        }
      },
      (error) => alert(JSON.stringify(error)),
    );
  }

  public ngOnDestroy() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
