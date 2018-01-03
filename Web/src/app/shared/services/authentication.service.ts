import { HttpUtility } from '../utilities/http/http-utility.service';
import { Injectable, Inject } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { User, ResponseModel } from '../models';
import { UserService } from './user.service';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/switchMap';

@Injectable()
export class AuthenticationService {
    constructor(private http: HttpUtility,
        private _userService: UserService) { }

        verifyUserAndGetToken(user: User) {
            return this.verifyUser(user)
                .switchMap((resp) => {
                    let response: ResponseModel = resp as ResponseModel;
                    if (response.isSuccess) {
                        // Store the user in sessionstorage
                        let currentUser = response.data as User;
                        currentUser.password = user.password;
                        currentUser.email = currentUser.email || user.email
                        currentUser.roleName = currentUser.roleName || user.roleName;
                        // Store User on the web  storage
                        this._userService.setCurrentUser(currentUser);
                        // Get Token
                        return this.getAuthenticationToken(user);
                    } else {
                        return Observable.throw(new Error('User verification failed'))
                    }
                })
        }
    
        private verifyUser(user: User): Observable<any> {
            // let body = "email=" + user.email + "&password=" + user.password;
            let body = JSON.stringify({ email: user.email, password: user.password });
            return this.http.post('api/authentication/VerifyUser',  body)
        }
    
        logout() {
            // remove user from local storage to log user out
            this._userService.removeCurrentUser();
        }
        public getAuthenticationToken(user: User) {
            let body = "email=" + user.email + "&password=" + user.password;
            return this.http.post('api/token', body)
        }
    
        getTokenFromStorage() {
            return this._userService.getCurrentUser().accessToken;
        }
}
