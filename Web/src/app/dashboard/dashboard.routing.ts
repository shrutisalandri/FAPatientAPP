/// <reference path="../searchbooking/searchbooking.module.ts" />
/// <reference path="../searchpatient/searchpatient.component.ts" />
import { Routes, RouterModule } from '@angular/router';
import { Dashboard } from './dashboard.component';
import { PatientComponent } from '../Patient';
import { SearchPatientComponent } from '../searchpatient';
import { SearchBookingComponent } from '../searchbooking';



const routes: Routes = [
  {
    path: '',
    component: Dashboard
  },
  {
    path: 'SearchPatient',
    component: SearchPatientComponent
  },
   {
     path: 'Patient',
     component: PatientComponent
  },
    {
      path: 'Booking',
      component: SearchBookingComponent
   }
];

export const routing = RouterModule.forChild(routes);
