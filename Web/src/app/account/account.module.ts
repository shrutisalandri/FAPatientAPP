import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LoginComponent } from './login/login.component';
import { MessagesModule } from 'primeng/primeng';
import { NgaModule } from '../layout/nga.module';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { routing } from './account.routing';
import { AccountComponent } from './account.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';

@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        FormsModule,
        NgaModule,
        MessagesModule,
        routing
    ],
    declarations: [
        LoginComponent,
        AccountComponent,
        ForgotPasswordComponent,
        ResetPasswordComponent
    ]
})
export class AccountModule { }
