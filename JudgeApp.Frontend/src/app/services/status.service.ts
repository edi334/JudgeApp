import { Injectable } from '@angular/core';
import {environment} from '../../environments/environment';
import {HttpClient} from '@angular/common/http';
import {map} from 'rxjs';

interface Status {
  name: string;
}

@Injectable({
  providedIn: 'root'
})
export class StatusService {
  private readonly _baseUrl = environment.apiUrl + 'api/status/';

  constructor(
    private readonly _http: HttpClient
  ) { }

  public async isStatus(name: string): Promise<boolean | undefined> {
    const url = this._baseUrl + 'active';
    return this._http.get<Status>(url).pipe(map(s => s.name === name)).toPromise();
  }
}
