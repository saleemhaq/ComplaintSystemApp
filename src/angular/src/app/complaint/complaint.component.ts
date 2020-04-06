import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { User } from '../_models/user';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { AlertifyService } from '../_services/alertify.service';
import { Router } from '@angular/router';
import { Complaint } from '../_models/complaint';
import { CustomerService } from '../_services/customer.service';

@Component({
  selector: 'app-complaint',
  templateUrl: './complaint.component.html',
  styleUrls: ['./complaint.component.css']
})
export class ComplaintComponent implements OnInit {

  // This will allow output from this component which is an emit event
  // and make sure EventEmitter is from @angular/core
  @Output() cancelComplaint = new EventEmitter();

  // We are now swapping this for a strongly typed User object
  // model: any = {};
  complaint: Complaint;

  // Tracks the value and validity state of a group of FormControl instances.
  registerComplaintForm: FormGroup;

  // tslint:disable-next-line: no-shadowed-variable
  constructor(private customerService: CustomerService,
    private alertify: AlertifyService, private fb: FormBuilder,
    private router: Router) { }

  ngOnInit() {
    this.createregisterComplaintForm();
  }

  // Create a registration form using FormBuilder
  createregisterComplaintForm() {
    this.registerComplaintForm = this.fb.group({
      complaintName: ['', Validators.required],
      description: ['', Validators.required],
      email: ['', Validators.required],
      complaintRegarding: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required]
    });
  }

  register() {
    if (this.registerComplaintForm.valid) {
      this.complaint = Object.assign({}, this.registerComplaintForm.value);
      console.log(this.complaint);
      // We will now register the user
      this.customerService.registerComplaint(this.complaint).subscribe(() => {
        this.alertify.success('Complaint Registered');
      }, error => {
        this.alertify.error(error);
      });
    }
  }
  cancel() {
    this.cancelComplaint.emit(false);
  }
}
