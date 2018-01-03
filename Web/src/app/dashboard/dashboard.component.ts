import * as _ from 'lodash';
import { ActivatedRoute } from '@angular/router';
import {
  Component,
  Inject,
  OnInit,
  ViewChild,
  ViewEncapsulation
} from '@angular/core';
import { DashBoardService } from './dashboard.service';
import { fadeInAnimation, slideInOut } from '../layout/animations/shared-animations';
import { GlobalEventsManager } from '../shared/utilities/global-events-manager';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-dashboard',
  styleUrls: ['./dashboard.scss'],
  templateUrl: './dashboard.html',
  animations: [fadeInAnimation, slideInOut]
})
export class Dashboard implements OnInit {

  constructor(private route: ActivatedRoute,
    private globalEventsManager: GlobalEventsManager,
    private dashBoardService: DashBoardService,
    @Inject('Store') private storage: Storage
  ) {
    this.route.params.subscribe(params => {
    })
  }

  public ngOnInit(): void {
  }

}
