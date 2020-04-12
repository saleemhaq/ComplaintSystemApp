import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Complaint } from '../_models/complaint';
import { Observable } from 'rxjs';

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
  getComplaintss() {
    return this.http.get(this.baseUrl + 'getComplaints');
  }
  getComplaintById(id): Observable<Complaint> {
      return this.http.get<Complaint>(this.baseUrl + 'getComplaintById?Id=' + id);
  }
  updateComplaint(id: number, complaint: Complaint) {  
    return this.http.put(this.baseUrl + id, complaint);
  }


}
