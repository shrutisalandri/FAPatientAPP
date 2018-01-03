
import { SideBarSearchResultsComponent } from './sidebar-search-results.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AutoCompleteModule } from 'primeng/primeng';
@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        AutoCompleteModule
    ],
    declarations: [
        SideBarSearchResultsComponent
    ],
    exports: [
        SideBarSearchResultsComponent
    ]
})
export class SidebarModule { }
