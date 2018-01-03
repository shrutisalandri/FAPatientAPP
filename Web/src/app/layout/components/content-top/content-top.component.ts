import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

import { GlobalState } from '../../../app.global.state';
import { fadeInAnimation } from '../../animations/shared-animations';

@Component({
  selector: 'app-content-top',
  styleUrls: ['./content-top.scss'],
  templateUrl: './content-top.html',
  animations: [fadeInAnimation]
})
export class ContentTopComponent implements OnInit {

  showOverlay: boolean;
  @Input() headerTitle:string;
  @Output() addNew = new EventEmitter();
  constructor(private _state: GlobalState) {
  }

  toggleSearcher(event: any) {
    this.showOverlay = !this.showOverlay;
    jQuery('.searcher').toggleClass('active');
  }

  public ngOnInit(): void {
  }

  public add() {
    this.addNew.emit();
  }
}
