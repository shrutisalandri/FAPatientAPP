import { ErrorPageComponent } from './layout/components/error-page/error-page.component';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuardService } from "./shared/services/auth-guard.service";
import { CustomerGuardService } from './shared/services/customer-gaurd.service';

export const routes: Routes = [

  //{
  //  path: '',
  //  redirectTo: 'account',
  //  pathMatch: 'full'
  //},
  //{
  //    path: 'account',
  //    loadChildren: 'app/account/account.module#AccountModule'
  //},
  //{
  //  path: 'error',
  //  component: ErrorPageComponent,
  //  data: { animation: 'error' }
  //},
  {
    path: '',
    loadChildren: 'app/dashboard/dashboard.module#DashboardModule',
    //canActivate: [AuthGuardService],
  },
  
  { path: '**', redirectTo: 'error' }

];

export const routing = RouterModule.forRoot(routes, { useHash: false });
