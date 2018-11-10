import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private authServices: AuthService, private router: Router,
    private alertify: AlertifyService) {}
// Check if user is logged in to authorise routes
  canActivate(): Observable<boolean> | Promise<boolean> | boolean {
    if (this.authServices.loggedIn()){
      return true;
    }
// if not authorised, display message and route to home page
    this.alertify.error('You cannot get in or authorised!');
    this.router.navigate(['/home']);
    return false;
  }
}
