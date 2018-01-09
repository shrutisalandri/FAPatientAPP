/// <reference path="../shared/services/patient.service.ts" />
/// <reference path="../searchpatient/searchpatient.component.ts" />
import {
  AutoCompleteModule,
  CalendarModule,
  DataTableModule,
  DropdownModule,
  GrowlModule,
  MessagesModule,
  SharedModule,
  TooltipModule,
  DialogModule
} from 'primeng/primeng';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './dashboard.component';
import { DashBoardService } from './dashboard.service';
import { HttpClientModule } from '@angular/common/http';
import { MessageService } from 'primeng/components/common/messageservice';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { routing } from './dashboard.routing';
import { NgaModule } from '../layout/nga.module';
import { PatientComponent } from '../Patient';
import { SearchPatientComponent } from '../searchpatient';
import { SearchBookingComponent } from '../searchbooking';
import {PatientService } from '../shared/services/patient.service';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,
    routing,
    DataTableModule,
    SharedModule,
    DropdownModule,
    CalendarModule,
    TooltipModule,
    AutoCompleteModule,
    MessagesModule,
    GrowlModule,
    DialogModule,
    NgaModule,
    ReactiveFormsModule
  ],
  declarations: [
    DashboardComponent,
    PatientComponent,
    SearchPatientComponent,
    SearchBookingComponent
  ],
  providers: [
    DashBoardService,
    PatientService
  ]
})
export class DashboardModule { }
