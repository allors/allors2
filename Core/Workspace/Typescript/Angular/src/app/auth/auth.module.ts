import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AuthorizationService } from './authorization.service';
import { LoginComponent } from './login.component';

export { AuthorizationService } from './authorization.service';
export { LoginComponent } from './login.component';

@NgModule({
  declarations: [
    LoginComponent,
  ],
  exports: [
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  providers: [
    AuthorizationService
  ]
})
export class AuthModule {
  static forRoot(): ModuleWithProviders<AuthModule> {
    return {
      ngModule: AuthModule,
      providers: [ AuthorizationService ]
    };
  }
}
