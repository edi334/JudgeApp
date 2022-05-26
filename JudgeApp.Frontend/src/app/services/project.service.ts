import {Injectable} from '@angular/core';
import {IProject} from "../models/project";
import {environment} from "../../environments/environment";
import {AuthService} from "./auth.service";
import {HttpClient} from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class ProjectService {
  private readonly _baseUrl = environment.apiUrl;

  constructor(private authService: AuthService,
              private http: HttpClient) {
  }

  public async getAll(): Promise<IProject[] | undefined> {
    const options = await this.authService.getOptions(true);
    const session = await this.authService.getSession();
    const url = this._baseUrl + `api/projects/${session.userId}`;
    return await this.http.get<IProject[]>(url, options).toPromise();
  }

  public async create(data: IProject): Promise<IProject | undefined> {
    const url = this._baseUrl + 'api/projects';
    const options = await this.authService.getOptions(true);
    return await this.http.post<IProject>(url, data, options).toPromise();
  }

  public async update(data: IProject): Promise<IProject | undefined> {
    const url = this._baseUrl + 'api/projects';
    const options = await this.authService.getOptions(true);
    return await this.http.patch<IProject>(url, data, options).toPromise();
  }

  public async getByUserId(id: string): Promise<IProject | undefined> {
    const url = this._baseUrl + 'api/projects/user/' + id;
    const options = await this.authService.getOptions(true);
    return await this.http.get<IProject>(url, options).toPromise();
  }
}
