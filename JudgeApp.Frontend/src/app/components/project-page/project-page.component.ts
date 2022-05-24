import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {SnackService} from "../../services/snack.service";
import {IProject} from "../../models/project";
import {ProjectService} from "../../services/project.service";

@Component({
  selector: 'app-project-page',
  templateUrl: './project-page.component.html',
  styleUrls: ['./project-page.component.scss']
})
export class ProjectPageComponent implements OnInit {
  private project?: IProject;

  constructor(private formBuilder: FormBuilder,
              private snack: SnackService,
              private projectService:ProjectService
  ) {
  }

  ngOnInit(){
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
        this.project = await this.projectService.update(this.form.value);
      } else {
        this.project = await this.projectService.create(this.form.value);
      }
      this.snack.display('Your project details have been updated!');
    } catch (e) {
      this.snack.showError(e);
    }


  }
}
