import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';
@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private authService: AuthService, private router: Router) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    const token = localStorage.getItem('token');

    if (!token) {
      this.router.navigate(['/auth/login']);
      return false;
    }
    const decodedToken = this.authService.getDecodedToken(token);

    if (!decodedToken || this.authService.isTokenExpired(decodedToken)) {
      this.router.navigate(['/auth/login']); // Token is expired
      return false;
    }
   if (!this.authService.hasPermissions()) {
    this.router.navigate(['/auth/login']);
    return false;
  }

    return true;
  }


}
