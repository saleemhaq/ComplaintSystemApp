import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  registerMode = false;
  complaintMode = false;


  constructor(private http: HttpClient) { }

  ngOnInit() { }

  setRegisterMode(mode: boolean) {
    this.registerMode = mode;
  }
  setComplaintMode(mode: boolean) {
    this.complaintMode = mode;
  }
}
