import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

import { loginAnimation } from '../../layout/animations/shared-animations';
import { ResponseModel } from '../../shared/models';
import { ResetPasswordService } from '../../shared/services';

@Component({
    selector: 'reset-password',
    templateUrl: './reset-password.component.html',
    styleUrls: ['../account.scss'],
    animations: [
        loginAnimation
    ]
})
export class ResetPasswordComponent {

    guid: string;
    responseMessage: any;

    constructor(private activatedRoute: ActivatedRoute, private resetPasswordService: ResetPasswordService) {
        activatedRoute.params.subscribe(params => {
            this.guid = params['id'];
        });
    }
    form = new FormGroup({
        email: new FormControl('',
            [Validators.required]
        ),
        newPassword: new FormControl('',
            [Validators.required],
        ),
        confirmedPassword: new FormControl('',
            [Validators.required],
        )
    })

    resetPassword() {
        if (this.form.valid) {
            if ((this.newPassword.value as String) === (this.confirmedPassword.value as string)) {
                this.resetPasswordService.resetPassword(this.guid, this.email.value, this.newPassword.value).subscribe(resp => {
                    let response = resp as ResponseModel;
                    this.responseMessage = response.data;
                })
            }
            else {
                this.form.setErrors({
                    invalidEmail: true
                });
            }
        }
    }

    get newPassword() {
        return this.form.get('newPassword');
    }

    get confirmedPassword() {
        return this.form.get('confirmedPassword');
    }

    get email() {
        return this.form.get('email');
    }
}