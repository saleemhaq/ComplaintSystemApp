import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Complaint } from '../_models/complaint';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {

  // This is defined in the environment.ts
  baseUrl = environment.apiUrl + 'complaint/';
  constructor(private http: HttpClient) { }

  registerComplaint(complaint: Complaint) {
    console.log(complaint);
    return this.http.post(this.baseUrl + 'registerComplaint', complaint);
  }
}
