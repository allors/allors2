import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AuthenticationService } from '../allors/angular';
import { LoginComponent } from './auth/login.component';

import { HomeComponent } from './home/home.component';
import { FormComponent } from './form/form.component';
import { QueryComponent } from './query/query.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent},
  {
    canActivate: [ AuthenticationService ],
    path: '',
    children: [{
        path: '',
        component: HomeComponent,
      },
      {
        path: 'form',
        component: FormComponent,
      },
      {
        path: 'query',
        component: QueryComponent,
      }]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
