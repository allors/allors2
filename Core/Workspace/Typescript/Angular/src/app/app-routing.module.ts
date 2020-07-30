import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AuthorizationService } from './auth/authorization.service';

import { LoginComponent } from './auth/login.component';
import { HomeComponent } from './home/home.component';
import { FormComponent } from './form/form.component';
import { QueryComponent } from './query/query.component';
import { FetchComponent } from './fetch/fetch.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  {
    path: '',
    canActivate: [AuthorizationService],
    children: [
      {
        component: HomeComponent,
        path: '',
      },
      {
        component: FormComponent,
        path: 'form',
      },
      {
        component: QueryComponent,
        path: 'query',
      },
      {
        component: FetchComponent,
        path: 'fetch/:id',
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule { }
