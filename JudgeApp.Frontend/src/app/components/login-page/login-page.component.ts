import {Component, OnInit} from '@angular/core';
import {FormBuilder, Validators} from "@angular/forms";
import {AuthService} from "../../services/auth.service";
import {ActivatedRoute, Router} from "@angular/router";
import {SnackService} from "../../services/snack.service";

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.scss']
})
export class LoginPageComponent implements OnInit {
  public form = this.formBuilder.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', Validators.required]
  })
  private saving?: boolean;

  constructor(private formBuilder: FormBuilder,
              private authService: AuthService,
              private router: Router,
              private actr: ActivatedRoute,
              private snack: SnackService) {
  }

  ngOnInit(): void {
  }

  public async submit(e: Event) {
    this.snack.clear();
    this.form.markAllAsTouched();
    if (!this.form.valid) {
      this.snack.showErrorMessage('Check the form before submitting.');
      return;
    }
    this.saving = true;
    try {
      await this.authService.login(this.form.value);
      const returnUrl = this.actr.snapshot.queryParams['returnUrl'];
      await this.router.navigate(returnUrl ? [returnUrl] : ['/']);
      this.snack.display('Logged in successfully!');
    } catch (e) {
      this.snack.showError(e);
    }
    this.saving = false;
  }


}
