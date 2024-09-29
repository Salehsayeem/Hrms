import { Routes } from '@angular/router';
import { LoginComponent } from './authentication/login/login.component';
import { RegisterComponent } from './authentication/register/register.component';
import { NotFoundComponent } from './features/not-found/not-found.component';
import { ServerDownComponent } from './features/server-down/server-down.component';
import { UnauthorizedComponent } from './features/unauthorized/unauthorized.component';
import { AuthGuard } from './core/auth.guard';
import { MainBodyComponent } from './features/main-body/main-body.component';
import { DashboardComponent } from './features/dashboard/dashboard.component';
import { HouseComponent } from './features/house/house.component';
import { RoomCategoriesComponent } from './features/room-categories/room-categories.component';

export const routes: Routes = [
  // { path: '', redirectTo: 'auth/login', pathMatch: 'full' },
  {
    path: 'auth',
    children: [
      {
        path: 'login',
        component: LoginComponent
      },
      {
        path: 'register',
        component: RegisterComponent
      }
    ]
  },
  { path: 'server-down', component: ServerDownComponent },
  { path: 'unauthorized', component: UnauthorizedComponent },
  {
    path: '', component: MainBodyComponent,
    canActivate: [AuthGuard],
    children:[
      {
        path:'dashboard',
        component:DashboardComponent
      },
      {
        path:'house',
        component:HouseComponent
      },
      {
        path:'room-categories',
        component:RoomCategoriesComponent
      },
    ]
  },
  {
    path: '**',
    component: NotFoundComponent
  }
];
