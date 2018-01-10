/// <reference path="../shared/models/patient.ts" />
/// <reference path="../shared/services/booking.service.ts" />
import * as _ from 'lodash';
import { FormGroup, FormControl, Validators, FormBuilder, FormArray } from '@angular/forms';
import { Router, ActivatedRoute, Params, NavigationExtras } from '@angular/router';
import { BookingService} from '../shared/services/booking.service';

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
  providers: [BookingService],
  selector: 'searchBooking',
  animations: [fadeInAnimation, slideInOut]
})


export class SearchBookingComponent implements OnInit {
  bookingID: string;

  
  bookingList: SearchBooking[] = new Array<SearchBooking>();

  bookingcols: any[];
  constructor(private _bookingService: BookingService) {

    //var queryParam = this._routeParams.get('bookingID');
    //console.log(queryParam);

    

  }
  ngOnInit() {
    this.bookingcols = [
      { field: 'bookingId', header: 'Booking Id' },
      { field: 'optom', header: 'Optom' },
      { field: 'location', header: 'Location' },
      { field: 'bookingDate', header: 'Booking Date' },
      { field: ' bookingTime', header: ' Booking Time' },
      { field: 'patientId', header: 'Patient Id' },
      { field: 'title', header: 'Title' },
      { field: 'firstName', header: 'First Name' },
      { field: 'lastName', header: 'Last Name' },
      { field: 'doB', header: 'DoB' },
      { field: 'gender', header: 'Gender' },
    ];
    console.log("sdds");
  }
  
  SearchBooking() {
   this._bookingService.getBookings(this.bookingID).subscribe(data => {
      if (data != null ) {
                //let searchBooking = new SearchBooking();
                //searchBooking.bookingId = data.bookingId;
                //searchBooking.firstName = data.firstName;
                //searchBooking.lastName = data.lastName;
                //searchBooking.doB = data.doB;
                //searchBooking.gender = data.gender;
                //searchBooking.patientId = data.patientId;
                //searchBooking.optom = data.optom;
                //searchBooking.location = data.location;
                //searchBooking.bookingDate = data.bookingDate;
                //searchBooking.bookingTime = data.bookingTime;
                //searchBooking.title = data.title;
                //this.bookingList.push(searchBooking);
      }
    }, err => {
      console.log(err);
    });

    //let booking: SearchBooking = new SearchBooking();
    ////booking= new booking();
    //booking.bookingId = "adad";
    //booking.optom = "Optom";
    //booking.location = "Location";
    //booking.bookingDate = new Date(Date.now());
    //booking.bookingTime = "BookingTime";
    //booking.title = "Title";
    //booking.firstName = "FirstName";
    //booking.lastName = "LastName";
    //booking.doB = new Date(Date.now());
    //booking.gender = "F";
    //booking.patientId = "adad";
    //this.bookingList.push(booking);
    //console.log(this.bookingList.length);
    //console.log(this.bookingList);
  }
  
}
