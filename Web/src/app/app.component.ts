import { CommonUtility } from './shared/utilities/common-utility.service';
import { GlobalEventsManager } from './shared/utilities/global-events-manager';
import { GlobalState } from './app.global.state';
import { layoutPaths } from './layout/theme.constants';
import { routeAnimation } from './layout/animations/shared-animations';
import { Router } from '@angular/router';
import {
  Component,
  NgZone,
  OnInit,
  ViewContainerRef,
  ViewEncapsulation,
  } from '@angular/core';

/*
 * App Component
 * Top Level Component
 */
@Component({
  selector: 'app-root',
  encapsulation: ViewEncapsulation.None,
  styleUrls: ['./app.component.scss'],
  templateUrl: './app.component.html',
  animations:[routeAnimation]
})
export class AppComponent implements OnInit {
  
  public isMenuCollapsed: boolean = false;
  public isMenuExpandedonHover: boolean = true;
  constructor(
    private _state: GlobalState,
    private _globalEventsManager: GlobalEventsManager,
    private viewContainerRef: ViewContainerRef,
    private zone: NgZone,
    private router: Router,
  ) {
    this._state.subscribe('menu.isCollapsed', (isCollapsed) => {
      this.isMenuCollapsed = isCollapsed;
    });
    this._state.subscribe('menu.isExpandedOnHover', (isExpandedOnHover) => {
      this.isMenuExpandedonHover = isExpandedOnHover;
    });
  }

  public ngOnInit(): void {
    // After Loggin Success fire this event
  
    window.addEventListener('keydown', this.handleKeyboardZoom);
    window.addEventListener('mousewheel', this.handleMouseScrollZoom);
    window.addEventListener('DOMMouseScroll', this.handleMouseScrollZoom);
  }


  getRouteAnimation(outlet) {
    return outlet.activatedRouteData.animation
  }

  private handleKeyboardZoom(event) {
    if ((event.keyCode == 107 && event.ctrlKey == true) || (event.keyCode == 109 && event.ctrlKey == true)) {
      event.preventDefault();
    }
  }

  private handleMouseScrollZoom(event){
    if(event.ctrlKey == true){
      event.preventDefault();
    }
  }

}
