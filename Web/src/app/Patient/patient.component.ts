/// <reference path="../shared/models/patient.ts" />
import * as _ from 'lodash';
import { ActivatedRoute } from '@angular/router';
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

  constructor( ) {    
    this.patientDetails.ResidentialAddress = new Address();
    this.patientDetails.PostalAddress = new Address();
    this.patientDetails.PrivateHealthFund = new HealthFund();
    this.patientDetails.HasHealthFund = true;
    this.patientDetails.PostAddressSameAsResidentialAddress = true;
  }
   ngOnInit() {
  }

   public SubmitPatient() {
   }
}
