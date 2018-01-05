import { APP_BASE_HREF } from '@angular/common';
import {
    APP_INITIALIZER,
    ApplicationRef,
    ErrorHandler,
    NgModule
} from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BrowserModule } from '@angular/platform-browser';
import { Config } from './shared/utilities/config';
import { CustomExceptionHandler } from './shared/utilities/exception-handler/custom-exception-handler';
import { ErrorPageComponent } from './layout/components/error-page/error-page.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { GlobalEventsManager } from './shared/utilities/global-events-manager';
import { GlobalState } from './app.global.state';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { HttpModule } from '@angular/http';
import { HttpUtility } from './shared/utilities/http/http-utility.service';
import { NgaModule } from './layout/nga.module';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { routing } from './app.routing';
import { RadioButtonModule, CalendarModule, InputSwitchModule, CheckboxModule} from 'primeng/primeng';

export function moduleResoveFactory(config: Config) {
    return () => config.load();
}

export function moduleResoveFactoryGem(a: GlobalEventsManager) {
    return () => a;
}

export function sessionResolveFactory() {
    return sessionStorage;
}

/**
 * `AppModule` is the main entry point into Angular2's bootstraping process
 */
@NgModule({
    bootstrap: [AppComponent],
    declarations: [
        AppComponent,
        ErrorPageComponent
    ],
    imports: [ // import Angular's modules
        HttpModule,
        BrowserModule,
        BrowserAnimationsModule,
        HttpClientModule,
        RouterModule,
        FormsModule,
        ReactiveFormsModule,
        NgaModule.forRoot(),
      routing,
      RadioButtonModule,
      CalendarModule,
      InputSwitchModule,
      CheckboxModule
      
    ],
    providers: [
        GlobalState,
        { provide: APP_BASE_HREF, useValue: '/' },
        { provide: ErrorHandler, useClass: CustomExceptionHandler },
        GlobalEventsManager,
        {
            provide: APP_INITIALIZER,
            useFactory: moduleResoveFactoryGem,
            deps: [GlobalEventsManager],
            multi: true
        },
        Config,
        {
            provide: APP_INITIALIZER,
            useFactory: moduleResoveFactory,
            deps: [Config],
            multi: true
        },
        HttpUtility,
        {
            provide: 'Store',
            useFactory: sessionResolveFactory,
        },
    ]
})

export class AppModule {
}
