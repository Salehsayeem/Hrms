import { Component } from '@angular/core';
import { AuthService } from '../../core/auth.service';
import { Router } from '@angular/router';
import { DecodedToken } from '../../core/interfaces/auth.interface';

@Component({
  selector: 'app-not-found',
  templateUrl: './not-found.component.html',
  styleUrl: './not-found.component.css'
})
export class NotFoundComponent {
  decodedToken: DecodedToken | null = null;
  constructor(private authService: AuthService, private router: Router,) {
    const jwt = localStorage.getItem('token')
    if (jwt) {
      this.decodedToken = this.authService.getDecodedToken(jwt);
    }
    else {
      this.decodedToken = null
    }

  }
  Redirect(){
    if (this.decodedToken === null || this.decodedToken === undefined) {
      this.router.navigate(['/auth/login']);
    }
  }
}
