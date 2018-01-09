/// <reference path="../models/patient.ts" />
/// <reference path="../utilities/http/http-utility.service.ts" />
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/toPromise';
import { Observable } from "rxjs/Observable";
import { Injectable } from '@angular/core';
import { HttpUtility } from '../utilities/http/http-utility.service';
import {   HttpParams } from '@angular/common/http';
import { Patient } from '../models/patient';


@Injectable()
export class PatientService  {
  public patient: Patient[];
  //private requestOptions: HttpParams;
  constructor(
    private _http: HttpUtility,
  ) {

  }

  public getPatients(patientid: string, firstName: string, lastName: string): Observable<Patient[]> {

    let httpParams = new HttpParams()
      .set('PMS', 'OptomateTouch')
      .set('patientid', patientid)
    .set('firstName', firstName)
    .set('lastName', lastName);

    return this._http.getParams("/api/SearchPatients", httpParams)// new HttpParams({ fromString: 'PMS=OptomateTouch&ID='+patientid }))
      .map((items: Patient[]) => {
        this.set(items);
        return items;
        });
    }
 
    //return this._http.getParams("/api/GetPatient", params)
    //  .map((res) => {
    //    return res;// { Rows: (res.json()).length ? res.json() : undefined };
    //  });

    //return this._http.getParams("/api/GetPatient", params)
    //  .takeUntil(this.unsubscribe).map((items: Patient) => {
    //    this.patient = items;
    //    return this.patient;
      //});
      //.subscribe(
      //res => {
      //  console.log(res);
      //},
      //err => {
      //  console.log("Error occured");
      //}
      //);

  get() {

    return this.patient;
  }
  set(data) {
    this.patient = data;
  }

  

}



