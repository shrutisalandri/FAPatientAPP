/// <reference path="searchbooking.component.ts" />
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule, FormGroup, FormControl, Validators, FormBuilder, FormArray } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CommonModule } from '@angular/common';
import { SearchBookingComponent } from './searchbooking.component';
import { DataTableModule, SharedModule } from 'primeng/primeng';

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
    SearchBookingComponent
  ],
  providers: [
    
  ],
  exports: [
    SearchBookingComponent
    ],
})
export class SearchBookingModule {

}
