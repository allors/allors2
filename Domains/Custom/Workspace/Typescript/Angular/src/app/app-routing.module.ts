import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AuthGuard } from './auth/auth-guard.service';
import { LoginComponent } from './auth/login.component';

import { DashboardComponent } from './dashboard/dashboard.component';
import { FormComponent } from './form/form.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent},
  {
    canActivate: [ AuthGuard ],
    path: '',
    children: [{
        path: '',
        component: DashboardComponent,
      },
      {
        path: 'form',
        component: FormComponent,
      }]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
