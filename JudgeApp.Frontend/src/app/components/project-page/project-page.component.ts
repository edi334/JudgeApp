import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from "@angular/forms";
import {SnackService} from "../../services/snack.service";
import {IProject} from "../../models/project";
import {ProjectService} from "../../services/project.service";
import {AuthService} from "../../services/auth.service";
import {IAuthSession} from "../../models/login";

@Component({
  selector: 'app-project-page',
  templateUrl: './project-page.component.html',
  styleUrls: ['./project-page.component.scss']
})
export class ProjectPageComponent implements OnInit {
  private project?: IProject;
  private session?: IAuthSession;

  constructor(private formBuilder: FormBuilder,
              private snack: SnackService,
              private projectService: ProjectService,
              private authService: AuthService
  ) {
  }

  async ngOnInit() {
    this.session = await this.authService.getSession();
    this.project = await this.projectService.getByUserId(this.session.userId);
    this.form.controls['name'].patchValue(this.project?.name);
    this.form.controls['description'].patchValue(this.project?.description);
    this.form.controls['videoLink'].patchValue(this.project?.videoLink);
    this.form.controls['githubLink'].patchValue(this.project?.githubLink);
  }

  form: FormGroup = this.formBuilder.group({
    name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
    description: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(1000)]],
    videoLink: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(200)]],
    githubLink: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]]
  })

  public async submit() {
    this.snack.clear();

    if (!this.form.valid) {
      this.form.markAllAsTouched();
      this.snack.showErrorMessage('Check the form before submitting.');
      return;
    }
    try {
      if (this.project) {
        this.form.addControl('id', new FormControl(''));
        this.form.controls['id'].patchValue(this.project?.id);
        this.project = await this.projectService.update(this.form.value);
      } else {
        this.form.addControl('userId', new FormControl(''));
        this.form.controls['userId'].patchValue(this.session?.userId);
        this.project = await this.projectService.create(this.form.value);
      }
      this.snack.display('Your project details have been updated!');
    } catch (e) {
      this.snack.showError(e);
    }


  }
}
