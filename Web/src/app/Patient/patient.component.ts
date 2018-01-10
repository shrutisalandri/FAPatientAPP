/// <reference path="../shared/models/patient.ts" />
import * as _ from 'lodash';
import { Router, ActivatedRoute, Params, NavigationExtras } from '@angular/router';

import { FormBuilder, FormGroup, Validators } from '@angular/forms';


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
import { Patient} from '../shared/models/patient';
import { PatientService } from '../shared/services/patient.service'

@Component({
  templateUrl: './patient.component.html',
  providers: [],
  selector: 'patient',
    animations: [fadeInAnimation, slideInOut]
})


export class PatientComponent implements OnInit {
  public patientDetails: Patient = new Patient();
  value: Date;
  //heroForm: FormGroup;

  
  checked: boolean;

  constructor(private router: Router, private activatedRoute: ActivatedRoute, private fb: FormBuilder, private patientService: PatientService) {

  }
  ngOnInit() {

    var PatientID = this.activatedRoute.snapshot.queryParams["PatientID"];

    this.patientDetails = this.patientService.getPatientByID(PatientID);

    
  }

  public SubmitPatient() {



   }
}
