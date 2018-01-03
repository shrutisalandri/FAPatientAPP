import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
declare var prodModeOn;

@Injectable()
export class Config {
    public _config: Object;
    private _env: Object;
    constructor(private http: HttpClient) {
    }
    public load() {
        // json files will be loaded here
        return new Promise((resolve, reject) => {
            this.http.get('json/environment.json')
                .subscribe((envData) => {
                    this._env = envData;

                    this.http.get('json/settings.' + envData['env'] + '.json')
                        .subscribe((data) => {
                            this._config = data;
                            resolve(true);
                        },
                        (error) => {
                            console.error(error);
                            return Observable.throw(error.json().error || 'Server error');
                        });
                });
        });
    }
    public getEnv(key: any) {
        return this._env[key];
    }
    public getByKey(key: any) {
        return this._config[key];
    }
}
