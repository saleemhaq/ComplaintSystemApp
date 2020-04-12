import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { Complaint } from '../../_models/complaint';
import { CustomerService } from '../../_services/customer.service';
import { Router } from '@angular/router';
import { AlertifyService } from '../../_services/alertify.service';
import jsPDF from 'jspdf';
import 'jspdf-autotable';

@Component({
  selector: 'app-complaint-management',
  templateUrl: './complaint-management.component.html',
  styleUrls: ['./complaint-management.component.css']
})
export class ComplaintManagementComponent implements OnInit {
  bsModalRef: any;
  modalService: any;
  adminService: any;
  @ViewChild('content') content: ElementRef;

  constructor(private _router: Router, private customerService: CustomerService, private alertify: AlertifyService) { }
  complaints: Complaint[];
  ngOnInit() {
    this.getComplaints();
  }

  getComplaints(){
    this.customerService.getComplaintss().subscribe((data: Complaint[]) => {
      this.complaints = data;
    }, error => {
      this.alertify.error(error);
    });
  }
  public SavePDF():void{

    const doc = new jsPDF()
    doc.autoTable({ html: '#content' })
    doc.save('ComplaintMetricsReport.pdf')

    // let content=this.content.nativeElement;
    // let doc = new jsPDF();
    // let _elementHandlers =
    // {
    //   '#editor':function(element,renderer){
    //     return true;
    //   }
    // };
    // doc.fromHTML(content.innerHTML,15,15,{

    //   'width':190,
    //   'elementHandlers':_elementHandlers
    // });

    // doc.save('ComplaintMetricsReport.pdf');
  }
}
