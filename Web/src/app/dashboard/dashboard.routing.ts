import { Routes, RouterModule } from '@angular/router';
import { Dashboard } from './dashboard.component';
import { PatientComponent } from '../Patient';


const routes: Routes = [
  {
    path: '',
    component: Dashboard
  },
   {
     path: 'Patient',
     component: PatientComponent
  },
    {
      path: 'Booking',
      component: PatientComponent
   }
];

export const routing = RouterModule.forChild(routes);
