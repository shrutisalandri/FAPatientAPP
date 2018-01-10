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

    this._patientService.getPatients(this.PatientID.toString(), this.FirstName, this.LastName)
      .subscribe(data => {
        if (data != null && data.length > 0) {
          data.forEach(Patient => {
            let searchPatient = new SearchPatient();
            searchPatient.Title = Patient.title;
            searchPatient.FirstName = Patient.firstName;
            searchPatient.LastName = Patient.lastName;
            searchPatient.DOB = Patient.dateOfBirth;
            searchPatient.Gender = Patient.gender;
            searchPatient.PatientId = Patient.id;
            this.PatientList.push(searchPatient);
          });
        }
    }, err => {
      console.log(err);
      });

   
 
       
        
    
  }

  
  
}
