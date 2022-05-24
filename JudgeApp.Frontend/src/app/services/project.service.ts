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

  public async create(data: IProject): Promise<any> {
    const url = this._baseUrl + 'api/projects';
    const options = await this.authService.getOptions(true);
    return await this.http.post<{ data: IProject }>(url, data, options).toPromise();
  }

  public async update(data: IProject): Promise<any> {
    const url = this._baseUrl + 'api/projects';
    const options = await this.authService.getOptions(true);
    return await this.http.patch<{ data: IProject }>(url, data, options).toPromise();
  }

  public async getByUserId(id: string): Promise<any> {
    const url = this._baseUrl + 'api/projects/user/' + id;
    const options = await this.authService.getOptions(true);

    try {
      return await this.http.get<{ data: IProject }>(url, options).toPromise();
    } catch {
      return null;
    }
  }

}
