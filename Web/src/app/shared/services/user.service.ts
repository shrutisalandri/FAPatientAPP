import { HttpUtility } from '../utilities/http/http-utility.service';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { User } from '../models/user';
import 'rxjs/add/observable/of';


@Injectable()
export class UserService {
    constructor(
        private http: HttpUtility,
        @Inject('Store') private store: Storage
    ) { }

    getCurrentUser(): User {
        return JSON.parse(this.store.getItem('currentUser'));
    }

    setCurrentUser(user: User) {
        this.store.setItem('currentUser', JSON.stringify(user));
    }

    removeCurrentUser() {
        this.store.removeItem('currentUser');
    }
}
