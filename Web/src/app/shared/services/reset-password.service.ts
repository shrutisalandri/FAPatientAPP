import { HttpUtility } from '../utilities/http/http-utility.service';
import { Injectable, Inject } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { User, ResponseModel } from '../models';
import { UserService } from './user.service';
import {HttpClient} from '@angular/common/http';
import 'rxjs/add/operator/map';

@Injectable()
export class ResetPasswordService {
    constructor(private http: HttpUtility) { }

    sendResetPasswordLink(email){
        let body = JSON.stringify({ "email": email });
        return this.http.post('api/forgetpassword',body);
        // return Observable.of('abc');
    }

    resetPassword(id,email,password){
        let body = JSON.stringify({ "email": email, "newPassword": password });
        return this.http.put('api/forgetpassword/'+id,body);
    }
}
