import { Component, ViewEncapsulation } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

import { loginAnimation } from '../../layout/animations/shared-animations';
import { ResponseModel } from '../../shared/models';
import { ResetPasswordService } from '../../shared/services';

@Component({
    selector: 'forgot-password',
    encapsulation: ViewEncapsulation.Emulated,
    templateUrl: './forgot-password.component.html',
    styleUrls: ['../account.scss'],
    animations: [
        loginAnimation
    ]
})
export class ForgotPasswordComponent {

    responseMessage: any;

    constructor(private resetPasswordService: ResetPasswordService) { }
    form = new FormGroup({
        email: new FormControl('',[Validators.required])
    })

    sendEmail() {
        if (this.form.valid) {
            this.resetPasswordService.sendResetPasswordLink(this.email.value).subscribe(
                resp => {
                    let response = resp as ResponseModel;
                    this.responseMessage = response.data;
                }
            );
        }
    }

    get email() {
        return this.form.get('email');
    }
}