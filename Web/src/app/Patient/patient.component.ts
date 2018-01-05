/// <reference path="../shared/models/patient.ts" />
import * as _ from 'lodash';
import { Router, ActivatedRoute, Params, NavigationExtras } from '@angular/router';

import { FormGroup, FormControl, Validators, FormBuilder, FormArray } from '@angular/forms';



import {
  Component,
  Inject,
  OnInit,
  ViewChild,
  ViewEncapsulation
} from '@angular/core';
import { fadeInAnimation, slideInOut } from '../layout/animations/shared-animations';
import { GlobalEventsManager } from '../shared/utilities/global-events-manager';
import { HttpClient } from '@angular/common/http';
import { Patient, Address, HealthFund } from '../shared/models/patient';


@Component({
  templateUrl: './patient.component.html',
  providers: [],
  selector: 'patient',
    animations: [fadeInAnimation, slideInOut]
})


export class PatientComponent implements OnInit {
  public patientDetails: Patient = new Patient();
  value: Date;
  
  checked: boolean;

  constructor(private router: Router, private activatedRoute: ActivatedRoute) {
    
    this.patientDetails.ResidentialAddress = new Address();
    this.patientDetails.PostalAddress = new Address();
    this.patientDetails.PrivateHealthFund = new HealthFund();
    this.patientDetails.HasHealthFund = true;
    this.patientDetails.PostAddressSameAsResidentialAddress = true;
  }
  ngOnInit() {
    var queryParam = this.activatedRoute.snapshot.queryParams["PatientID"];

    if (queryParam != null && queryParam.length > 0) {
      this.patientDetails.Title = "title";
      this.patientDetails.FirstName = "FirstName";
      this.patientDetails.LastName = "LastName";
    }

  }

   public SubmitPatient() {
   }
}
