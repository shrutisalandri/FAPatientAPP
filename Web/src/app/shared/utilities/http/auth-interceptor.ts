import { HttpClient, HttpRequest } from '@angular/common/http';
import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpHeaders,
  HttpInterceptor,
  HttpResponse
} from '@angular/common/http';
import { Inject, Injectable, Injector } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import { User } from '../../models/index';
import 'rxjs/add/operator/do';
import * as _ from 'lodash';
import { AuthenticationService } from '../../services/authentication.service';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/mergeMap';
import 'rxjs/add/operator/retryWhen';


@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(
    private _injector: Injector,
    @Inject('Store') private store: Storage
  ) {
  }

  intercept(req: HttpRequest<any>,
    next: HttpHandler): Observable<HttpEvent<any>> {
    if (req.url.search('AppBackend/Login') >= 0) {
      return next.handle(req.clone({
        headers: this.addHeader(req)
      }))
    } else {
      return Observable.of(1).mergeMap(x => {
        return next.handle(req.clone({
          headers: this.addHeader(req)
        }))
      })
        .retryWhen((errors) => {
          return errors
            .zip(Observable.range(1, 2))
            .flatMap(([error, i]) => {
              if (error.status === 401 && i < 2) {
                return this.reGenerateToken();
              }
              // change status as we are restrcting user to navigate to error page on 401 status
              let err: any = _.cloneDeep(error);
              err.status = err.status || 500;
              return Observable.throw(err)
            })
        })
    }

  }

  addHeader(req: HttpRequest<any>) {
    let headers: HttpHeaders = req.headers;
    if (req.url.search('api/token') === -1) {
      let token = this.getToken();
      // For each Request
      return headers.set('Authorization', `Bearer  ${token}`)
        .set('Content-Type', 'application/json');
    } else {
      // To get Api Token
      return headers.set('Content-Type', 'application/x-www-form-urlencoded');
    }

  }

  private getToken() {
    let user: User = JSON.parse(this.store.getItem('currentUser'))
    return user ? user.accessToken || '' : '';
  }

  private reGenerateToken() {
    let authService = this._injector.get(AuthenticationService)
    let user: User = JSON.parse(this.store.getItem('currentUser')) || new User();

    var subject = new Subject<boolean>();
    // Convert async call to sync call
    authService.getAuthenticationToken(user)
      .subscribe(
      res => {
        if (res['access_token']) {
          user.accessToken = res['access_token'];
          // Store the user in sessionstorage
          this.store.setItem('currentUser', JSON.stringify(user));
          subject.next(false);
        } else {
          subject.next(true);
        }
      },
      (err) => {
        subject.next(true);
      }
      );
    return subject.asObservable();
  }
}
