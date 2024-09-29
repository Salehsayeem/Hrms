import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { jwtDecode } from "jwt-decode";
@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private router: Router) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = localStorage.getItem('token');

    if (token) {
      // Check if token is expired
      const decodedToken: any = jwtDecode(token);
      const currentTime = Math.floor(Date.now() / 1000);  // Current time in seconds

      if (decodedToken.exp && decodedToken.exp < currentTime) {
        // Token is expired, remove it from localStorage and redirect to login
        localStorage.removeItem('token');
        localStorage.removeItem('permissions');
        this.router.navigate(['/auth/login']);
        return next.handle(req);  // Stop processing the request
      }

      // Clone the request and attach the token to the Authorization header
      const clonedRequest = req.clone({
        headers: req.headers.set('Authorization', `Bearer ${token}`)
      });

      return next.handle(clonedRequest);
    }

    // If no token, pass the request as is
    return next.handle(req);
  }
}
