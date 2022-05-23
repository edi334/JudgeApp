import {Component} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {SnackService} from "../../services/snack.service";
import {AuthService} from "../../services/auth.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-register-page',
  templateUrl: './register-page.component.html',
  styleUrls: ['./register-page.component.scss']
})
export class RegisterPageComponent {

  constructor(private formBuilder: FormBuilder,
              private snack: SnackService,
              private authService: AuthService,
              private router: Router
  ) {
  }

  form: FormGroup = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(100)]],
      confirmPassword: ['', [Validators.required]],
      firstName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
      lastName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]]
    },
    {
      validators: this.mustMatch('password', 'confirmPassword')
    });

  mustMatch(controlName: string, matchingControlName: string) {
    return (formGroup: FormGroup) => {
      const control = formGroup.controls[controlName];
      const matchingControl = formGroup.controls[matchingControlName];
      if (matchingControl.errors && !matchingControl.errors['mustMatch']) {
        return;
      }
      if (control.value !== matchingControl.value) {
        matchingControl.setErrors({mustMatch: true});
      } else {
        matchingControl.setErrors(null);
      }
    };
  }

  public async submit() {
    this.snack.clear();

    if (!this.form.valid) {
      this.form.markAllAsTouched();
      this.snack.display('Fill all fields');
      return;
    }

    try {
      await this.authService.register(this.form.value);
      this.snack.display('Succes');
      await this.router.navigate(['/']);
    } catch (e) {
      this.snack.showError(e);
    }
  }
}
