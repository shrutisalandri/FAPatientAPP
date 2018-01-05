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
import { SearchBooking } from '../shared/models/searchBooking';


@Component({
  templateUrl: './searchBooking.component.html',
  providers: [],
  selector: 'searchBooking',
  animations: [fadeInAnimation, slideInOut]
})


export class SearchBookingComponent implements OnInit {
  bookingID: string;
  FirstName: string;
  LastName: string;
  checked: boolean;
  
  bookingList: SearchBooking[] = new Array<SearchBooking>();

  bookingcols: any[];
  constructor() {

    //var queryParam = this._routeParams.get('bookingID');
    //console.log(queryParam);

    

  }
  ngOnInit() {
    this.bookingcols = [
      { field: 'BookingId', header: 'Booking Id' },
      { field: 'Optom', header: 'Optom' },
      { field: 'Location', header: 'Location' },
      { field: 'BookingDate', header: 'Booking Date' },
      { field: ' BookingTime', header: ' Booking Time' },
      { field: 'PatientId', header: 'Patient Id' },
      { field: 'Title', header: 'Title' },
      { field: 'FirstName', header: 'First Name' },
      { field: 'LastName', header: 'Last Name' },
      { field: 'DOB', header: 'DoB' },
      { field: 'Gender', header: 'Gender' },
    ];
    console.log("sdds");
  }
  
  SearchBooking() {
    let booking: SearchBooking = new SearchBooking();
    //booking= new booking();
    booking.BookingId = "adad";
    booking.Optom = "Optom";
    booking.Location = "Location";
    booking.BookingDate = new Date(Date.now());
    booking.BookingTime = "BookingTime";
    booking.Title = "Title";
    booking.FirstName = "FirstName";
    booking.LastName = "LastName";
    booking.DoB = new Date(Date.now());
    booking.Gender = "F";
    booking.PatientId = "adad";
    this.bookingList.push(booking);
    console.log(this.bookingList.length);
    console.log(this.bookingList);
  }
  
}
