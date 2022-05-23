import {Injectable, EventEmitter} from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {LocalStorage} from "@ngx-pwa/local-storage";
import {map, tap} from "rxjs";
import {IAuthSession, ILogin} from "../models/login";
import {Router} from "@angular/router";
import {environment} from "../../environments/environment";
import {IRegister} from "../models/register";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private _token?: string;
  private readonly _baseUrl= environment.apiUrl;
  private _session?: IAuthSession;
  private static readonly tokenStorageKey: string = 'token';
  private static readonly sessionStorageKey: string = 'session';
  private _authState: EventEmitter<boolean> = new EventEmitter();


  constructor(private http: HttpClient,
              private storage: LocalStorage,
              private router: Router) {
  }

  public async login(requestModel: ILogin): Promise<any> {
    const url = this._baseUrl + 'api/auth/login';
    return this.http.post<{ data: IAuthSession }>(url, requestModel)
      .pipe(tap(async res => {
        const authSession = res.data;
        await this.saveSession(authSession);
      }))
      .pipe(map(() => {
        return true;
      })).toPromise();
  }

  public register(data: IRegister): Promise<any> {
    const url = this._baseUrl+'api/auth/register';
    return this.http.post<IRegister>(url, data).toPromise();
  }

  public async saveSession(authSession?: IAuthSession): Promise<void> {
    if (authSession) {
      await this.storage.setItem(AuthService.tokenStorageKey, authSession.token).toPromise();
      await this.storage.setItem(AuthService.sessionStorageKey, authSession).toPromise();
    } else {
      await this.storage.removeItem(AuthService.tokenStorageKey).toPromise();
      await this.storage.removeItem(AuthService.sessionStorageKey).toPromise();
    }
    await this.loadSession();
  }

  private async loadSession(): Promise<void> {
    const initialStatus = !!this._token;
    const oldToken = this._token;
    this._token = <string>await this.storage.getItem(AuthService.tokenStorageKey).toPromise();
    if (this._token) {
      this._session = <IAuthSession>await this.storage.getItem(AuthService.sessionStorageKey).toPromise();
    } else {
      this._session = undefined;
    }
    const differentStatus = initialStatus !== !!this._token || oldToken !== this._token;
    if (differentStatus) {
      this._authState.emit(!!this._token);
    }
  }

  public async hasRole(role: string): Promise<boolean> {
    const session = await this.getSession();
    if (!session || !session.role) {
      return false;
    }

    return session.role.indexOf(role) !== -1;
  }

  public async getOptions(needsAuth?: boolean): Promise<{ headers?: HttpHeaders }> {
    return {headers: await this.getHeaders(needsAuth)};
  }

  public async getHeaders(needsAuth?: boolean): Promise<HttpHeaders> {
    if (!needsAuth) {
      return new HttpHeaders();
    }
    const session = await this.getSession();

    if (!session) {
      return new HttpHeaders();
    }

    return new HttpHeaders().append('Authorization', `${session.tokenType} ${session.token}`);
  }

  public async getSession(): Promise<IAuthSession> {
    if (!this._session) {
      this._session = <IAuthSession>await this.storage.getItem(AuthService.sessionStorageKey).toPromise();
    }
    return this._session;
  }


  public async logout(): Promise<void> {
    await this.saveSession();
    await this.router.navigate(['/']);
  }

}
