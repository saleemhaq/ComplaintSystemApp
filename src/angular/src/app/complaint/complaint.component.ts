import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { User } from '../_models/user';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { AlertifyService } from '../_services/alertify.service';
import { Router, ActivatedRoute } from '@angular/router';
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
  complaintId: number; 
  errorMessage: any;
  // We are now swapping this for a strongly typed User object
  // model: any = {};
  complaint: Complaint;

  // Tracks the value and validity state of a group of FormControl instances.
  registerComplaintForm: FormGroup;

  // tslint:disable-next-line: no-shadowed-variable
  constructor(private customerService: CustomerService,
    private alertify: AlertifyService, private fb: FormBuilder,
    private router: Router , private _avRoute: ActivatedRoute) { 
      if (this._avRoute.snapshot.params["id"]) {  
        this.complaintId = this._avRoute.snapshot.params["id"];  
    }
  }

  ngOnInit() {
  
    if (this.complaintId > 0) {      
      this.customerService.getComplaintById(this.complaintId)
          .subscribe(resp => {
            this.registerComplaintForm.setValue(resp);
            // console.log(resp);
          }
            );
   }
   this.createregisterComplaintForm();
  }

  // Create a registration form using FormBuilder
  createregisterComplaintForm() {
    this.registerComplaintForm = this.fb.group({
      id: new FormControl(''),
      status: new FormControl(''),
      isActive : new FormControl(''),
      isDeleted: new FormControl(''),
      creationTime: new FormControl(''),
      updatedTime: new FormControl(''),
      creatorUserId: new FormControl(''),
      updatedBy : new FormControl(''),
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

      if(this.complaint.id > 0 )
      {
        console.log(this.complaint.id);
        this.customerService.updateComplaint(this.complaint.id, this.complaint)
        .subscribe(() => {
          this.alertify.success('Complaint updated successfully!');  
          // Reset the form control.  This means by default, it is marked as pristine, marked as
          // untouched and value is set to null.
          // Add the this.user as the parameter so that it will reset the elements and fill
          // the controls with the bindings coming from the this.user variable
          this.registerComplaintForm.reset(this.complaint);
        }, error => {
          this.alertify.error(error);
        });
      } else {
      console.log(this.complaint);
      // We will now register the user
      this.customerService.registerComplaint(this.complaint).subscribe(() => {
        this.alertify.success('Complaint Registered');
      }, error => {
        this.alertify.error(error);
      });
     }
    }
  }
  cancel() {
    this.cancelComplaint.emit(false);
  }
}
