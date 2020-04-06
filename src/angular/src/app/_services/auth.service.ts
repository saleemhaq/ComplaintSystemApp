import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from '../../environments/environment';
import { User } from '../_models/user';

@Injectable({
  
  providedIn: 'root'
})
export class AuthService {

  // This is defined in the environment.ts
  baseUrl = environment.apiUrl + 'auth/';

  jwtHelper = new JwtHelperService();
  decodedToken: any;
  currentUser: User;

  photoUrl = new BehaviorSubject<string>('../../assets/user.png');
  
  currentPhotoUrl = this.photoUrl.asObservable();

  constructor(private http: HttpClient) { }

    changeMemberPhoto(photoUrl: string) {
    if (photoUrl) {
      this.photoUrl.next(photoUrl);
    }
  }

  login(model: any) {
  
    return this.http.post(this.baseUrl + 'login', model)
    
      .pipe(
       
        map((response: any) => {
        
          const responseUser = response;

          if (responseUser) {
            localStorage.setItem('token', responseUser.token);
            localStorage.setItem('user', JSON.stringify(responseUser.user));
            this.decodedToken = this.jwtHelper.decodeToken(responseUser.token);
            this.currentUser = responseUser.user;
            this.changeMemberPhoto(this.currentUser.photoUrl);
          }
        })
      );
  }

  register(user: User) {
    return this.http.post(this.baseUrl + 'register', user);
  }
  loggedIn() {
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }

  roleMatch(allowedRoles): boolean {
    const userRoles = this.decodedToken.role as Array<string>;

    let isMatch = false;
    allowedRoles.forEach(element => {    
      if (userRoles.includes(element)) {      
        isMatch = true;
        return;
      }
    });

    return isMatch;
  }
}
