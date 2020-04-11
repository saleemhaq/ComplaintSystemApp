import { Component, OnInit } from '@angular/core';
import { Complaint } from '../_models/complaint';
import { Chart } from 'chart.js';
import { CustomerService } from '../_services/customer.service';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.css']
})
export class AdminDashboardComponent implements OnInit {
  data: Complaint[];  
  url = 'http://localhost:58617/API/Charts/GetCharts';
  Player = [];
  Run = [];  
  chart = [];  
  constructor(private customerService: CustomerService) { }  
  ngOnInit() {  
    this.customerService.getComplaintss().subscribe((data: Complaint[]) => {
      console.log('fresh data '+  data);  
      data.forEach(x => {  
        this.Player.push(x.complaintName);  
        this.Run.push(x.status);
      });          
      this.chart = new Chart('canvas', {  
        type: 'doughnut',
        data: {  
          labels: this.Player,  
          datasets: [  
            {  
              data: this.Run,  
              borderColor: '#3cba9f',  
              backgroundColor: [  
                "#3cb371",  
                "#0000FF",  
                "#9966FF",  
                "#4C4CFF",  
                "#00FFFF",  
                "#f990a7",  
                "#aad2ed",  
                "#FF00FF",  
                "Blue",  
                "Red",  
                "Blue"  
              ],  
              fill: true  
            }  
          ]  
        },  
        options: {  
          legend: {  
            display: true  
          },  
          scales: {  
            xAxes: [{  
              display: false  
            }],  
            yAxes: [{  
              display: true  
            }],  
          }  
        }  
      });  
    });  
  }  
}
