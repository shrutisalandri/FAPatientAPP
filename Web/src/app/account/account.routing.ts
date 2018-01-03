import { LoginComponent } from './login/login.component';
import { AccountComponent } from './account.component'
import { ModuleWithProviders } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';

// Routes
export const routes: Routes = [
    {
        path: '',
        component: AccountComponent,
        children:[
            {
                path: '',
                component: LoginComponent
            }
            ,
            {
                path: 'forgot-password',
                component: ForgotPasswordComponent
            },
            {
                path: 'reset-password/:id',
                component: ResetPasswordComponent
            }
        ]
    },
];
export const routing = RouterModule.forChild(routes);

