/// <reference path="../shared/models/patient.ts" />
import * as _ from 'lodash';
import { FormGroup, FormControl, Validators, FormBuilder, FormArray } from '@angular/forms';
import { Router, ActivatedRoute, Params, NavigationExtras } from '@angular/router';

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
import { SearchPatient } from '../shared/models/searchPatient';


@Component({
  templateUrl: './searchPatient.component.html',
  providers: [],
  selector: 'searchPatient',
  animations: [fadeInAnimation, slideInOut]
})


export class SearchPatientComponent implements OnInit {
  PatientID: string;
  FirstName: string;
  LastName: string;
  checked: boolean;
  
  PatientList: SearchPatient[] = new Array<SearchPatient>();

  Patientcols: any[];
  constructor() {

    //var queryParam = this._routeParams.get('PatientID');
    //console.log(queryParam);

    

  }
  ngOnInit() {
    this.Patientcols = [
      { field: 'PatientId', header: 'Patient Id' },
      { field: 'Title', header: 'Title' },
      { field: 'FirstName', header: 'First Name' },
      { field: 'LastName', header: 'Last Name' },
      { field: 'DOB', header: 'DoB' },
      { field: 'Gender', header: 'Gender' },
    ];
    console.log("sdds");
  }
  SearchPatient() {
    let Patient: SearchPatient = new SearchPatient();
    //Patient= new Patient();
    Patient.Title = "Title";
    Patient.FirstName = "FirstName";
    Patient.LastName = "LastName";
    Patient.DOB = new Date(Date.now());
    Patient.Gender = "F";
    Patient.PatientId = "adad";
    this.PatientList.push(Patient);
    console.log(this.PatientList.length);
    console.log(this.PatientList);
  }
  
}
