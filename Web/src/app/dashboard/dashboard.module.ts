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
import { CommonModule } from '@angular/common';
import { Dashboard } from './dashboard.component';
import { DashBoardService } from './dashboard.service';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { MessageService } from 'primeng/components/common/messageservice';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { routing } from './dashboard.routing';
import { NgaModule } from '../layout/nga.module';


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
    NgaModule
  ],
  declarations: [
    Dashboard,
  ],
  providers: [
    DashBoardService
  ]
})
export class DashboardModule { }
