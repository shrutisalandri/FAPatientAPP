import * as _ from 'lodash';
import {
  Component,
  Inject,
  OnInit,
  ViewChild,
  ViewEncapsulation,
  Input,
  Output,
  EventEmitter
} from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  FormsModule,
  NgForm
} from '@angular/forms';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/takeUntil';
import { Subject } from 'rxjs/Subject';
// import { SelectedListItem } from '../../../client/models/selected-item.model'
import { HttpUtility } from '../../../shared/utilities/http/http-utility.service';

@Component({
  selector: 'app-sidebar-search-results',
  templateUrl: './sidebar-search-results.html',
  styleUrls: ['./sidebar-search.scss']
})
export class SideBarSearchResultsComponent implements OnInit {
  public searchText: string;
  private unsubscribe: Subject<void> = new Subject<void>();
  @Output() emitSelectedItem = new EventEmitter<any>();
  @Input() apiRequestUrl: string;
  @Input() placeholder: string;
  private showOverlay: boolean = true;
  public selectedSearchItemId: any;

  filteredResults: any[];
  searchItem: string;
  constructor(public httpUtility: HttpUtility) {
  }
  public ngOnInit(): void {
  }
  private request(): Observable<any> {
    return Object.keys(this.apiRequestUrl).length > 0 ? this.httpUtility.get(this.apiRequestUrl) : Observable.empty<Response>();
  }

  public ngOnChanges(changes) {
    if (changes['apiRequest'] != null) {
    }
  }
  public ngOnDestroy() {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }
  public selectResultItem(itemId) {
    this.selectedSearchItemId = itemId.value;
    this.emitSelectedItem.emit(itemId.value);
  }
  
}
