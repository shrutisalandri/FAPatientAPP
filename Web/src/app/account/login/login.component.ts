import { Component, ViewEncapsulation } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { loginAnimation } from '../../layout/animations/shared-animations';
import { User } from '../../shared/models';
import { AuthenticationService } from '../../shared/services';
import { UserService } from '../../shared/services';
import { GlobalEventsManager } from '../../shared/utilities/global-events-manager';

@Component({
  selector: 'login',
  encapsulation: ViewEncapsulation.Emulated,
  templateUrl: './login.component.html',
  styleUrls: ['../account.scss'],
  animations: [
    loginAnimation
  ]
})
export class LoginComponent {
  public loginform: FormGroup;
  public email: AbstractControl;
  public password: AbstractControl;
  public submitted: boolean;
  public model: any = {};
  public loading = false;
  public returnUrl: string;
  public currentUser: User;
  public displayMessages: any = [];
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private authenticationService: AuthenticationService,
    private _userService: UserService,
    private formBuilder: FormBuilder,
    private _globalEventsManager: GlobalEventsManager
  ) {
    this.currentUser = this._userService.getCurrentUser();
    if (this.currentUser != null) {
      this.router.navigate(['/dashboard']);
    }
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/dashboard';

    this.buildForm();
    if (this.currentUser != null) {
      this.router.navigate([this.returnUrl]);
    } else {
      this.authenticationService.logout();
      this.buildForm();
    }
  }

  public ngOnInit() {
  }

  public buildForm(): void {
    this.loginform = this.formBuilder.group({
      'email': [null, Validators.compose([Validators.required
        , this.checkIfEmpty
      ])],
      'password': [null, Validators.compose([Validators.required
        , Validators.minLength(1)
      ])]
    });

    this.email = this.loginform.controls['email'];
    this.password = this.loginform.controls['password'];
  }

  checkIfEmpty(fieldControl: FormControl) {
    let length = 0;
    if (fieldControl.value != null) {
      length = fieldControl.value.trim().length;
    }
    return length !== 0 ? null : { isEmpty: true };
  }

  public onSubmit(values: Object): void {
    this.submitted = true;
    if (this.loginform.valid) {
      this.login({
        email: this.loginform.value.email.trim(),
        password: this.loginform.value.password.trim()
      } as User);
    }
  }

  public login(loginData: User) {
    this.loading = true;
    this.authenticationService.verifyUserAndGetToken(loginData).subscribe(tokenResp => {
        if (tokenResp['access_token']) {
            this.currentUser = this._userService.getCurrentUser();
            this.currentUser.accessToken = tokenResp['access_token'];
            this._userService.setCurrentUser(this.currentUser);
            this._globalEventsManager.isUserLoggedIn(true);
            this.loading = false;
            this.router.navigate(['/dashboard']);
        }
        else {
            this.displayMessages = [];
            this.displayMessages.push({ severity: 'error', summary: 'Login failed', detail: 'Token generation Failed, please provide proper credentials' })
        }
    },
        (error) => {
            this.displayMessages = [];
            this.displayMessages.push({ severity: 'error', summary: 'Login failed', detail: 'Error occured while login' })
            this.loading = false;
        },
        () => {
            this.loading = false;
        }
    )
  }
}
