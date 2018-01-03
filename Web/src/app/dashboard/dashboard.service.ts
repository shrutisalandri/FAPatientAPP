import { Observable, Subject } from 'rxjs/Rx';
import {
  Headers,
  Http,
  RequestOptions,
  Response
} from '@angular/http';
import { HttpUtility } from '../shared/utilities/http/http-utility.service';
import { Injectable, Inject } from '@angular/core';
import 'rxjs/add/observable/of';
import 'rxjs/add/observable/from';


@Injectable()
export class DashBoardService {
  constructor(
    @Inject('Store') private storage: Storage,
    private httpUtility: HttpUtility) {
  }
}
