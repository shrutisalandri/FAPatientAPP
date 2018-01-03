import {
    AfterViewInit,
    Component,
    OnDestroy,
    ViewEncapsulation, NgZone, Inject
} from '@angular/core';
import { GlobalEventsManager } from '../../../shared/utilities/global-events-manager';
import { GlobalState } from '../../../app.global.state';
import { Observable } from 'rxjs/Rx';
import { Router, ActivatedRoute } from '@angular/router';
import { UserService } from "../../../shared/services/user.service";

import { User } from '../../../shared/models/user';
import { Role } from '../../../shared/models/role.enum';
import * as _ from 'lodash';
@Component({
    selector: 'app-page-top',
    styleUrls: ['./page-top.scss'],
    templateUrl: './page-top.html',
    encapsulation: ViewEncapsulation.None
})
export class PageTopComponent implements AfterViewInit {
    public isScrolled: boolean;
    public name: string;
    public lastName: string;
    public menuList: Array<any>;
    public activeMenuItem: string;

    //public loggedinCustomer: SelectedListItem = new SelectedListItem();
    private selectedCustomer: string;
    private selectedClient: string;
    private userDetails: User;
    private role = Role;
    private currentUrl: string;
    constructor(
        private route: ActivatedRoute,
        private _state: GlobalState,
        private router: Router,
        private _userService: UserService,
       
        private _globalEventsManager: GlobalEventsManager,
        @Inject('Store') private storage: Storage,
        private zone: NgZone) {
       

        this.userDetails = this._userService.getCurrentUser();
        if (this.userDetails) {
            this.name = this.userDetails.name;
            // Prepare menu
            this.prepareMenus();
            this._globalEventsManager.addEmitter.subscribe(
                (isReload) => this.zone.run(() => {
                    // mode will be null the first time it is created,
                    if (_globalEventsManager !== null) {
                        
                    }
                }));
            this.activeMenuItem = this.menuList[0].title;
        }
    }
    public ngAfterViewInit(): void {
        jQuery('[data-toggle="tooltip"]').tooltip({
            trigger: 'hover'
        });
    }
    public routeItemClick(menuItem) {
        this.activeMenuItem = menuItem.title;
    }
    public signOut() {
        this.storage.clear();
        localStorage.clear();
        sessionStorage.clear();
        // Load layout after logged in
        this._globalEventsManager.isUserLoggedIn(false);
        this.router.navigate(['/account']);
    }

    
    public scrolledChanged(isScrolled) {
        this.isScrolled = isScrolled;
    }

    redirectToCurrentUrl() {
        let path = this.router.url.split('?')[0];
        this.router.navigate([path], { queryParams: { refresh: Math.floor(Math.random() * 100) } });
    }
    private prepareMenus() {
        this.menuList = this.userDetails.roleName == Role[Role.Admin].toString() ?
            [{
                title: 'Customer',
                link: '/customer',
                icon: 'fa fa-address-card'
            },
            {
                title: 'Client',
                link: '/client/add',
                icon: 'fa fa-user-plus'
            },
            {
                title: 'Client Api',
                link: '/client',
                icon: 'fa fa-cogs'
            }]
            : [{
                title: 'Client',
                link: '/client/add',
                icon: 'fa fa-user-plus'
            },
            {
                title: 'Client Api',
                link: '/client',
                icon: 'fa fa-cogs'
            }];
    }
}
