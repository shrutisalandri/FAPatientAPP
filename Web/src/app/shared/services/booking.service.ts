/// <reference path="../models/searchbooking.ts" />
/// <reference path="../models/patient.ts" />
/// <reference path="../utilities/http/http-utility.service.ts" />
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/toPromise';
import { Observable } from "rxjs/Observable";
import { Injectable } from '@angular/core';
import { HttpUtility } from '../utilities/http/http-utility.service';
import { HttpParams } from '@angular/common/http';
import { SearchBooking } from '../models/searchbooking';


@Injectable()
export class BookingService {
  constructor(
    private _http: HttpUtility,
  ) {

  }

  public getBookings(bookingid: string): Observable<SearchBooking> {

    let httpParams = new HttpParams()
      .set('PMS', 'OptomateTouch')
      .set('id', bookingid)

    return this._http.getParams("/api/GetAppointment", httpParams)
      .map((items: SearchBooking) => {
        return items;
      });
  }
  


}



