import {Component, OnInit} from '@angular/core';
import {AuthService} from "../../services/auth.service";
import {Route} from "@angular/router";

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss']
})
export class MenuComponent implements OnInit {
  isJudge?: boolean;

  constructor(private _authService: AuthService)
  {
  }

  async ngOnInit(){
    this.isJudge= await this._authService.hasRole('Judge');
  }

  logout() {
    this._authService.logout();
  }

}
