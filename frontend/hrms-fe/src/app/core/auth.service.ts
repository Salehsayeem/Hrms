import { Injectable } from '@angular/core';
import { jwtDecode } from 'jwt-decode';
import { DecodedToken } from './interfaces/auth.interface';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor() { }
  // Store token and permissions in localStorage
  storeUserData(token: string, permissions: any): void {
    localStorage.setItem('token', token);
    localStorage.setItem('permissions', JSON.stringify(permissions));
  }
  // Helper method to decode JWT token
  getDecodedToken(token: string): DecodedToken|null {
    try {
      return jwtDecode(token);
    } catch (error) {
      return null;
    }
  }

  // Helper method to check if token is expired
  isTokenExpired(decodedToken: any): boolean {
    if (!decodedToken || !decodedToken.exp) {
      return true;
    }
    const expiryTime = decodedToken.exp * 1000; // Convert to milliseconds
    return expiryTime < Date.now();
  }

  hasPermissions(): boolean {
    const permissions = localStorage.getItem('permissions');
    return permissions ? true : false;
  }

}

