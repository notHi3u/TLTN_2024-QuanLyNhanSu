import { Routes } from '@angular/router';


import { AuthLayoutComponent } from './layouts/auth-layout/auth-layout.component';
import { MainLayoutComponent } from './layouts/main-layout/main-layout.component';
import { LoginComponent } from './core/auth/login/login.component';

export const routes: Routes = [
  // Auth routes
  {
    path: '',
    component: AuthLayoutComponent, // Wrap auth-related routes in this layout
    children: [
      { path: 'login', component: LoginComponent }, // Login page under auth layout
    ],
  },
  // Main layout routes
  {
    path: 'main',
    component: MainLayoutComponent, // Wrap main application routes in this layout
    children: [

    ],
  },
  // Fallback route
  { path: '**', redirectTo: 'login' }, // Redirect any unknown path to login
];
