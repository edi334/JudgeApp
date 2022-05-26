import {Component, OnInit} from '@angular/core';
import {ProjectService} from "../../services/project.service";
import {IProject} from "../../models/project";
import {StatusService} from "../../services/status.service";

@Component({
  selector: 'app-result-page',
  templateUrl: './result-page.component.html',
  styleUrls: ['./result-page.component.scss']
})
export class ResultPageComponent implements OnInit {
  projects?: IProject[];
  statusProjectUpload: boolean | undefined;

  constructor(private _projectService: ProjectService,
              private _statusService: StatusService
  ) {
  }

  async ngOnInit() {
    this.projects = await this._projectService.getAll();
    this.statusProjectUpload = await this._statusService.isStatus('Getting Results');
  }

}
