import { Component, OnInit } from '@angular/core';
import { Complaint } from '../../_models/complaint';
import { CustomerService } from '../../_services/customer.service';
import { Router } from '@angular/router';
import { AlertifyService } from '../../_services/alertify.service';

@Component({
  selector: 'app-complaint-management',
  templateUrl: './complaint-management.component.html',
  styleUrls: ['./complaint-management.component.css']
})
export class ComplaintManagementComponent implements OnInit {
  bsModalRef: any;
  modalService: any;
  adminService: any;

  constructor(private _router: Router, private customerService: CustomerService, private alertify: AlertifyService) { }
  complaints: Complaint[];
  ngOnInit() {
    this.getComplaints();
  }

  getComplaints(){
    this.customerService.getComplaintss().subscribe((data: Complaint[]) => {
      console.log(data);
      this.complaints = data;
    }, error => {
      this.alertify.error(error);
    });
  }
  // editComplaintModal(complaint: Complaint) {
  //   const initialState = {
  //     complaint,
  //     roles: this.getRolesArray(user)
  //   };
  //   this.bsModalRef = this.modalService.show(RolesModalComponent, {initialState});
  //   // We need to make use of the 'updateSelecteRoles' output property from the
  //   // roles-modal.component.ts inside this parent component.  We will get this from
  //   // the 'bsModalRef.content' property.
  //   this.bsModalRef.content.updateSelectedRoles.subscribe((values: any) => {
  //     const rolesToUpdate = {
  //       // We are going use the .filter operator to filter out any of the role names that
  //       // are not checked. After the filter, we are just going to return only the name so we
  //       // will use the .map operator since our API AdminController.EditRoles(string userName, RoleEditDto roleEditDto)
  //       // only needs the role names.
  //       // We are using the spread '...' operator which is a great feature in javascript
  //       // which spreads the values into a new array
  //       roleNames: [...values.filter((el: any) => el.checked === true)
  //         .map((el: any) => el.name)]
  //     };

  //     if (rolesToUpdate) {
  //       this.adminService.updateUserRoles(user, rolesToUpdate).subscribe(() => {
  //         user.roles = [...rolesToUpdate.roleNames];
  //       }, error => {
  //         this.alertify.error(error);
  //       });
  //     }
  //   });
  // }
  // getRolesArray(user: any) {
  //   throw new Error("Method not implemented.");
  // }
}
