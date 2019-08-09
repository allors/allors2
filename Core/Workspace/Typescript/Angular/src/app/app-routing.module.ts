import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AuthorizationService } from './auth/auth.module';

import { LoginComponent } from './auth/auth.module';
import { FetchComponent } from './fetch/fetch.module';
import { FormComponent } from './form/form.module';
import { HomeComponent } from './home/home.module';
import { QueryComponent } from './query/query.module';

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
