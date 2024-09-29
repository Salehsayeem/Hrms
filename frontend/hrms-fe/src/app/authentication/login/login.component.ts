import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../core/auth.service';
import { ApiService } from '../../core/api.service';
import { LoginRequest } from '../../core/interfaces/auth.interface';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  buttonDisabled: boolean = false;
  password: string | undefined;
  showPassword: boolean = false;
  submitted = false;
  loginForm!: FormGroup;
  loginModel: LoginRequest = {
    phone: '',
    password: ''
  };

  constructor( private fb: FormBuilder,
    private authService: AuthService,
     private apiService: ApiService,
     private router: Router,
     private toastrService : ToastrService
    ) { }

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      phone: ['', Validators.required, ],
      password: ['', Validators.required],
    });
  }
  onSubmit() {
    this.submitted = true;
    if (this.loginForm.invalid) {
      return;
    }
    this.loginModel.phone = this.loginForm.value.phone;
    this.loginModel.password = this.loginForm.value.password;

    // Check API connection
    this.apiService.checkConnection().subscribe((connected: boolean) => {
      if (connected) {
        // Proceed with login
        this.apiService.login(this.loginModel).subscribe((response: any) => {
          if (response.succeed) {
            // Store token and permissions
            this.authService.storeUserData(response.data.token, response.data.permissions);
            this.toastrService.success(response.message)
            this.router.navigateByUrl('/dashboard');
          } else {
            this.toastrService.error(response.data.message)
            this.router.navigateByUrl('/auth/login');
          }
        });
      } else {
        this.toastrService.error("Server down");
        this.router.navigateByUrl('/server-down');
      }
    });
  }
  get formControls() {
    return this.loginForm.controls;
  }
  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
    const passwordInput = document.getElementById('floatingpassword') as HTMLInputElement | null;
    if (passwordInput) {
      if (this.showPassword) {
        passwordInput.type = 'text';
      } else {
        passwordInput.type = 'password';
      }
    }
  }
}
