/// <reference path="../shared/utilities/http/http-utility.service.ts" />
/// <reference path="../shared/services/patient.service.ts" />
/// <reference path="../shared/models/patient.ts" />
import * as _ from 'lodash';
import { FormGroup, FormControl, Validators, FormBuilder, FormArray } from '@angular/forms';
import { Router, ActivatedRoute, Params, NavigationExtras } from '@angular/router';
import { PatientService} from '../shared/services/patient.service'
import { HttpUtility } from '../shared/utilities/http/http-utility.service';
import {
  Component,
  Inject,
  OnInit,
  ViewChild,
  ViewEncapsulation
} from '@angular/core';
import { fadeInAnimation, slideInOut } from '../layout/animations/shared-animations';
import { GlobalEventsManager } from '../shared/utilities/global-events-manager';
import { HttpClient, HttpParams } from '@angular/common/http';
import { SearchPatient } from '../shared/models/searchPatient';
import { Patient } from '../shared/models/patient';
import { Subject } from "rxjs/Subject";
import { Observable } from 'rxjs/Observable';

@Component({
  templateUrl: './searchPatient.component.html',
  providers: [],
  selector: 'searchPatient',
  animations: [fadeInAnimation, slideInOut]
})


export class SearchPatientComponent implements OnInit {
  PatientID: number = 0;
  FirstName: string = '';
  LastName: string = '';
  checked: boolean;
 
  private unsubscribe: Subject<void> = new Subject<void>();
  PatientList: SearchPatient[] = new Array<SearchPatient>();
  searchPatient: SearchPatient = new SearchPatient();
  private patient: Patient;
  Patientcols: any[];
  constructor(
    private _http: HttpUtility,
    private _patientService: PatientService) {

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
    let Patientres1: SearchPatient = new SearchPatient();

    this._patientService.getPatients(this.PatientID, this.FirstName, this.LastName)
      .subscribe(data => {
        data.forEach(Patient => {
          let searchPatient = new SearchPatient();
          this.searchPatient.Title = Patient.title;
          this.searchPatient.FirstName = Patient.firstName;
          this.searchPatient.LastName = Patient.lastName;
          this.searchPatient.DOB = Patient.dateOfBirth;
          this.searchPatient.Gender = Patient.gender;
          this.searchPatient.PatientId = Patient.id;
          this.PatientList.push(this.searchPatient);
        });

    }, err => {
      console.log(err);
      });

   
 
       
        
    
  }

  
  
}
