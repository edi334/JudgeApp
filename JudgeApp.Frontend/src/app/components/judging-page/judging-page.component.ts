import { Component, OnInit } from '@angular/core';
import {IProject} from '../../models/project';
import {ProjectService} from '../../services/project.service';
import {AuthService} from '../../services/auth.service';
import {JudgingService} from '../../services/judging.service';
import {CdkDragDrop, moveItemInArray} from '@angular/cdk/drag-drop';
import {StatusService} from '../../services/status.service';
import {SnackService} from '../../services/snack.service';
import {IJudging} from '../../models/judging';

@Component({
  selector: 'app-judging-page',
  templateUrl: './judging-page.component.html',
  styleUrls: ['./judging-page.component.scss']
})
export class JudgingPageComponent implements OnInit {
  projects: IProject[] = [];
  isJudging = false;

  constructor(
    private readonly _projectService: ProjectService,
    private readonly _judgingService: JudgingService,
    private readonly _statusService: StatusService,
    private readonly _snackService: SnackService,
    private readonly _authService: AuthService
  ) { }

  async ngOnInit(): Promise<void> {
    const isJudging = await this._statusService.isStatus('Judging');
    this.isJudging = isJudging!;
    const projects = await this._projectService.getAll();
    this.projects = projects!;
  }

  drop(event: CdkDragDrop<string[]>): void {
    moveItemInArray(this.projects, event.previousIndex, event.currentIndex);
  }

  public async judge(): Promise<void> {
    const session = await this._authService.getSession();
    try {
      const payload: IJudging[] = [];

      this.projects.forEach(p => {
        const judging: IJudging = {
          judgeId: session.userId,
          projectId: p.id!,
          standing: this.projects.findIndex(pr => pr.id === p.id) + 1
        };
        payload.push(judging);
      });

      await this._judgingService.judge(payload);
      this._snackService.display('Judging order saved!');
    } catch {
      this._snackService.display('Oops! Something went wrong!');
    }
  }
}
