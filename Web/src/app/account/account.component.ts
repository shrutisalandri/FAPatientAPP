import {
    AbstractControl,
    FormBuilder,
    FormGroup,
    Validators,
    FormControl
  } from '@angular/forms';
  import { ActivatedRoute, Route, Router } from '@angular/router';
  import { AuthenticationService } from '../shared/services';
  import {
    animate,
    group,
    style,
    transition,
    trigger
  } from '@angular/animations';
  import {
    Component,
    EventEmitter,
    Input,
    OnInit,
    Output,
    ViewEncapsulation
  } from '@angular/core';
  import { GlobalEventsManager } from '../shared/utilities/global-events-manager';
  import { loginAnimation } from "../layout/animations/shared-animations";
  import { ResponseModel, User } from '../shared/models';
  import { UserService } from '../shared/services';
  
  @Component({
    selector: 'account',
    encapsulation: ViewEncapsulation.Emulated,
    templateUrl: './account.component.html',
    styleUrls: ['./account.scss'],
    animations: [
      loginAnimation
    ]
  })
  export class AccountComponent {
    
    
  }
  