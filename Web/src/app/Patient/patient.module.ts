import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule,FormGroup, FormControl, Validators, FormBuilder, FormArray } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CommonModule } from '@angular/common';
import { PatientComponent } from './patient.component';
import { MyDatePickerModule } from 'mydatepicker';
import { RadioButtonModule, CalendarModule, InputSwitchModule, CheckboxModule } from 'primeng/primeng';

@NgModule({
  imports: [
    FormsModule,
    BrowserModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    CommonModule,
    MyDatePickerModule,
    RadioButtonModule, CalendarModule, InputSwitchModule, CheckboxModule
  ],

  declarations: [
    PatientComponent

  ],
  providers: [
  ],
  exports: [
    PatientComponent],
})
export class PatientModule {

}
