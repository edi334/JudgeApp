import { Injectable } from '@angular/core';
import {environment} from '../../environments/environment';
import {HttpClient} from '@angular/common/http';
import {IJudging} from '../models/judging';
import {AuthService} from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class JudgingService {
  private readonly _baseUrl = environment.apiUrl + 'api/judging';

  constructor(
    private readonly _http: HttpClient,
    private readonly _authService: AuthService
  ) {
  }

  public async judge(judgeEntities: IJudging[]): Promise<IJudging[] | undefined> {
    const options = await this._authService.getOptions(true);
    return this._http.post<IJudging[]>(this._baseUrl, judgeEntities, options).toPromise();
  }
}
