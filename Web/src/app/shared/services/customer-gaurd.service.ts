import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { User } from '../models/index';
import { UserService } from './user.service';
import { Role } from '../models/role.enum';

@Injectable()
export class CustomerGuardService implements CanActivate {

    constructor(private router: Router,
        private _userService: UserService) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        // check guards for stopping tester or project admin from
        // accessing user crud and user role crud pages
        let currentUser: User = this._userService.getCurrentUser();
        if (currentUser != null) {
            // check admin or customer logged in
            if (currentUser.roleName == Role[Role.Admin]) {
                return true;
            }
            else {
                //not admin redirect to dashboard
                this.router.navigate(['/dashboard']);
                return false;
            }
        }
        // not logged in so redirect to login page with the return url
        this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
        return false;
    }
}
