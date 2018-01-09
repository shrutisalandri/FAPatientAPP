/// <reference path="searchpatient.component.ts" />
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule, FormGroup, FormControl, Validators, FormBuilder, FormArray } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CommonModule } from '@angular/common';
import { SearchPatientComponent } from './searchpatient.component';
import { DataTableModule, SharedModule } from 'primeng/primeng';
import { PatientService } from '../shared/services/patient.service'

@NgModule({
  imports: [
    FormsModule,
    BrowserModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    CommonModule,
    DataTableModule,
    SharedModule
  ],

  declarations: [
    SearchPatientComponent
  ],
  providers: [
    PatientService
    
  ],
  exports: [
    SearchPatientComponent
    ],
})
export class SearchPatientModule {

}
